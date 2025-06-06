// site.js
// Denne fil initialiserer siden og sørger for generelle funktioner (som sortering)

document.addEventListener("DOMContentLoaded", function () {
    const table = document.querySelector(".table");
    if (table) {
        const headers = table.querySelectorAll("th");
        const tbody = table.querySelector("tbody");
        let sortDirections = Array.from(headers).map(() => 1);

        headers.forEach((header, columnIndex) => {
            header.style.cursor = "pointer";
            header.addEventListener("click", () => sortTable(columnIndex));
        });

        function sortTable(columnIndex) {
            const rows = Array.from(tbody.querySelectorAll("tr"));
            const isNumeric = rows.every(row => !isNaN(row.cells[columnIndex]?.innerText.trim()));

            rows.sort((rowA, rowB) => {
                let a = rowA.cells[columnIndex].innerText.trim();
                let b = rowB.cells[columnIndex].innerText.trim();

                if (isNumeric) {
                    a = parseFloat(a.replace(',', '.')) || 0;
                    b = parseFloat(b.replace(',', '.')) || 0;
                } else {
                    a = a.toLowerCase();
                    b = b.toLowerCase();
                }

                return a > b ? sortDirections[columnIndex] : a < b ? -sortDirections[columnIndex] : 0;
            });

            sortDirections[columnIndex] *= -1;
            tbody.innerHTML = "";
            rows.forEach(row => tbody.appendChild(row));
        }
    }
});

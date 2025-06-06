// weapon.js
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.weapon-item').forEach(el => {
        el.addEventListener('click', e => {
            e.preventDefault();
            openEditOverlay(el);
        });
    });

    const btn = document.getElementById('btnNewWeapon');
    if (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            openOverlay();
        });
    }
});

function openOverlay() {
    document.getElementById('overlayNew').style.display = 'block';
}

function closeOverlay() {
    document.getElementById('overlayNew').style.display = 'none';
}

function openEditOverlay(el) {
    console.log("openEditOverlay triggered", el);
    document.getElementById('overlayEdit').style.display = 'block';

    const fields = [
        'Id', 'Item', 'Bouns1', 'Bouns1pct', 'Bouns2', 'Bouns2pct',
        'Type', 'Lokation', 'Owner', 'Quality', 'Damage', 'Accuracy'
    ];

    fields.forEach(f => {
        const value = el.dataset[f.toLowerCase()] ?? '';
        const input = document.querySelector(`[name="SelectedForUpdate.${f}"]`);
        if (input) input.value = value;
    });

    document.getElementById('delete-id').value = el.dataset.id;
}

function closeEditOverlay() {
    document.getElementById('overlayEdit').style.display = 'none';
}

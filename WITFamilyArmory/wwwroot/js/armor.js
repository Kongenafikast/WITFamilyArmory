// armor.js
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.armor-item').forEach(el => {
        el.addEventListener('click', e => {
            e.preventDefault();
            console.log('Armor item clicked:', el);
            openEditArmorOverlay(el);
        });
    });

    const btn = document.getElementById('btnNewArmor');
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

function openEditArmorOverlay(el) {
    console.log("openEditArmorOverlay triggered", el);
    document.getElementById('overlayEdit').style.display = 'block';

    const fields = [
        'Id', 'Item', 'Bonus', 'Bounspct',
        'Lokation', 'Quality', 'ArmorPct', 'Coverage'
    ];

    fields.forEach(f => {
        const value = el.dataset[f.toLowerCase()] ?? '';
        const input = document.querySelector(`[name="SelectedForUpdate.${f}"]`);
        if (input) input.value = value;
    });

    const deleteInput = document.getElementById('delete-id');
    if (deleteInput && el.dataset.id) {
        deleteInput.value = el.dataset.id;
    }
}

function closeEditOverlay() {
    document.getElementById('overlayEdit').style.display = 'none';
}

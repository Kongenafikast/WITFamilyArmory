using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using WITFamilyArmory.Repository;
using static WITFamilyArmory.Models.Inventory;

namespace WITFamilyArmory.Pages
{
    public class RW_WeaponsModel : PageModel
    {
        private readonly DatabaseRepo _repo;

        [BindProperty]
        public Weapon NewWeapon { get; set; }

        [BindProperty]
        public Weapon SelectedForUpdate { get; set; }

        public List<Weapon> weapons { get; set; } = new();
        public List<SelectListItem> LocationOptions { get; set; }

        public string ItemFilter { get; set; }
        public string BounsFilter { get; set; }
        public string TypeFilter { get; set; }

        public bool LoggedOn { get; set; }

        public RW_WeaponsModel(DatabaseRepo repo)
        {
            _repo = repo;
        }

        public List<Weapon> FilteredWeapons => weapons
            .Where(w => (string.IsNullOrEmpty(ItemFilter) || w.Item.Contains(ItemFilter, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrEmpty(BounsFilter) || w.Bouns1.Contains(BounsFilter, StringComparison.OrdinalIgnoreCase) || w.Bouns2.Contains(BounsFilter, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrEmpty(TypeFilter) || w.Type.Contains(TypeFilter, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(w => w.Item)
            .ToList();

        public List<string> ItemOptions => weapons.Select(w => w.Item).Distinct().OrderBy(x => x).ToList();
        public List<string> BounsOptions => weapons.SelectMany(w => new[] { w.Bouns1, w.Bouns2 }).Distinct().OrderBy(x => x).ToList();
        public List<string> TypeOptions => weapons.Select(w => w.Type).Distinct().OrderBy(x => x).ToList();

        public void OnGet(string itemFilter, string bounsFilter, string typeFilter)
        {
            LoggedOn = User.Identity.IsAuthenticated;
            LoadLocationOptions();

            weapons = _repo.GetWeapons("SELECT * FROM weapon JOIN factionInfo ON weapon.location = factionInfo.factionId");

            ItemFilter = itemFilter;
            BounsFilter = bounsFilter;
            TypeFilter = typeFilter;
        }

        public IActionResult OnPostNew()
        {
            LoadLocationOptions();

            string query = MakeInsertQuery(NewWeapon);
            _repo.CreateItem(query);

            return RedirectToPage();
        }

        public IActionResult OnPostEdit()
        {

            string query = MakeUpdateQuery(SelectedForUpdate);
            _repo.UpdateItem(query);

            LoadLocationOptions();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(string idForDelete)
        {

            if (SelectedForUpdate?.Id != null)
            {
                string query = $"DELETE FROM weapon WHERE id = {idForDelete};";
                _repo.DeleteItem(query);
            }
            LoadLocationOptions();

            return RedirectToPage();
        }

        private void LoadLocationOptions()
        {
            LocationOptions = _repo.GetFactions()
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                })
                .ToList();
        }

        private string MakeInsertQuery(Weapon w)
        {
            var columns = new List<string>();
            var values = new List<string>();

            void AddStringField(string column, string? value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    columns.Add(column);
                    values.Add($"'{Escape(value)}'");
                }
            }

            void AddNumericField(string column, decimal? value)
            {
                if (value.HasValue)
                {
                    columns.Add(column);
                    values.Add(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            AddStringField("weaponName", w.Item);
            AddStringField("weaponType", w.Type);
            AddStringField("weaponBouns1", w.Bouns1);
            AddNumericField("weaponBouns1pct", w.Bouns1pct);
            AddStringField("weaponBouns2", w.Bouns2);
            AddNumericField("weaponBouns2pct", w.Bouns2pct);
            AddStringField("owner", w.Owner);
            if (w.Lokation != null)
            {
                columns.Add("location");
                values.Add(w.Lokation.Id.ToString());
            }
            AddNumericField("quality", w.Quality);
            AddNumericField("accuracy", w.Accuracy);
            AddNumericField("damage", w.Damage);

            return $@"
INSERT INTO weapon ({string.Join(", ", columns)}) 
VALUES ({string.Join(", ", values)});";
        }



        private string MakeUpdateQuery(Weapon w)
        {
            var sets = new List<string>();

            void AddStringSet(string column, string? value)
            {
                if (!string.IsNullOrEmpty(value))
                    sets.Add($"{column} = '{Escape(value)}'");
            }

            void AddNumericSet(string column, decimal? value)
            {
                if (value.HasValue)
                    sets.Add($"{column} = {value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }

            AddStringSet("weaponName", w.Item);
            AddStringSet("weaponType", w.Type);
            AddStringSet("weaponBouns1", w.Bouns1);
            AddNumericSet("weaponBouns1pct", w.Bouns1pct);
            AddStringSet("weaponBouns2", w.Bouns2);
            AddNumericSet("weaponBouns2pct", w.Bouns2pct);
            AddStringSet("owner", w.Owner);
            if (w.Lokation != null)
                sets.Add($"location = {w.Lokation.Id}");
            AddNumericSet("quality", w.Quality);
            AddNumericSet("accuracy", w.Accuracy);
            AddNumericSet("damage", w.Damage);

            if (sets.Count == 0)
                throw new InvalidOperationException("No fields to update.");

            return $@"
UPDATE weapon SET 
{string.Join(",\n", sets)}
WHERE id = {w.Id};";
        }
        private string Escape(string? input)
        {
            return input?.Replace("'", "''") ?? "";
        }
    }
}

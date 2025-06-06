using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WITFamilyArmory.Repository;
using static WITFamilyArmory.Models.Inventory;

namespace WITFamilyArmory.Pages
{
    public class RW_ArmorModel : PageModel
    {
        private readonly DatabaseRepo _repo;

        [BindProperty]
        public Armor NewArmor { get; set; }

        [BindProperty]
        public Armor SelectedForUpdate { get; set; }

        public List<Armor> armors { get; set; } = new();
        public List<SelectListItem> LocationOptions { get; set; }

        public string ItemFilter { get; set; }
        public bool LoggedOn { get; set; }

        public RW_ArmorModel(DatabaseRepo repo)
        {
            _repo = repo;
        }

        public List<Armor> FilteredArmors => armors
            .Where(a => string.IsNullOrEmpty(ItemFilter) || a.Item.Contains(ItemFilter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.Item)
            .ToList();

        public List<string> ItemOptions => armors.Select(a => a.Item).Distinct().OrderBy(x => x).ToList();

        public void OnGet(string itemFilter)
        {
            LoggedOn = User.Identity.IsAuthenticated;
            LoadLocationOptions();

            armors = _repo.GetArmors("SELECT * FROM armor JOIN factionInfo ON armor.location = factionInfo.factionId");

            ItemFilter = itemFilter;
        }

        public IActionResult OnPostNew()
        {
            LoadLocationOptions();

            string query = MakeInsertQuery(NewArmor);
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
                string query = $"DELETE FROM armor WHERE id = {idForDelete};";
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

        private string MakeInsertQuery(Armor a)
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

            AddStringField("armorName", a.Item);
            AddStringField("armorBonus", a.Bonus);
            AddNumericField("armorBonuspct", a.Bounspct);
            if (a.Lokation != null)
            {
                columns.Add("location");
                values.Add(a.Lokation.Id.ToString());
            }
            AddNumericField("quality", a.Quality);
            AddNumericField("armor", a.ArmorPct);
            AddNumericField("coverage", a.Coverage);

            return $@"
INSERT INTO armor ({string.Join(", ", columns)})
VALUES ({string.Join(", ", values)});";
        }

        private string MakeUpdateQuery(Armor a)
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

            AddStringSet("armorName", a.Item);
            AddStringSet("armorBonus", a.Bonus);
            AddNumericSet("armorBonuspct", a.Bounspct);
            if (a.Lokation != null)
                sets.Add($"location = {a.Lokation.Id}");
            AddNumericSet("quality", a.Quality);
            AddNumericSet("armor", a.ArmorPct);
            AddNumericSet("coverage", a.Coverage);
            AddStringSet("owner", a.Owner);

            if (sets.Count == 0)
                throw new InvalidOperationException("No fields to update.");

            return $@"
UPDATE armor SET
{string.Join(",\n", sets)}
WHERE id = {a.Id};";
        }

        private string Escape(string? input)
        {
            return input?.Replace("'", "''") ?? "";
        }
    }
}

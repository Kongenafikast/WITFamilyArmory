using Microsoft.AspNetCore.Mvc.RazorPages;
using WITFamilyArmory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static WITFamilyArmory.Models.Inventory;
using WITFamilyArmory.Repository;

namespace WITFamilyArmory.Pages
{
    public class ArmoryModel : PageModel
    {
        private readonly APIRepo _repo;
        public string typeOfArmory { get; set; }

        public Dictionary<string, FactionDrugsResponse> drugs { get; set; }
        public Dictionary<string, FactionMedicalsResponse> meds { get; set; }
        public Dictionary<string, FactionTemporaryResponse> temps { get; set; }
        public Dictionary<string, FactionBoosterResponse> boosters { get; set; }

        public Dictionary<string, TotalItem> TotalItems { get; set; } = new();

        public ArmoryModel(APIRepo repo)
        {
            _repo = repo;
        }

        public void OnGet(string armorytype)
        {
            typeOfArmory = armorytype;

            switch (typeOfArmory)
            {
                case "drugs":
                    drugs = _repo.GetDrugs();
                    foreach (var kvp in drugs)
                    {
                        var factionCode = kvp.Key;
                        var items = kvp.Value.drugs;

                        foreach (var item in items)
                        {
                            AddToTotal(item.name, item.quantity, 0);
                        }

                        var display = _repo.GetXanaxForFaction(factionCode).Result;
                        var xanax = display?.display.FirstOrDefault(x => x.name.Equals("Xanax", StringComparison.OrdinalIgnoreCase));
                        if (xanax != null)
                        {
                            AddToTotal("Xanax", 0, xanax.quantity);
                        }
                    }
                    break;

                case "meds":
                    meds = _repo.GetMeds();
                    foreach (var kvp in meds)
                    {
                        foreach (var item in kvp.Value.medical)
                        {
                            AddToTotal(item.name, item.quantity, 0);
                        }
                    }
                    break;

                case "temps":
                    temps = _repo.GetTemps();
                    foreach (var kvp in temps)
                    {
                        foreach (var item in kvp.Value.temporary)
                        {
                            AddToTotal(item.name, item.quantity, 0);
                        }
                    }
                    break;

                case "boosters":
                    boosters = _repo.GetBoosters();
                    foreach (var kvp in boosters)
                    {
                        foreach (var item in kvp.Value.boosters)
                        {
                            AddToTotal(item.name, item.quantity, 0);
                        }
                    }
                    break;
            }
        }

        private void AddToTotal(string name, int armoryQty, int displayQty)
        {
            if (!TotalItems.ContainsKey(name))
            {
                TotalItems[name] = new TotalItem { Name = name };
            }

            TotalItems[name].QuantityArmory += armoryQty;
            TotalItems[name].QuantityDisplay += displayQty;
        }
    }
}

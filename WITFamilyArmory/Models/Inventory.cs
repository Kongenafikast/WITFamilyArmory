using Microsoft.AspNetCore.Mvc.Rendering;

namespace WITFamilyArmory.Models
{
    public class Inventory
    {
        public class Weapon
        {
            public int Id { get; set; }
            public string Item { get; set; }
            public string Bouns1 { get; set; }
            public decimal? Bouns1pct { get; set; }
            public string Bouns2 { get; set; }
            public decimal? Bouns2pct { get; set; }
            public decimal? Quality {  get; set; }
            public decimal? Accuracy { get; set; }
            public decimal? Damage{ get; set; }
            public string Type { get; set; }
            public string Owner { get; set; }
            public FactionInfo Lokation { get; set; }
        }
        public class Armor
        {
            public int Id { get; set; }
            public string Item { get; set; }
            public string Bonus { get; set; }
            public decimal? Bounspct { get; set; }
            public decimal? Quality { get; set; }
            public decimal? ArmorPct { get; set; }
            public decimal? Coverage { get; set; }
            public FactionInfo Lokation { get; set; }
            public string? Owner { get; set; }
        }
        public class FileModel
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        public class FactionInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class WeaponViewModel
        {
            public Weapon SelectedForUpdate { get; set; }
            public List<SelectListItem> LocationOptions { get; set; }
        }
        public class Drug
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int quantity { get; set; }
        }
        public class Medical
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int quantity { get; set; }
        }
        public class Temporary
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int quantity { get; set; }
            public int available { get; set; }
            public int loaned { get; set; }
            public object loaned_to { get; set; }
        }
        public class Booster
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int quantity { get; set; }
        }

        public class Display
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int quantity { get; set; }
            public int circulation { get; set; }
            public int market_price { get; set; }
        }
        public class FactionDrugsResponse
        {
            public List<Drug> drugs { get; set; }
        }
        public class FactionMedicalsResponse
        {
            public List<Medical> medical { get; set; }
        }
        public class FactionTemporaryResponse
        {
            public List<Temporary> temporary { get; set; }
        }
        public class FactionBoosterResponse
        {
            public List<Booster> boosters { get; set; }
        }
        public class UserDisplayResponse
        {
            public List<Display> display { get; set; }
        }

        public class TotalItem
        {
            public string Name { get; set; }
            public int QuantityArmory { get; set; }
            public int QuantityDisplay { get; set; }
            public int QuantityTotal => QuantityArmory + QuantityDisplay;
        }

        public class Basicicons
        {
            public string icon5 { get; set; }
            public string icon6 { get; set; }
            public string icon4 { get; set; }
            public string icon8 { get; set; }
            public string icon73 { get; set; }
            public string icon74 { get; set; }
        }

        public class Competition
        {
            public string name { get; set; }
            public string status { get; set; }
            public int current_hp { get; set; }
            public int max_hp { get; set; }
        }

        public class Faction
        {
            public string position { get; set; }
            public int faction_id { get; set; }
            public int days_in_faction { get; set; }
            public string faction_name { get; set; }
            public string faction_tag { get; set; }
            public string faction_tag_image { get; set; }
        }

        public class Job
        {
            public string job { get; set; }
            public string position { get; set; }
            public int company_id { get; set; }
            public string company_name { get; set; }
            public int company_type { get; set; }
        }

        public class LastAction
        {
            public string status { get; set; }
            public int timestamp { get; set; }
            public string relative { get; set; }
        }

        public class Life
        {
            public int current { get; set; }
            public int maximum { get; set; }
            public int increment { get; set; }
            public int interval { get; set; }
            public int ticktime { get; set; }
            public int fulltime { get; set; }
        }

        public class Married
        {
            public int spouse_id { get; set; }
            public string spouse_name { get; set; }
            public int duration { get; set; }
        }

        public class UserData
        {
            public string rank { get; set; }
            public int level { get; set; }
            public int honor { get; set; }
            public string gender { get; set; }
            public string property { get; set; }
            public string signup { get; set; }
            public int awards { get; set; }
            public int friends { get; set; }
            public int enemies { get; set; }
            public int forum_posts { get; set; }
            public int karma { get; set; }
            public int age { get; set; }
            public string role { get; set; }
            public int donator { get; set; }
            public int player_id { get; set; }
            public string name { get; set; }
            public int property_id { get; set; }
            public int revivable { get; set; }
            public string profile_image { get; set; }
            public Life life { get; set; }
            public Status status { get; set; }
            public Job job { get; set; }
            public Faction faction { get; set; }
            public Married married { get; set; }
            public Basicicons basicicons { get; set; }
            public States states { get; set; }
            public LastAction last_action { get; set; }
            public Competition competition { get; set; }
        }

        public class States
        {
            public int hospital_timestamp { get; set; }
            public int jail_timestamp { get; set; }
        }

        public class Status
        {
            public string description { get; set; }
            public string details { get; set; }
            public string state { get; set; }
            public string color { get; set; }
            public int until { get; set; }
        }

        public class userInfo
        {
            public int ID { get; set; }
            public int TornId { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public FactionInfo faction { get; set; }
        }
    }
}

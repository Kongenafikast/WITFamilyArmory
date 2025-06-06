using System.Text.Json;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using static WITFamilyArmory.Models.Inventory;

namespace WITFamilyArmory.Repository
{
    public class APIRepo
    {
        private readonly Dictionary<string, string> APIKeys;
        private readonly HttpClient _httpClient;
        public APIRepo(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            APIKeys = new Dictionary<string, string>();
            APIKeys.Add("Pr", configuration.GetSection("APIkeys")["PR"]);
            APIKeys.Add("Mh", configuration.GetSection("APIkeys")["MH"]); 
            APIKeys.Add("Rev", configuration.GetSection("APIkeys")["REV"]);

        }

        public Dictionary<string, FactionDrugsResponse> GetDrugs()
        {
            Dictionary<string, FactionDrugsResponse> drugs = new Dictionary<string, FactionDrugsResponse>();  
            foreach (var key in APIKeys)
            {
                drugs.Add(key.Key, GetFactionDrugsAsync(key.Value.ToString()).Result);
            }
            return drugs;
        }

        private async Task<FactionDrugsResponse> GetFactionDrugsAsync(string key)
        {
            string url = $"https://api.torn.com/faction/?selections=drugs&key={key}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var parsed = JsonSerializer.Deserialize<FactionDrugsResponse>(
                    json);

                return parsed;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Dictionary<string, FactionMedicalsResponse> GetMeds()
        {
            Dictionary<string, FactionMedicalsResponse> meds = new Dictionary<string, FactionMedicalsResponse>();
            foreach (var key in APIKeys)
            {
                meds.Add(key.Key, GetFactionMedsAsync(key.Value.ToString()).Result);
            }
            return meds;
        }

        private async Task<FactionMedicalsResponse> GetFactionMedsAsync(string key)
        {
            string url = $"https://api.torn.com/faction/?selections=medical&key={key}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                FactionMedicalsResponse parsed = JsonSerializer.Deserialize<FactionMedicalsResponse>(json);

                return parsed;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Dictionary<string, FactionTemporaryResponse> GetTemps()
        {
            Dictionary<string, FactionTemporaryResponse> temps = new Dictionary<string, FactionTemporaryResponse>();
            foreach (var key in APIKeys)
            {
                temps.Add(key.Key, GetFactionTempsAsync(key.Value.ToString()).Result);
            }
            return temps;
        }

        private async Task<FactionTemporaryResponse> GetFactionTempsAsync(string key)
        {
            string url = $"https://api.torn.com/faction/?selections=temporary&key={key}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                FactionTemporaryResponse parsed = JsonSerializer.Deserialize<FactionTemporaryResponse>(json);

                return parsed;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Dictionary<string, FactionBoosterResponse> GetBoosters()
        {
            Dictionary<string, FactionBoosterResponse> temps = new Dictionary<string, FactionBoosterResponse>();
            foreach (var key in APIKeys)
            {
                temps.Add(key.Key, GetFactionBoosterAsync(key.Value.ToString()).Result);
            }
            return temps;
        }

        private async Task<FactionBoosterResponse> GetFactionBoosterAsync(string key)
        {
            string url = $"https://api.torn.com/faction/?selections=boosters&key={key}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                FactionBoosterResponse parsed = JsonSerializer.Deserialize<FactionBoosterResponse>(json);

                return parsed;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<UserDisplayResponse> GetXanaxForFaction(string faction)
        {
            int userId = faction switch
            {
                "Pr" => 422025,
                "Mh" => 671593,
                "Rev" => 1847797,
                _ => 0
            };

            if (userId == 0) return null;

            string url = $"https://api.torn.com/user/{userId}?selections=display&key=0gib9iHzi1NHaU6w";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDisplayResponse>(json);
            }
            catch
            {
                return null;
            }
        }
        public async Task<UserData> GetUserData(string userId)
        {
            string url = $"https://api.torn.com/user/{userId}?selections=&key=0gib9iHzi1NHaU6w";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserData>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}


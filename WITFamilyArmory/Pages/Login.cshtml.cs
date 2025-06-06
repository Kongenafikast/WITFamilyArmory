using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WITFamilyArmory.Repository;
using Org.BouncyCastle.Bcpg;
using static WITFamilyArmory.Models.Inventory;
using System.Security.Cryptography;

namespace WITFamilyArmory.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
        private readonly StampHolder _stamp;
        public APIRepo _apiRepo;
        public DatabaseRepo _databaseRepo;
        private const int SaltSize = 32;
        private const int HashSize = 64;
        private const int Iterations = 200000;

        public LoginModel(StampHolder stampHolder, APIRepo repo, DatabaseRepo dbrepo)
        {
            _stamp = stampHolder;
            _apiRepo = repo;
            _databaseRepo = dbrepo;
        }

        public async Task<IActionResult> OnPost()
        {
            string query = "";
            if (int.TryParse(Username, out var username))
            {
                query = $"SELECT * FROM userInfo JOIN factionInfo ON userInfo.faction = factionInfo.factionId WHERE userInfo.tornId = {Username}";
            }
            else
            {
                query = $"SELECT * FROM userInfo JOIN factionInfo ON userInfo.faction = factionInfo.factionId WHERE userInfo.tornUsername = '{Username}'";
            }

            userInfo userInfo = _databaseRepo.GetUserInfo(query);
            UserData user = await GetAccessLevel();
            List<string> roles = new List<string>() { "Leader", "Co-leader", "Renown SME", "WIT Council", "Co leader", "Council", "Lieutenant", "Sergeant", "Assistant Chief", "Deputy Chief", "Chief", "Captain" };
            if (userInfo != null)
            {

                if (roles.Contains(user.faction.position) && VerifyPassword(Password, userInfo.Password))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.name),
                new Claim(ClaimTypes.SerialNumber, user.player_id.ToString()),
                new Claim(ClaimTypes.Role, user.faction.position),
                new Claim("SecurityStamp",_stamp.Stamp)
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToPage("/Index");
                }
            }

            ErrorMessage = "Use your torn Id and the password provided by KongBoll";
            return Page();
        }
        private async Task<UserData> GetAccessLevel()
        {
            return _apiRepo.GetUserData(Username).Result;
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            byte[] storedPasswordHash = new byte[HashSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, storedPasswordHash, 0, HashSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(HashSize);

            return CryptographicOperations.FixedTimeEquals(storedPasswordHash, computedHash);
        }

    }
}

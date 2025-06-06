using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using static WITFamilyArmory.Models.Inventory;

namespace WITFamilyArmory.Pages
{
    public class factionModel : PageModel
    {
        public string folder {  get; set; }
        public List<FileModel> FileList { get; set; }

        // Inject IWebHostEnvironment to get the root path
        private readonly IWebHostEnvironment _env;

        public factionModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet(string folderName)
        {
            folder = folderName;
            // Get the path to the "files" folder inside wwwroot/folderName
            string folderPath = Path.Combine(_env.WebRootPath, folderName);

            // Ensure the folder exists
            if (Directory.Exists(folderPath))
            {
                // Get all files from the folder
                var files = Directory.GetFiles(folderPath)
                    .Select(file => new FileInfo(file))
                    .Select(fileInfo => new FileModel
                    {
                        Name = fileInfo.Name,
                        Url = $"/{folderName}/{fileInfo.Name}"}).ToList();

                // Assign the files to the FileList
                FileList = files;
            }
            else
            {
                // If folder doesn't exist, initialize an empty list
                FileList = new List<FileModel>();
            }
        }
    }
}

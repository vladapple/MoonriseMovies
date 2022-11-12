using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;  
using System.IO;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;


namespace MoonriseMovies.Pages.Admin
{
    [Authorize(Roles="Admin")]
    public class DeleteInBlobModel : PageModel
    {
        private readonly string _connectionString;
        private readonly string _containerName;
         public DeleteInBlobModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("BlobConnectionString");
            _containerName = configuration.GetValue<string>("BlobContainerName");
        }
        public void OnGet()
        {
        }

        public async Task < IActionResult > OnPostAsync(string fileName) 
        {  
            string blobstorageconnection = _connectionString;  
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();  
            string strContainerName = _containerName;  
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);  
            var blob = cloudBlobContainer.GetBlobReference(fileName);  
            await blob.DeleteIfExistsAsync();  
            return RedirectToPage("../Admin/Index");  
        }   
    }
}

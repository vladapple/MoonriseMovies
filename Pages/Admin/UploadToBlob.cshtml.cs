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
    public class UploadToBlobModel : PageModel
    {
        private readonly string _connectionString;
        private readonly string _containerName;
         public UploadToBlobModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("BlobConnectionString");
            _containerName = configuration.GetValue<string>("BlobContainerName");
        }
        public void OnGet()
        {
        }

        public async Task < IActionResult > OnPostAsync(IFormFile files) 
        {  
            string systemFileName = files.FileName;  
            string blobstorageconnection = _connectionString;  
            // Retrieve storage account from connection string.    
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
            // Create the blob client.    
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();  
            // Retrieve a reference to a container.    
            CloudBlobContainer container = blobClient.GetContainerReference(_containerName); 
            // This also does not make a service call; it only creates a local object.    
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);  
            await using(var data = files.OpenReadStream()) 
            {  
                await blockBlob.UploadFromStreamAsync(data);  
            }  
            return RedirectToPage("../Admin/Index");  
        }
    }
}

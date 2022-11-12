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
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace MoonriseMovies.Pages.Admin
{
    [Authorize(Roles="Admin")]
    public class DownloadFromBlobModel : PageModel
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public List<BlobItem> blobs { get; set; } = new List<BlobItem>();

         public DownloadFromBlobModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("BlobConnectionString");
            _containerName = configuration.GetValue<string>("BlobContainerName");
        }
        public void OnGet()
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_connectionString, _containerName);
            blobContainerClient.CreateIfNotExists();
            blobs = blobContainerClient.GetBlobs().ToList();
        }

        public async Task < IActionResult > OnPostAsync(string fileName) 
        { 
            try
            {
                CloudBlockBlob blockBlob;  
                await using(MemoryStream memoryStream = new MemoryStream()) 
                {  
                    string blobstorageconnection = _connectionString;  
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();  
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_containerName);  
                    blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);  
                    await blockBlob.DownloadToStreamAsync(memoryStream);  
                }  
                Stream blobStream = blockBlob.OpenReadAsync().Result;  
                
                return File(blobStream, blockBlob.Properties.ContentType, blockBlob.Name);
            }catch{
                return RedirectToPage("../../Admin/Blobs/FailFromBlob");
            }      
        } 
    }
}

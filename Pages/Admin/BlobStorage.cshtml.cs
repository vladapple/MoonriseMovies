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
    public class BlobStorageModel : PageModel
    {
        private readonly string _connectionString;
        private readonly string _containerName;
         public BlobStorageModel(IConfiguration configuration)
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
/*
        [HttpPost(nameof(DownloadFile))]
        public async Task < IActionResult > DownloadFile(string fileName) 
        {  
            CloudBlockBlob blockBlob;  
            await using(MemoryStream memoryStream = new MemoryStream()) 
            {  
                string blobstorageconnection = _configuration.GetValue < string > ("BlobConnectionString");  
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();  
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_configuration.GetValue < string > ("BlobContainerName"));  
                blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);  
                await blockBlob.DownloadToStreamAsync(memoryStream);  
            }  
            Stream blobStream = blockBlob.OpenReadAsync().Result;  
            return File(blobStream, blockBlob.Properties.ContentType, blockBlob.Name);  
        } 

        [HttpDelete(nameof(DeleteFile))]
        public async Task < IActionResult > DeleteFile(string fileName) 
        {  
            string blobstorageconnection = _configuration.GetValue < string > ("BlobConnectionString");  
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();  
            string strContainerName = _configuration.GetValue < string > ("BlobContainerName");  
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);  
            var blob = cloudBlobContainer.GetBlobReference(fileName);  
            await blob.DeleteIfExistsAsync();  
            return RedirectToPage("../Admin/Index");  
        } 
*/    
    }
}

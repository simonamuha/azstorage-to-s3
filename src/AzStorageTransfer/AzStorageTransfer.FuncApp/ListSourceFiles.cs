using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzStorageTransfer.FuncApp
{
    public class ListSourceFiles
    {
        private readonly CloudBlobClient cloudBlobClient;

        public ListSourceFiles()
        {
            this.cloudBlobClient = CloudStorageAccount.Parse(Config.DataStorageConnection).CreateCloudBlobClient();
        }

        [FunctionName(nameof(ListSourceFiles))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "sourcefiles")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var blobs = this.cloudBlobClient.GetContainerReference(Config.ScheduledContainer).ListBlobs();
                var blobList = blobs.ToList();
                return new OkObjectResult(blobList);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new { error = ex.ToString() });
            }
        }
    }
}
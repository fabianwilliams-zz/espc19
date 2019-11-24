using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;
using System.Net.Http;

namespace FunctWithFabs.http2Blob
{
    //credit for my modivied versioin of this function belongs to fellow MVP Michal Jankowski
    //https://www.jankowskimichal.pl/en/2018/02/azure-function-uploading-photos-to-azure-blob-storage/ 

    public static class PictureToBlobStor
    {
        [FunctionName("PictureB64ToBlobStor")]
        public static async Task<HttpResponseMessage> AddPicToBlobStor(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Fabian Sending Image from Webhook to Blob Storage");

            dynamic data = await req.Content.ReadAsAsync<object>();
            string photoBase64String = data.photoBase64;
            Uri uri = await UploadBlobAsync(photoBase64String);
            return req.CreateResponse(HttpStatusCode.Accepted, uri);
        }

        public static async Task<Uri> UploadBlobAsync(string photoBase64String)
        {
            var match = new Regex(
              $@"^data\:(?<type>image\/(jpeg|jpg|gif|png));base64,(?<data>[A-Z0-9\+\/\=]+)$",
              RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase)
              .Match(photoBase64String);
            string contentType = match.Groups["type"].Value;
            //string contentType = match.Groups["data"].Value;
            string extension = contentType.Split('/')[1];
            string fileName = $"{Guid.NewGuid().ToString()}.{extension}";
            byte[] photoBytes = Convert.FromBase64String(match.Groups["data"].Value);


            CloudStorageAccount storageAccount = CloudStorageAccount
              .Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("fwf-selfie-stor");

            await container.CreateIfNotExistsAsync(
              BlobContainerPublicAccessType.Blob,
              new BlobRequestOptions(),
              new OperationContext());
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = contentType;

            using (Stream stream = new MemoryStream(photoBytes, 0, photoBytes.Length))
            {
                await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);
            }

            return blob.Uri;
        }
    }
}

using Abp.Configuration.Startup;
using Abp.Dependency;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Integration.Modules.Interfaces;


namespace TDV.Integration.Modules.BlobStorage
{
    public class BlobStorageModule : IBlobStorageModule, ITransientDependency
    {
        private BlobServiceClient blobServiceClient { get; set; }
        private BlobContainerClient documentContainer { get; set; }
        public BlobStorageModule(BlobStorageConfig configuration) {
            blobServiceClient = new(configuration.ConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient("documents");
            containerClient.CreateIfNotExists(PublicAccessType.BlobContainer);
            documentContainer = containerClient;
        }

        private string GetMimeType(string fileExt)
        {
            switch (true)
            {
                case true when fileExt.Contains(".pdf"):
                    return "application/pdf";
                case true when (fileExt.Contains(".jpg") || fileExt.Contains(".jpeg")):
                    return "image/jpeg";
                case true when fileExt.Contains(".png"):
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }

        public async Task<UploadedModel> Upload(Stream stream, string name)
        {
            var uniqID= Guid.NewGuid().ToString("N");

            name = name.Replace("{UUID}", uniqID);

            Dictionary<string, string> tags = new();
            tags.Add("uuid", uniqID);

            var blobClient = documentContainer.GetBlobClient(name);
            await blobClient.DeleteIfExistsAsync(); // overwrite
            await blobClient.UploadAsync(stream, new BlobUploadOptions{ HttpHeaders= new BlobHttpHeaders { ContentType= GetMimeType(name) }  });
            await blobClient.SetTagsAsync(tags);
            return new UploadedModel() { 
                Guid= uniqID,
                Path= blobClient.Uri.AbsolutePath
            };
        }
        private async Task<BlobClient> GetDocumentBlobClient(string uuid)
        {
            List<TaggedBlobItem> blobs = new List<TaggedBlobItem>();
            var gentr = $@"""uuid""=='{uuid}'";
            await foreach (TaggedBlobItem taggedBlobItem in documentContainer.FindBlobsByTagsAsync($@"""uuid""='{uuid}'"))
            {
                blobs.Add(taggedBlobItem);
            }

            var findedBlob = blobs.FirstOrDefault();

            return findedBlob == null ? null : documentContainer.GetBlobClient(findedBlob.BlobName);
        }

        public async Task<string> GetDocumentBlob(string uuid)
        {
            var findedBlob = await GetDocumentBlobClient(uuid);
            return findedBlob==null ? null : findedBlob.Uri.AbsoluteUri;
        }

        public async Task DeleteDocumentBlob(string uuid)
        {
            var findedBlob = await GetDocumentBlobClient(uuid);
            if(findedBlob!=null) await findedBlob?.DeleteIfExistsAsync();
        }
    }
}

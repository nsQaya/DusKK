using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDV.Integration.Modules.BlobStorage;

namespace TDV.Integration.Modules.Interfaces
{
    public interface IBlobStorageModule
    {
        Task<UploadedModel> Upload(Stream stream, string name);
        Task<string> GetDocumentBlob(string uuid);

        Task DeleteDocumentBlob(string uuid);
    }
}

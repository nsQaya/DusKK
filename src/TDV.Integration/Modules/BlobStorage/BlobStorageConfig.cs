using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDV.Integration.Modules.BlobStorage
{
    public class BlobStorageConfig
    {
        public string ConnectionString { get; set; }
        public long MaxSize { get; set; }
        public string MaxSizeFriendly { get; set; }
        public string[] AllowedMimes { get; set; }
    }
}

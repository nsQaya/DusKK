using System;
using System.Collections.Generic;
using System.Text;

namespace TDV.Burial.Dtos
{
    public class GetFuneraPackagelForViewDto
    {
        public FuneralPackageDto Package { get; set; }
        public List<FuneralDto> Funerals { get; set; }
    }
}

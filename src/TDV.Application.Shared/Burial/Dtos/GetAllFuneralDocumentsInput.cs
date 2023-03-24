using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralDocumentsInput : PagedAndSortedResultRequestDto
    {
        public int FuneralID { get; set; }
        public string Filter { get; set; }

        public int? TypeFilter { get; set; }

        public string PathFilter { get; set; }

        public Guid? GuidFilter { get; set; }

        public string FuneralDisplayPropertyFilter { get; set; }

    }
}
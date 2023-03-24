using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TDV.Editions.Dto;

namespace TDV.Web.Areas.Portal.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}
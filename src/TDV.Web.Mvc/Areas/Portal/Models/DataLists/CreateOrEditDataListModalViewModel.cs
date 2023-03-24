using TDV.Constants.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.DataLists
{
    public class CreateOrEditDataListModalViewModel
    {
        public CreateOrEditDataListDto DataList { get; set; }

        public bool IsEditMode => DataList.Id.HasValue;
    }
}
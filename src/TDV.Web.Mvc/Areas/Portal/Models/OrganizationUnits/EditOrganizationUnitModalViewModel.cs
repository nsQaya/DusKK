﻿using Abp.AutoMapper;
using Abp.Organizations;

namespace TDV.Web.Areas.Portal.Models.OrganizationUnits
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class EditOrganizationUnitModalViewModel
    {
        public long? Id { get; set; }

        public string DisplayName { get; set; }
    }
}
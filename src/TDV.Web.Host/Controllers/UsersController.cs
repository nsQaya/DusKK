﻿using Abp.AspNetCore.Mvc.Authorization;
using TDV.Authorization;
using TDV.Storage;
using Abp.BackgroundJobs;

namespace TDV.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}
using Abp.AspNetCore.Mvc.Authorization;
using TDV.Authorization.Users.Profile;
using TDV.Graphics;
using TDV.Storage;

namespace TDV.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageFormatValidator imageFormatValidator) :
            base(tempFileCacheManager, profileAppService, imageFormatValidator)
        {
        }
    }
}
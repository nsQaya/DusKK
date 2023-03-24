using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Controllers;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize]
    public class WelcomeController : TDVControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
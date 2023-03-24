using Microsoft.AspNetCore.Mvc;
using TDV.Web.Controllers;

namespace TDV.Web.Public.Controllers
{
    public class AboutController : TDVControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
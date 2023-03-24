using Microsoft.AspNetCore.Mvc;
using TDV.Web.Controllers;

namespace TDV.Web.Public.Controllers
{
    public class HomeController : TDVControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
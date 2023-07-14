using System.Web.Mvc;

namespace SofaOnSofa.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Только для зареганых юзеров
        [Authorize(Roles = "Пользователь")]
        public ActionResult UserArea()
        {
            return View();
        }

        //Админ
        [Authorize(Roles = "Админ")]
        public ActionResult AdminArea()
        {
            return View();
        }
    }
}
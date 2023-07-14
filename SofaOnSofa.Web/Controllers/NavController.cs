using SofaOnSofa.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SofaOnSofa.Web.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public PartialViewResult Menu(string style = null)
        {
            ViewBag.SelectedStyle = style;

            IEnumerable<string> styles = repository.Products
                .Select(x => x.Style)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(styles);
        }
    }
}
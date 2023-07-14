using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Options;
using SofaOnSofa.Web.Binders;
using SofaOnSofa.Web.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SofaOnSofa.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            using(DB db = new DB())
            {
                if (db.Roles.Count() == 0)
                {
                    var role1 = new UserRole();
                    var role2 = new UserRole();

                    role1.RoleName = "Пользователь";
                    role2.RoleName = "Админ";

                    db.Roles.Add(role1);
                    db.Roles.Add(role2);
                    db.SaveChanges();
                }
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            ModelBinders.Binders.Add(typeof(ShoppingCart), new CartModelBinder());
        }
    }
}

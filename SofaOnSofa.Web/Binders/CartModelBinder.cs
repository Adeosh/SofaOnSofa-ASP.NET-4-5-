using SofaOnSofa.Domain.Entities;
using System.Web.Mvc;

namespace SofaOnSofa.Web.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ShoppingCart shoppingCart = (ShoppingCart)controllerContext.HttpContext.Session[sessionKey];

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                controllerContext.HttpContext.Session[sessionKey] = shoppingCart;
            }
            return shoppingCart;
        }
    }
}
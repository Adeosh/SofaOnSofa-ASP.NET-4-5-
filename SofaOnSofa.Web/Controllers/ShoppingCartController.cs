using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using SofaOnSofa.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace SofaOnSofa.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public ShoppingCartController(IProductRepository repository, IOrderProcessor orderProcessor)
        {
            this.repository = repository;
            this.orderProcessor = orderProcessor;
        }

        public ViewResult Index(ShoppingCart shoppingCart, string returnUrl)
        {
            return View(new ShoppingCartIndex
            {
                ShoppingCart = shoppingCart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToShoppingCart(ShoppingCart shoppingCart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                shoppingCart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromShoppingCart(ShoppingCart shoppingCart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                shoppingCart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public ViewResult Checkout(ShoppingCart shoppingCart, ShippingDetails shippingDetails)
        {
            if (shoppingCart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(shoppingCart, shippingDetails);
                shoppingCart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        public PartialViewResult Summary(ShoppingCart shoppingCart)
        {
            return PartialView(shoppingCart);
        }
    }
}
using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using SofaOnSofa.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace SofaOnSofa.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ViewResult List(string style, int page = 1)
        {
            ProductList viewModel = new ProductList
            {
                Products = repository.Products
                    .Where(p => style == null || p.Style == style)
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = style == null ?
                        repository.Products.Count() :
                        repository.Products.Where(e => e.Style == style).Count()
                },
                CurrentStyle = style
            };
            return View(viewModel);
        }
    }
}
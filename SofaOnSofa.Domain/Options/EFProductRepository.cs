using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using System.Linq;

namespace SofaOnSofa.Domain.Options
{
    public class EFProductRepository : IProductRepository
    {
        private DB context = new DB();
        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Mechanism = product.Mechanism;
                    dbEntry.Style = product.Style;
                    dbEntry.FabricType = product.FabricType;
                    dbEntry.Features = product.Features;
                    dbEntry.Price = product.Price;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = context.Products.Find(productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}

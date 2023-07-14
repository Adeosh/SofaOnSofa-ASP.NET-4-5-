using SofaOnSofa.Domain.Entities;
using System.Linq;

namespace SofaOnSofa.Domain.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);
    }
}

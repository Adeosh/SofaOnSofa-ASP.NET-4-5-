using SofaOnSofa.Domain.Entities;

namespace SofaOnSofa.Domain.Interfaces
{
    public interface IOrderProcessor
    {
        void ProcessOrder(ShoppingCart shoppingCart, ShippingDetails shippingDetails);
    }
}

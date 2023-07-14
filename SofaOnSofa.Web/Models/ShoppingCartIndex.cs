using SofaOnSofa.Domain.Entities;

namespace SofaOnSofa.Web.Models
{
    public class ShoppingCartIndex
    {
        public ShoppingCart ShoppingCart { get; set; }
        public string ReturnUrl { get; set; }
    }
}
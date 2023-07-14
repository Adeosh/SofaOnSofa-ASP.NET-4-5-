using SofaOnSofa.Domain.Entities;
using System.Collections.Generic;

namespace SofaOnSofa.Web.Models
{
    public class ProductList
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentStyle { get; set; }
    }
}
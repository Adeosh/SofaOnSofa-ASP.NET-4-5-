using System.Collections.Generic;
using System.Linq;

namespace SofaOnSofa.Domain.Entities
{
    public class ShoppingCart
    {
        private List<SelectedProduct> selectedProduct = new List<SelectedProduct>();

        public void AddItem(Product product, int quantity)
        {
            SelectedProduct select = selectedProduct
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (select == null)
            {
                selectedProduct.Add(new SelectedProduct
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                select.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            selectedProduct.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue()
        {
            return selectedProduct.Sum(e => e.Product.Price * e.Quantity);
        }

        public void Clear()
        {
            selectedProduct.Clear();
        }

        public IEnumerable<SelectedProduct> Lines
        {
            get { return selectedProduct; }
        }
    }
}

using SPApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPalApp.Services
{
    //Simple in-memory cart kept scoped for the user session.
    public class CartService
    {
        private readonly List<CartItemDto> _Items = new();
        public IReadOnlyList<CartItemDto> GetItems() => _Items.AsReadOnly();
        public void AddProduct(ProductDto product, int quantity = 1)
        {
            if (product == null || quantity <= 0) return;

            var existing = _Items.FirstOrDefault(i => i.ProductId == product.ProductId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _Items.Add(new CartItemDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity
                });
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var e = _Items.FirstOrDefault(i => i.ProductId == productId);
            if (e == null) return;

            if(quantity <= 0) _Items.Remove(e);
            else e.Quantity = quantity;
        }

        public void Remove(int productId)
        {
            var e = _Items.FirstOrDefault(i => i.ProductId == productId);
            if (e != null) _Items.Remove(e);
        }

        public decimal GetTotal() => _Items.Sum(i => i.Price * i.Quantity);

        public void Clear() => _Items.Clear();
    }
}

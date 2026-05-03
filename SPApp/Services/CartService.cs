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
        private readonly object _sync = new();
        public event Action? CartChanged;

        public IReadOnlyList<CartItemDto> GetItems()
        {
            lock (_sync)
            {
                return _Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList().AsReadOnly();
            }
        }

        public int GetCount()
        {
            lock (_sync) { return _Items.Count; }
        }

        public bool Contains(int productId)
        {
            lock (_sync) { return _Items.Any(i => i.ProductId == productId); }
        }

        public void AddProduct(ProductDto product, int quantity = 1)
        {
            if (product == null || quantity <= 0) return;

            lock (_sync)
            {
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
            CartChanged?.Invoke();
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            lock (_sync)
            {
                var e = _Items.FirstOrDefault(i => i.ProductId == productId);
                if (e == null) return;

                if (quantity <= 0) _Items.Remove(e);
                else e.Quantity = quantity;
            }
            CartChanged?.Invoke();
        }

        public void Remove(int productId)
        {
            lock (_sync)
            {
                var e = _Items.FirstOrDefault(i => i.ProductId == productId);
                if (e != null) _Items.Remove(e);
            }
            CartChanged?.Invoke();
        }

        public decimal GetTotal()
        {
            lock (_sync)
            {
                return _Items.Sum(i => i.Price * i.Quantity);
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                _Items.Clear();
            }
            CartChanged?.Invoke();
        }
    }
}

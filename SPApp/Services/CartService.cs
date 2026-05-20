using SPApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPalApp.Services
{
    public class CartService
    {
        // liste over varer i kurven
        private readonly List<CartItemDto> _Items = new();
        // synkroniseringsobjekt for tråd-sikker tilgang
        private readonly object _sync = new();
        // Event som udløses, når kurven ændres (tilføj, opdater, fjern)
        public event Action? CartChanged;

        // Hent en læsbar liste over varer i kurven
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

        // Hent det samlede antal varer i kurven
        public int GetCount()
        {
            lock (_sync) { return _Items.Count; }
        }

        // Tjek om en vare med det givne produkt-id findes i kurven
        public bool Contains(int productId)
        {
            lock (_sync) { return _Items.Any(i => i.ProductId == productId); }
        }

        // Tilføj en vare til kurven, eller opdater mængden hvis den allerede findes
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

        // Opdater mængden af en vare i kurven, eller fjern den hvis mængden er 0 eller mindre
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

        // Fjern en vare fra kurven baseret på produkt-id
        public void Remove(int productId)
        {
            lock (_sync)
            {
                var e = _Items.FirstOrDefault(i => i.ProductId == productId);
                if (e != null) _Items.Remove(e);
            }
            CartChanged?.Invoke();
        }

        // Beregn det samlede beløb for alle varer i kurven
        public decimal GetTotal()
        {
            lock (_sync)
            {
                return _Items.Sum(i => i.Price * i.Quantity);
            }
        }

        // Ryd alle varer fra kurven
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

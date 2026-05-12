using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPApp.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderProductDetailDto> OrderProducts { get; set; } = new();
    }

    public class OrderProductDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }
}



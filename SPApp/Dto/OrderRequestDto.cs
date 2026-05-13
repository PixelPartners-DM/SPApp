using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPApp.Dto
{
    public class OrderRequestDto
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public int? DeliveryId { get; set; }
        public List<OrderProductDto> Products { get; set; } = new();
    }
}

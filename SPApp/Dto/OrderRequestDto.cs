using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPApp.Dto
{
    // Matches server Orders + OrderProduct shape in a way the API can accept
    public class OrderRequestDto
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public int DeliveryId { get; set; }
        public int UserId { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; } = new();
    }
}

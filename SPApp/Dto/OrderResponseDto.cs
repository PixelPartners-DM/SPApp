using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPApp.Dto
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = "";
        public string? Message { get; set; }
    }
}

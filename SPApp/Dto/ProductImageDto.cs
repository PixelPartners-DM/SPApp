using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPApp.Dto
{
    public class ProductImageDto
    {
        public int ProductImageId { get; set; }
        public string ImageUrl { get; set; } = "";
        public int ProductId { get; set; }
    }
}

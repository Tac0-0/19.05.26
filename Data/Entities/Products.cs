using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Products
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Categories Category { get; set; }
        public ProductSize ProductSize { get; set; }
        public MeatType MeatType { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}

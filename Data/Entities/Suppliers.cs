using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doner.Data.Entities
{
    public class Suppliers
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}

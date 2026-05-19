using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public int AddressId { get; set; }
        public UserAddresses Address { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}

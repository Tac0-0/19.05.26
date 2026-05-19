using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Deliveries
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public Orders Order { get; set; }
        public int DeliveryWorkerId { get; set; }
        public Users DeliveryWorker { get; set; }
        public int AddressId { get; set; }
        public UserAddresses Address { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public decimal DeliveryFee { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
    }
}

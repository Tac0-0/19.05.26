using Doner.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doner.Data.Entities
{
    public class Payments
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public Orders Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}

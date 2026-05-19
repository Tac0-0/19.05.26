using Doner.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doner.Data.Entities
{
    public class UserAddresses
    {
        public int UserAddressId { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public AddressType AddressType { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string? Notes { get; set; }
        public bool IsDefault { get; set; }
    }
}

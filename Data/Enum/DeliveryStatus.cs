using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doner.Data.Enum
{
    public enum DeliveryStatus
    {
        WaitingForCourier,
        Assigned,
        PickedUp,
        Delivered,
        Failed,
        Cancelled
    }
}

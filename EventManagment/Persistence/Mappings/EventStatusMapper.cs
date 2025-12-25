using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Mappings
{
    public static class EventStatusMapper
    {
        public static CapacityAvailability MapCapacityAvailability(this int remaining)
        {
            if (remaining > 5) return CapacityAvailability.AVAILABLE;
            if (remaining >= 1) return CapacityAvailability.LIMITED;
            return CapacityAvailability.FULL;
        }

        public static MyStatus MapMyStatus(this string? statusName)
        {
            return statusName switch
            {
                "Confirmed" => MyStatus.CONFIRMED,
                "Waitlisted" => MyStatus.WAITLISTED,
                "Cancelled" => MyStatus.CANCELLED,
                null => MyStatus.NOT_REGISTERED,
                _ => MyStatus.NOT_REGISTERED
            };
        }

        public static MyStatus? MapMyStatusNullable(this string? statusName)
        {
            return statusName switch
            {
                "Confirmed" => MyStatus.CONFIRMED,
                "Waitlisted" => MyStatus.WAITLISTED,
                "Cancelled" => MyStatus.CANCELLED,
                _ => null
            };
        }
    }

}

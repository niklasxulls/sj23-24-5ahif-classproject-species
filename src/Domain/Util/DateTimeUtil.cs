using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Util;
public static class DateTimeUtil
{
    public static DateTime Now()
    {
        return DateTime.UtcNow;
    }

    public static bool IsValid(this DateTime date)
    {
        return date > SqlDateTime.MinValue.Value && date < SqlDateTime.MaxValue.Value;
    }

    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(new TimeOnly(0, 0, 0));
    }
}

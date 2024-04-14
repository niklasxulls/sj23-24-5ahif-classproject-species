using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Util;
public static class GuidUtil
{
    public static Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}

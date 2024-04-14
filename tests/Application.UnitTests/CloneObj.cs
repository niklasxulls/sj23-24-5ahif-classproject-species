using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests;

public static class CloneObj
{
    public static object Clone<T>(T from) {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var clone = Activator.CreateInstance(type);

        foreach (var field in fields)
        {
            field.SetValue(clone, field.GetValue(from));
        }

        return clone!;
    }
}

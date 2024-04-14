using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Util;
public static class StringUtil
{
    public static string ReplaceSpecialCharacters(string str)
    {
         return Regex.Replace(str, "[^0-9a-zA-Z]+", "-");
    }
}

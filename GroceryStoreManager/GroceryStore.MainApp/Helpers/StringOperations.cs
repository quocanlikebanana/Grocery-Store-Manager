using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Helpers;

public static class StringOperations
{
    public static string UnsignVie(this string s)
    {
        var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
        var temp = s.Normalize(NormalizationForm.FormD);
        return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }

    public static string TextNormalize(this string str)
    {
        // Trim and remove duplicate spaces, characters like: (), -
        var res = str.ToLower().Trim().UnsignVie();
        var regex = new Regex(@"[\s]{2,}", RegexOptions.None);
        res = regex.Replace(res, " ");
        var regexChar = new Regex(@"[-()]", RegexOptions.None);
        res = regexChar.Replace(res, "");
        return res;
    }
}

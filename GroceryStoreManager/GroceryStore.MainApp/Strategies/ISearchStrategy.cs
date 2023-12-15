using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.MainApp.Helpers;

namespace GroceryStore.MainApp.Strategies;

public interface ISearchStrategy<T>
{
    List<T> Search(string query);
}

public class SearchByUniqueString<T> : ISearchStrategy<T>
{
    private readonly List<T> _source;
    private readonly Func<T, string> _toUniqueString;

    public SearchByUniqueString(List<T> source, Func<T, string> toUniqueString)
    {
        _source = source;
        _toUniqueString = toUniqueString;
    }

    public List<T> Search(string query)
    {
        List<T> result = new();
        query = query.TextNormalize();
        foreach (var cus in _source)
        {
            var compareStr = _toUniqueString(cus);
            if (!compareStr.Contains(query))
            {
                continue;
            }
            result.Add(cus);
        }
        return result;
    }


}
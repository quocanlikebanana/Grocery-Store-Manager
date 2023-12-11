using System.Collections.ObjectModel;

namespace GroceryStore.MainApp.Models.Extensions;
public static class ListRecreateExtension
{
    public static void Refresh<T>(this Collection<T> source, IEnumerable<T> newList)
    {
        source.Clear();
        foreach (var customer in newList)
        {
            source.Add(customer);
        }
    }
}

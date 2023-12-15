using Microsoft.UI.Xaml.Data;

namespace GroceryStore.MainApp.Converters;

public class DummyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}

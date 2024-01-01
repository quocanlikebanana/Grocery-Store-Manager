using System.Globalization;
using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Decorators;
using GroceryStore.MainApp.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace GroceryStore.MainApp.Converters;


public class ASBCustomerDisplayStringFormat : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            return (string)value;
        }
        if (value is not Customer customer)
        {
            return string.Empty;
        }
        return CustomerDecorator.ToDisplayString(customer);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}

public class ASBProductDisplayStringFormat : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            return (string)value;
        }
        if (value is not Product product)
        {
            return string.Empty;
        }
        return ProductDecorator.ToDisplayString(product);
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}

public class ASBProductTypeDisplayStringFormat : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            return (string)value;
        }
        if (value is not ProductType product)
        {
            return string.Empty;
        }
        return ProductTypeDecorator.ToDisplayString(product);
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}

public class DoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not null && value.ToString() is string sval)
        {
            return sval;
        }
        return value!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is not null && double.TryParse(value.ToString(), out double dval))
        {
            return dval;
        }
        return null!;
    }
}

public class CurrencyVNDConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) { return string.Empty; }
        var dval = (double)value;
        var ival = (int)dval;
        var cul = new CultureInfo("vi-VN")
        {
            NumberFormat = new NumberFormatInfo()
            {
                CurrencySymbol = "đ",
                CurrencyDecimalDigits = 0,
                CurrencyGroupSeparator = ",",
                CurrencyPositivePattern = 3,
            },
        };
        var vnd = ival.ToString("c", cul.NumberFormat);
        return vnd;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var sval = (string)value;
        var cul = new CultureInfo("vi-VN")
        {
            NumberFormat = new NumberFormatInfo()
            {
                CurrencySymbol = "đ",
                CurrencyDecimalDigits = 0,
                CurrencyGroupSeparator = ",",
                CurrencyPositivePattern = 3,
            },
        };
        if (int.TryParse(sval, cul.NumberFormat, out int ival))
        {
            return ival;
        }
        return int.MinValue;
    }
}

public class BoolToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolVal)
        {
            if (boolVal)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class RandomIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var ival = value.GetHashCode();
        ival = ival < 0 ? (-1) * ival : ival;
        var mapval = ival % 6;
        var result = "\uEE57";
        switch (mapval)
        {
            case 0:
                result = "\uEA8C";
                break;
            case 1:
                result = "\uED53";
                break;
            case 2:
                result = "\uF1AD";
                break;
            case 3:
                result = "\uEFA9";
                break;
            case 4:
                result = "\uED54";
                break;
            case 5:
                result = "\uE77B";
                break;
        }
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}


﻿using System.Globalization;
using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Decorators;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

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

public class CurrencyVNDConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
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
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
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
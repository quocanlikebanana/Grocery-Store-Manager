using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Core.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp.Views;

public sealed partial class CustomerDetailControl : UserControl
{
    public Customer? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as Customer;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(Customer), typeof(CustomerDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public CustomerDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CustomerDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}

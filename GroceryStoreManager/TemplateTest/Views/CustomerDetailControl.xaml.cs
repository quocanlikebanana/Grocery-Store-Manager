using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using TemplateTest.Core.Models;

namespace TemplateTest.Views;

public sealed partial class CustomerDetailControl : UserControl
{
    public SampleOrder? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as SampleOrder;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(CustomerDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

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

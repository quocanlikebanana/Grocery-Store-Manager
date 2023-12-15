using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;

namespace GroceryStore.MainApp.ControlHelper;

//My saviour: https://blog.magnusmontin.net/2022/01/20/bind-to-a-parent-element-in-winui-3/
//Attached Props: https://learn.microsoft.com/en-us/windows/uwp/xaml-platform/custom-attached-properties?WT.mc_id=WD-MVP-5001077
// 2 more workaround:
// https://stackoverflow.com/questions/70691884/winui-3-how-to-bind-command-to-viewmodel-property-when-using-a-datatemplate
// https://stackoverflow.com/questions/74067709/winui-3-binding-from-a-datatemplate-to-the-parent-viewmodel (create new UserControl)

public static class AncestorSource
{
    public static readonly DependencyProperty AncestorTypeProperty =
        DependencyProperty.RegisterAttached(
            "AncestorType",
            typeof(Type),
            typeof(AncestorSource),
            new PropertyMetadata(default(Type), OnAncestorTypeChanged)
    );

    public static void SetAncestorType(FrameworkElement element, Type value) =>
        element.SetValue(AncestorTypeProperty, value);

    public static Type GetAncestorType(FrameworkElement element) =>
        (Type)element.GetValue(AncestorTypeProperty);

    private static void OnAncestorTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        FrameworkElement target = (FrameworkElement)d;
        if (target.IsLoaded)
            SetDataContext(target);
        else
            target.Loaded += OnTargetLoaded;
    }

    private static void OnTargetLoaded(object sender, RoutedEventArgs e)
    {
        FrameworkElement target = (FrameworkElement)sender;
        target.Loaded -= OnTargetLoaded;
        SetDataContext(target);
    }

    private static void SetDataContext(FrameworkElement target)
    {
        Type ancestorType = GetAncestorType(target);
        if (ancestorType != null)
            target.DataContext = FindParent(target, ancestorType);
    }

    private static object? FindParent(DependencyObject dependencyObject, Type ancestorType)
    {
        DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
        if (parent == null)
            return null;

        if (ancestorType.IsAssignableFrom(parent.GetType()))
            return parent;

        return FindParent(parent, ancestorType);
    }
}
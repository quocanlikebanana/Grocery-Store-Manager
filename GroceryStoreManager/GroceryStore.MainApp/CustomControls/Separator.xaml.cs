using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GroceryStore.MainApp.CustomControls;

public sealed partial class Separator : UserControl
{
    public Separator()
    {
        this.InitializeComponent();
        this.main.Fill = Color;
        if (IsHorizontal)
        {
            this.main.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.main.VerticalAlignment = VerticalAlignment.Center;
            this.main.Height = 1;
        }
        else
        {
            this.main.HorizontalAlignment = HorizontalAlignment.Center;
            this.main.VerticalAlignment = VerticalAlignment.Stretch;
            this.main.Width = 1;
        }
    }

    public bool IsHorizontal
    {
        get { return (bool)GetValue(IsHorizontalProperty); }
        set { SetValue(IsHorizontalProperty, value); }
    }

    public static readonly DependencyProperty IsHorizontalProperty =
        DependencyProperty.Register(nameof(IsHorizontal), typeof(bool), typeof(Separator), new PropertyMetadata(true));


    public Brush Color
    {
        get { return (Brush)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Separator.Color), typeof(Brush), typeof(Separator), new PropertyMetadata(null));
}

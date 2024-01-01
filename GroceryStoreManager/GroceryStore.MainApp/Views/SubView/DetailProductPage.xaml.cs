using GroceryStore.MainApp.ViewModels.SubVM;
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

namespace GroceryStore.MainApp.Views.SubView;


public sealed partial class DetailProductPage : Page
{
    public DetailProductVM? ViewModel { get; private set; }

    public DetailProductPage()
    {
        this.InitializeComponent();
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel = (DetailProductVM)e.Parameter;
        ViewModel.GoBackHandle = () =>
        {
            Frame.GoBack();
        };
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel = null;
        base.OnNavigatedFrom(e);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GroceryStore.MainApp.CustomControls;
public sealed partial class ContentDialogBase : ContentDialog
{
    private bool _shoudClose = false;
    public ContentDialogBase()
    {
        this.InitializeComponent();
        this.Closing += ContentDialogBase_Closing;
    }

    public void Close()
    {
        _shoudClose = true;
        Hide();
    }

    // Prevent un-wanted Closing
    private void ContentDialogBase_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
    {
        if (!_shoudClose)
        {
            args.Cancel = true;
        }
        else
        {
            _shoudClose = false;
            // Closes
        }
    }
}

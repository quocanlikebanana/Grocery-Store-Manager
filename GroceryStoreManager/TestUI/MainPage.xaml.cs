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

namespace TestUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DiaSer cds;
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += (s, e) =>
            {
                var tb = new Button();
                tb.Click += (s, e) =>
                {
                    cds.Close();
                };
                cds = new DiaSer(this.XamlRoot, tb);
            };
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            cds.Show();
        }

        private void btn_Click_1(object sender, RoutedEventArgs e)
        {
            cds.Close();
        }

    }
}

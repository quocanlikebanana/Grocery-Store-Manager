using GroceryStore.MainApp.ViewModels.PopupVM;
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

namespace GroceryStore.MainApp.Views.PopupView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPopup : Page
    {
        public LoginPopupVM ViewModel { get; }

        public LoginPopup(LoginPopupVM popupVM)
        {
            ViewModel = popupVM;
            InitializeComponent();  // Put this on top so the passwordBox is initialized (not null)
            passwordBox.Password = ViewModel.LoadPassword.Invoke();
            ViewModel.RetrivePassword = () =>
            {
                return passwordBox.Password;
            };
        }
    }
}

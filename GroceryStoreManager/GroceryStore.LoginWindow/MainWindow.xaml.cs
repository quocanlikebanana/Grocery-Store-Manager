using GroceryStoreManager.DatabaseConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroceryStore.LoginWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        DatabaseConnectionManager dbInstance;

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.rememberChbx.IsChecked == true)
            {
                dbInstance.saveInformation(this.serverTxbx.Text.ToString(), this.databaseTxbx.Text, this.usernameTxbx.Text, this.passwordPwbx.Password);
            }
            this.loggingIn.Height = 30;
            this.loggingIn.IsIndeterminate = true;
            bool isSuccessfull = await dbInstance.connectToDB(this.serverTxbx.Text, this.databaseTxbx.Text, this.usernameTxbx.Text, this.passwordPwbx.Password);
            this.loggingIn.Height = 0;
            this.loggingIn.IsIndeterminate = false;

            if (isSuccessfull == true)
            {
                MessageBox.Show("Đăng nhập thành công!");
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!");
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbInstance = DatabaseConnectionManager.Intance;

            var savedInfor = dbInstance.getSavedInformation();
            this.usernameTxbx.Text = savedInfor["Username"];
            this.passwordPwbx.Password = savedInfor["Password"];
            this.databaseTxbx.Text = savedInfor["Database"];
            this.serverTxbx.Text = savedInfor["Server"];
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void minimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void unmaskPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.passwordPwbx.Width = 0;
            this.passwordUnmaskTxbl.Text = this.passwordPwbx.Password;
            this.passwordUnmaskTxbl.Width = 255;
            e.Handled = true;
            this.unmaskPassword.Text = "☠";
        }

        private void unmaskPassword_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.passwordPwbx.Width = 255;
            this.passwordUnmaskTxbl.Width = 0;
            this.unmaskPassword.Text = "👁";

        }
    }
}

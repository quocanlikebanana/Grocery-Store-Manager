using Microsoft.Win32;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using GroceryStore.Data.Backup;

namespace Test_BackupRestore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestoreAndBackupService _dbBackupRestore;

        public MainWindow()
        {
            InitializeComponent();

            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLEXPRESS";
            builder.InitialCatalog = "newestDataBase";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            string connectionString = builder.ConnectionString;

            _dbBackupRestore = new RestoreAndBackupService(connectionString);
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string backupFilePath = txtBackupPath.Text;
                _dbBackupRestore.BackupDatabase(backupFilePath);
                System.Windows.MessageBox.Show("Backup completed!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string backupFilePath = txtRestorePath.Text;
                _dbBackupRestore.RestoreDatabase(backupFilePath);
                System.Windows.MessageBox.Show("Restore completed!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnSelectBackupPath_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txtBackupPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnSelectRestorePath_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "Backup Files (*.bak)|*.bak";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtRestorePath.Text = dialog.FileName;
                }
            }
        }
    }
}
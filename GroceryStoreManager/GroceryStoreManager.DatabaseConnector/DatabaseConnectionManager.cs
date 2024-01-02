using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Protocols;

namespace GroceryStoreManager.DatabaseConnector
{
    public class DatabaseConnectionManager
    {
        private static DatabaseConnectionManager? instance = null;
        public static string? ConnectionString { get; set; }
        public static DatabaseConnectionManager Intance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseConnectionManager();
                }
                return instance;
            }
        }
        public async Task<bool> connectToDB(string dataSource, string initialCatalog, string userName, string passWord)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = dataSource;
            builder.InitialCatalog = initialCatalog;
            builder.UserID = userName;
            builder.Password = passWord;
            builder.TrustServerCertificate = true;

            string connectionString = builder.ConnectionString;
            var connection = new SqlConnection(connectionString);

            connection = await Task.Run(() =>
            {
                var _connection = new SqlConnection(connectionString);

                try
                {
                    _connection.Open();
                }
                catch (Exception ex)
                {

                    _connection = null;
                }

                return _connection;
            });

            if (connection != null)
            {
                ConnectionString = connectionString;
                return true;
            }
            return false;
        }

        public void saveInformation(string dataSource, string initialCatalog, string userName, string passWord)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(passWord);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var cypherText = ProtectedData.Protect(passwordInBytes, entropy,
                DataProtectionScope.CurrentUser);
            var passwordIn64 = Convert.ToBase64String(cypherText);
            var entropyIn64 = Convert.ToBase64String(entropy);

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Server"].Value = dataSource;
            config.AppSettings.Settings["Database"].Value = initialCatalog;
            config.AppSettings.Settings["Username"].Value = userName;
            config.AppSettings.Settings["Password"].Value = passwordIn64;
            config.AppSettings.Settings["Entropy"].Value = entropyIn64;
            config.Save(ConfigurationSaveMode.Minimal);

            ConfigurationManager.RefreshSection("appSettings");
        }
        public Dictionary<string, string> getSavedInformation()
        {
            var passwordIn64 = ConfigurationManager.AppSettings["Password"];
            var password = "";
            if (passwordIn64!.Length != 0)
            {
                var entropyIn64 = ConfigurationManager.AppSettings["Entropy"];

                var cyperTextInBytes = Convert.FromBase64String(passwordIn64);
                var entropyInBytes = Convert.FromBase64String(entropyIn64!);

                var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes,
                    DataProtectionScope.CurrentUser);
                password = Encoding.UTF8.GetString(passwordInBytes);
            }
            var dicInfor = new Dictionary<string, string>();
            dicInfor.Add("Server", ConfigurationManager.AppSettings["Server"]);
            dicInfor.Add("Database", ConfigurationManager.AppSettings["Database"]);
            dicInfor.Add("Username", ConfigurationManager.AppSettings["Username"]);
            dicInfor.Add("Password", password);

            return dicInfor;
        }
    }
}

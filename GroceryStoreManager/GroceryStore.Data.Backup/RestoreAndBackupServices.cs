using System;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace GroceryStore.Data.Backup
{
    public class RestoreAndBackupService
    {
        private SqlConnection _sqlConnection;

        public RestoreAndBackupService(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        public void BackupDatabase(string backupFilePath)
        {
            string database = _sqlConnection.Database.ToString();
            string cmd = "BACKUP DATABASE [" + database + "] TO DISK= '" + backupFilePath + "\\" + "database" + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak'";
            _sqlConnection.Open();
            SqlCommand command = new SqlCommand(cmd, _sqlConnection);
            command.ExecuteNonQuery();
            _sqlConnection.Close();
        }

        public void RestoreDatabase(string backupFilePath)
        {
            string database = _sqlConnection.Database.ToString();
            _sqlConnection.Open();

            try
            {
                string str1 = string.Format("ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                SqlCommand cmd1 = new SqlCommand(str1, _sqlConnection);
                cmd1.ExecuteNonQuery();

                string str2 = "USE MASTER RESTORE DATABASE [" + database + "] FROM DISK= '" + backupFilePath + "' WITH REPLACE;";
                SqlCommand cmd2 = new SqlCommand(str2, _sqlConnection);
                cmd2.ExecuteNonQuery();

                string str3 = string.Format("ALTER DATABASE [" + database + "] SET MULTI_USER");
                SqlCommand cmd3 = new SqlCommand(str3, _sqlConnection);
                cmd3.ExecuteNonQuery();

                _sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

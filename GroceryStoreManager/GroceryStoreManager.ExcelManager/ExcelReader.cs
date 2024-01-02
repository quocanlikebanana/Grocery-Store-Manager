using ExcelDataReader;
using GroceryStoreManager.DatabaseConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreManager.ExcelManager
{
    public class ExcelReader
    {
        public string FilePath;
        public ExcelReader(string filePath)
        {
            FilePath = filePath;
        }

        public void readFile()
        {
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    foreach (DataTable table in result.Tables)
                    {
                        SqlBulkCopy bulkcopy = new SqlBulkCopy(DatabaseConnectionManager.ConnectionString);

                        bulkcopy.DestinationTableName = table.TableName;

                        //can make error here, the following statement should be placed in a try catch statement.
                        bulkcopy.WriteToServer(table);

                    }
                }
            }
        }
    }
}

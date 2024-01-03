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
        public string ConnectionString;
        private int ProductTypeIdDistance;
        private int ProductTypeOldId;
        public ExcelReader(string filePath, string connectionString)
        {
            FilePath = filePath;
            ConnectionString = connectionString;
        }

        public async Task run()
        {
            var tables = readTables();
            var productTypeTable = tables["ProductType"];
            await writeToSql(productTypeTable!, "ProductType");

            var productTable = tables["Product"];
            await writeToSql(productTable!, "Product");
        }

        List<string> getHeader(DataTable table)
        {
            List<string> header = new List<string>();
            var headerRow = table.Rows[0];
            for (var i = 0; i < table.Columns.Count; i++)
            {
                header.Add(headerRow[i]!.ToString());
            }
            return header;
        }
        async Task writeToSql(DataTable table, string tableName)
        {
            using (SqlCommand command = new SqlCommand())
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                await connection.OpenAsync();
                command.Connection = connection;

                var headers = getHeader(table!);

                for (var i = 1; i < table!.Rows.Count; i++)
                {
                    string sqlQuery = $"INSERT INTO {tableName} (";

                    for (var k = 0; k < headers.Count; k++)
                    {
                        if (headers[k] != "Id")
                        {
                            sqlQuery += headers[k];
                            if (k == headers.Count - 1)
                            {
                                sqlQuery += ") VALUES(";
                            }
                            else
                            {
                                sqlQuery += ",";
                            }
                        }
                    }
                    for (var j = 0; j < table.Columns.Count; j++)
                        if (table.Rows[i][j] != null && headers[j] != "Id")
                        {
                            sqlQuery += $"@Param{j}";
                            string paramName = $"@Param{j}";
                            if (tableName == "Product" && headers[j] == "TypeId" && Convert.ToInt32(Convert.ToDouble(table.Rows[i][j])) >= ProductTypeOldId)
                            {
                                int newId = ProductTypeIdDistance + Convert.ToInt32(Convert.ToDouble(table.Rows[i][j]));
                                command.Parameters.AddWithValue(paramName, newId);
                            }
                            else
                            {
                                command.Parameters.AddWithValue(paramName, table.Rows[i][j]);
                            }
                            if (j == table.Columns.Count - 1)
                            {
                                sqlQuery += ");";
                            }
                            else
                            {
                                sqlQuery += ",";
                            }
                        }
                    if (i == 1 && tableName == "ProductType")
                    {
                        sqlQuery += "select ident_current('ProductType');";
                        command.CommandText = sqlQuery;

                        int currentId = Convert.ToInt32(Convert.ToDouble(command.ExecuteScalar()));
                        int oldId = Convert.ToInt32(Convert.ToDouble(table.Rows[i][headers.IndexOf("Id")]));
                        ProductTypeOldId = oldId;
                        ProductTypeIdDistance = currentId - oldId;
                    }
                    else
                    {
                        command.CommandText = sqlQuery;
                        await command.ExecuteNonQueryAsync();
                    }
                    command.Parameters.Clear();
                }
                await connection.CloseAsync();
            }

        }
        DataTableCollection readTables()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var tables = result.Tables;
                    return tables;
                }
            }
        }
    }
}

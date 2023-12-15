using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;

namespace Temporary_Appendix;

public class GenDatabase
{
    private static IEnumerable<string> SplitSqlStatements(string sqlScript)
    {
        // Make line endings standard to match RegexOptions.Multiline
        sqlScript = Regex.Replace(sqlScript, @"(\r\n|\n\r|\n|\r)", "\n");

        // Split by "GO" statements
        var statements = Regex.Split(
                sqlScript,
                @"^[\t ]*(GO|go)[\t ]*\d*[\t ]*(?:--.*)?$",
                RegexOptions.Multiline |
                RegexOptions.IgnorePatternWhitespace |
                RegexOptions.IgnoreCase);

        // Remove empties, trim, and return
        return statements
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim(' ', '\n'));
    }

    // Exit Database before runing the script
    // Exit Database before runing the script
    // Exit Database before runing the script

    public static void run(string connectionString, string fileName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            var script = System.IO.File.ReadAllText(fileName);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            Server server = new Server(new ServerConnection(sqlConnection));
            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }

    // For script that has capitalize GO only (old version)
    public static void run_CapGO(string connectionString, string fileName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            var data = System.IO.File.ReadAllText(fileName);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                var scripts = SplitSqlStatements(data);
                foreach (var splitScript in scripts)
                {
                    command.CommandText = splitScript;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

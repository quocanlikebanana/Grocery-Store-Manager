using Microsoft.Data.SqlClient;
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
                @"^[\t ]*GO[\t ]*\d*[\t ]*(?:--.*)?$",
                RegexOptions.Multiline |
                RegexOptions.IgnorePatternWhitespace |
                RegexOptions.IgnoreCase);

        // Remove empties, trim, and return
        return statements
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim(' ', '\n'));
    }

    // Exit Database before runing the script
    public static void run(string connectionString, string fileName)
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

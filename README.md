# Grocery-Store-Manager
Usage of ExcelReader

{{async function}}\n
{
    ExcelReader excelReader = new ExcelReader({{absolute .xlsx file path}}, {{connection string}});  //"F:\\OneDrive - VNU-HCMUS\\test.xlsx, "DatabaseConnectionManager.ConnectionString!
    await excelReader.run();
}

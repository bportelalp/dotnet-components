// See https://aka.ms/new-console-template for more information
using Infrastructure.Spreadsheets.Tables;

Console.WriteLine("Hello, World!");

var myList = new List<MyData>()
{
    new MyData(1, "Hola"),
    new MyData(2, "Mundo")
};


var excel = new TableSpreadsheet<MyData>();
excel.AddItems(myList);
excel.AddColumn(c => c.Integer).WithTitle("Entero");
excel.AddColumn(c => c.Text);
excel.AddColumn(c=> c);

excel.AddRow(new MyData(5, "que tal"));


var path = @"C:\Users\bportela\Datos\Dev\dotnet-components\Test\SpreadsheetsTestbench\hoja.csv";
excel.CreateSpreadsheet(path, Infrastructure.Spreadsheets.Common.ESpreadsheetType.CsvSemicolonSeparated);
public class MyData
{
    public int Integer { get; set; }
    public string Text { get; set; }
    public MyData(int integer, string text)
    {
        Integer = integer;
        Text = text;
    }
}




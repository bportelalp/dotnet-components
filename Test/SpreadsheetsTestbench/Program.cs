// See https://aka.ms/new-console-template for more information
using Infrastructure.Spreadsheets.Tables;

Console.WriteLine("Hello, World!");

var myList = new List<MyData>()
{
    new MyData(1, "Hola"),
    new MyData(2, "Mundo")
};


var excel = new TableSpreadsheet<MyData>();
excel.AddItems(myList).SetSheetName("Mi tabla");
excel.AddColumn(c => c.Integer);
excel.AddColumn(c => c.Text).WithTitle("Texto");
excel.AddColumn(c => c);

excel.AddRow(new MyData(5, "que tal"));

var enviroment = System.Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;

var path = Path.Combine(projectDirectory, $"Resultados\\{DateTime.Now.ToString("yyMMdd_HHmm")}_hoja.xlsx");
excel.CreateSpreadsheet(path, Infrastructure.Spreadsheets.Common.ESpreadsheetType.Excel);


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




using Infrastructure.Spreadsheets.Tables;
using SpreadsheetsTestbench;

Console.WriteLine("Hello, World!");

var myList = new List<MyTestClass>()
{
    new MyTestClass(1, "Hola", DateTime.Today),
    new MyTestClass(2, "Mundo", DateTime.Now)
};

var tableExcel = new TableSpreadsheet<MyTestClass>();
tableExcel.AddItems(myList).SetSheetName($"Tabla de objetos {nameof(MyTestClass)}");
tableExcel.AddColumn(c => c.Integer).SetTitle("Número entero");
tableExcel.AddColumn(c => c.Text).SetTitle("Texto");
tableExcel.AddColumn(c => c.Date, "Fecha");
tableExcel.AddColumn(c => c);

tableExcel.AddRow(new MyTestClass(5, "que tal", DateTime.MinValue));

string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
var path = Path.Combine(projectDirectory, $"Resultados\\{DateTime.Now.ToString("yyMMdd_HHmm")}_hoja.xlsx");
tableExcel.CreateSpreadsheet(path, Infrastructure.Spreadsheets.Common.ESpreadsheetType.Excel);




using BP.Components.Spreadsheets.Tables;
using BP.Test.SpreadSheets.Console;

Console.WriteLine("Hello, World!");

var myList = new List<MyTestClass>()
{
    new MyTestClass(1, "Hola", DateTime.Today),
    new MyTestClass(2, "Mundo", DateTime.Now)
};

var tableExcel = new TableExcel<MyTestClass>();
tableExcel.AddItems(myList);
tableExcel.SetSheetName($"Tabla de objetos {nameof(MyTestClass)}")
          .SetAutoWidth(true);
tableExcel.AddColumn(c => c.Integer).SetTitle("Número entero");
tableExcel.AddColumn(c => c.Text).SetTitle("Texto");
tableExcel.AddColumn(c => c.Date, "Fecha");
tableExcel.AddColumn(c => c);

tableExcel.Add(new MyTestClass(5, "que tal", DateTime.MinValue));

var tableCsv = tableExcel.ToCsv();


string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
var path = Path.Combine(projectDirectory, $"Resultados\\{DateTime.Now.ToString("yyMMdd_HHmm")}_hoja.xlsx");
var path2 = Path.Combine(projectDirectory, $"Resultados\\{DateTime.Now.ToString("yyMMdd_HHmm")}_hoja.csv");

tableExcel.Create(path);
tableCsv.ConfigureSeparator(BP.Components.Spreadsheets.Common.ECsvSeparator.Semicolon).Create(path2);




using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Spreadsheets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Spreadsheets
{
    internal static class Tools
    {

        internal static Cell CreateCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }




        internal static WorksheetPart InsertWorksheet(WorkbookPart workbook, string sheetName, Columns columns)
        {
            WorksheetPart newWSP = workbook.AddNewPart<WorksheetPart>();
            newWSP.Worksheet = new Worksheet();

            newWSP.Worksheet.Append(columns);

            newWSP.Worksheet.AppendChild(new SheetData());
            newWSP.Worksheet.Save();

            Sheets sheets = workbook.Workbook.GetFirstChild<Sheets>();
            string relationID = workbook.GetIdOfPart(newWSP);

            uint sheetID = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetID = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            Sheet sheet = new() { Id = relationID, SheetId = sheetID, Name = sheetName };

            sheets.Append(sheet);
            workbook.Workbook.Save();
            return newWSP;
        }



        internal static string CsvSeparator(ESpreadsheetType spreadsheetType)
        {
            switch (spreadsheetType)
            {
                case ESpreadsheetType.CsvCommaSeparated: return ",";
                case ESpreadsheetType.CsvSemicolonSeparated: return ";";
                case ESpreadsheetType.Excel: return string.Empty;
                default: return string.Empty;
            }
        }
    }
}

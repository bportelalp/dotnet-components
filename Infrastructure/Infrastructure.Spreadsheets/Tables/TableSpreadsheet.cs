using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using System.Linq.Expressions;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Spreadsheets.Common;

namespace Infrastructure.Spreadsheets.Tables
{
    public class TableSpreadsheet<T>
    {

        public ICollection<T> RowItems { get; private set; }
        public string SheetName { get; set; } = string.Empty;
        public bool AutoWidth { get; private set; } = false;

        private List<TableColumn<T>> _columns = new List<TableColumn<T>>();
        private string _fileName = nameof(T);



        #region Fluent API Methods
        public TableSpreadsheet<T> AddIndexColumn(Expression<Func<T, object>> predicate)
        {
            return this;
        }
        public TableSpreadsheet<T> SetAutoWidth(bool enable)
        {
            AutoWidth = enable;
            return this;
        }

        public TableSpreadsheet<T> AddItems(IEnumerable<T> items)
        {
            this.RowItems = items.ToList();
            return this;
        }

        public TableColumn<T> AddColumn(Expression<Func<T, object>> predicate)
        {
            var column = new TableColumn<T> { ColData = predicate };
            this._columns.Add(column);
            return column;
        }
        public TableColumn<T> AddColumn(Expression<Func<T, object>> predicate, string headerName) => this.AddColumn(predicate).SetTitle(headerName);

        public TableSpreadsheet<T> AddRow(T row)
        {
            RowItems.Add(row);
            return this;
        }


        public TableSpreadsheet<T> SetSheetName(string name)
        {
            SheetName = name;
            return this;
        }
        #endregion

        #region Export Spreadsheet methods
        public void CreateSpreadsheet(string path, ESpreadsheetType spreadsheetType)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (MemoryStream ms = this.CreateSpreadsheet(spreadsheetType))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
            }
        }
        public MemoryStream CreateSpreadsheet(ESpreadsheetType spreadsheetType)
        {
            switch (spreadsheetType)
            {
                case ESpreadsheetType.CsvCommaSeparated:
                    return CreateCsv(spreadsheetType);
                case ESpreadsheetType.CsvSemicolonSeparated:
                    return CreateCsv(spreadsheetType);
                case ESpreadsheetType.Excel:
                    return CreateXlsx();
                default: return null;
            }
        }
        #endregion

        #region Private methods to create Spreadsheet document
        private MemoryStream CreateCsv(ESpreadsheetType spreadsheetType)
        {
            var lines = this.CreateLines(Tools.CsvSeparator(spreadsheetType));

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
            sw.Flush();
            sw.Dispose();
            return ms;

        }


        private MemoryStream CreateXlsx()
        {
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookpart = spreadSheet.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Add Sheets to the Workbook.
                Sheets sheets = spreadSheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = spreadSheet.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = string.IsNullOrWhiteSpace(this.SheetName) ? $"Lista de {typeof(T).Name}" : this.SheetName
                };
                sheets.Append(sheet);


                SheetData sheetData = new SheetData();
                //Header
                Row row = new Row();
                foreach (var column in this._columns)
                {
                    row.Append(Tools.CreateCell(column.Title, CellValues.String));
                }
                sheetData.AppendChild(row);

                //Rows
                foreach (var item in this.RowItems)
                {
                    row = new Row();
                    foreach (var column in this._columns)
                    {
                        object value = column.Evaluate(item);
                        row.Append(Tools.CreateCell(value));
                    }
                    sheetData.AppendChild(row);
                }


                //Append columns with width calculated
                worksheetPart.Worksheet.Append(this.CreateColumnsWithWidth(sheetData));
                worksheetPart.Worksheet.AppendChild(sheetData);
                workbookpart.Workbook.Save();
                spreadSheet.Close();
            }
            return ms;
        }


        private Columns CreateColumnsWithWidth(SheetData sheetData)
        {
            var widths = this.CalculateColumnsWidth(sheetData);
            Columns columns = new Columns();
            foreach (var item in widths)
            {
                UInt32 index = (uint)(item.Key + 1);
                columns.Append(new Column() { Min = index, Max = index, Width = item.Value, CustomWidth = true });
            }
            return columns;
        }

        private Dictionary<int, double> CalculateColumnsWidth(SheetData sheetData)
        {
            Dictionary<int, double> colWidths = new Dictionary<int, double>();
            foreach (var row in sheetData.Descendants<Row>())
            {
                int index = 0;
                foreach (var cell in row.Descendants<Cell>())
                {
                    if (!colWidths.ContainsKey(index)) colWidths.Add(index, 0);
                    var width = Tools.GetMinColumnWidth(cell.CellValue.Text);
                    colWidths[index] = width > colWidths[index] ? width : colWidths[index];
                    index++;
                }
            }

            return colWidths;
        }

        private List<string> CreateLines(string separator)
        {
            List<string> lines = new List<string>();
            string inputLine = string.Empty;

            //Copy Header
            foreach (var column in _columns)
            {
                inputLine += column.Title + separator;
            }
            lines.Add(inputLine);

            //Copy items
            foreach (var item in RowItems)
            {
                inputLine = string.Empty;
                foreach (var column in _columns)
                {
                    inputLine += column.Evaluate(item) + separator;
                }
                lines.Add(inputLine);
            }
            return lines;
        }
        #endregion

    }
}

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Spreadsheets.Tables
{
    public class TableExcel<T> : TableBase<T> where T : class
    {

        public bool AutoWidth { get; private set; } = false;
        public string SheetName { get; set; } = string.Empty;

        public TableExcel() { }
        public TableExcel(IEnumerable<T> data) : base(data) { }

        /// <summary>
        /// Determine if Excel columns must set width according to largest text in column
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public TableExcel<T> SetAutoWidth(bool enable)
        {
            AutoWidth = enable;
            return this;
        }

        public TableExcel<T> SetSheetName(string name)
        {
            SheetName = name;
            return this;
        }

        public void CreateExcel(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (MemoryStream ms = this.CreateXlsx())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
            }
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
                foreach (var item in this.Items)
                {
                    row = new Row();
                    foreach (var column in this._columns)
                    {
                        object value = column.Evaluate(item);
                        row.Append(Tools.CreateCell(value));
                    }
                    sheetData.AppendChild(row);
                }

                if (AutoWidth)
                {
                    //Append columns with width calculated
                    worksheetPart.Worksheet.Append(this.CreateColumnsWithWidth(sheetData));
                }
                worksheetPart.Worksheet.AppendChild(sheetData);
                workbookpart.Workbook.Save();
                spreadSheet.Close();
            }
            return ms;
        }

        protected Columns CreateColumnsWithWidth(SheetData sheetData)
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

        protected Dictionary<int, double> CalculateColumnsWidth(SheetData sheetData)
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
    }
}

﻿using System;
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
        private List<T> RowItems { get; set; }


        private MemoryStream _documentStream = new MemoryStream();
        private SpreadsheetDocument _document;
        private List<TableColumn<T>> _columns = new List<TableColumn<T>>();


        public TableSpreadsheet<T> AddIndexColumn(Expression<Func<T, object>> predicate)
        {
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

        public TableSpreadsheet<T> AddRow(T row)
        {
            RowItems.Add(row);
            return this;
        }


        public TableSpreadsheet<T> Compile()
        {
            List<string> lines = new List<string>();
            foreach (var item in RowItems)
            {
                var currentLine = string.Empty;
                foreach (var column in _columns)
                {
                    currentLine = currentLine + Convert.ToString(column.Evaluate(item)) + "; ";
                }
                lines.Add(currentLine);
            }
            return this;
        }

        public void CreateSpreadsheet(string path, ESpreadsheetType spreadsheetType)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (MemoryStream ms = CreateSpreadsheet(spreadsheetType))
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
                    return CreateExcelSpreadsheet();
                default: return null;
            }
        }

        private MemoryStream CreateCsv(ESpreadsheetType spreadsheetType)
        {
            string separator = Tools.CsvSeparator(spreadsheetType);
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

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
            sw.Flush();
            return ms;

        }




        private MemoryStream CreateExcelSpreadsheet()
        {
            _document = SpreadsheetDocument.Create(@"C:\Users\bportela\Datos\Dev\dotnet-components\Test\SpreadsheetsTestbench\hoja.xlsx", SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookpart = _document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = _document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            Sheet sheet = new Sheet()
            {
                Id = _document.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Table"
            };

            sheets.Append(sheet);
            workbookpart.Workbook.Save();
            _document.Close();

            return _documentStream;
        }

        private void CheckCreate()
        {
            if (_document is null)
                throw new InvalidOperationException("Call Create method before init");

        }
    }
}

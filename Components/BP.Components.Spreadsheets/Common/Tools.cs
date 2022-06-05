using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using BP.Components.Spreadsheets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BP.Components.Spreadsheets
{
    internal static class Tools
    {
        /// <summary>
        /// Calculate lenght of text in inches. This Method is used to adjust columns width of excel spreadsheets.
        /// NOTE: it works only on Windows
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static double GetTextInches(string text)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) //Porque System.Drawing.Graphics solo funciona en Windows
            {
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(text, new System.Drawing.Font("Calibri", 11.0f, FontStyle.Regular, GraphicsUnit.Point));
                    double ExcelWidth = (Convert.ToDouble(size.Width) - 12) / 7d + 1 + 2;//From pixels to inches.
                    return ExcelWidth;
                }
            }
            else
            {
                return 10.71; //TODO-Default width? 
            }
        }

        /// <summary>
        /// Create cell based on data converted to string and datatype
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        internal static Cell CreateCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        /// <summary>
        /// Create Cell with <see cref="object"/> value, where <see cref="CellType.DataType"/> is calculated based on <see cref="Type"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static Cell CreateCell(object value)
        {
            CellValue cellContent;
            Type type = value.GetType();
            CellValues dataType;
            if (type == typeof(string))
            {
                dataType = CellValues.String;
                cellContent = new CellValue(Convert.ToString(value)?? string.Empty);
            }
            else if (type == typeof(Int32))
            {
                dataType = CellValues.Number;
                cellContent = new CellValue(Convert.ToInt32(value));
            }
            else if (type == typeof(double) || type == typeof(float))
            {
                dataType = CellValues.Number;
                cellContent = new CellValue(Convert.ToDouble(value));
            }
            else if (type == typeof(decimal))
            {
                dataType = CellValues.Number;
                cellContent = new CellValue(Convert.ToDecimal(value));
            }
            else if (type == typeof(bool))
            {
                dataType = CellValues.Boolean;
                cellContent = new CellValue(Convert.ToBoolean(value));
            }
            else if (type == typeof(DateTime))
            {
                dataType = CellValues.String;
                cellContent = new CellValue(Convert.ToString(value)?? string.Empty);
            }
            else
            {
                dataType = CellValues.String;
                cellContent = new CellValue(Convert.ToString(value)?? string.Empty);
            }

            return new Cell()
            {
                CellValue = cellContent,
                DataType = dataType
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

    }
}

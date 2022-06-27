using BP.Components.Spreadsheets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Spreadsheets.Tables
{
    public class TableCsv<T> : TableBase<T> where T : class
    {
        public ECsvSeparator CsvSeparator { get; private set; }

        #region Constructors
        public TableCsv() { }
        public TableCsv(IEnumerable<T> items) : base(items) { }
        public TableCsv(ECsvSeparator csvSeparator) => this.CsvSeparator = csvSeparator;
        #endregion

        #region Fluent API Methods
        /// <summary>
        /// Select type of separator: comma or semicolon
        /// </summary>
        /// <param name="csvSeparator"></param>
        /// <returns></returns>
        public TableCsv<T> ConfigureSeparator(ECsvSeparator csvSeparator)
        {
            this.CsvSeparator = csvSeparator;
            return this;
        }
        #endregion

        #region Creation methods
        /// <summary>
        /// Create csv file at specific path
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">When path doesn't point to .csv file</exception>
        public override void Create(string path)
        {
            if (!path.ToLower().EndsWith(".csv"))
                throw new ArgumentException($"Path is not related to file with extension .csv");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (MemoryStream ms = this.Create())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
            }
        }


        /// <summary>
        /// Create Csv file as MemoryStream
        /// </summary>
        /// <returns></returns>
        public override MemoryStream Create() => Create(Encoding.Latin1);

        /// <summary>
        /// Create csv file as MemoryStream with the specific encoding
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public MemoryStream Create(Encoding encoding)
        {
            var lines = this.CreateLines(CsvSeparator.GetSeparator());

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, encoding);
            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
            sw.Flush();
            return ms;
        }
        #endregion

        #region Conversion Methods
        public TableExcel<T> ToExcel(string sheetName)
        {
            var excel = this.ToExcel();
            excel.SetSheetName(sheetName);
            return excel;
        }
        public TableExcel<T> ToExcel()
        {
            var excel = new TableExcel<T>();
            excel.AddItems(this.Items);
            foreach (var col in this._columns)
            {
                excel.AddColumn(col.ColData, col.Title);
            }
            return excel;
        }
        #endregion

        private List<string> CreateLines(string separator)
        {
            List<string> lines = new List<string>();
            string inputLine = string.Empty;

            //Copy Header
            foreach (var column in _columns)
            {
                inputLine += column.Title + separator;
            }
            inputLine = inputLine.Trim(separator.ToCharArray()[0]);
            lines.Add(inputLine);


            //Copy items
            foreach (var item in Items)
            {
                inputLine = string.Empty;
                foreach (var column in _columns)
                {
                    inputLine += column.Evaluate(item) + separator;
                }
                inputLine = inputLine.Trim(separator.ToCharArray()[0]);
                lines.Add(inputLine);
            }
            return lines;
        }
    }
}

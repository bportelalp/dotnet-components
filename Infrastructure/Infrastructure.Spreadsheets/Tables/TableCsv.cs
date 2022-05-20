using Infrastructure.Spreadsheets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Spreadsheets.Tables
{
    public class TableCsv<T> : TableBase<T> where T : class
    {



        public MemoryStream CreateCsv(ECsvSeparator csvSeparator)
        {
            var lines = this.CreateLines(csvSeparator.GetSeparator());

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
            foreach (var item in Items)
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
    }
}

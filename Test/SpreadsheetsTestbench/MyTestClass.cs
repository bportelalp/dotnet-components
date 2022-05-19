using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetsTestbench
{
    internal class MyTestClass
    {
        public int Integer { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public MyTestClass(int integer, string text, DateTime dateTime)
        {
            Integer = integer;
            Text = text;
            Date = dateTime;
        }
    }
}

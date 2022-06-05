using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Spreadsheets.Common
{
    public enum ECsvSeparator
    {
        Comma = 0,
        Semicolon = 1,
    }

    internal static class ECsvSeparatorExtensions
    {
        internal static string GetSeparator(this ECsvSeparator csvSeparator)
        {
            switch (csvSeparator)
            {
                case ECsvSeparator.Comma: return ",";
                case ECsvSeparator.Semicolon: return ";";
                default: return string.Empty;
            }
        }
    }
}

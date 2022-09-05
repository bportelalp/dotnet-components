using BP.Components.Blazor.ChartJS.Configuration.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS.Datasets
{
    public class LineDataset<T> : Dataset<T>
    {
        public LineDataset() : base(EChartType.line)
        {
        }

    }
}

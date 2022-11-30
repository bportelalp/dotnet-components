using BP.Components.Blazor.ChartJS.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS.Datasets
{
    public interface IDataset<T> : IList<T>
    {
        ChartType Type { get; }
        string Name { get; set; }
    }
}

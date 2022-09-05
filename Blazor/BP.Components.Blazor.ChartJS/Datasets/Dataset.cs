using BP.Components.Blazor.ChartJS.Configuration;
using BP.Components.Blazor.ChartJS.Configuration.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS.Datasets
{
    public abstract class Dataset<T> : Collection<T>, IDataset<T>
    {
        public EChartType Type { get;}
        public string Label { get; set; }
        public IndexableOption<string> BackgroundColor { get; set; } = new();

        public Dataset(EChartType type)
        {
            Type = type;
        }

        public Dataset<T> SetItems(IEnumerable<T> data, Color color)
        {
            if (data is null) throw new ArgumentNullException(nameof(data));
            this.ClearItems();
            ((List<T>)Items).AddRange(data);
            var colorHtml = ColorTranslator.ToHtml(color);
            BackgroundColor = new IndexableOption<string>(colorHtml);
            return this;
        }

        public Dataset<T> Add(T data, Color color)
        {
            Items.Add(data);
            BackgroundColor.Values.Add(ColorTranslator.ToHtml(color));
            return this;
        }
    }
}

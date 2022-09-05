using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.ChartJS.Configuration
{
    public class IndexableOption<T>
    {
        public bool IsIndexed { get; set; }

        public List<T> Values
        {
            get => _values;
            set
            {
                _value = default(T);
                _values = value;
            }
        }
        public T Value
        {
            get => _value;
            set
            {
                _values = default(List<T>);
                _value = value;
            }
        }

        public IndexableOption(IEnumerable<T> data)
        {
            Values = data.ToList();
        }
        public IndexableOption(T value)
        {
            Value = value;
        }

        public IndexableOption()
        {

        }

        private List<T> _values;
        private T _value;
    }
}

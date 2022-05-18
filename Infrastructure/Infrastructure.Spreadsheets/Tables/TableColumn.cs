using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Spreadsheets.Tables
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class TableColumn<T>
    {
        public string Title { get; set; } 
        public Expression<Func<T, object>> ColData { get => _colData; set { _colData = value; _colDataCompiled = ColData.Compile(); } }


        public TableColumn() { }

        public TableColumn(Expression<Func<T, object>> predicate, string title)
        {
            this.Title = title;
            this.ColData = predicate;
        }
        public TableColumn(Expression<Func<T, object>> predicate)
        {
            this.ColData = predicate;
        }

        private Func<T, object>? _colDataCompiled;
        private Expression<Func<T, object>> _colData;

        public TableColumn<T> WithTitle(string title)
        {
            Title = title;
            return this;
        }
        public object Evaluate(T arg)
        {
            return _colDataCompiled(arg);
        }


    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

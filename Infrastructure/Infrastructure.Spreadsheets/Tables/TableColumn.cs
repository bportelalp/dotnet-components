using DocumentFormat.OpenXml.Spreadsheet;
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
        public string Title { get => GetTitle(); set => _title = value; }
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

        private string _title;
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



        public CellValues GetCellValues(T arg)
        {
            Type type = this.GetType(arg);
            if (type == typeof(string))
                return CellValues.String;
            if (type == typeof(Int32))
                return CellValues.Number;
            if (type == typeof(Int64))
                return CellValues.Number;
            if (type == typeof(double))
                return CellValues.Number;
            if (type == typeof(float))
                return CellValues.Number;
            if (type == typeof(bool))
                return CellValues.Boolean;
            if (type == typeof(DateTime))
                return CellValues.Date;
            else
                return CellValues.String;

        }

        private Type GetType(T arg)
        {
            return _colDataCompiled(arg).GetType();
        }
        private string GetTitle()
        {
            if(!string.IsNullOrWhiteSpace(_title))
                return _title;

            try
            {
                return GetName(ColData);
            }
            catch (Exception)
            {
                return "Nombre desconocido";
            }
        }

        public static string GetName(Expression<Func<T, object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }



    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

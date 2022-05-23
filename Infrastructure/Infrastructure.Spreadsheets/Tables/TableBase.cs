using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using System.Linq.Expressions;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Spreadsheets.Common;

namespace Infrastructure.Spreadsheets.Tables
{
    public abstract class TableBase<T> where T : class
    {
        public ICollection<T> Items { get; private set; }

        protected List<TableColumn<T>> _columns = new List<TableColumn<T>>();
        protected string _fileName = nameof(T);

        public TableBase() { }
        public TableBase(IEnumerable<T> Items)
        {
            this.Items = Items.ToList();
        }

        public abstract void Create(string path);
        public abstract MemoryStream Create();

        #region Fluent API Methods
        /// <summary>
        /// Add the elements of the specified collection to <see cref="TableBase{T}.Items"/>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual TableBase<T> AddItems(IEnumerable<T> items)
        {
            this.Items = items.ToList();
            return this;
        }

        /// <summary>
        /// Add the elements of the specified collection to the end of the <see cref="TableBase{T}.Items"/>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual TableBase<T> AddRange(IEnumerable<T> items)
        {
            this.Items.ToList().AddRange(items);
            return this;
        }

        /// <summary>
        /// Add the element to the end of the <see cref="TableBase{T}.Items"/>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual TableBase<T> Add(T items)
        {
            this.Items.Add(items);
            return this;
        }

        /// <summary>
        /// Specify fields to add as columns based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TableColumn<T> AddColumn(Expression<Func<T, object>> predicate)
        {
            var column = new TableColumn<T> { ColData = predicate };
            this._columns.Add(column);
            return column;
        }
        /// <summary>
        /// Specify fields to add as columns based on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="headerName">Name to define column</param>
        /// <returns></returns>
        public TableColumn<T> AddColumn(Expression<Func<T, object>> predicate, string headerName) => this.AddColumn(predicate).SetTitle(headerName);
        #endregion


        protected TableBase<T> AddColumns(List<TableColumn<T>> tableColumns)
        {
            _columns = tableColumns;
            return this;
        }

    }
}

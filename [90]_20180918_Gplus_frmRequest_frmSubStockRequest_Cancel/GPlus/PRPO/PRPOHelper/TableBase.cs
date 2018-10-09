using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public abstract class TableBase
    {
        public DataTable Table { get; set; }

        protected TableBase()
        {
            Table = new DataTable();
            CreateColumns();
        }

        protected TableBase(DataTable table)
        {
            Table = table;
        }

        protected abstract void CreateColumns();
    }
}
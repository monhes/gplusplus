using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public class PRPOUploadFileTable : TableBase
    {
        protected override void CreateColumns()
        {
            Table.Columns.Add("Id", typeof(int));
            Table.Columns.Add("FileName");
        }

        public PRPOUploadFileTable()
        {
            HttpContext.Current.Session[PRPOSession.UploadFileTable] = Table;
        }

        public PRPOUploadFileTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        /// <summary>
        ///     เพิ่มจากรายการจากข้อมูลในฐานข้อมูล
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        public void AddItem(int id, string fileName)
        {
            DataRow row = Table.NewRow();

            if (id < 0) return;

            row["Id"] = id;
            row["FileName"] = fileName;

            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        public void AddItem(string fileName)
        {
            DataRow row = Table.NewRow();

            if (MinID == 0 || MinID > 0)
                row["Id"] = -1;
            else
                row["Id"] = MinID - 1;

            row["FileName"] = fileName;

            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        public DataRow FindItem(int prpoAttachId)
        {
            return Table.Select("Id = " + prpoAttachId).FirstOrDefault();
        }

        public int MinID
        {
            get
            {
                if (Table.Rows.Count == 0)
                    return 0;
                else
                    return (int) Table.AsEnumerable().Min(id => id["Id"]);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    /// <summary>
    ///     จัดเก็บ POItemID, PrID, PrItemID ที่ทำการลบในขณะดูรายละเอียด
    /// </summary>
    public class PRPODeleteItemTable : TableBase
    {
        public PRPODeleteItemTable()
        {
            HttpContext.Current.Session[PRPOSession.DeleteItemTable] = Table;
        }

        public PRPODeleteItemTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        protected override void CreateColumns()
        {
            Table.Columns.Add("PoItemID");
            Table.Columns.Add("PrItemID");
            Table.Columns.Add("PrID");
        }

        /// <summary>
        ///      หาก PoItemID มีค่าเป็นว่าง "" แสดงว่า เกิดจากการเพิ่มข้อมูลเข้ามาใหม่
        ///      เมธอดนี้จะเพิ่มรายการเฉพาะกรณีที่ poItemID มีค่าแล้ว (poItemID มีอยู่ในฐานข้อมูล)
        ///      เมธอดนี้จะถูกเรียกในหน้า PO
        /// </summary>
        /// <param name="poItemID"></param>
        /// <param name="ppat"></param>
        public void AddPoItem(string poItemID, PRPOActualTable ppat)
        {
            if (!string.IsNullOrEmpty(poItemID))
            {
                DataRow[] rows = ppat.FindItem(poItemID);
                foreach (DataRow r in rows)
                {
                    string prID = r["PrID"].ToString();
                    string prItemID = r["PrItemID"].ToString();

                    DataRow row = Table.NewRow();

                    row["PoItemID"] = poItemID;
                    row["PrItemID"] = prItemID;
                    row["PrID"] = prID;

                    Table.Rows.Add(row);
                }

                Table.AcceptChanges();
            }
        }

        /// <summary>
        ///     เมธอดนี้จะถูกเรียกในหน้า PR
        /// </summary>
        /// <param name="prItemID"></param>
        public void AddPrItem(string prItemID)
        {
            if (string.IsNullOrEmpty(prItemID))
                return;

            int prItemId = Convert.ToInt32(prItemID);
            if (prItemId > 0)
            {
                DataRow row = Table.NewRow();
                row["PrItemID"] = prItemID;
                Table.Rows.Add(row);
                Table.AcceptChanges();
            }
        }
    }

    public class PRPOPrintFormDeleteTable : TableBase
    {
        public PRPOPrintFormDeleteTable()
        {
            HttpContext.Current.Session[PRPOSession.PrintFormDeleteTable] = Table;
        }

        public PRPOPrintFormDeleteTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        protected override void CreateColumns()
        {
            Table.Columns.Add("PrintFormID");
            Table.Columns.Add("PoItemID");
            Table.Columns.Add("PrItemID");
        }

        public void AddItem(string printFormID, string poItemID)
        {
            if (!string.IsNullOrEmpty(printFormID))
            {
                DataRow row = Table.NewRow();

                row["PrintFormID"] = printFormID;
                row["PoItemID"] = poItemID;

                Table.Rows.Add(row);
                Table.AcceptChanges();
            }
        }

        public void AddItemByPR(string printFormID, string prItemID)
        {
            if (!string.IsNullOrEmpty(printFormID))
            {
                DataRow row = Table.NewRow();

                row["PrintFormID"] = printFormID;
                row["PrItemID"] = prItemID;

                Table.Rows.Add(row);
                Table.AcceptChanges();
            }
        }
    }

    /// <summary>
    ///     คลาสนี้ใช้สำหรับลบ ID ของ PO_Form2 หรือ PR_Form2
    /// </summary>
    public class PRPOForm2DeleteTable : TableBase
    {
        public PRPOForm2DeleteTable()
        {
            HttpContext.Current.Session[PRPOSession.Form2DeleteTable] = Table;
        }

        public PRPOForm2DeleteTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        protected override void CreateColumns()
        {
            Table.Columns.Add("PoPrForm2ID");
        }

        /// <summary>
        ///     เพิ่มรายการลงตาราง PRPOForm2DeleteTable ก็ต่อเมื่อ 
        ///     poprForm2ID ไม่ติดลบ
        /// </summary>
        /// <param name="poForm2ID"></param>
        public void AddItem(string poprForm2ID)
        {
            if (string.IsNullOrEmpty(poprForm2ID))
                return;

            if (Convert.ToInt32(poprForm2ID) > 0)
            {
                DataRow row = Table.NewRow();

                row["PoPrForm2ID"] = poprForm2ID;

                Table.Rows.Add(row);
                Table.AcceptChanges();
            }
        }
    }

    public class PRPOAttachDeleteTable : TableBase
    {
        public PRPOAttachDeleteTable()
        {
            HttpContext.Current.Session[PRPOSession.AttachDeleteTable] = Table;
        }

        public PRPOAttachDeleteTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        protected override void CreateColumns()
        {
            Table.Columns.Add("PRPOAttachID", typeof(int));
            Table.Columns.Add("FileName");
        }

        public void AddItem(int prpoAttachID, string fileName)
        {
            if (prpoAttachID > 0)
            {
                DataRow row = Table.NewRow();

                row["PRPOAttachID"] = prpoAttachID;
                row["FileName"] = fileName;

                Table.Rows.Add(row);
                Table.AcceptChanges();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public class PRPOForm2ActualTable : TableBase
    {
        protected override void CreateColumns()
        {
            Table.Columns.Add("PoForm2ID");
            Table.Columns.Add("PrForm2ID");
            Table.Columns.Add("ExpenseID");
            Table.Columns.Add("AccExpenseID");
            Table.Columns.Add("PercentAllocate");
            Table.Columns.Add("AmountAllocate");
        }

        public PRPOForm2ActualTable()
        {
            HttpContext.Current.Session[PRPOSession.Form2Table] = Table;
        }

        public PRPOForm2ActualTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        /// <summary>
        ///     สร้างเลขที่ PoForm2ID แบบเทียม
        ///     โดยใช้เลข -1 เป็นเลขเริ่มต้น e.g. (-1, -2, ...) ป้องกันเลข PoForm2ID ซ้ำกับเลขที่ดึงมาจากฐานข้อมูล
        /// </summary>
        public void AddNewRow(PRPOType type)
        {
            DataRow row = Table.NewRow();
            int minId;
            string prpoType = "";

            if (PRPOType.PO == type)
                prpoType = "PoForm2ID";
            else if (PRPOType.PR == type)
                prpoType = "PrForm2ID";


            DataRow[] rows = Table.Select(prpoType + " < '0' AND " + prpoType + " <> ''");
            if (rows.Length > 0)
            {
                minId = rows.Min(r => Convert.ToInt32(r[prpoType]));
                minId = minId > 0 ? -1 : minId - 1;
            }
            else
                minId = -1;

            row[prpoType] = minId;
            
            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        public void AddPrForm2Row()
        {
            DataRow row = Table.NewRow();

            if (MinPrForm2ID == 0 || MinPrForm2ID > 0)
                row["PrForm2ID"] = -1;
            else
                row["PrForm2ID"] = MinPrForm2ID - 1;

            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        private int MinPrForm2ID
        {
            get
            {
                if (Table.Rows.Count == 0)
                    return 0;
                else
                    return (int)Table.AsEnumerable().Min(prForm2ID => Convert.ToInt32(prForm2ID["PrForm2ID"]));
            }
        }

        public void AddItem
        (
            string poForm2ID
            , string prForm2ID
            , string expenseID
            , string accExpenseID
            , string percentAllocate
            , string amountAllocate
        )
        {
            DataRow row = Table.NewRow();

            row["PoForm2ID"] = poForm2ID;
            row["PrForm2ID"] = prForm2ID;
            row["ExpenseID"] = expenseID;
            row["AccExpenseID"] = accExpenseID;
            row["PercentAllocate"] = percentAllocate;
            row["AmountAllocate"] = amountAllocate;

            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        public void SaveItem
        (
            string poForm2ID,
            string prForm2ID,
            string expenseID,
            string accExpenseID,
            string percentAllocate,
            string amountAllocate
        )
        {
            DataRow[] rows = null;

            if (string.IsNullOrEmpty(prForm2ID))
                rows = Table.Select(string.Format("PoForm2ID = '{0}'", poForm2ID));
            else if (string.IsNullOrEmpty(poForm2ID))
                rows = Table.Select(string.Format("PrForm2ID = '{0}'", prForm2ID));

            if (rows.Length > 0)
            {
                rows[0]["ExpenseID"] = expenseID;
                rows[0]["AccExpenseID"] = accExpenseID;
                rows[0]["PercentAllocate"] = percentAllocate;
                rows[0]["AmountAllocate"] = amountAllocate;

                Table.AcceptChanges();
            }
        }

        public void DeleteItem(string poForm2ID, string prForm2ID)
        {
            DataRow[] rows = null;

            if (!string.IsNullOrEmpty(prForm2ID))
            {
                rows = Table.Select(string.Format("PrForm2ID = '{0}'", prForm2ID));
            }
            else
            {
                rows = Table.Select(string.Format("PoForm2ID = '{0}'", poForm2ID));
            }

            if (rows.Length > 0)
            {
                Table.Rows.Remove(rows[0]);
                Table.AcceptChanges();
            }
        }

        public DataRow FindPRForm2ID(string prForm2ID)
        {
            return FindPRPOForm2ID(PRPOType.PR, prForm2ID);
        }

        public DataRow FindPOForm2ID(string poForm2ID)
        {
            return FindPRPOForm2ID(PRPOType.PO, poForm2ID);
        }

        private DataRow FindPRPOForm2ID(PRPOType type, string prpoform2ID)
        {
            DataRow[] row = null;

            if (type == PRPOType.PO)
            {
                row = Table.Select("PoForm2ID = '{0}'", prpoform2ID);
            }
            else if (type == PRPOType.PR)
            {
                row = Table.Select("PrForm2ID = '{0}'", prpoform2ID);
            }

            if (row.Length > 0)
                return row[0];

            return null;
        }
    }
}
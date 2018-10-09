using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GPlus.DataAccess;

namespace GPlus.PRPO.PRPOHelper
{
    public abstract class PrintFormType : Pagebase
    {
        protected pop_PrintForm2 PrintForm;

        protected PrintFormType(pop_PrintForm2 printForm) { PrintForm = printForm; }
        public abstract string Refresh();
        public abstract void IndexChanged();
        public abstract void LoadForm(PRPOPrintFormTable ppft);
        public abstract void Save(PRPOPrintFormTable ppft);

        protected void UpdateTableRow(DataRow row)
        {
            row["Remark2"] = PrintForm.TBRemark2.Text;
            if (PrintForm.CCBorrowDate.Text != "")
                row["NewBorrowDate"] = PrintForm.CCBorrowDate.Value;
            else
                row["NewBorrowDate"] = DBNull.Value;

            if (PrintForm.RBBorrowType0.Checked)
                row["BorrowType"] = "0";
            else if (PrintForm.RBBorrowType1.Checked)
                row["BorrowType"] = "1";
            else if (PrintForm.RBBorrowType2.Checked)
                row["BorrowType"] = "2";
            else if (PrintForm.RBBorrowType3.Checked)
                row["BorrowType"] = "3";
        }

        protected void LoadSharedUIs(DataRow row)
        {
            PrintForm.TBRemark2.Text = row["Remark2"].ToString();

            if (row["BorrowType"].ToString() == "0")
                PrintForm.RBBorrowType0.Checked = true;
            else if (row["BorrowType"].ToString() == "1")
                PrintForm.RBBorrowType1.Checked = true;
            else if (row["BorrowType"].ToString() == "2")
                PrintForm.RBBorrowType2.Checked = true;
            else if (row["BorrowType"].ToString() == "3")
                PrintForm.RBBorrowType3.Checked = true;

            PrintForm.CCBorrowDate.Text = row["NewBorrowDate"].ToString();
        }

        public string ScriptSharedUIs(DataRow row)
        {
            string script = "document.getElementById('" + PrintForm.TBRemark2.ClientID + "').value = '" + row["Remark2"].ToString() + "';";

            if (row["BorrowType"].ToString() == "0")
                script += "document.getElementById('" + PrintForm.RBBorrowType0.ClientID + "').checked = true;";
            else if (row["BorrowType"].ToString() == "1")
                script += "document.getElementById('" + PrintForm.RBBorrowType1.ClientID + "').checked = true;";
            else if (row["BorrowType"].ToString() == "2")
                script += "document.getElementById('" + PrintForm.RBBorrowType2.ClientID + "').checked = true;";
            else if (row["BorrowType"].ToString() == "3")
                script += "document.getElementById('" + PrintForm.RBBorrowType3.ClientID + "').checked = true;";

            string str = string.Format("{0:dd/MM/YYYY}", row["NewBorrowDate"]);

            int lastSlash = row["NewBorrowDate"].ToString().LastIndexOf('/');
            if (lastSlash > 0)
            {
                str = str.Replace("YYYY", row["NewBorrowDate"].ToString().Substring(lastSlash + 1, 4));
                script += "document.getElementById('" + PrintForm.CCBorrowDate.ClientID + "').value = '" + str + "';";
            }

            return script; 
        }

        public abstract string GetUnitPrice();
        // For internal using
        protected virtual string GetUnitPrice(string itemID, string packID)
        {
            DataTable dt = new ItemDAO().GetItemPack(itemID, packID);

            if (dt.Rows.Count == 0)
                return "";

            return dt.Rows[0]["Avg_Cost"].ToString();
        }

        protected virtual void AcceptRowChanges(DataTable table, DataRow row)
        {
            if (table.Rows.Count == 0)
                table.Rows.Add(row);
            table.AcceptChanges();
        }
    }

    public static class PrintFormFactory
    {
        public static PrintFormType CreatePrintForm(pop_PrintForm2 popPrintForm, string type)
        {
            if (type == "1")
                return new PrintFormOld(popPrintForm);
            else if (type == "0")
                return new PrintFormNew(popPrintForm);
            else
                return null;
        }
    }
}
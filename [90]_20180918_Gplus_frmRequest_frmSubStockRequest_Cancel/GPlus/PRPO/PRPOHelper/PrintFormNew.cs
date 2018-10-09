using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GPlus.DataAccess;

namespace GPlus.PRPO.PRPOHelper
{
    public sealed class PrintFormNew : PrintFormType
    {
        public PrintFormNew(pop_PrintForm2 printForm) : base(printForm) { }

        public override string Refresh()
        {
            BindDropDownListBorrow();

            string script = "";

            //if (PRPOSession.Action == PRPOAction.ADD_PO)
            //{
            //    int invItemID = Convert.ToInt32(PrintForm.HFNewItemID.Value);
            //    int packID = Convert.ToInt32(PrintForm.HFNewPackID.Value);

            //    DataTable dt = new PODAO2().GetLastestPOPrintForm(invItemID, packID);

            //    if (dt.Rows.Count > 0)
            //    {
            //        DataRow row = dt.Rows[0];

            //        PrintForm.TBNewFormPrintCode.Text = row["FormPrintCode"].ToString();
            //        PrintForm.TBNewFormPrintName.Text = row["FormPrintName"].ToString();
            //        PrintForm.TBNewPaperType.Text = row["PaperType"].ToString();
            //        PrintForm.TBNewPaperColor.Text = row["PaperColor"].ToString();
            //        PrintForm.TBNewPaperGram.Text = row["PaperGram"].ToString();
            //        PrintForm.TBNewFontColor.Text = row["FontColor"].ToString();
            //        if (!string.IsNullOrEmpty(row["Format"].ToString()))
            //            PrintForm.RBLNewFormat.SelectedValue = row["Format"].ToString();

            //        BindDropDownListBorrow();

            //        if (row["PrintType"].ToString() != "")
            //            PrintForm.RBLNewPrintType.SelectedValue = row["PrintType"].ToString();

            //        if (row["FormBorrowType"].ToString() == "1")
            //        {
            //            PrintForm.RBNewFormBorrowType1.Checked = true;
            //            PrintForm.TBNewBorrowQuantity.Text = PRPOUtility.ToIntegerString(row["BorrowQuantity"].ToString());
            //            if (row["BorrowUnitID"].ToString() != "")
            //            {
            //                PrintForm.DDLNewBorrowUnit.SelectedValue = row["BorrowUnitID"].ToString();
            //            }
            //        }
            //        else if (row["FormBorrowType"].ToString() == "2")
            //        {
            //            PrintForm.RBNewFormBorrowType2.Checked = true;
            //            PrintForm.TBNewBorrowMonthQuantity.Text = PRPOUtility.ToIntegerString(row["BorrowMonthQuantity"].ToString());
            //            PrintForm.TBNewBorrowFirstQuantity.Text = PRPOUtility.ToIntegerString(row["BorrowFirstQuantity"].ToString());
            //            if (row["BorrowMonthUnitID"].ToString() != "")
            //            {
            //                PrintForm.DDLNewBorrowMonthUnit.SelectedValue = row["BorrowMonthUnitID"].ToString();
            //            }
            //            PrintForm.TBNewUnitQuantity.Text = PRPOUtility.ToIntegerString(row["UnitQuantity"].ToString());
            //        }

            //        PrintForm.TBNewSizeDetail.Text = row["SizeDetail"].ToString();
            //        PrintForm.TBNewRemark.Text = row["Remark"].ToString();

            //        script = ScriptSharedUIs(row);
            //    } 
            //}

            PrintForm.UPPnlNew.Update();

            return script;
        }

        private void BindDropDownListBorrow()
        {
            DataTable dt = new ItemDAO().GetItemPackID(PrintForm.TBNewFormPrintCode.Text);

            PrintForm.DDLNewBorrowUnit.DataSource = dt;
            PrintForm.DDLNewBorrowMonthUnit.DataSource = dt;
            PrintForm.DDLNewBorrowMonthUnitDisabled.DataSource = dt;

            PrintForm.DDLNewBorrowUnit.DataBind();
            PrintForm.DDLNewBorrowMonthUnit.DataBind();
            PrintForm.DDLNewBorrowMonthUnitDisabled.DataBind();
        }

        public override void IndexChanged()
        {
            PrintForm.PNLNew.Visible = true;
            PrintForm.PNLOld.Visible = false;

            PrintForm.RBLFormType.SelectedValue = "0";
        }

        public override void Save(PRPOPrintFormTable ppft)
        {
            DataRow row = ppft.Table.Rows[0];

            row["InvItemID"] = PrintForm.HFNewItemID.Value;
            row["PackID"] = PrintForm.HFNewPackID.Value;
            row["InvItemCode"] = PrintForm.TBNewFormPrintCode.Text;
            row["InvItemName"] = PrintForm.TBNewFormPrintName.Text;

            row["FormPrintCode"] = PrintForm.TBNewFormPrintCode.Text;
            row["FormPrintName"] = PrintForm.TBNewFormPrintName.Text;
            row["FormType"] = "0";
            row["Format"] = PrintForm.RBLNewFormat.SelectedValue;
            row["PaperType"] = PrintForm.TBNewPaperType.Text;
            row["PaperColor"] = PrintForm.TBNewPaperColor.Text;
            row["PaperGram"] = PrintForm.TBNewPaperGram.Text;
            row["FontColor"] = PrintForm.TBNewFontColor.Text;
            row["PrintType"] = PrintForm.RBLNewPrintType.SelectedValue;

            if (PrintForm.RBNewFormBorrowType1.Checked)
            {
                row["FormBorrowType"] = "1";
                row["BorrowQuantity"] = PrintForm.TBNewBorrowQuantity.Text;
                row["BorrowUnitID"] = PrintForm.DDLNewBorrowUnit.SelectedValue;
                if (PrintForm.DDLNewBorrowUnit.SelectedItem != null)
                {
                    row["PackName"] = PrintForm.DDLNewBorrowUnit.SelectedItem.Text;
                    row["PackID"]   = PrintForm.DDLNewBorrowUnit.SelectedValue;
                }
                row["UnitQuantity"] = PrintForm.TBNewBorrowQuantity.Text;
            }
            else if (PrintForm.RBNewFormBorrowType2.Checked)
            {
                row["FormBorrowType"] = "2";
                row["BorrowMonthQuantity"] = PrintForm.TBNewBorrowMonthQuantity.Text;
                row["BorrowFirstQuantity"] = PrintForm.TBNewBorrowFirstQuantity.Text;
                row["BorrowMonthUnitID"] = PrintForm.DDLNewBorrowMonthUnit.SelectedValue;
                if (PrintForm.DDLNewBorrowMonthUnit.SelectedItem != null)
                {
                    row["PackName"] = PrintForm.DDLNewBorrowMonthUnit.SelectedItem.Text;
                    row["PackID"]   = PrintForm.DDLNewBorrowMonthUnit.SelectedValue;
                }
                row["UnitQuantity"] = PrintForm.TBNewUnitQuantity.Text;
            }

            row["SizeDetail"] = PrintForm.TBNewSizeDetail.Text;
            row["Remark"] = PrintForm.TBNewRemark.Text;

            row["UnitPrice"] = GetUnitPrice();

            UpdateTableRow(row);
        }

        public override void LoadForm(PRPOPrintFormTable ppft)
        {
            DataRow row = ppft.Table.Rows[0];

            if (row["FormType"].ToString() != "0")
                return;

            PrintForm.HFNewItemID.Value = row["InvItemID"].ToString();
            PrintForm.HFNewPackID.Value = row["PackID"].ToString();
            PrintForm.TBNewFormPrintCode.Text = row["FormPrintCode"].ToString();
            PrintForm.TBNewFormPrintName.Text = row["FormPrintName"].ToString();
            PrintForm.TBNewPaperType.Text = row["PaperType"].ToString();
            PrintForm.TBNewPaperColor.Text = row["PaperColor"].ToString();
            PrintForm.TBNewPaperGram.Text = row["PaperGram"].ToString();
            PrintForm.TBNewFontColor.Text = row["FontColor"].ToString();
            if (!string.IsNullOrEmpty(row["Format"].ToString()))
                PrintForm.RBLNewFormat.SelectedValue = row["Format"].ToString();

            Refresh();

            if (row["PrintType"].ToString() != "")
                PrintForm.RBLNewPrintType.SelectedValue = row["PrintType"].ToString();

            if (row["FormBorrowType"].ToString() == "1")
            {
                PrintForm.RBNewFormBorrowType1.Checked = true;
                PrintForm.TBNewBorrowQuantity.Text = row["BorrowQuantity"].ToString();
                if (row["BorrowUnitID"].ToString() != "")
                {
                    //Refresh();
                    PrintForm.DDLNewBorrowUnit.SelectedValue = row["BorrowUnitID"].ToString();
                }
            }
            else if (row["FormBorrowType"].ToString() == "2")
            {
                PrintForm.RBNewFormBorrowType2.Checked = true;
                PrintForm.TBNewBorrowMonthQuantity.Text = row["BorrowMonthQuantity"].ToString();
                PrintForm.TBNewBorrowFirstQuantity.Text = row["BorrowFirstQuantity"].ToString();
                if (row["BorrowMonthUnitID"].ToString() != "")
                {
                    //Refresh();
                    PrintForm.DDLNewBorrowMonthUnit.SelectedValue = row["BorrowMonthUnitID"].ToString();
                }
                PrintForm.TBNewUnitQuantity.Text = row["UnitQuantity"].ToString();
            }

            PrintForm.TBNewSizeDetail.Text = row["SizeDetail"].ToString();
            PrintForm.TBNewRemark.Text = row["Remark"].ToString();

            LoadSharedUIs(row);

        }

        public override string GetUnitPrice()
        {
            return GetUnitPrice(PrintForm.HFNewItemID.Value, PrintForm.HFNewPackID.Value);
        }
    }
}
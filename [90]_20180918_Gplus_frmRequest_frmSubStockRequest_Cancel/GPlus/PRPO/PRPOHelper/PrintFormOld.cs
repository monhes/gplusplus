using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GPlus.DataAccess;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public sealed class PrintFormOld : PrintFormType
    {
        public PrintFormOld(pop_PrintForm2 printForm) : base(printForm) { }

        public override string Refresh()
        {
            BindDropDownListBorrow();

            string script = "";

            //if (PRPOSession.Action == PRPOAction.ADD_PO)
            //{
            //    int invItemID = Convert.ToInt32(PrintForm.HFOldItemID.Value);
            //    int packID = Convert.ToInt32(PrintForm.HFOldPackID.Value);

            //    DataTable dt = new PODAO2().GetLastestPOPrintForm(invItemID, packID);

            //    if (dt.Rows.Count > 0)
            //    {
            //        DataRow row = dt.Rows[0];

            //        PrintForm.TBOldFormPrintCode.Text = row["FormPrintCode"].ToString();
            //        PrintForm.TBOldFormPrintName.Text = row["FormPrintName"].ToString();
            //        PrintForm.TBOldPaperType.Text = row["PaperType"].ToString();
            //        PrintForm.TBOldPaperColor.Text = row["PaperColor"].ToString();
            //        PrintForm.TBOldPaperGram.Text = row["PaperGram"].ToString();
            //        PrintForm.TBOldFontColor.Text = row["FontColor"].ToString();
            //        if (row["PrintType"].ToString() != "")
            //            PrintForm.RBLOldPrintType.SelectedValue = row["PrintType"].ToString();
            //        PrintForm.CBOldIsRequestModify.Checked = row["IsRequestModify"].ToString() == "1" ? true : false;
            //        PrintForm.CBOldIsFixedContent.Checked = row["IsFixedContent"].ToString() == "1" ? true : false;
            //        PrintForm.CBOldIsPaper.Checked = row["IsPaper"].ToString() == "1" ? true : false;
            //        PrintForm.CBOldIsFont.Checked = row["IsFont"].ToString() == "1" ? true : false;

            //        BindDropDownListBorrow();

            //        if (row["FormBorrowType"].ToString() == "1")
            //        {
            //            PrintForm.RBOldFormBorrowType1.Checked = true;
            //            PrintForm.TBOldBorrowQuantity.Text = PRPOUtility.ToIntegerString(row["BorrowQuantity"].ToString());

            //            if (row["BorrowUnitID"].ToString() != "")
            //            {
            //                PrintForm.DDLOldBorrowUnit.SelectedValue = row["BorrowUnitID"].ToString();
            //            }
            //        }
            //        else if (row["FormBorrowType"].ToString() == "2")
            //        {
            //            PrintForm.RBOldFormBorrowType2.Checked = true;
            //            PrintForm.TBOldBorrowMonthQuantity.Text = PRPOUtility.ToIntegerString(row["BorrowMonthQuantity"].ToString());
            //            PrintForm.TBOldBorrowFirstQuantity.Text = PRPOUtility.ToIntegerString(row["BorrowFirstQuantity"].ToString());

            //            if (row["BorrowMonthUnitID"].ToString() != "")
            //            {
            //                PrintForm.DDLOldBorrowMonthUnit.SelectedValue = row["BorrowMonthUnitID"].ToString();
            //            }
            //            PrintForm.TBOldUnitQuantity.Text = PRPOUtility.ToIntegerString(row["UnitQuantity"].ToString());
            //        }

            //        PrintForm.TBOldSizeDetail.Text = row["SizeDetail"].ToString();
            //        PrintForm.TBOldRequestModifyDesc.Text = row["RequestModifyDesc"].ToString();
            //        PrintForm.TBOldRemark.Text = row["Remark"].ToString();

            //        script = ScriptSharedUIs(row);
            //    }
            //}

            PrintForm.UPPnlOld.Update();

            return script;
        }

        private void BindDropDownListBorrow()
        {
            DataTable dt = new ItemDAO().GetItemPackID(PrintForm.TBOldFormPrintCode.Text);

            PrintForm.DDLOldBorrowUnit.DataSource = dt;
            PrintForm.DDLOldBorrowMonthUnit.DataSource = dt;
            PrintForm.DDLOldBorrowMonthUnitDisabled.DataSource = dt;

            PrintForm.DDLOldBorrowUnit.DataBind();
            PrintForm.DDLOldBorrowMonthUnit.DataBind();
            PrintForm.DDLOldBorrowMonthUnitDisabled.DataBind();
        }

        public override void IndexChanged()
        {
            PrintForm.PNLNew.Visible = false;
            PrintForm.PNLOld.Visible = true;

            PrintForm.RBLFormType.SelectedValue = "1";
        }

        public override void Save(PRPOPrintFormTable ppft)
        {
            DataRow row = ppft.Table.Rows[0];

            row["InvItemID"] = PrintForm.HFOldItemID.Value;
            row["PackID"] = PrintForm.HFOldPackID.Value;
            row["InvItemCode"] = PrintForm.TBOldFormPrintCode.Text;
            row["InvItemName"] = PrintForm.TBOldFormPrintName.Text;

            row["FormPrintCode"] = PrintForm.TBOldFormPrintCode.Text;
            row["FormPrintName"] = PrintForm.TBOldFormPrintName.Text;
            row["FormType"] = "1";
            row["PaperType"] = PrintForm.TBOldPaperType.Text;
            row["PaperColor"] = PrintForm.TBOldPaperColor.Text;
            row["PaperGram"] = PrintForm.TBOldPaperGram.Text;
            row["FontColor"] = PrintForm.TBOldFontColor.Text;
            row["PrintType"] = PrintForm.RBLOldPrintType.SelectedValue;
            row["IsRequestModify"] = PrintForm.CBOldIsRequestModify.Checked == true ? "1" : "0";
            row["IsFixedContent"] = PrintForm.CBOldIsFixedContent.Checked == true ? "1" : "0";
            row["IsPaper"] = PrintForm.CBOldIsPaper.Checked == true ? "1" : "0";
            row["IsFont"] = PrintForm.CBOldIsFont.Checked == true ? "1" : "0";

            if (PrintForm.RBOldFormBorrowType1.Checked)
            {
                row["FormBorrowType"] = "1";
                row["BorrowQuantity"] = PrintForm.TBOldBorrowQuantity.Text;
                row["BorrowUnitID"] = PrintForm.DDLOldBorrowUnit.SelectedValue;
                if (PrintForm.DDLOldBorrowUnit.SelectedItem != null)
                {
                    row["PackName"] = PrintForm.DDLOldBorrowUnit.SelectedItem.Text;
                    row["PackID"]   = PrintForm.DDLOldBorrowUnit.SelectedValue;
                }
                row["UnitQuantity"] = PrintForm.TBOldBorrowQuantity.Text;
            }
            else if (PrintForm.RBOldFormBorrowType2.Checked)
            {
                row["FormBorrowType"] = "2";
                row["BorrowMonthQuantity"] = PrintForm.TBOldBorrowMonthQuantity.Text;
                row["BorrowFirstQuantity"] = PrintForm.TBOldBorrowFirstQuantity.Text;
                row["BorrowMonthUnitID"] = PrintForm.DDLOldBorrowMonthUnit.SelectedValue;
                if (PrintForm.DDLOldBorrowMonthUnit.SelectedItem != null)
                {
                    row["PackName"] = PrintForm.DDLOldBorrowMonthUnit.SelectedItem.Text;
                    row["PackID"]   = PrintForm.DDLOldBorrowMonthUnit.SelectedValue;
                }
                row["UnitQuantity"] = PrintForm.TBOldUnitQuantity.Text;
            }

            row["SizeDetail"] = PrintForm.TBOldSizeDetail.Text;
            row["RequestModifyDesc"] = PrintForm.TBOldRequestModifyDesc.Text;
            row["Remark"] = PrintForm.TBOldRemark.Text;

            row["UnitPrice"] = GetUnitPrice();

            UpdateTableRow(row);
        }

        public override void LoadForm(PRPOPrintFormTable ppft)
        {
            DataRow row = ppft.Table.Rows[0];

            if (row["FormType"].ToString() != "1")
                return;

            PrintForm.HFOldItemID.Value = row["InvItemID"].ToString();
            PrintForm.HFOldPackID.Value = row["PackID"].ToString();
            PrintForm.TBOldFormPrintCode.Text = row["FormPrintCode"].ToString();
            PrintForm.TBOldFormPrintName.Text = row["FormPrintName"].ToString();
            PrintForm.TBOldPaperType.Text = row["PaperType"].ToString();
            PrintForm.TBOldPaperColor.Text = row["PaperColor"].ToString();
            PrintForm.TBOldPaperGram.Text = row["PaperGram"].ToString();
            PrintForm.TBOldFontColor.Text = row["FontColor"].ToString();
            if (row["PrintType"].ToString() != "")
                PrintForm.RBLOldPrintType.SelectedValue = row["PrintType"].ToString();
            PrintForm.CBOldIsRequestModify.Checked = row["IsRequestModify"].ToString() == "1" ? true : false;
            PrintForm.CBOldIsFixedContent.Checked = row["IsFixedContent"].ToString() == "1" ? true : false;
            PrintForm.CBOldIsPaper.Checked = row["IsPaper"].ToString() == "1" ? true : false;
            PrintForm.CBOldIsFont.Checked = row["IsFont"].ToString() == "1" ? true : false;

            Refresh();

            if (row["FormBorrowType"].ToString() == "1")
            {
                PrintForm.RBOldFormBorrowType1.Checked = true;
                PrintForm.TBOldBorrowQuantity.Text = row["BorrowQuantity"].ToString();
                    
                if (row["BorrowUnitID"].ToString() != "")
                {
                    //Refresh();
                    PrintForm.DDLOldBorrowUnit.SelectedValue = row["BorrowUnitID"].ToString();
                }
            }
            else if (row["FormBorrowType"].ToString() == "2")
            {
                PrintForm.RBOldFormBorrowType2.Checked = true;
                PrintForm.TBOldBorrowMonthQuantity.Text = row["BorrowMonthQuantity"].ToString();
                PrintForm.TBOldBorrowFirstQuantity.Text = row["BorrowFirstQuantity"].ToString();
                    
                if (row["BorrowMonthUnitID"].ToString() != "")
                {
                    //Refresh();
                    PrintForm.DDLOldBorrowMonthUnit.SelectedValue = row["BorrowMonthUnitID"].ToString();
                }
                PrintForm.TBOldUnitQuantity.Text = row["UnitQuantity"].ToString();
            }

            PrintForm.TBOldSizeDetail.Text = row["SizeDetail"].ToString();
            PrintForm.TBOldRequestModifyDesc.Text = row["RequestModifyDesc"].ToString();
            PrintForm.TBOldRemark.Text = row["Remark"].ToString();

            LoadSharedUIs(row);
        }

        public override string GetUnitPrice()
        {
            return GetUnitPrice(PrintForm.HFOldItemID.Value, PrintForm.HFOldPackID.Value);
        }
    }
}
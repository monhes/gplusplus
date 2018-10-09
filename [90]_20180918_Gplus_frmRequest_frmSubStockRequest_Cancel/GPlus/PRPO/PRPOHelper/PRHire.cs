using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GPlus.UserControls;
using System.Data;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.IO;

namespace GPlus.PRPO.PRPOHelper
{
    public class PRHire : PRType
    {
        public PRHire(PRControl control)
            : base(control)
        {
        }

        public override void BindPR(string prId, DataTable dtPR)
        {
            base.BindPR(prId, dtPR);
            IndexChanged();
            PRCtrl.RBLPRType.SelectedValue = "2";

            PRCtrl.DDLProject.Enabled = true;
            PRCtrl.TBObjective.Enabled = true;
            PRCtrl.CBPrintForm.Enabled = false;
            PRCtrl.BPrintForm.Enabled = false;
            PRCtrl.BProductSelect.Enabled = false;
            PRCtrl.PHire.Visible = true;
            PRCtrl.PPurchase.Visible = false;

            PRCtrl.DDLProject.SelectedValue = dtPR.Rows[0]["Project_ID"].ToString();
            PRCtrl.TBObjective.Text = dtPR.Rows[0]["Objective"].ToString();

            DataTable dtPRItem = new PRDAO2().GetPRItem(Convert.ToInt32(prId), GetPRType());

            PRPOHireActualTable phat = new PRPOHireActualTable();
            PRPOForm2ActualTable pfat = new PRPOForm2ActualTable();
            new PRPODeleteItemTable();
            new PRPOForm2DeleteTable();
            new PRPOAttachDeleteTable();
            new PRPOUploadFileTable();

            foreach (DataRow row in dtPRItem.Rows)
            {
                string tradeDiscountPercent = row["TradeDiscountPercent"].ToString();
                if (!string.IsNullOrEmpty(tradeDiscountPercent))
                {
                    if (Convert.ToDecimal(tradeDiscountPercent) == 0)
                        tradeDiscountPercent = "";
                }

                string packID = row["PackID"].ToString();
                string unitPrice = row["UnitPrice"].ToString();
                string unitQuantity = row["UnitQuantity"].ToString();
                string tradeDiscountAmount = row["TradeDiscountAmount"].ToString();
                string totalBeforeVat = row["TotalBeforeVat"].ToString();
                string vatPercent = row["VatPercent"].ToString();
                string vatAmount = row["VatAmount"].ToString();
                string netAmount = row["NetAmount"].ToString();
                string prID = row["PrID"].ToString();
                string prItemID = row["PrItemID"].ToString();
                string procureName = row["ProcureName"].ToString();
                string specify = row["Specify"].ToString();
                string specPurchase = row["InvSpecPurchase"].ToString();

                phat.AddItem
                (
                    ""
                    , ""
                    , packID
                    , ""
                    , ""
                    , ""
                    , unitPrice
                    , unitQuantity
                    , specPurchase
                    , tradeDiscountPercent
                    , tradeDiscountAmount
                    , ""
                    , totalBeforeVat
                    , vatPercent
                    , vatAmount
                    , netAmount
                    , ""
                    , prID
                    , prItemID
                    , ""
                    , procureName
                    , specify
                    , "Y"
                );
            }

            PRCtrl.GVHire.DataSource = phat.Table;
            PRCtrl.GVHire.DataBind();

            BindGridViewForm2(prId);
        }

        private void BindGridViewForm2(string prID)
        {
            DataTable dtPOForm2 = new PRDAO2().GetPRForm2(prID);

            PRPOForm2ActualTable pfat = new PRPOForm2ActualTable();

            foreach (DataRow r in dtPOForm2.Rows)
            {
                string prForm2ID = r["PR_Form2_ID"].ToString();
                string expenseID = r["Expense_ID"].ToString();
                string accExpenseID = r["AccExpense_ID"].ToString();
                string percentAllocate = string.Format("{0:0}", r["Percent_Allocate"]);
                string amountAllocate = r["Amount_Allocate"].ToString();

                pfat.AddItem("", prForm2ID, expenseID, accExpenseID, percentAllocate, amountAllocate);
            }

            PRCtrl.GVForm2.DataSource = pfat.Table;
            PRCtrl.GVForm2.DataBind();
        }

        public override void BindGridViewItems()
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);

            SaveGridViewItems();

            PRCtrl.GVHire.DataSource = phat.Table;
            PRCtrl.GVHire.DataBind();
        }

        public override void SaveGridViewItems()
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);

            GridView gv = PRCtrl.GVHire;

            for (int i = 0; i < gv.Rows.Count; ++i)
            {
                HiddenField hfItemID = gv.Rows[i].FindControl("hfItemID") as HiddenField;
                HiddenField hfPackID = gv.Rows[i].FindControl("hfPackID") as HiddenField;
                HiddenField hfPrID = gv.Rows[i].FindControl("hfPrID") as HiddenField;
                HiddenField hfPrItemID = gv.Rows[i].FindControl("hfPrItemID") as HiddenField;
                HiddenField hfInvSpecPurchase = gv.Rows[i].FindControl("hfInvSpecPurchase") as HiddenField;

                TextBox tbProcureName = gv.Rows[i].FindControl("tbProcureName") as TextBox;
                DropDownList ddlPackage = gv.Rows[i].FindControl("ddlPackage") as DropDownList;
                TextBox tbQuantity = gv.Rows[i].FindControl("tbQuantity") as TextBox;
                TextBox tbUnitPrice = gv.Rows[i].FindControl("tbUnitPrice") as TextBox;
                TextBox tbTradeDiscountPercent = gv.Rows[i].FindControl("tbTradeDiscountPercent") as TextBox;
                TextBox tbTradeDiscountAmount = gv.Rows[i].FindControl("tbTradeDiscountAmount") as TextBox;
                TextBox tbTotalBeforeVat = gv.Rows[i].FindControl("tbTotalBeforeVat") as TextBox;
                TextBox tbVatPercent = gv.Rows[i].FindControl("tbVatPercent") as TextBox;
                TextBox tbVatAmount = gv.Rows[i].FindControl("tbVatAmount") as TextBox;
                TextBox tbUnitTotal = gv.Rows[i].FindControl("tbUnitTotal") as TextBox;
                TextBox tbSpecify = gv.Rows[i].FindControl("tbSpecify") as TextBox;

                DataRow[] rows = phat.FindPrItemID(hfPrItemID.Value);
                if (rows.Length > 0)
                {
                    hfPackID.Value = ddlPackage.SelectedValue;

                    phat.UpdateItem
                    (
                        rows[0],
                        "",
                        ddlPackage.SelectedValue,
                        rows[0]["InvItemCode"].ToString(),
                        rows[0]["InvItemName"].ToString(),
                        rows[0]["PackName"].ToString(),
                        tbUnitPrice.Text,
                        tbQuantity.Text,
                        hfInvSpecPurchase.Value,
                        tbTradeDiscountPercent.Text,
                        tbTradeDiscountAmount.Text,
                        "",
                        "",
                        tbVatPercent.Text,
                        "",
                        "",
                        "",
                        rows[0]["PrID"].ToString(),
                        rows[0]["PrItemID"].ToString(),
                        "",
                        tbProcureName.Text,
                        tbSpecify.Text,
                        "Y"
                    );
                }
            }
        }

        public override void DeleteGridViewItems()
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);
            PRPODeleteItemTable pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);

            SaveGridViewItems();

            for (int i = 0; i < PRCtrl.GVHire.Rows.Count; ++i)
            {
                CheckBox cbDelete = PRCtrl.GVHire.Rows[i].FindControl("cbDelete") as CheckBox;

                if (cbDelete.Checked)
                {
                    GridViewRow row = PRCtrl.GVHire.Rows[i];

                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
                    HiddenField hfPoItemID = row.FindControl("hfPoItemID") as HiddenField;

                    pdit.AddPrItem(hfPrItemID.Value);
                    phat.DeleteItem(hfPrID.Value, hfPrItemID.Value);
                }
            }

            BindGridViewItems();
        }

        public override string Save(HttpRequest request)
        {
            PRDAO2 prDao2 = new PRDAO2();

            string prId = "";

            try
            {
                prDao2.BeginTransaction();

                Pagebase pb = new Pagebase();

                string prItemId = "";
                string prCode = GetPRCode();

                prId = prDao2.InsertInvPRForm1
                (
                    prCode
                    , "2"
                    , "S"
                    , PRCtrl.CCRequestDate.Value
                    , pb.OrgID
                    , PRCtrl.DDLProject.SelectedValue
                    , PRCtrl.TBObjective.Text
                    , PRCtrl.DDLSupplier.SelectedValue
                    , PRCtrl.CBPrintForm.Checked ? "1" : "0"
                    , "0"
                    , PRCtrl.TBQuotationCode.Text
                    , PRCtrl.CCQuotationDate.Value
                    , PRCtrl.CBTradeDiscountType.Checked ? "1" : "0"
                    , PRCtrl.RBLTradeDiscountType.SelectedValue
                    , PRCtrl.TBTradeDiscountPercent.Text
                    , PRCtrl.TBTradeDiscountAmount.Text
                    , "0"
                    , ""
                    , ""
                    , PRCtrl.RBLVatType.SelectedValue
                    , PRCtrl.TBVat.Text
                    , PRCtrl.RBLVatUnitType.SelectedValue
                    , ""
                    , pb.UserName
                    , PRCtrl.TBTotal.Text.Replace(",", "")
                    , PRCtrl.TBTotalDiscount.Text.Replace(",", "")
                    , PRCtrl.TBTotalBeforeVat.Text.Replace(",", "")
                    , PRCtrl.TBTotalVat.Text.Replace(",", "")
                    , PRCtrl.TBGrandTotal.Text.Replace(",", "")
                );

                for (int i = 0; i < PRCtrl.GVHire.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVHire.Rows[i];

                    HiddenField hfSpecPurchase = row.FindControl("hfInvSpecPurchase") as HiddenField;

                    TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbQuantity = row.FindControl("tbQuantity") as TextBox;
                    TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                    TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                    TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                    TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                    TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;
                    TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;
                    TextBox tbProcureName = row.FindControl("tbProcureName") as TextBox;
                    TextBox tbSpecify = row.FindControl("tbSpecify") as TextBox;
                    DropDownList ddlPackage = row.FindControl("ddlPackage") as DropDownList;

                    if (tbUnitPrice.Text.Trim().Length == 0) tbUnitPrice.Text = "0";
                    if (tbQuantity.Text.Trim().Length == 0) tbQuantity.Text = "0";
                    if (tbTradeDiscountPercent.Text.Trim().Length == 0) tbTradeDiscountPercent.Text = "0";
                    if (tbTradeDiscountAmount.Text.Trim().Length == 0) tbTradeDiscountAmount.Text = "0";
                    if (tbVatPercent.Text.Trim().Length == 0) tbVatPercent.Text = "0";
                    if (tbVatAmount.Text.Trim().Length == 0) tbVatAmount.Text = "0";
                    if (tbNetAmount.Text.Trim().Length == 0) tbNetAmount.Text = "0";
                    if (tbTotalBeforeVat.Text.Trim().Length == 0) tbTotalBeforeVat.Text = "0";

                    prItemId = prDao2.InsertInvPRItems
                    (
                        prId
                        , ""
                        , tbProcureName.Text
                        , tbSpecify.Text
                        , ddlPackage.SelectedValue
                        , tbUnitPrice.Text.Replace(",", "")
                        , tbQuantity.Text.Replace(",", "")
                        , tbTradeDiscountPercent.Text.Replace(",", "")
                        , tbTradeDiscountAmount.Text.Replace(",", "")
                        , ""
                        , ""
                        , tbTotalBeforeVat.Text.Replace(",", "")
                        , tbVatPercent.Text.Replace(",", "")
                        , tbVatAmount.Text.Replace(",", "")
                        , tbNetAmount.Text.Replace(",", "")
                        , hfSpecPurchase.Value
                    );
                }

                for (int i = 0; i < PRCtrl.GVForm2.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVForm2.Rows[i];

                    DropDownList ddlExpense = row.FindControl("ddlExpense") as DropDownList;
                    DropDownList ddlAccExpense = row.FindControl("ddlAccExpense") as DropDownList;
                    TextBox tbPercentAllocate = row.FindControl("tbPercentAllocate") as TextBox;
                    TextBox tbAmountAllocate = row.FindControl("tbAmountAllocate") as TextBox;

                    prDao2.InsertInvPRForm2
                    (
                        prId
                        , ddlExpense.SelectedValue
                        , ddlAccExpense.SelectedValue
                        , tbPercentAllocate.Text
                        , tbAmountAllocate.Text.Replace(",", "")
                    );
                }

                PRPOUploadFileTable puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);

                foreach (DataRow row in puft.Table.Rows)
                {
                    string filePath = Server.MapPath(Path.Combine(PRPOPath.PRTmpUpload, PRPOSession.UserID, row["FileName"].ToString()));
                    if (File.Exists(filePath))
                    {
                        File.Move(filePath, Server.MapPath(Path.Combine(PRPOPath.PRUpload, row["FileName"].ToString())));
                        prDao2.InsertInvPRAttach(prId, row["FileName"].ToString());
                    }
                }

                prDao2.CommitTransaction();
            }
            catch (Exception ex)
            {
                prDao2.RollbackTransaction();
                throw ex;
            }

            return prId;
        }

        public override void Update(HttpRequest request)
        {
            PRDAO2 prDao2 = new PRDAO2();

            try
            {
                prDao2.BeginTransaction();

                PRPODeleteItemTable     pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);
                PRPOForm2DeleteTable    pfdt = new PRPOForm2DeleteTable(PRPOSession.Form2DeleteTable);
                PRPOAttachDeleteTable   padt = new PRPOAttachDeleteTable(PRPOSession.AttachDeleteTable);
                PRPOUploadFileTable     puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);

                // ลบรายการ inv_pr_items
                foreach (DataRow r in pdit.Table.Rows)
                {
                    string prItemID = r["PrItemID"].ToString();
                    prDao2.DeleteInvPRItem(prItemID);
                }

                // ลบรายการ inv_pr_form2
                foreach (DataRow r in pfdt.Table.Rows)
                {
                    string prForm2ID = r["PoPrForm2ID"].ToString();
                    prDao2.DeleteInvPRForm2(prForm2ID);
                }

                // ลบรายการ inv_pr_attach
                foreach (DataRow r in padt.Table.Rows)
                {
                    string prAttachID = r["PRPOAttachID"].ToString();
                    string fileName = r["FileName"].ToString();

                    prDao2.DeleteInvPRAttach(prAttachID);
                    // ลบไฟล์ใน ~/Uploads/PR
                    string filePath = Path.Combine(Server.MapPath(PRPOPath.PRUpload), fileName);
                    if (File.Exists(filePath))
                    {
                        try { File.Delete(filePath); }
                        catch (Exception) { }
                    }
                }

                if ((PRCtrl.RBLTradeDiscountType.SelectedIndex == 1)
                    || (PRCtrl.CBTradeDiscountType.Checked == false))
                {
                    PRCtrl.TBTradeDiscountPercent.Text = "";
                    PRCtrl.TBTradeDiscountAmount.Text = "";
                }

                if (PRCtrl.RBLVatType.SelectedValue == "1")
                {
                    PRCtrl.TBVat.Text = "";
                }

                prDao2.UpdateInvPRForm1
                (
                    PRPOSession.PrID
                    , PRCtrl.CCRequestDate.Value
                    , OrgID
                    , PRCtrl.DDLProject.SelectedValue
                    , PRCtrl.TBObjective.Text
                    , PRCtrl.DDLSupplier.SelectedValue
                    , PRCtrl.CBPrintForm.Checked ? "1" : "0"
                    , "0"
                    , PRCtrl.TBQuotationCode.Text
                    , PRCtrl.CCQuotationDate.Value
                    , PRCtrl.CBTradeDiscountType.Checked ? "1" : "0"
                    , PRCtrl.RBLTradeDiscountType.SelectedValue
                    , PRCtrl.TBTradeDiscountPercent.Text
                    , PRCtrl.TBTradeDiscountAmount.Text
                    , "0"
                    , ""
                    , ""
                    , PRCtrl.RBLVatType.SelectedValue
                    , PRCtrl.TBVat.Text
                    , PRCtrl.RBLVatUnitType.SelectedValue
                    , ""
                    , UserName
                    , PRCtrl.TBTotal.Text.Replace(",", "")
                    , PRCtrl.TBTotalDiscount.Text.Replace(",", "")
                    , PRCtrl.TBTotalBeforeVat.Text.Replace(",", "")
                    , PRCtrl.TBTotalVat.Text.Replace(",", "")
                    , PRCtrl.TBGrandTotal.Text.Replace(",", "")
                );

                // กรณีเพิ่ม/แก้ไข inv_pr_items
                for (int i = 0; i < PRCtrl.GVHire.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVHire.Rows[i];

                    HiddenField hfSpecPurchase = row.FindControl("hfInvSpecPurchase") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;

                    TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbQuantity = row.FindControl("tbQuantity") as TextBox;
                    TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                    TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                    TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                    TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                    TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;
                    TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;
                    TextBox tbProcureName = row.FindControl("tbProcureName") as TextBox;
                    TextBox tbSpecify = row.FindControl("tbSpecify") as TextBox;
                    DropDownList ddlPackage = row.FindControl("ddlPackage") as DropDownList;

                    if (tbUnitPrice.Text.Trim().Length == 0) tbUnitPrice.Text = "0";
                    if (tbQuantity.Text.Trim().Length == 0) tbQuantity.Text = "0";
                    if (tbTradeDiscountPercent.Text.Trim().Length == 0) tbTradeDiscountPercent.Text = "0";
                    if (tbTradeDiscountAmount.Text.Trim().Length == 0) tbTradeDiscountAmount.Text = "0";
                    if (tbVatPercent.Text.Trim().Length == 0) tbVatPercent.Text = "0";
                    if (tbVatAmount.Text.Trim().Length == 0) tbVatAmount.Text = "0";
                    if (tbNetAmount.Text.Trim().Length == 0) tbNetAmount.Text = "0";
                    if (tbTotalBeforeVat.Text.Trim().Length == 0) tbTotalBeforeVat.Text = "0";

                    // กรณีเพิ่ม inv_pr_item
                    if (Convert.ToInt32(hfPrItemID.Value) < 0)
                    {
                        prDao2.InsertInvPRItems
                        (
                            PRPOSession.PrID
                            , ""
                            , tbProcureName.Text
                            , tbSpecify.Text
                            , ddlPackage.SelectedValue
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbQuantity.Text.Replace(",", "")
                            , tbTradeDiscountPercent.Text.Replace(",", "")
                            , tbTradeDiscountAmount.Text.Replace(",", "")
                            , ""
                            , ""
                            , tbTotalBeforeVat.Text.Replace(",", "")
                            , tbVatPercent.Text.Replace(",", "")
                            , tbVatAmount.Text.Replace(",", "")
                            , tbNetAmount.Text.Replace(",", "")
                            , hfSpecPurchase.Value
                        );
                    }
                    // กรณีอัพเดต inv_pr_item
                    else
                    {
                        prDao2.UpdateInvPRItem
                        (
                            hfPrItemID.Value
                            , tbProcureName.Text
                            , ""
                            , ddlPackage.SelectedValue
                            , tbSpecify.Text
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbQuantity.Text.Replace(",", "")
                            , tbTradeDiscountPercent.Text.Replace(",", "")
                            , tbTradeDiscountAmount.Text.Replace(",", "")
                            , ""
                            , ""
                            , tbTotalBeforeVat.Text.Replace(",", "")
                            , tbVatPercent.Text.Replace(",", "")
                            , tbVatAmount.Text.Replace(",", "")
                            , tbNetAmount.Text.Replace(",", "")
                            , hfSpecPurchase.Value
                        );
                    }
                }

                // กรณีเพิ่ม/แก้ไข inv_pr_form2
                for (int i = 0; i < PRCtrl.GVForm2.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVForm2.Rows[i];

                    HiddenField hfPrForm2ID = row.FindControl("hfPrForm2ID") as HiddenField;
                    DropDownList ddlExpense = row.FindControl("ddlExpense") as DropDownList;
                    DropDownList ddlAccExpense = row.FindControl("ddlAccExpense") as DropDownList;
                    TextBox tbPercentAllocate = row.FindControl("tbPercentAllocate") as TextBox;
                    TextBox tbAmountAllocate = row.FindControl("tbAmountAllocate") as TextBox;

                    if (Convert.ToInt32(hfPrForm2ID.Value) < 0)
                    {
                        prDao2.InsertInvPRForm2
                        (
                            PRPOSession.PrID
                            , ddlExpense.SelectedValue
                            , ddlAccExpense.SelectedValue
                            , tbPercentAllocate.Text
                            , tbAmountAllocate.Text.Replace(",", "")
                        );
                    }
                    else
                    {
                        prDao2.UpdateInvPRForm2
                        (
                            hfPrForm2ID.Value
                            , ddlExpense.SelectedValue
                            , ddlAccExpense.SelectedValue
                            , tbPercentAllocate.Text
                            , tbAmountAllocate.Text.Replace(",", "")
                        );
                    }
                }

                // กรณีเพิ่มรายการ inv_pr_attach
                foreach (DataRow row in puft.Table.Rows)
                {
                    if (Convert.ToInt32(row["Id"]) < 0)
                    {
                        string filePath = Server.MapPath(Path.Combine(PRPOPath.PRTmpUpload, PRPOSession.UserID, row["FileName"].ToString()));
                        if (File.Exists(filePath))
                        {
                            File.Move(filePath, Server.MapPath(Path.Combine(PRPOPath.PRUpload, row["FileName"].ToString())));
                            prDao2.InsertInvPRAttach(PRPOSession.PrID, row["FileName"].ToString());
                        }
                    }
                }

                prDao2.CommitTransaction();
            }
            catch (Exception ex)
            {
                prDao2.RollbackTransaction();
                throw ex;
            }
        }

        public override string GetPRCode()
        {
            return base.GetPRCode() + "H-x/";
        }

        public override string GetPRType()
        {
            return "hire";
        }

        public override void IndexChanged()
        {
            PRCtrl.TBObjective.Enabled = true;
            PRCtrl.DDLProject.Enabled = true;

            PRCtrl.PPurchase.Visible = false;
            PRCtrl.PHire.Visible = true;

            PRCtrl.BPrintForm.Enabled = false;
            PRCtrl.CBPrintForm.Checked = false;
            PRCtrl.CBPrintForm.Enabled = false;
            PRCtrl.BProductSelect.Enabled = false;

            PRCtrl.GVForm2.DataSource = null;
            PRCtrl.GVForm2.DataBind();

            PRCtrl.GVHire.DataSource = null;
            PRCtrl.GVHire.DataBind();

            new PRPOHireActualTable();
            new PRPOForm2ActualTable();
            new PRPOForm2DeleteTable();
            new PRPODeleteItemTable();

            PRPOSession.PrPoForm2Binded = false;
        }

        public void SaveGridViewForm2(PRPOForm2ActualTable pfat)
        {
            for (int i = 0; i < PRCtrl.GVForm2.Rows.Count; ++i)
            {
                GridViewRow r = PRCtrl.GVForm2.Rows[i];

                HiddenField hfPoForm2ID = r.FindControl("hfPoForm2ID") as HiddenField;
                HiddenField hfPrForm2ID = r.FindControl("hfPrForm2ID") as HiddenField;

                DropDownList ddlExpense = r.FindControl("ddlExpense") as DropDownList;        // กลุ่มค่าใช้จ่าย
                DropDownList ddlAccExpense = r.FindControl("ddlAccExpense") as DropDownList;  // บัญชี
                TextBox tbPercentAllocate = r.FindControl("tbPercentAllocate") as TextBox;    // อัตราส่วน %
                TextBox tbAmountAllocate = r.FindControl("tbAmountAllocate") as TextBox;      // รวมเงิน

                pfat.SaveItem
                (
                    hfPoForm2ID.Value
                    , hfPrForm2ID.Value
                    , ddlExpense.SelectedValue
                    , ddlAccExpense.SelectedValue
                    , tbPercentAllocate.Text
                    , tbAmountAllocate.Text
                );
            }
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GPlus.UserControls;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.DataAccess;
using System.IO;

namespace GPlus.PRPO.PRPOHelper
{
    public class PRPurchase : PRType
    {
        public PRPurchase(PRControl control)
            : base(control)
        {
        }

        public override void BindGridViewItems()
        {
            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

            SaveGridViewItems();

            if (!string.IsNullOrEmpty(ppft.GetFormType()))  // แบบพิมพ์
            {
                DataTable table = ppft.Table;
                PRCtrl.GVPurchase.DataSource = table;
                PRCtrl.GVPurchase.DataBind();

                if (string.IsNullOrEmpty(ppft.Table.Rows[0]["UnitQuantity"].ToString()))
                {
                    // จำนวนสั่ง
                    TextBox tbUnitQuantity = PRCtrl.GVPurchase.Rows[0].FindControl("tbUnitQuantity") as TextBox;
                    if (table.Rows[0]["FormBorrowType"].ToString() == "1")
                        tbUnitQuantity.Text = table.Rows[0]["BorrowQuantity"].ToString();
                    else if (table.Rows[0]["FormBorrowType"].ToString() == "2")
                        tbUnitQuantity.Text = table.Rows[0]["UnitQuantity"].ToString();
                }

                // หน่วยนับ
                PRCtrl.GVPurchase.Rows[0].Cells[4].Text = table.Rows[0]["PackName"].ToString();
            }
            else
            {
                PRCtrl.GVPurchase.DataSource = ppat.Table;
                PRCtrl.GVPurchase.DataBind();
            }

            if (!string.IsNullOrEmpty(ppft.GetFormType()))
            {
                PRCtrl.BProductSelect.Enabled = false;
                PRCtrl.CBPrintForm.Checked = true;
                PRCtrl.CBPrintForm.Enabled = true;
                PRCtrl.BPrintForm.Enabled = true;
                PRCtrl.CBTradeDiscountType.Enabled = true;
            }
            else if (ppat.Table.Rows.Count > 0)
            {
                PRCtrl.CBPrintForm.Checked = false;
                PRCtrl.CBPrintForm.Enabled = false;
                PRCtrl.BPrintForm.Enabled = false;
                PRCtrl.BProductSelect.Enabled = true;
                PRCtrl.CBTradeDiscountType.Enabled = true;
            }
            else
            {
                PRCtrl.CBPrintForm.Enabled = true;
                PRCtrl.BPrintForm.Enabled = true;
                PRCtrl.BProductSelect.Enabled = true;
                PRCtrl.SetUIsWhenGridViewHasNoRow(PRCtrl.GVPurchase);
            }
        }

        public override void BindPR(string prId, DataTable dtPR)
        {
            base.BindPR(prId, dtPR);
            IndexChanged();
            PRCtrl.RBLPRType.SelectedValue = "1";
            PRCtrl.DDLProject.Enabled = false;
            PRCtrl.TBObjective.Enabled = false;
            new PRPOUploadFileTable();
            new PRPOAttachDeleteTable();
            new PRPODeleteItemTable();
            new PRPOPrintFormDeleteTable();

            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable();
            PRPOPrintFormTable ppft = new PRPOPrintFormTable();
            
            PRDAO2 prDao2 = new PRDAO2();

            DataTable dtPRItem = prDao2.GetPRItem(Convert.ToInt32(prId), GetPRType());
            string formPrintID = dtPRItem.Rows[0]["FormPrintID"].ToString();

            if (dtPRItem.Rows.Count == 1 && !string.IsNullOrEmpty(formPrintID))
            {
                DataTable dtPrintForm = prDao2.GetPrintForm(Convert.ToInt32(formPrintID));

                DataRow r = dtPrintForm.Rows[0];

                string borrowQuantity = (r["BorrowQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowQuantity"]).ToString("0") : "";
                string borrowMonthQuantity = (r["BorrowMonthQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowMonthQuantity"]).ToString("0") : "";
                string borrowFirstQuantity = (r["BorrowFirstQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowFirstQuantity"]).ToString("0") : "";

                string newBorrowDate = string.Format("{0:dd/MM/YYYY}", r["NewBorrowDate"].ToString());

                ppft.AddItem
                (
                    formPrintID
                    , dtPRItem.Rows[0]["PrItemID"].ToString()
                    , ""
                    , r["FormPrintCode"].ToString()
                    , r["FormPrintName"].ToString()
                    , r["FormType"].ToString()
                    , r["Format"].ToString()
                    , r["PaperType"].ToString()
                    , r["PaperGram"].ToString()
                    , r["PaperColor"].ToString()
                    , r["FontColor"].ToString()
                    , r["PrintType"].ToString()
                    , r["BorrowType"].ToString()
                    , newBorrowDate
                    , r["Remark"].ToString()
                    , r["FormBorrowType"].ToString()
                    , borrowQuantity
                    , borrowMonthQuantity
                    , borrowFirstQuantity
                    , dtPRItem.Rows[0]["UnitPrice"].ToString()
                    , r["BorrowUnitID"].ToString()
                    , r["BorrowMonthUnitID"].ToString()
                    , r["IsRequestModify"].ToString()
                    , r["IsFixedContent"].ToString()
                    , r["IsPaper"].ToString()
                    , r["IsFont"].ToString()
                    , r["Remark2"].ToString()
                    , r["RequestModifyDesc"].ToString()
                    , r["SizeDetail"].ToString()
                    , dtPRItem.Rows[0]["InvItemID"].ToString()
                    , dtPRItem.Rows[0]["PackID"].ToString()
                    , dtPRItem.Rows[0]["InvItemCode"].ToString()
                    , dtPRItem.Rows[0]["InvItemName"].ToString()
                    , dtPRItem.Rows[0]["PackName"].ToString()
                    , r["UnitQuantity"].ToString()
                    , dtPRItem.Rows[0]["InvSpecPurchase"].ToString()
                );

                // ดึงค่าจากตาราง PrItem มาใส่ในตาราง PRPOPrintFormTable
                string tradeDiscountPercent = dtPRItem.Rows[0]["TradeDiscountPercent"].ToString();
                string tradeDiscountAmount = dtPRItem.Rows[0]["TradeDiscountAmount"].ToString();
                string totalBeforeVat = dtPRItem.Rows[0]["TotalBeforeVat"].ToString();
                string vatPercent = dtPRItem.Rows[0]["VatPercent"].ToString();
                string vatAmount = dtPRItem.Rows[0]["VatAmount"].ToString();
                string netAmount = dtPRItem.Rows[0]["NetAmount"].ToString();

                if (!string.IsNullOrEmpty(tradeDiscountPercent))
                {
                    if (Convert.ToDecimal(tradeDiscountPercent) == 0)
                        tradeDiscountPercent = "";
                }

                ppft.Table.Rows[0]["TradeDiscountPercent"] = tradeDiscountPercent;
                ppft.Table.Rows[0]["TradeDiscountAmount"] = tradeDiscountAmount;
                ppft.Table.Rows[0]["TotalBeforeVat"] = totalBeforeVat;
                ppft.Table.Rows[0]["VatPercent"] = vatPercent;
                ppft.Table.Rows[0]["VatAmount"] = vatAmount;
                ppft.Table.Rows[0]["NetAmount"] = netAmount;

                ppft.Table.AcceptChanges();
            }

            dtPRItem.Columns.Add("UnitOrder");
            dtPRItem.Columns.Add("PopupType");
            dtPRItem.AcceptChanges();

            // เพิ่มรายการลงตาราง PRPOPurchaseActualTable
            foreach (DataRow row in dtPRItem.Rows)
            {
                string tradeDiscountPercent = row["TradeDiscountPercent"].ToString();
                if (!string.IsNullOrEmpty(tradeDiscountPercent))
                {
                    if (Convert.ToDecimal(tradeDiscountPercent) == 0)
                        tradeDiscountPercent = "";
                }

                ppat.AddItem
                (
                    row["PrItemID"].ToString()
                    , row["InvItemID"].ToString()
                    , row["PackID"].ToString()
                    , row["InvItemCode"].ToString()
                    , row["InvItemName"].ToString()
                    , row["PackName"].ToString()
                    , row["UnitPrice"].ToString()
                    , row["UnitQuantity"].ToString()
                    , row["InvSpecPurchase"].ToString()
                    , tradeDiscountPercent
                    , row["TradeDiscountAmount"].ToString()
                    , row["FormPrintID"].ToString()
                    , row["TotalBeforeVat"].ToString()
                    , row["VatPercent"].ToString()
                    , row["VatAmount"].ToString()
                    , row["NetAmount"].ToString()
                    , row["UnitOrder"].ToString()
                    , row["PrID"].ToString()
                    , row["PrItemID"].ToString()
                    , row["PopupType"].ToString()
                    , ""
                    , ""
                    , "Y"
                );
            }

            BindGridViewItems();
        }

        public override void SaveGridViewItems()
        {
            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);

            for (int i = 0; i < PRCtrl.GVPurchase.Rows.Count; ++i)
            {
                GridViewRow row = PRCtrl.GVPurchase.Rows[i];

                HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
                HiddenField hfSpecPurchase = row.FindControl("hfSpecPurchase") as HiddenField;

                TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                TextBox tbUnitQuantity = row.FindControl("tbUnitQuantity") as TextBox;
                TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;
                TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;

                DataRow r = ppat.FindItem(hfItemID.Value, hfPackID.Value).FirstOrDefault();
                if (r != null)
                {
                    r["UnitPrice"] = tbUnitPrice.Text;
                    r["UnitQuantity"] = tbUnitQuantity.Text;
                    r["TradeDiscountPercent"] = tbTradeDiscountPercent.Text;
                    r["TradeDiscountAmount"] = tbTradeDiscountAmount.Text;
                    r["TotalBeforeVat"] = tbTotalBeforeVat.Text;
                    r["VatPercent"] = tbVatPercent.Text;
                    r["VatAmount"] = tbVatAmount.Text;
                    r["NetAmount"] = tbNetAmount.Text;
                    r["InvSpecPurchase"] = hfSpecPurchase.Value;
                    
                    r.AcceptChanges();
                }
            }
        }

        public override void DeleteGridViewItems()
        {
            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);
            PRPODeleteItemTable pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);

            for (int i = 0; i < PRCtrl.GVPurchase.Rows.Count; ++i)
            {
                GridViewRow row = PRCtrl.GVPurchase.Rows[i];

                CheckBox cbDelete = row.FindControl("cbDelete") as CheckBox;

                if (cbDelete.Checked)
                {
                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;

                    pdit.AddPrItem(hfPrItemID.Value);
                    ppat.DeleteItem(hfItemID.Value, hfPackID.Value);
                }
            }

            if (!string.IsNullOrEmpty(ppft.GetFormType()))
            {
                PRPOPrintFormDeleteTable ppfdt = new PRPOPrintFormDeleteTable(PRPOSession.PrintFormDeleteTable);
                string printFormID = ppft.Table.Rows[0]["FormPrintID"].ToString();
                string prItemID = ppft.Table.Rows[0]["PrItemID"].ToString();
                ppfdt.AddItemByPR(printFormID, prItemID);
                ppft.ClearRow(0);
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

                for (int i = 0; i < PRCtrl.GVPurchase.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVPurchase.Rows[i];

                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;

                    if (string.IsNullOrEmpty(hfPackID.Value))
                    {
                        throw new Exception("กรุณาระบุหน่วยบรรจุของวัสดุอุปกรณ์");
                    }
                }

                Pagebase pb = new Pagebase();

                string prItemId = "";
                string prCode = GetPRCode();

                prId = prDao2.InsertInvPRForm1
                (
                    prCode
                    , "1" // ขอซื้อ
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

                for (int i = 0; i < PRCtrl.GVPurchase.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVPurchase.Rows[i];

                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                    HiddenField hfSpecPurchase = row.FindControl("hfSpecPurchase") as HiddenField;

                    TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbUnitQuantity = row.FindControl("tbUnitQuantity") as TextBox;
                    TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                    TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                    TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                    TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                    TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;
                    TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;

                    if (tbUnitPrice.Text.Trim().Length == 0) tbUnitPrice.Text = "0";
                    if (tbUnitQuantity.Text.Trim().Length == 0) tbUnitQuantity.Text = "0";
                    if (tbTradeDiscountPercent.Text.Trim().Length == 0) tbTradeDiscountPercent.Text = "0";
                    if (tbTradeDiscountAmount.Text.Trim().Length == 0) tbTradeDiscountAmount.Text = "0";
                    if (tbVatPercent.Text.Trim().Length == 0) tbVatPercent.Text = "0";
                    if (tbVatAmount.Text.Trim().Length == 0) tbVatAmount.Text = "0";
                    if (tbNetAmount.Text.Trim().Length == 0) tbNetAmount.Text = "0";
                    if (tbTotalBeforeVat.Text.Trim().Length == 0) tbTotalBeforeVat.Text = "0";

                    prItemId = prDao2.InsertInvPRItems
                    (
                        prId
                        , hfItemID.Value
                        , ""
                        , ""
                        , hfPackID.Value
                        , tbUnitPrice.Text.Replace(",", "")
                        , tbUnitQuantity.Text.Replace(",", "")
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

                PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

                if (!string.IsNullOrEmpty(ppft.GetFormType()) && PRCtrl.CBPrintForm.Checked)
                {
                    DataRow row = ppft.Table.Rows[0];

                    string formPrintCode = row["FormPrintCode"].ToString();
                    string formPrintName = row["FormPrintName"].ToString();
                    string formType = row["FormType"].ToString();
                    string format = row["Format"].ToString();
                    string paperType = row["PaperType"].ToString();
                    string paperColor = row["PaperColor"].ToString();
                    string paperGram = row["PaperGram"].ToString();
                    string fontColor = row["FontColor"].ToString();
                    string printType = row["PrintType"].ToString();
                    string borrowType = row["BorrowType"].ToString();
                    DateTime newBorrowDate = row["NewBorrowDate"].ToString().Trim().Length > 0 ? Convert.ToDateTime(row["NewBorrowDate"].ToString()) : DateTime.MinValue;
                    string remark = row["Remark"].ToString();
                    string formBorrowType = row["FormBorrowType"].ToString();
                    string borrowQuantity = row["BorrowQuantity"].ToString();
                    string borrowMonthQuantity = row["BorrowMonthQuantity"].ToString();
                    string borrowFirstQuantity = row["BorrowFirstQuantity"].ToString();
                    string borrowUnitID = row["BorrowUnitID"].ToString();
                    string borrowMonthUnitID = row["BorrowMonthUnitID"].ToString();
                    string isRequestModify = row["IsRequestModify"].ToString();
                    string isFixedContent = row["IsFixedContent"].ToString();
                    string isPaper = row["IsPaper"].ToString();
                    string isFont = row["IsFont"].ToString();
                    string remark2 = row["Remark2"].ToString();
                    string requestModifyDesc = row["RequestModifyDesc"].ToString();
                    string sizeDetail = row["SizeDetail"].ToString();

                    TextBox tbUnitQuantity = PRCtrl.GVPurchase.Rows[0].FindControl("tbUnitQuantity") as TextBox;
                    int unitQuantity = tbUnitQuantity.Text.Trim().Length > 0 ? Convert.ToInt32(tbUnitQuantity.Text) : 0;

                    if (formBorrowType == "1")
                    {
                        borrowMonthQuantity = "";
                        borrowFirstQuantity = "";
                    }
                    else if (formBorrowType == "2")
                    {
                        borrowQuantity = "";
                    }

                    string printFormID = prDao2.InsertInvPRPOFormPrint
                    (
                        prId
                        , ""
                        , formPrintCode
                        , formPrintName
                        , formType
                        , format
                        , paperType
                        , paperColor
                        , paperGram
                        , fontColor
                        , printType
                        , borrowType
                        , newBorrowDate
                        , remark
                        , formBorrowType
                        , borrowQuantity
                        , borrowMonthQuantity
                        , borrowFirstQuantity
                        , borrowUnitID
                        , borrowMonthUnitID
                        , isRequestModify
                        , isFixedContent
                        , isPaper
                        , isFont
                        , remark2
                        , unitQuantity
                        , requestModifyDesc
                        , sizeDetail
                    );

                    prDao2.UpdatePRItem(Convert.ToInt32(printFormID), Convert.ToInt32(prItemId));
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

                string prItemId = "";

                PRPODeleteItemTable     pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);
                PRPOAttachDeleteTable   padt = new PRPOAttachDeleteTable(PRPOSession.AttachDeleteTable);
                PRPOUploadFileTable     puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);

                // ลบรายการ inv_pr_items
                foreach (DataRow r in pdit.Table.Rows)
                {
                    string prItemID = r["PrItemID"].ToString();
                    prDao2.DeleteInvPRItem(prItemID);
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

                // ลบรายการที่เป็นแบบฟอร์ม
                PRPOPrintFormDeleteTable ppfdt = new PRPOPrintFormDeleteTable(PRPOSession.PrintFormDeleteTable);
                if (ppfdt.Table.Rows.Count > 0)
                {
                    string printFormID = ppfdt.Table.Rows[0]["PrintFormID"].ToString();
                    string prItemID = ppfdt.Table.Rows[0]["PrItemID"].ToString();

                    prDao2.DeletePrintForm(Convert.ToInt32(printFormID));
                    prDao2.DeleteInvPRItem(prItemID);
                }
                // ลบรายการที่มาจาก รายการสินค้า
                else
                {
                    for (int i = 0; i < pdit.Table.Rows.Count; ++i)
                    {
                        string prItemID = pdit.Table.Rows[i]["PrItemID"].ToString();
                        prDao2.DeleteInvPRItem(prItemID);
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

                for (int i = 0; i < PRCtrl.GVPurchase.Rows.Count; ++i)
                {
                    GridViewRow row = PRCtrl.GVPurchase.Rows[i];

                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                    HiddenField hfSpecPurchase = row.FindControl("hfSpecPurchase") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;

                    TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbUnitQuantity = row.FindControl("tbUnitQuantity") as TextBox;
                    TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                    TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                    TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                    TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                    TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;
                    TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;

                    if (tbUnitPrice.Text.Trim().Length == 0) tbUnitPrice.Text = "0";
                    if (tbUnitQuantity.Text.Trim().Length == 0) tbUnitQuantity.Text = "0";
                    if (tbTradeDiscountPercent.Text.Trim().Length == 0) tbTradeDiscountPercent.Text = "0";
                    if (tbTradeDiscountAmount.Text.Trim().Length == 0) tbTradeDiscountAmount.Text = "0";
                    if (tbVatPercent.Text.Trim().Length == 0) tbVatPercent.Text = "0";
                    if (tbVatAmount.Text.Trim().Length == 0) tbVatAmount.Text = "0";
                    if (tbNetAmount.Text.Trim().Length == 0) tbNetAmount.Text = "0";
                    if (tbTotalBeforeVat.Text.Trim().Length == 0) tbTotalBeforeVat.Text = "0";

                    if (string.IsNullOrEmpty(hfPrItemID.Value))
                    {
                        prItemId = prDao2.InsertInvPRItems
                        (
                            PRPOSession.PrID
                            , hfItemID.Value
                            , ""
                            , ""
                            , hfPackID.Value
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbUnitQuantity.Text.Replace(",", "")
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
                    else
                    {
                        prDao2.UpdateInvPRItem
                        (
                            hfPrItemID.Value
                            , ""
                            , hfItemID.Value
                            , hfPackID.Value
                            , ""
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbUnitQuantity.Text.Replace(",", "")
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

                // แบบพิมพ์
                PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);
                if (!string.IsNullOrEmpty(ppft.GetFormType()) && PRCtrl.CBPrintForm.Checked)
                {
                    DataRow row = ppft.Table.Rows[0];

                    string formPrintID = row["FormPrintID"].ToString();
                    string formPrintCode = row["FormPrintCode"].ToString();
                    string formPrintName = row["FormPrintName"].ToString();
                    string formType = row["FormType"].ToString();
                    string format = row["Format"].ToString();
                    string paperType = row["PaperType"].ToString();
                    string paperColor = row["PaperColor"].ToString();
                    string paperGram = row["PaperGram"].ToString();
                    string fontColor = row["FontColor"].ToString();
                    string printType = row["PrintType"].ToString();
                    string borrowType = row["BorrowType"].ToString();
                    DateTime newBorrowDate = row["NewBorrowDate"].ToString().Trim().Length > 0 ? Convert.ToDateTime(row["NewBorrowDate"].ToString()) : DateTime.MinValue;
                    string remark = row["Remark"].ToString();
                    string formBorrowType = row["FormBorrowType"].ToString();
                    string borrowQuantity = row["BorrowQuantity"].ToString();
                    string borrowMonthQuantity = row["BorrowMonthQuantity"].ToString();
                    string borrowFirstQuantity = row["BorrowFirstQuantity"].ToString();
                    string borrowUnitID = row["BorrowUnitID"].ToString();
                    string borrowMonthUnitID = row["BorrowMonthUnitID"].ToString();
                    string isRequestModify = row["IsRequestModify"].ToString();
                    string isFixedContent = row["IsFixedContent"].ToString();
                    string isPaper = row["IsPaper"].ToString();
                    string isFont = row["IsFont"].ToString();
                    string remark2 = row["Remark2"].ToString();
                    string requestModifyDesc = row["RequestModifyDesc"].ToString();
                    string sizeDetail = row["SizeDetail"].ToString();

                    TextBox tbUnitQuantity = PRCtrl.GVPurchase.Rows[0].FindControl("tbUnitQuantity") as TextBox;
                    int unitQuantity = tbUnitQuantity.Text.Trim().Length > 0 ? Convert.ToInt32(tbUnitQuantity.Text) : 0;

                    if (formBorrowType == "1")
                    {
                        borrowMonthQuantity = "";
                        borrowFirstQuantity = "";
                    }
                    else if (formBorrowType == "2")
                    {
                        borrowQuantity = "";
                    }

                    if (string.IsNullOrEmpty(row["PrItemID"].ToString()))
                    {
                        string printFormID = prDao2.InsertInvPRPOFormPrint
                        (
                            PRPOSession.PrID
                            , ""
                            , formPrintCode
                            , formPrintName
                            , formType
                            , format
                            , paperType
                            , paperColor
                            , paperGram
                            , fontColor
                            , printType
                            , borrowType
                            , newBorrowDate
                            , remark
                            , formBorrowType
                            , borrowQuantity
                            , borrowMonthQuantity
                            , borrowFirstQuantity
                            , borrowUnitID
                            , borrowMonthUnitID
                            , isRequestModify
                            , isFixedContent
                            , isPaper
                            , isFont
                            , remark2
                            , unitQuantity
                            , requestModifyDesc
                            , sizeDetail
                        );

                        prDao2.UpdatePRItem(Convert.ToInt32(printFormID), Convert.ToInt32(prItemId));
                    }
                    else
                    {
                        prDao2.UpdatePrintForm
                        (
                            formPrintID
                            , formPrintCode
                            , formPrintName
                            , formType
                            , format
                            , paperType
                            , paperColor
                            , paperGram
                            , fontColor
                            , printType
                            , borrowType
                            , newBorrowDate
                            , remark
                            , formBorrowType
                            , borrowQuantity
                            , borrowMonthQuantity
                            , borrowFirstQuantity
                            , borrowUnitID
                            , borrowMonthUnitID
                            , isRequestModify
                            , isFixedContent
                            , isPaper
                            , isFont
                            , remark2
                            , unitQuantity
                            , requestModifyDesc
                            , sizeDetail
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
            return base.GetPRCode() + "P-x/";
        }

        public override string GetPRType()
        {
            return "purchase";
        }

        public override void IndexChanged()
        {
            PRCtrl.TBObjective.Enabled = false;
            PRCtrl.DDLProject.Enabled = false;
            PRCtrl.PHire.Visible = false;
            PRCtrl.PPurchase.Visible = true;
            PRCtrl.BPrintForm.Enabled = true;
            PRCtrl.CBPrintForm.Enabled = true;
            PRCtrl.CBPrintForm.Checked = false;
            PRCtrl.BProductSelect.Enabled = true;

            PRCtrl.GVPurchase.DataSource = null;
            PRCtrl.GVPurchase.DataBind();

            new PRPOPurchaseActualTable();
            new PRPOPrintFormTable();
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GPlus.UserControls;
using System.Data;

using GPlus.DataAccess;
using System.Web.UI.WebControls;
using System.IO;

namespace GPlus.PRPO.PRPOHelper
{
    // สั่งจ้าง
    public class POHire : POType
    {
        public POHire(POControl control) : base(control) { }

        public override void BindPO(string poID, DataTable dtPO)
        {
            if (dtPO.Rows.Count < 1) return;

            base.BindPO(poID, dtPO);

            IndexChanged();

            POCtrl.RBLPOType.SelectedValue = "2";
            POCtrl.TBObjective.Text = dtPO.Rows[0]["Objective"].ToString();
            POCtrl.DDLProject.SelectedValue = dtPO.Rows[0]["Project_ID"].ToString();
            POCtrl.BDeletePO.Visible = true;

            DataTable dtPOItem = new PODAO2().GetPOItem(Convert.ToInt32(poID), GetPOType());

            PRPOHireActualTable phat = new PRPOHireActualTable();
            new PRPOForm2DeleteTable();
            new PRPODeleteItemTable();
            new PRPOAttachDeleteTable();
            new PRPOUploadFileTable();

            foreach (DataRow row in dtPOItem.Rows)
            {
                string poItemID = row["POItemID"].ToString();
                string packID = row["PackID"].ToString();
                string unitPrice = row["UnitPrice"].ToString();
                string unitQuantity = row["UnitQuantity"].ToString();
                string tradeDiscountPercent = row["TradeDiscountPercent"].ToString();
                string tradeDiscountAmount = row["TradeDiscountAmount"].ToString();
                string totalBeforeVat = row["TotalBeforeVat"].ToString();
                string vatPercent = row["VatPercent"].ToString();
                string vatAmount = row["VatAmount"].ToString();
                string netAmount = row["NetAmount"].ToString();
                string prID = row["PrID"].ToString();
                string prItemID = row["PrItemID"].ToString();
                string procureName = row["ProcureName"].ToString();
                string specify = row["Specify"].ToString();

                if (!string.IsNullOrEmpty(tradeDiscountPercent))
                {
                    if (Convert.ToDecimal(tradeDiscountPercent) == 0)
                        tradeDiscountPercent = "";
                }

                phat.AddItem
                (
                    poItemID
                    , ""
                    , packID
                    , ""
                    , ""
                    , ""
                    , unitPrice
                    , unitQuantity
                    , ""
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

            POCtrl.GVHire.DataSource = phat.Table;
            POCtrl.GVHire.DataBind();

            POCtrl.PHire.Visible = true;
            POCtrl.PPurchase.Visible = false;

            BindGridViewForm2(poID);
        }

        public override void BindGridviewItems()
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);

            DataRow[] rows = null;

            SaveGridViewItems();

            string prItemIds = phat.GetPrItemIdsThatGroupedAsN();
            if (prItemIds != null)
            {
                DataTable dtPr = GetPrItems(prItemIds);

                foreach (DataRow r in dtPr.Rows)
                {
                    //string invItemID = r["Inv_ItemID"].ToString();
                    string packID = r["Pack_ID"].ToString();
                    string prID = r["PR_ID"].ToString();
                    string prItemID = r["PRItem_ID"].ToString();
                    string unitPrice = r["Unit_Price"].ToString();
                    string unitQuantity = r["Unit_Quantity"].ToString();
                    //string invSpecPurchase = r["Inv_SpecPurchase"].ToString();
                    string tradeDiscountPercent = r["TradeDiscount_Percent"].ToString();
                    string tradeDiscountAmount = r["TradeDiscount_Amount"].ToString();
                    string totalBeforeVat = r["Total_before_Vat"].ToString();
                    string vatPercent = r["Vat"].ToString();
                    string vatAmount = r["Vat_Amount"].ToString();
                    string netAmount = r["Net_Amount"].ToString();
                    string procureName = r["Procure_Name"].ToString();
                    string specify = r["Specify"].ToString();

                    rows = phat.FindItem("", packID, prID, prItemID);

                    // อัพเดตข้อมูลในตาราง PRPOHireActualTable
                    if (rows.Length > 0)
                    {
                        phat.UpdateItem
                        (
                            rows[0],
                            "",//invItemID,
                            packID,
                            "",
                            rows[0]["InvItemName"].ToString(),
                            rows[0]["PackName"].ToString(),
                            unitPrice,
                            unitQuantity,
                            "", //invSpecPurchase,
                            tradeDiscountPercent,
                            tradeDiscountAmount,
                            "",
                            totalBeforeVat,
                            vatPercent,
                            vatAmount,
                            netAmount,
                            "",
                            prID,
                            prItemID,
                            PRPOPopup.PR,
                            procureName,
                            specify,
                            "Y"
                        );
                    }
                }
            }

            POCtrl.GVHire.DataSource = phat.Table;
            POCtrl.GVHire.DataBind();

            POCtrl.PHire.Visible = true;
            POCtrl.PPurchase.Visible = false;
        }

        private void BindGridViewForm2(string poID)
        {
            DataTable dtPOForm2 = new PODAO2().GetPOForm2(poID);

            PRPOForm2ActualTable pfat = new PRPOForm2ActualTable();

            foreach (DataRow r in dtPOForm2.Rows)
            {
                string poForm2ID = r["PO_Form2_ID"].ToString();
                string expenseID = r["Expense_ID"].ToString();
                string accExpenseID = r["AccExpense_ID"].ToString();
                string percentAllocate = string.Format("{0:0}", r["Percent_Allocate"]);
                string amountAllocate = r["Amount_Allocate"].ToString();

                pfat.AddItem(poForm2ID, "", expenseID, accExpenseID, percentAllocate, amountAllocate);
            }

            POCtrl.GVForm2.DataSource = pfat.Table;
            POCtrl.GVForm2.DataBind();
        }

        private DataTable GetPrItems(string prItemIds)
        {
            return new DatabaseHelper().ExecuteQuery(
             "SELECT"
           + " PRItem_ID, PR_ID, Inv_ItemID, Pack_ID, Procure_Name, Specify, Unit_Price, Unit_Quantity, TradeDiscount_Percent, TradeDiscount_Amount, "
           + " Total_before_Vat, Vat, Vat_Amount, Net_Amount, Inv_SpecPurchase "
           + "FROM Inv_PR_Items "
           + "WHERE PRItem_ID IN (" + prItemIds + ");").Tables[0];
        }

        private void SaveGridViewItems()
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);

            GridView gv = POCtrl.GVHire;

            for (int i = 0; i < gv.Rows.Count; ++i)
            {
                HiddenField hfItemID = gv.Rows[i].FindControl("hfItemID") as HiddenField;
                HiddenField hfPackID = gv.Rows[i].FindControl("hfPackID") as HiddenField;
                HiddenField hfPrID = gv.Rows[i].FindControl("hfPrID") as HiddenField;
                HiddenField hfPrItemID = gv.Rows[i].FindControl("hfPrItemID") as HiddenField;

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

                DataRow[] rows = phat.FindItem(hfItemID.Value, hfPackID.Value, hfPrID.Value, hfPrItemID.Value);
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
                            rows[0]["InvSpecPurchase"].ToString(),
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
                            PRPOPopup.PR,
                            tbProcureName.Text,
                            tbSpecify.Text,
                            "Y"
                        );
                }
            }
        }

        public override void DeleteGridviewItems()
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);
            PRPODeleteItemTable pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);

            for (int i = 0; i < POCtrl.GVHire.Rows.Count; ++i)
            {
                CheckBox cbDelete = POCtrl.GVHire.Rows[i].FindControl("cbDelete") as CheckBox;

                if (cbDelete.Checked)
                {
                    GridViewRow row = POCtrl.GVHire.Rows[i];

                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
                    HiddenField hfPoItemID = row.FindControl("hfPoItemID") as HiddenField;

                    pdit.AddPoItem(hfPoItemID.Value, phat);

                    phat.DeleteItem(hfPrID.Value, hfPrItemID.Value);
                }
            }

            BindGridviewItems();

            if (POCtrl.GVHire.Rows.Count == 0)
            {
                PRPOForm2ActualTable pfat = new PRPOForm2ActualTable(PRPOSession.Form2Table);

                for (int i = 0; i < POCtrl.GVForm2.Rows.Count; ++i)
                {
                    GridViewRow r = POCtrl.GVForm2.Rows[i];

                    HiddenField hfPoForm2ID = r.FindControl("hfPoForm2ID") as HiddenField;
                    HiddenField hfPrForm2ID = r.FindControl("hfPrForm2ID") as HiddenField;

                    pfat.DeleteItem(hfPoForm2ID.Value, hfPrForm2ID.Value);
                }

                POCtrl.GVForm2.DataSource = pfat.Table;
                POCtrl.GVForm2.DataBind();
            }
        }

        public override void IndexChanged()
        {
            POCtrl.RBLItem.Items[0].Enabled = false;
            POCtrl.RBLItem.Items[1].Enabled = false;
            POCtrl.LReorderPoint.Visible = false;
            POCtrl.CCReorderPointDate.Visible = false;
            POCtrl.CCReorderPointDate.Text = "";

            POCtrl.RBLItem.SelectedValue = "3";

            POCtrl.TBObjective.Enabled = true;
            POCtrl.DDLProject.Enabled = true;

            POCtrl.PPurchase.Visible = false;
            POCtrl.PHire.Visible = true;

            POCtrl.BPrintForm.Enabled = false;
            POCtrl.CBPrintForm.Checked = false;
            POCtrl.CBPrintForm.Enabled = false;

            POCtrl.GVForm2.DataSource = null;
            POCtrl.GVForm2.DataBind();

            PRPOSession.InitializePOHire();
            PRPOSession.PrPoForm2Binded = false;
        }

        public override string Save()
        {
            PODAO2 poDao2 = new PODAO2();

            string poCode = GetPOCode();
            string poID = "";

            try
            {
                poDao2.BeginTransaction();

                if (POCtrl.RBLVatType.SelectedValue == "1")
                {
                    POCtrl.TBVat.Text = "";
                }


                // เพิ่ม Inv_PO_Form1
                poID = poDao2.InsertInvPoForm1
                (
                  poCode                                        // รหัส PO
                , POCtrl.RBLPOType.SelectedValue                // ชนิด PO; 1 = สั่งซื้อ, 2 = สั่งจ้าง
                , POCtrl.RBLTypeAsset.SelectedValue             // ชนิด Asset; 1 = Stock, 2 = Asset
                , POCtrl.CCPODate.Value                         // วันเวลาที่สั่งซื้อ
                , POCtrl.OrgID                                  // รหัสหน่วยงาน
                , POCtrl.TBObjective.Text                       // วัตถุประสงค์
                , POCtrl.DDLProject.SelectedValue               // โครงการ        (NULL)
                , "1"                                           // รายการสินค้ามาจากไหน (ไม่สำคัญมากเพราะรายการในตัวใบผสมกันได้) 
                , "0"                                           // มีแบบพิมพ์; 1 = มี, 0 = ไม่มี
                , POCtrl.DDLSupplier.SelectedValue              // Supplier ID
                , POCtrl.TBQuotationCode1.Text                  // QuotationCode1
                , POCtrl.CCQuotationDate1.Value                 // QuotationDate1 (NULL)
                , POCtrl.TBQuotationCode2.Text                  // QuotationCode2
                , POCtrl.CCQuotationDate2.Value                 // QuatationDate2 (NULL)
                , POCtrl.CBIsPayCheque.Checked ? "1" : "0"      // IsPayCheck
                , POCtrl.CBIsPayCash.Checked ? "1" : "0"        // IsPayCash
                , POCtrl.TBCreditTermDay.Text                   // Credit term day
                , POCtrl.CCShippingDate.Value                   // วันที่ส่งของ (NULL)
                , POCtrl.TBShippingAt.Text                      // สถานที่ส่งของ (EMPTY)
                , POCtrl.CBTradeDiscount.Checked ? "1" : "0"    // ส่วนลดการค้า
                , POCtrl.RBLTradeDiscountType.SelectedValue     // ประเภทส่วนลดการค้า ; 0 = ส่วนลดรวม , 1 = ส่วนลดแต่ละรายการ (EMPTY เมื่อผู้ใช้ไม่เลือก)
                , POCtrl.TBTradeDiscountPercent.Text            // ส่วนลด % (NULL)
                , POCtrl.TBTradeDiscountAmount.Text             // ส่วนลด บาท (NULL)
                , "0"                                           // CashDiscountType  (ALWAYS 0)  
                , ""                                            // CashDiscountPercent (ALWAYS NULL)
                , ""                                            // CashDiscountAmount (ALWAYS NULL)
                , POCtrl.RBLVatType.SelectedValue               // 0 = Vat รวม, 1 = vat แต่ละรายการ
                , POCtrl.TBVat.Text                             // ภาษี %
                , POCtrl.RBLVatUnitType.SelectedValue           // 1 = Include Vat, 0 = Exclude Vat
                , POCtrl.UserName                               // CreateBy
                , POCtrl.TBTotal.Text.Replace(",", "")          // ราคารวม
                , POCtrl.TBTotalDiscount.Text.Replace(",", "")  // ส่วนลด
                , POCtrl.TBTotalBeforeVat.Text.Replace(",", "") // ราคารวมก่อนภาษี
                , POCtrl.TBTotalVat.Text.Replace(",", "")       // ภาษีมูลค่าเพิ่ม
                , POCtrl.TBGrandTotal.Text.Replace(",", "")     // จำนวนเงินรวมทั้งสิ้น
                , POCtrl.TBContractName.Text
                , POCtrl.CCReorderPointDate.Value               // วันเวลา ReorderPoint
                );

                // เพิ่ม Inv_PO_Items และเพิ่ม Inv_PO_PR
                for (int i = 0; i < POCtrl.GVHire.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVHire.Rows[i];

                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;

                    TextBox tbProcureName = row.FindControl("tbProcureName") as TextBox;
                    DropDownList ddlPackage = row.FindControl("ddlPackage") as DropDownList;
                    TextBox tbQuantity = row.FindControl("tbQuantity") as TextBox;
                    TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                    TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                    TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;
                    TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                    TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                    TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;
                    TextBox tbSpecify = row.FindControl("tbSpecify") as TextBox;

                    if (tbUnitPrice.Text.Trim().Length == 0) tbUnitPrice.Text = "0";
                    if (tbQuantity.Text.Trim().Length == 0) tbQuantity.Text = "0";
                    if (tbTradeDiscountPercent.Text.Trim().Length == 0) tbTradeDiscountPercent.Text = "0";
                    if (tbTradeDiscountAmount.Text.Trim().Length == 0) tbTradeDiscountAmount.Text = "0";
                    if (tbVatPercent.Text.Trim().Length == 0) tbVatPercent.Text = "0";
                    if (tbVatAmount.Text.Trim().Length == 0) tbVatAmount.Text = "0";
                    if (tbNetAmount.Text.Trim().Length == 0) tbNetAmount.Text = "0";

                    string poItemID = poDao2.InsertInvPOItem
                    (
                        poID
                        , null
                        , tbProcureName.Text
                        , tbSpecify.Text
                        , ddlPackage.SelectedValue
                        , tbUnitPrice.Text.Replace(",", "")
                        , tbQuantity.Text.Replace(",", "")
                        , tbTradeDiscountPercent.Text.Replace(",", "")
                        , tbTradeDiscountAmount.Text.Replace(",", "")
                        , "0"
                        , "0"
                        , tbTotalBeforeVat.Text.Replace(",", "")
                        , tbVatPercent.Text.Replace(",", "")
                        , tbVatAmount.Text.Replace(",", "")
                        , tbNetAmount.Text.Replace(",", "")
                        , ""
                    );

                    poDao2.InsertInvPOPR
                    (
                        Convert.ToInt32(poID)
                        , Convert.ToInt32(poItemID)
                        , Convert.ToInt32(hfPrID.Value)
                        , Convert.ToInt32(hfPrItemID.Value)
                    );

                    poDao2.UpdatePrItemStatus(Convert.ToInt32(hfPrItemID.Value), "1");
                }

                UpdatePrForm1Status(poDao2, Convert.ToInt32(poID));

                // เพิ่ม Inv_PO_Form2
                for (int i = 0; i < POCtrl.GVForm2.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVForm2.Rows[i];

                    DropDownList ddlExpense = row.FindControl("ddlExpense") as DropDownList;
                    DropDownList ddlAccExpense = row.FindControl("ddlAccExpense") as DropDownList;
                    TextBox tbPercentAllocate = row.FindControl("tbPercentAllocate") as TextBox;
                    TextBox tbAmountAllocate = row.FindControl("tbAmountAllocate") as TextBox;

                    poDao2.InsertInvPOForm2
                    (
                        poID
                        , ddlExpense.SelectedValue
                        , ddlAccExpense.SelectedValue
                        , tbPercentAllocate.Text
                        , tbAmountAllocate.Text.Replace(",", "")
                    );
                }

                if (!string.IsNullOrEmpty(POCtrl.TBPaymentNo.Text))
                {
                    poDao2.UpdatePOForm1PaymentNo(Convert.ToInt32(poID), POCtrl.TBPaymentNo.Text);
                }

                PRPOUploadFileTable puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);

                foreach (DataRow row in puft.Table.Rows)
                {
                    string filePath = Server.MapPath(Path.Combine(PRPOPath.POTmpUpload, PRPOSession.UserID, row["FileName"].ToString()));
                    if (File.Exists(filePath))
                    {
                        File.Move(filePath, Server.MapPath(Path.Combine(PRPOPath.POUpload, row["FileName"].ToString())));
                        poDao2.InsertInvPOAttach(poID, row["FileName"].ToString());
                    }
                }

                poDao2.CommitTransaction();
            }
            catch (Exception ex)
            {
                poDao2.RollbackTransaction();
                throw ex;
            }

            return poID;
        }

        public override void Update()
        {
            PODAO2 poDao2 = new PODAO2();

            try
            {
                poDao2.BeginTransaction();

                if ((POCtrl.RBLTradeDiscountType.SelectedIndex == 1)
                    || (POCtrl.CBTradeDiscount.Checked == false))
                {
                    POCtrl.TBTradeDiscountPercent.Text = "";
                    POCtrl.TBTradeDiscountAmount.Text = "";
                }

                if (POCtrl.RBLVatType.SelectedValue == "1")
                {
                    POCtrl.TBVat.Text = "";
                }

                // ลบรายการที่เลือก
                PRPODeleteItemTable pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);
                foreach (DataRow r in pdit.Table.Rows)
                {
                    string poItemID = r["PoItemID"].ToString();
                    string prItemID = r["PrItemID"].ToString();
                    string prID = r["PrID"].ToString();

                    poDao2.DeletePOPR
                    (
                        Convert.ToInt32(PRPOSession.PoID)
                        , Convert.ToInt32(prID)
                        , Convert.ToInt32(prItemID)
                        , Convert.ToInt32(poItemID)
                    );

                    poDao2.DeletePOItem(Convert.ToInt32(poItemID));
                }

                PRPOForm2DeleteTable pfdt = new PRPOForm2DeleteTable(PRPOSession.Form2DeleteTable);
                foreach (DataRow r in pfdt.Table.Rows)
                {
                    string poForm2ID = r["PoPrForm2ID"].ToString();
                    poDao2.DeletePOForm2(poForm2ID);
                }

                // ลบรายการ inv_po_attach
                PRPOAttachDeleteTable padt = new PRPOAttachDeleteTable(PRPOSession.AttachDeleteTable);
                foreach (DataRow r in padt.Table.Rows)
                {
                    string poAttachID = r["PRPOAttachID"].ToString();
                    string fileName = r["FileName"].ToString();

                    poDao2.DeletePOAttach(poAttachID);
                    // ลบไฟล์ใน ~/Uploads/PO
                    string filePath = Path.Combine(Server.MapPath(PRPOPath.POUpload), fileName);
                    if (File.Exists(filePath))
                    {
                        try { File.Delete(filePath); }
                        catch (Exception) { }
                    }
                }

                poDao2.UpdatePOForm1
                (
                    PRPOSession.PoID
                    , "2"
                    , POCtrl.RBLTypeAsset.SelectedValue
                    , POCtrl.CCPODate.Value
                    , POCtrl.OrgID
                    , POCtrl.TBObjective.Text
                    , POCtrl.DDLProject.SelectedValue
                    , "1"
                    , POCtrl.CBPrintForm.Checked ? "1" : "0"
                    , POCtrl.DDLSupplier.SelectedValue
                    , POCtrl.TBQuotationCode1.Text
                    , POCtrl.CCQuotationDate1.Value
                    , POCtrl.TBQuotationCode2.Text
                    , POCtrl.CCQuotationDate2.Value
                    , POCtrl.CBIsPayCheque.Checked ? "1" : "0"
                    , POCtrl.CBIsPayCash.Checked ? "1" : "0"
                    , POCtrl.TBCreditTermDay.Text
                    , POCtrl.CCShippingDate.Value
                    , POCtrl.TBShippingAt.Text
                    , POCtrl.CBTradeDiscount.Checked ? "1" : "0"
                    , POCtrl.RBLTradeDiscountType.SelectedValue
                    , POCtrl.TBTradeDiscountPercent.Text
                    , POCtrl.TBTradeDiscountAmount.Text
                    , "0"
                    , ""
                    , ""
                    , POCtrl.RBLVatType.SelectedValue
                    , POCtrl.TBVat.Text
                    , POCtrl.RBLVatUnitType.SelectedValue
                    , POCtrl.UserName
                    , POCtrl.TBTotal.Text.Replace(",", "")
                    , POCtrl.TBTotalDiscount.Text.Replace(",", "")
                    , POCtrl.TBTotalBeforeVat.Text.Replace(",", "")
                    , POCtrl.TBTotalVat.Text.Replace(",", "")
                    , POCtrl.TBGrandTotal.Text.Replace(",", "")
                    , POCtrl.TBContractName.Text
                    , POCtrl.CCReorderPointDate.Value               // วันเวลา ReorderPoint
                );

                poDao2.UpdatePOForm1Status(Convert.ToInt32(PRPOSession.PoID), "1");

                for (int i = 0; i < POCtrl.GVForm2.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVForm2.Rows[i];

                    HiddenField hfPoForm2ID = row.FindControl("hfPoForm2ID") as HiddenField;
                    DropDownList ddlExpense = row.FindControl("ddlExpense") as DropDownList;
                    DropDownList ddlAccExpense = row.FindControl("ddlAccExpense") as DropDownList;
                    TextBox tbPercentAllocate = row.FindControl("tbPercentAllocate") as TextBox;
                    TextBox tbAmountAllocate = row.FindControl("tbAmountAllocate") as TextBox;

                    if (Convert.ToInt32(hfPoForm2ID.Value) < 0)
                    {
                        poDao2.InsertInvPOForm2
                        (
                            PRPOSession.PoID
                            , ddlExpense.SelectedValue
                            , ddlAccExpense.SelectedValue
                            , tbPercentAllocate.Text
                            , tbAmountAllocate.Text.Replace(",", "")
                        );
                    }
                    else
                    {
                        poDao2.UpdatePOForm2
                        (
                            hfPoForm2ID.Value
                            , ddlExpense.SelectedValue
                            , ddlAccExpense.SelectedValue
                            , tbPercentAllocate.Text
                            , tbAmountAllocate.Text.Replace(",", "")
                        );
                    }
                }

                for (int i = 0; i < POCtrl.GVHire.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVHire.Rows[i];

                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
                    HiddenField hfPoItemID = row.FindControl("hfPoItemID") as HiddenField;

                    TextBox tbProcureName = row.FindControl("tbProcureName") as TextBox;
                    DropDownList ddlPackage = row.FindControl("ddlPackage") as DropDownList;
                    TextBox tbQuantity = row.FindControl("tbQuantity") as TextBox;
                    TextBox tbUnitPrice = row.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbTradeDiscountPercent = row.FindControl("tbTradeDiscountPercent") as TextBox;
                    TextBox tbTradeDiscountAmount = row.FindControl("tbTradeDiscountAmount") as TextBox;
                    TextBox tbTotalBeforeVat = row.FindControl("tbTotalBeforeVat") as TextBox;
                    TextBox tbVatPercent = row.FindControl("tbVatPercent") as TextBox;
                    TextBox tbVatAmount = row.FindControl("tbVatAmount") as TextBox;
                    TextBox tbNetAmount = row.FindControl("tbNetAmount") as TextBox;
                    TextBox tbSpecify = row.FindControl("tbSpecify") as TextBox;

                    if (tbUnitPrice.Text.Trim().Length == 0) tbUnitPrice.Text = "0";
                    if (tbQuantity.Text.Trim().Length == 0) tbQuantity.Text = "0";
                    if (tbTradeDiscountPercent.Text.Trim().Length == 0) tbTradeDiscountPercent.Text = "0";
                    if (tbTradeDiscountAmount.Text.Trim().Length == 0) tbTradeDiscountAmount.Text = "0";
                    if (tbVatPercent.Text.Trim().Length == 0) tbVatPercent.Text = "0";
                    if (tbVatAmount.Text.Trim().Length == 0) tbVatAmount.Text = "0";
                    if (tbNetAmount.Text.Trim().Length == 0) tbNetAmount.Text = "0";

                    // Insert new Item
                    if (string.IsNullOrEmpty(hfPoItemID.Value))
                    {
                        string poItemID = poDao2.InsertInvPOItem
                        (
                            PRPOSession.PoID
                            , null
                            , tbProcureName.Text
                            , tbSpecify.Text
                            , ddlPackage.SelectedValue
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbQuantity.Text.Replace(",", "")
                            , tbTradeDiscountPercent.Text.Replace(",", "")
                            , tbTradeDiscountAmount.Text.Replace(",", "")
                            , "0"
                            , "0"
                            , tbTotalBeforeVat.Text.Replace(",", "")
                            , tbVatPercent.Text.Replace(",", "")
                            , tbVatAmount.Text.Replace(",", "")
                            , tbNetAmount.Text.Replace(",", "")
                            , ""
                        );

                        poDao2.InsertInvPOPR
                        (
                            Convert.ToInt32(PRPOSession.PoID)
                            , Convert.ToInt32(poItemID)
                            , Convert.ToInt32(hfPrID.Value)
                            , Convert.ToInt32(hfPrItemID.Value)
                        );

                        poDao2.UpdatePrItemStatus(Convert.ToInt32(hfPrItemID.Value), "1");
                    }
                    // Update existing item
                    else
                    {
                        poDao2.UpdatePOItem
                        (
                            hfPoItemID.Value
                            , tbProcureName.Text
                            , tbSpecify.Text
                            , ""
                            , Convert.ToInt32(ddlPackage.SelectedValue)
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbQuantity.Text.Replace(",", "")
                            , tbTradeDiscountPercent.Text.Replace(",", "")
                            , tbTradeDiscountAmount.Text.Replace(",", "")
                            , "0"
                            , "0"
                            , tbTotalBeforeVat.Text.Replace(",", "")
                            , tbVatPercent.Text.Replace(",", "")
                            , tbVatAmount.Text.Replace(",", "")
                            , tbNetAmount.Text.Replace(",", "")
                            , ""
                        );
                    }
                }

                // กรณีเพิ่มรายการ inv_po_attach
                PRPOUploadFileTable puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);
                foreach (DataRow row in puft.Table.Rows)
                {
                    if (Convert.ToInt32(row["Id"]) < 0)
                    {
                        string filePath = Server.MapPath(Path.Combine(PRPOPath.POTmpUpload, PRPOSession.UserID, row["FileName"].ToString()));
                        if (File.Exists(filePath))
                        {
                            File.Move(filePath, Server.MapPath(Path.Combine(PRPOPath.POUpload, row["FileName"].ToString())));
                            poDao2.InsertInvPOAttach(PRPOSession.PoID, row["FileName"].ToString());
                        }
                    }
                }

                poDao2.CommitTransaction();
            }
            catch (Exception ex)
            {
                poDao2.RollbackTransaction();
                throw ex;
            }

        }

        public override string GetScript()
        {
            return "";
        }

        public override string GetPOCode()
        {
            return base.GetPOCode() + "H-x/";
        }

        public override string GetPOType()
        {
            return "hire";
        }

        public void SaveGridViewForm2(PRPOForm2ActualTable pfat)
        {
            for (int i = 0; i < POCtrl.GVForm2.Rows.Count; ++i)
            {
                GridViewRow r = POCtrl.GVForm2.Rows[i];

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
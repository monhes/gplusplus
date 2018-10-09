using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.UserControls;
using GPlus.DataAccess;
using System.IO;

namespace GPlus.PRPO.PRPOHelper
{
    public class POPurchase : POType
    {
        public POPurchase(POControl control)
            : base(control)
        {
        }

        public override void BindPO(string poID, DataTable dtPO)
        {
            base.BindPO(poID, dtPO);
            IndexChanged();
            POCtrl.RBLPOType.SelectedValue = "1";

            PRPOPurchaseVirtualTable ppvt = new PRPOPurchaseVirtualTable();
            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable();
            PRPOPrintFormTable ppft = new PRPOPrintFormTable();
            new PRPODeleteItemTable();
            new PRPOPrintFormDeleteTable();
            new PRPOAttachDeleteTable();

            DataTable dtPOItem = new PODAO2().GetPOItem(Convert.ToInt32(poID), GetPOType());
            string formPrintID = dtPOItem.Rows[0]["FormPrintID"].ToString();
            
            // รายการที่เป็นแบบพิมพ์มีเพียงหนึ่งรายการ
            if (dtPOItem.Rows.Count == 1 && !string.IsNullOrEmpty(formPrintID))
            {
                DataTable dtPrintForm = new GPlus.DataAccess.PODAO2().GetPrintForm(Convert.ToInt32(formPrintID));

                DataRow r = dtPrintForm.Rows[0];

                string borrowQuantity = (r["BorrowQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowQuantity"]).ToString("0") : "";
                string borrowMonthQuantity = (r["BorrowMonthQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowMonthQuantity"]).ToString("0") : "";
                string borrowFirstQuantity = (r["BorrowFirstQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowFirstQuantity"]).ToString("0") : "";

                string newBorrowDate = string.Format("{0:dd/MM/YYYY}", r["NewBorrowDate"].ToString());

                ppft.AddItem
                (
                    formPrintID
                    , ""
                    , dtPOItem.Rows[0]["PoItemID"].ToString()
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
                    , dtPOItem.Rows[0]["UnitPrice"].ToString()
                    , r["BorrowUnitID"].ToString()
                    , r["BorrowMonthUnitID"].ToString()
                    , r["IsRequestModify"].ToString()
                    , r["IsFixedContent"].ToString()
                    , r["IsPaper"].ToString()
                    , r["IsFont"].ToString()
                    , r["Remark2"].ToString()
                    , r["RequestModifyDesc"].ToString()
                    , r["SizeDetail"].ToString()
                    , dtPOItem.Rows[0]["InvItemID"].ToString()
                    , dtPOItem.Rows[0]["PackID"].ToString()
                    , dtPOItem.Rows[0]["InvItemCode"].ToString()
                    , dtPOItem.Rows[0]["InvItemName"].ToString()
                    , dtPOItem.Rows[0]["PackName"].ToString()
                    , r["UnitQuantity"].ToString()
                    , dtPOItem.Rows[0]["InvSpecPurchase"].ToString()
                );

                // ดึงค่าจากตาราง PoItem มาใส่ในตาราง PRPOPrintFormTable
                string tradeDiscountPercent = dtPOItem.Rows[0]["TradeDiscountPercent"].ToString();
                string tradeDiscountAmount = dtPOItem.Rows[0]["TradeDiscountAmount"].ToString();
                string totalBeforeVat = dtPOItem.Rows[0]["TotalBeforeVat"].ToString();
                string vatPercent = dtPOItem.Rows[0]["VatPercent"].ToString();
                string vatAmount = dtPOItem.Rows[0]["VatAmount"].ToString();
                string netAmount = dtPOItem.Rows[0]["NetAmount"].ToString();

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

            dtPOItem.Columns.Add("UnitOrder");
            dtPOItem.Columns.Add("PopupType");
            dtPOItem.AcceptChanges();

            var result = from r in dtPOItem.AsEnumerable()
                            group r by new
                            {
                                InvItemID = r["InvItemID"],
                                PackID = r["PackID"],
                            } into grp
                            select new
                            {
                                POItemID = grp.Select(col => col["PoItemID"]).First(),
                                InvItemID = grp.Select(col => col["InvItemID"]).First(),
                                PackID = grp.Select(col => col["PackID"]).First(),
                                InvItemCode = grp.Select(col => col["InvItemCode"]).First(),
                                InvItemName = grp.Select(col => col["InvItemName"]).First(),
                                PackName = grp.Select(col => col["PackName"]).First(),
                                UnitPrice = grp.Select(col => col["UnitPrice"]).First(),
                                UnitQuantity = grp.Select(col => col["UnitQuantity"]).First(),
                                InvSpecPurchase = grp.Select(col => col["InvSpecPurchase"]).First(),
                                TradeDiscountPercent = grp.Select(col => col["TradeDiscountPercent"]).First(),
                                TradeDiscountAmount = grp.Select(col => col["TradeDiscountAmount"]).First(),
                                FormPrintID = grp.Select(col => col["FormPrintID"]).First(),
                                TotalBeforeVat = grp.Select(col => col["TotalBeforeVat"]).First(),
                                VatPercent = grp.Select(col => col["VatPercent"]).First(),
                                VatAmount = grp.Select(col => col["VatAmount"]).First(),
                                NetAmount = grp.Select(col => col["NetAmount"]).First(),
                                UnitOrder = grp.Select(col => col["UnitOrder"]).First(),
                                PrID = grp.Select(col => col["PrID"]).First(),
                                PrItemID = grp.Select(col => col["PrItemID"]).First(),
                                PopupType = grp.Select(col => col["PopupType"]).First(),
                            };

            // เพิ่มรายการลง PRPOPurchaseVirtualTable
            foreach (var group in result)
            {
                string tradeDiscountPercent = group.TradeDiscountPercent.ToString();
                if (!string.IsNullOrEmpty(tradeDiscountPercent))
                {
                    if (Convert.ToDecimal(tradeDiscountPercent) == 0)
                        tradeDiscountPercent = "";
                }

                ppvt.AddItem
                (
                    group.POItemID.ToString()
                    , group.InvItemID.ToString()
                    , group.PackID.ToString()
                    , group.InvItemCode.ToString()
                    , group.InvItemName.ToString()
                    , group.PackName.ToString()
                    , group.UnitPrice.ToString()
                    , group.UnitQuantity.ToString()
                    , group.InvSpecPurchase.ToString()
                    , tradeDiscountPercent
                    , group.TradeDiscountAmount.ToString()
                    , group.FormPrintID.ToString()
                    , group.TotalBeforeVat.ToString()
                    , group.VatPercent.ToString()
                    , group.VatAmount.ToString()
                    , group.NetAmount.ToString()
                    , group.UnitOrder.ToString()
                    , group.PrID.ToString()
                    , group.PrItemID.ToString()
                    , group.PopupType.ToString()
                    , ""
                    , ""
                    , "Y"
                );
            }

            // เพิ่มรายการลงตาราง PRPOPurchaseActualTable
            foreach (DataRow row in dtPOItem.Rows)
            {
                ppat.AddItem(
                    row["PoItemID"].ToString()
                    , row["InvItemID"].ToString()
                    , row["PackID"].ToString()
                    , row["InvItemCode"].ToString()
                    , row["InvItemName"].ToString()
                    , row["PackName"].ToString()
                    , row["UnitPrice"].ToString()
                    , row["UnitQuantity"].ToString()
                    , row["InvSpecPurchase"].ToString()
                    , row["TradeDiscountPercent"].ToString()
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

            BindGridviewItems();
        }

        public override void BindGridviewItems()
        {
            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
            PRPOPurchaseVirtualTable ppvt = new PRPOPurchaseVirtualTable(PRPOSession.PurchaseVirtualTable);
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

            SaveGridViewItems();

            if (ppft.GetFormType() != "")
            {
                DataTable table = ppft.Table;
                POCtrl.GVPurchase.DataSource = table;
                POCtrl.GVPurchase.DataBind();

                if (string.IsNullOrEmpty(ppft.Table.Rows[0]["UnitQuantity"].ToString()))
                {
                    // จำนวนสั่ง
                    TextBox tbUnitQuantity = POCtrl.GVPurchase.Rows[0].FindControl("tbUnitQuantity") as TextBox;
                    if (table.Rows[0]["FormBorrowType"].ToString() == "1")
                        tbUnitQuantity.Text = table.Rows[0]["BorrowQuantity"].ToString();
                    else if (table.Rows[0]["FormBorrowType"].ToString() == "2")
                        tbUnitQuantity.Text = table.Rows[0]["UnitQuantity"].ToString();
                }

                // หน่วยนับ
                POCtrl.GVPurchase.Rows[0].Cells[4].Text = table.Rows[0]["PackName"].ToString();
            }
            else
            {
                // เลือกรายการที่มาร์กว่ายังไม่มีการกรุ๊ป (Grouped = 'N')
                // กรุ๊ปแต่ละรายการด้วย InvItemID และ PackId
                // จากนั้นทำการ sum UnitQuantity ของแต่ละกรุ๊ป
                // การ sum จะมีผลกับ "รายการที่เลือกจาก PR"

                var result = from row in ppat.Table.AsEnumerable()
                             where row["Grouped"].Equals("N")
                             group row by new
                             {
                                 InvItemID = row["InvItemID"],
                                 PackID = row["PackID"],
                             } into grp
                             select new
                             {
                                 PoItemID = grp.Select(col => col["PoItemID"]).First(),
                                 InvItemID = grp.Select(col => col["InvItemID"]).First(),
                                 PackID = grp.Select(col => col["PackID"]).First(),
                                 InvItemCode = grp.Select(col => col["InvItemCode"]).First(),
                                 InvItemName = grp.Select(col => col["InvItemName"]).First(),
                                 PackName = grp.Select(col => col["PackName"]).First(),
                                 UnitPrice = grp.Select(col => col["UnitPrice"]).First(),
                                 UnitQuantity = grp.Sum(col => Convert.ToDecimal(col["UnitQuantity"])),
                                 InvSpecPurchase = grp.Select(col => col["InvSpecPurchase"]).First(),
                                 TradeDiscountPercent = grp.Select(col => col["TradeDiscountPercent"]).First(),
                                 TradeDiscountAmount = grp.Select(col => col["TradeDiscountAmount"]).First(),
                                 FormPrintID = grp.Select(col => col["FormPrintID"]).First(),
                                 TotalBeforeVat = grp.Select(col => col["TotalBeforeVat"]).First(),
                                 VatPercent = grp.Select(col => col["VatPercent"]).First(),
                                 VatAmount = grp.Select(col => col["VatAmount"]).First(),
                                 NetAmount = grp.Select(col => col["NetAmount"]).First(),
                                 UnitOrder = grp.Select(col => col["UnitOrder"]).First(),
                                 PrID = grp.Select(col => col["PrID"]).First(),
                                 PrItemID = grp.Select(col => col["PrItemID"]).First(),
                                 PopupType = grp.Select(col => col["PopupType"]).First(),
                             };

                foreach (var group in result)
                {
                    DataRow r = ppvt.Table.NewRow();

                    string itemID = group.InvItemID as string;
                    string packID = group.PackID as string;

                    r["PoItemID"] = group.PoItemID as string;
                    r["InvItemID"] = itemID;
                    r["PackID"] = packID;
                    r["InvItemCode"] = group.InvItemCode as string;
                    r["InvItemName"] = group.InvItemName as string;
                    r["PackName"] = group.PackName as string;
                    r["UnitPrice"] = group.UnitPrice as string;
                    r["UnitQuantity"] = group.UnitQuantity.ToString();
                    r["InvSpecPurchase"] = group.InvSpecPurchase as string;
                    r["TradeDiscountPercent"] = group.TradeDiscountPercent as string;
                    r["TradeDiscountAmount"] = group.TradeDiscountAmount as string;
                    r["FormPrintID"] = group.FormPrintID as string;
                    r["TotalBeforeVat"] = group.TotalBeforeVat as string;
                    r["VatPercent"] = group.VatPercent as string;
                    r["VatAmount"] = group.VatAmount as string;
                    r["NetAmount"] = group.NetAmount as string;
                    r["UnitOrder"] = group.UnitOrder as string;
                    r["PrID"] = group.PrID as string;
                    r["PrItemID"] = group.PrItemID as string;
                    r["PopupType"] = group.PopupType as string;

                    // ค้นหารายการ PR ที่เคยเลือกแล้ว หากพบรายการ PR ที่มี ItemID และ PackID เดียวกัน
                    // แต่เป็นรายการต่างใบกันให้นำจำนวนที่สั่งของที่เคยเลือกไว้แล้วมาบวกเพิ่ม
                    DataRow[] row = ppvt.FindItem(itemID, packID);

                    if ((row.Length > 0) && (row[0]["PrID"].ToString() != ""))
                    {
                        int unitQuantity = Convert.ToInt32(row[0]["UnitQuantity"]) + Convert.ToInt32(group.UnitQuantity);
                        row[0]["UnitQuantity"] = unitQuantity.ToString();
                        row[0].AcceptChanges();
                    }
                    else
                    {
                        ppvt.Table.Rows.Add(r);
                        r.AcceptChanges();
                    }
                }

                ppat.MarkGrouped();

                POCtrl.GVPurchase.DataSource = ppvt.Table;
                POCtrl.GVPurchase.DataBind();

                // Default Supplier ล่าสุดที่เคยซื้อของรายการแรก
                if (POCtrl.GVPurchase.Rows.Count == 1)
                {
                    GridViewRow row = POCtrl.GVPurchase.Rows[0];
                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;

                    if (hfItemID.Value != "" && hfPackID.Value != "")
                    {
                        if (PRPOSession.InitializeAction)
                        {
                            if (PRPOSession.Action == PRPOAction.ADD_PO)
                            {
                                DataTable dt = new DataAccess.DatabaseHelper().ExecuteQuery
                                             (
                                               "SELECT "
                                             + "    Latest_Supplier_ID "
                                             + "FROM Inv_ItemPack "
                                             + "WHERE Inv_ItemID = " + hfItemID.Value
                                             + "    AND Pack_Id = " + hfPackID.Value
                                             ).Tables[0];

                                if (dt.Rows.Count > 0)
                                {
                                    string latestSupplierID = dt.Rows[0]["Latest_Supplier_ID"].ToString();
                                    POCtrl.DDLSupplier.SelectedValue = latestSupplierID;
                                    POCtrl.UPSupplier.Update();
                                }
                            }
                        }
                    }
                }
            }

            if (ppft.GetFormType() != "")
            {
                POCtrl.BProductItem.Enabled = false;
                POCtrl.CBPrintForm.Checked = true;
            }
            else if (ppvt.Table.Rows.Count > 0)
            {
                POCtrl.CBPrintForm.Checked = false;
                POCtrl.BPrintForm.Enabled = false;
                POCtrl.BProductItem.Enabled = true;
            }
            else
            {
                POCtrl.BProductItem.Enabled = true;
                POCtrl.BPrintForm.Enabled = true;
                POCtrl.CBPrintForm.Checked = false;

                POCtrl.TBTotal.Text = "";
                POCtrl.TBTotalDiscount.Text = "";
                POCtrl.TBTotalBeforeVat.Text = "";
                POCtrl.TBTotalVat.Text = "";
                POCtrl.TBGrandTotal.Text = "";

                POCtrl.SetUIsWhenGridViewHasNoRow(POCtrl.GVPurchase);
            }
        }

        private void SaveGridViewItems()
        {
            PRPOPurchaseVirtualTable ppvt = new PRPOPurchaseVirtualTable(PRPOSession.PurchaseVirtualTable);

            GridView gv = POCtrl.GVPurchase;

            for (int i = 0; i < gv.Rows.Count; ++i)
            {
                HiddenField hfItemID = gv.Rows[i].FindControl("hfItemID") as HiddenField;           // รหัส
                HiddenField hfPackID = gv.Rows[i].FindControl("hfPackID") as HiddenField;           // หน่วยนับ
                HiddenField hfPopupType = gv.Rows[i].FindControl("hfPopupType") as HiddenField;     // ชนิด popup
                HiddenField hfPrID = gv.Rows[i].FindControl("hfPrID") as HiddenField;
                HiddenField hfPrItemID = gv.Rows[i].FindControl("hfPrItemID") as HiddenField;
                TextBox tbUnitPrice = gv.Rows[i].FindControl("tbUnitPrice") as TextBox;             // ราคาต่อหน่วย
                TextBox tbUnitQuantity = gv.Rows[i].FindControl("tbUnitQuantity") as TextBox;               // จำนวนที่สั่ง
                TextBox tbTradeDiscountPercent = gv.Rows[i].FindControl("tbTradeDiscountPercent") as TextBox; // ส่วนลด %
                TextBox tbTradeDiscountAmount = gv.Rows[i].FindControl("tbTradeDiscountAmount") as TextBox;       // ส่วนลดบาท
                TextBox tbTotalBeforeVat = gv.Rows[i].FindControl("tbTotalBeforeVat") as TextBox;   // จำนวนเงินก่อน vat
                TextBox tbVatPercent = gv.Rows[i].FindControl("tbVatPercent") as TextBox;           // vat%
                TextBox tbVatAmount = gv.Rows[i].FindControl("tbVatAmount") as TextBox;                 // Vat จำนวนเงิน
                TextBox tbNetAmount = gv.Rows[i].FindControl("tbNetAmount") as TextBox;             // จำนวนเงิน
                HiddenField hfSpecPurchase = gv.Rows[i].FindControl("hfSpecPurchase") as HiddenField;

                DataRow row = ppvt.FindItem(hfItemID.Value, hfPackID.Value, hfPrID.Value, hfPrItemID.Value).FirstOrDefault();
                if (row != null)
                {
                    row["UnitPrice"] = tbUnitPrice.Text;
                    row["UnitQuantity"] = tbUnitQuantity.Text;
                    row["TradeDiscountPercent"] = tbTradeDiscountPercent.Text;
                    row["TradeDiscountAmount"] = tbTradeDiscountAmount.Text;
                    row["TotalBeforeVat"] = tbTotalBeforeVat.Text;
                    row["VatPercent"] = tbVatPercent.Text;
                    row["VatAmount"] = tbVatAmount.Text;
                    row["NetAmount"] = tbNetAmount.Text;
                    row["InvSpecPurchase"] = hfSpecPurchase.Value;

                    row.AcceptChanges();
                }
            }
        }

        public override void DeleteGridviewItems()
        {
            PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
            PRPOPurchaseVirtualTable ppvt = new PRPOPurchaseVirtualTable(PRPOSession.PurchaseVirtualTable);
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);
            PRPODeleteItemTable pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);

            for (int i = 0; i < POCtrl.GVPurchase.Rows.Count; ++i)
            {
                CheckBox cbDelete = POCtrl.GVPurchase.Rows[i].FindControl("cbDelete") as CheckBox;

                if (cbDelete.Checked)
                {
                    GridViewRow row = POCtrl.GVPurchase.Rows[i];

                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
                    HiddenField hfPopupType = row.FindControl("hfPopupType") as HiddenField;
                    HiddenField hfPOItemID = row.FindControl("hfPOItemID") as HiddenField;

                    pdit.AddPoItem(hfPOItemID.Value, ppat);

                    ppat.DeleteItem(hfItemID.Value, hfPackID.Value);
                    ppvt.DeleteItem(hfItemID.Value, hfPackID.Value);
                }
            }

            if (ppft.GetFormType() != "")
            {
                PRPOPrintFormDeleteTable ppfdt = new PRPOPrintFormDeleteTable(PRPOSession.PrintFormDeleteTable);
                string printFormID = ppft.Table.Rows[0]["FormPrintID"].ToString();
                string poItemID = ppft.Table.Rows[0]["PoItemID"].ToString();
                ppfdt.AddItem(printFormID, poItemID);
                ppft.ClearRow(0);
            }

            BindGridviewItems();
        }

        public override void IndexChanged()
        {
            POCtrl.RBLItem.Items[0].Enabled = true;
            POCtrl.RBLItem.Items[1].Enabled = true;
            POCtrl.LReorderPoint.Visible = true;
            POCtrl.CCReorderPointDate.Visible = true;

            POCtrl.RBLItem.SelectedValue = "2";

            POCtrl.TBObjective.Enabled = false;
            POCtrl.DDLProject.Enabled = false;

            POCtrl.PHire.Visible = false;
            POCtrl.PPurchase.Visible = true;

            POCtrl.BPrintForm.Enabled = true;
            POCtrl.CBPrintForm.Enabled = true;
            POCtrl.CBPrintForm.Checked = false;

            PRPOSession.InitializePOPurchase();
        }

        public override string Save()
        {
            string poCode = GetPOCode();
            PODAO2 poDao2 = new PODAO2();

            try
            {
                poDao2.BeginTransaction();

                for (int i = 0; i < POCtrl.GVPurchase.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVPurchase.Rows[i];

                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;

                    if (string.IsNullOrEmpty(hfPackID.Value))
                    {
                        throw new Exception("กรุณาระบุหน่วยบรรจุของวัสดุอุปกรณ์");
                    }
                }

                if (POCtrl.RBLVatType.SelectedValue == "1")
                {
                    POCtrl.TBVat.Text = "";
                }

                string poID = poDao2.InsertInvPoForm1
                (
                    poCode                                          // รหัส PO
                    , POCtrl.RBLPOType.SelectedValue                // ชนิด PO; 1 = สั่งซื้อ, 2 = สั่งจ้าง
                    , POCtrl.RBLTypeAsset.SelectedValue             // ชนิด Asset; 1 = Stock, 2 = Asset
                    , POCtrl.CCPODate.Value                         // วันเวลาที่สั่งซื้อ
                    , POCtrl.OrgID                                  // รหัสหน่วยงาน
                    , ""                                            // วัตถุประสงค์
                    , ""                                            // โครงการ
                    , "1"                                           // รายการสินค้ามาจากไหน (ไม่สำคัญมากเพราะรายการในตัวใบผสมกันได้) 
                    , POCtrl.CBPrintForm.Checked ? "1" : "0"        // มีแบบพิมพ์; 1 = มี, 0 = ไม่มี
                    , POCtrl.DDLSupplier.SelectedValue              // Supplier ID
                    , POCtrl.TBQuotationCode1.Text                  // QuotationCode1
                    , POCtrl.CCQuotationDate1.Value                 // QuotationDate1
                    , POCtrl.TBQuotationCode2.Text                  // QuotationCode2
                    , POCtrl.CCQuotationDate2.Value                 // QuatationDate2
                    , POCtrl.CBIsPayCheque.Checked ? "1" : "0"      // IsPayCheck
                    , POCtrl.CBIsPayCash.Checked ? "1" : "0"        // IsPayCash
                    , POCtrl.TBCreditTermDay.Text                   // Credit term day
                    , POCtrl.CCShippingDate.Value                   // วันที่ส่งของ
                    , POCtrl.TBShippingAt.Text                      // สถานที่ส่งของ
                    , POCtrl.CBTradeDiscount.Checked ? "1" : "0"    // ส่วนลดการค้า
                    , POCtrl.RBLTradeDiscountType.SelectedValue     // ประเภทส่วนลดการค้า ; 0 = ส่วนลดรวม , 1 = ส่วนลดแต่ละรายการ
                    , POCtrl.TBTradeDiscountPercent.Text            // ส่วนลด %
                    , POCtrl.TBTradeDiscountAmount.Text             // ส่วนลด บาท
                    , "0"                                           // CashDiscountType     
                    , ""                                            // CashDiscountPercent 
                    , ""                                            // CashDiscountAmount 
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

                PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
                PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

                string poItemID = "";

                for (int i = 0; i < POCtrl.GVPurchase.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVPurchase.Rows[i];

                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
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

                    poItemID = poDao2.InsertInvPOItem
                    (
                        poID
                        , hfItemID.Value
                        , ""
                        , ""
                        , hfPackID.Value
                        , tbUnitPrice.Text.Replace(",", "")
                        , tbUnitQuantity.Text.Replace(",", "")
                        , tbTradeDiscountPercent.Text.Replace(",", "")
                        , tbTradeDiscountAmount.Text.Replace(",", "")
                        , "0"
                        , "0"
                        , tbTotalBeforeVat.Text.Replace(",", "")
                        , tbVatPercent.Text.Replace(",", "")
                        , tbVatAmount.Text.Replace(",", "")
                        , tbNetAmount.Text.Replace(",", "")
                        , hfSpecPurchase.Value
                    );

                    // หา Pr ที่สัมพันธ์กันกับ item และ pack นี้
                    // เพิ่มลงตาราง inv_po_pr
                    DataRow[] rows = ppat.FindItem(hfItemID.Value, hfPackID.Value);
                    foreach (DataRow r in rows)
                    {
                        string prID = r["PrID"].ToString();
                        string prItemID = r["PrItemID"].ToString();

                        if (!string.IsNullOrEmpty(prID) && !string.IsNullOrEmpty(prItemID))
                        {
                            poDao2.InsertInvPOPR
                            (
                                Convert.ToInt32(poID)
                                , Convert.ToInt32(poItemID)
                                , Convert.ToInt32(prID)
                                , Convert.ToInt32(prItemID)
                            );

                            poDao2.UpdatePrItemStatus
                            (
                                Convert.ToInt32(prItemID)
                                , "1" // ออก PO
                            );
                        }
                    }
                }

                UpdatePrForm1Status(poDao2, Convert.ToInt32(poID));

                // เพิ่ม print_form ลงตาราง inv_pr_po_formPrint
                if (!string.IsNullOrEmpty(ppft.GetFormType()) && POCtrl.CBPrintForm.Checked)
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

                    TextBox tbUnitQuantity = POCtrl.GVPurchase.Rows[0].FindControl("tbUnitQuantity") as TextBox;
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

                    string printFormID = poDao2.InsertInvPRPOFormPrint
                    (
                        ""
                        , poID
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

                    poDao2.UpdatePOItem(Convert.ToInt32(printFormID), Convert.ToInt32(poItemID));
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

                return poID;
            }
            catch (Exception ex)
            {
                poDao2.RollbackTransaction();
                throw ex;
            }
        }

        public override void Update()
        {
            PODAO2 poDao2 = new PODAO2();
            
            try
            {
                poDao2.BeginTransaction();

                string poItemID = "";

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

                poDao2.UpdatePOForm1
                (
                    PRPOSession.PoID
                    , "1"
                    , POCtrl.RBLTypeAsset.SelectedValue
                    , POCtrl.CCPODate.Value
                    , POCtrl.OrgID
                    , ""
                    , ""
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

                // ลบรายการที่สัมพันธ์กับ PR 
                PRPODeleteItemTable pdit = new PRPODeleteItemTable(PRPOSession.DeleteItemTable);
                foreach (DataRow row in pdit.Table.Rows)
                {
                    poItemID = row["PoItemID"].ToString();
                    string prID = row["PrID"].ToString();
                    string prItemID = row["PrItemID"].ToString();

                    if (!string.IsNullOrEmpty(prID) && !string.IsNullOrEmpty(prItemID))
                    {
                        // ลบรายการในตาราง Inv_PO_PR
                        poDao2.DeletePOPR
                        (
                            Convert.ToInt32(PRPOSession.PoID)
                            , Convert.ToInt32(prID)
                            , Convert.ToInt32(prItemID)
                            , Convert.ToInt32(poItemID)
                        );
                        // กำหนดค่ารายการ PR ให้มีสถานะเป็น NULL
                        poDao2.UpdatePrItemStatus(Convert.ToInt32(prItemID), null);
                    }
                }

                // ลบรายการที่เป็นแบบฟอร์ม
                PRPOPrintFormDeleteTable ppfdt = new PRPOPrintFormDeleteTable(PRPOSession.PrintFormDeleteTable);
                if (ppfdt.Table.Rows.Count > 0)
                {
                    string printFormID = ppfdt.Table.Rows[0]["PrintFormID"].ToString();
                    poItemID = ppfdt.Table.Rows[0]["PoItemID"].ToString();

                    poDao2.DeletePrintForm(Convert.ToInt32(printFormID));
                    poDao2.DeletePOItem(Convert.ToInt32(poItemID));
                }
                // ลบรายการที่มาจาก ReorderPoint, รายการสินค้า, PR
                else
                {
                    var result = from row in pdit.Table.AsEnumerable()
                                 group row by new
                                 {
                                     PoItemID = row["PoItemID"]
                                 } into grp
                                 select new
                                 {
                                     PoItemID = grp.Select(col => col["PoItemID"]).First()
                                 };

                    foreach (var group in result)
                    {
                        poItemID = group.PoItemID.ToString();
                        poDao2.DeletePOItem(Convert.ToInt32(poItemID));
                    }
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

                // --------------------------------- เพิ่มข้อมูล/แก้ไขข้อมูล -----------------------------
                PRPOPurchaseActualTable ppat = new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
                for (int i = 0; i < POCtrl.GVPurchase.Rows.Count; ++i)
                {
                    GridViewRow row = POCtrl.GVPurchase.Rows[i];

                    HiddenField hfItemID = row.FindControl("hfItemID") as HiddenField;
                    HiddenField hfPackID = row.FindControl("hfPackID") as HiddenField;
                    HiddenField hfPrID = row.FindControl("hfPrID") as HiddenField;
                    HiddenField hfPrItemID = row.FindControl("hfPrItemID") as HiddenField;
                    HiddenField hfPoItemID = row.FindControl("hfPOItemID") as HiddenField;
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

                    // เพิ่มข้อมูล
                    if (string.IsNullOrEmpty(hfPoItemID.Value))
                    {
                        poItemID = poDao2.InsertInvPOItem
                        (
                            PRPOSession.PoID
                            , hfItemID.Value
                            , ""
                            , ""
                            , hfPackID.Value
                            , tbUnitPrice.Text.Replace(",", "")
                            , tbUnitQuantity.Text.Replace(",", "")
                            , tbTradeDiscountPercent.Text.Replace(",", "")
                            , tbTradeDiscountAmount.Text.Replace(",", "")
                            , "0"
                            , "0"
                            , tbTotalBeforeVat.Text.Replace(",", "")
                            , tbVatPercent.Text.Replace(",", "")
                            , tbVatAmount.Text.Replace(",", "")
                            , tbNetAmount.Text.Replace(",", "")
                            , hfSpecPurchase.Value
                        );

                        // เพิ่ม Inv_PO_PR
                        DataRow[] rows = ppat.FindItem(hfItemID.Value, hfPackID.Value);
                        foreach (DataRow r in rows)
                        {
                            string prID = r["PrID"].ToString();
                            string prItemID = r["PrItemID"].ToString();

                            if (!string.IsNullOrEmpty(prID) && !string.IsNullOrEmpty(prItemID))
                            {
                                poDao2.InsertInvPOPR
                                (
                                    Convert.ToInt32(PRPOSession.PoID)
                                    , Convert.ToInt32(poItemID)
                                    , Convert.ToInt32(prID)
                                    , Convert.ToInt32(prItemID)
                                );

                                poDao2.UpdatePrItemStatus(Convert.ToInt32(prItemID), "1"); // ออก PO

                                poDao2.UpdatePRForm1(prID, "5");
                            }
                        }
                    }
                    // อัพเดตข้อมูล
                    else
                    {
                        poDao2.UpdatePOItem
                        (
                            hfPoItemID.Value
                            , ""
                            , ""
                            , hfItemID.Value
                            , Convert.ToInt32(hfPackID.Value)
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
                if (!string.IsNullOrEmpty(ppft.GetFormType()) && POCtrl.CBPrintForm.Checked)
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

                    TextBox tbUnitQuantity = POCtrl.GVPurchase.Rows[0].FindControl("tbUnitQuantity") as TextBox;
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

                    if (string.IsNullOrEmpty(row["PoItemID"].ToString()))
                    {
                        string printFormID = poDao2.InsertInvPRPOFormPrint
                        (
                            ""
                            , PRPOSession.PoID
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

                        poDao2.UpdatePOItem(Convert.ToInt32(printFormID), Convert.ToInt32(poItemID));
                    }
                    else
                    {
                        poDao2.UpdatePrintForm
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

        #region GET

        public override string GetPOCode()
        {
            return base.GetPOCode() + "P-x/";
        }
        public override string GetScript()
        {
            return

               // เมื่อมีการคลิกเลือก "ส่วนลดรวม"
                 "document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').onclick = "
               + "function ()"
               + "{"
               + "    document.getElementById('" + POCtrl.TBTradeDiscountPercent.ClientID + "').disabled = false;"
               + "    document.getElementById('" + POCtrl.TBTradeDiscountAmount.ClientID + "').disabled = false;"
               + "    CalculatePriceItem();"
               + "};"
                // เมื่อมีการคลิกเลือก "ส่วนลดแต่ละรายการ"
               + "document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').onclick = "
               + "function ()"
               + "{"
               + "    document.getElementById('" + POCtrl.TBTradeDiscountPercent.ClientID + "').disabled = true;"
               + "    document.getElementById('" + POCtrl.TBTradeDiscountAmount.ClientID + "').disabled = true;"
               + "    document.getElementById('" + POCtrl.TBTradeDiscountPercent.ClientID + "').value = '';"
               + "    document.getElementById('" + POCtrl.TBTradeDiscountAmount.ClientID + "').value = '';"
               + "    if (cbTradeDiscountTypeState)"
               + "    {"
               + "        EnableTradeDiscounts(true);"
               + "     }"
               + "    CalculatePriceItem();"
               + "};"
               + "document.getElementById('" + POCtrl.RBLVatType.ClientID + "_0').onclick = CalculatePriceItem;"
               + "document.getElementById('" + POCtrl.RBLVatType.ClientID + "_1').onclick = CalculatePriceItem;"
               + "document.getElementById('" + POCtrl.RBLVatUnitType.ClientID + "_0').onclick = "
               + "function() { CalculatePriceItem(); if (this.checked) document.getElementById('lblHeaderP').innerHTML = 'ราคา/หน่วย (รวม Vat)'; };"
               + "document.getElementById('" + POCtrl.RBLVatUnitType.ClientID + "_1').onclick = "
               + "function() { CalculatePriceItem(); if (this.checked) document.getElementById('lblHeaderP').innerHTML = 'ราคา/หน่วย';};"
               + "document.getElementById('" + POCtrl.TBVat.ClientID + "').onclick = CalculatePriceItem;"

           // เมื่อมีการ Click "ส่วนลดการค้า"
           + "var cbTradeDiscountTypeState = document.getElementById('" + POCtrl.CBTradeDiscount.ClientID + "').checked;"
           + "var $cbTradeDiscountType = document.getElementById('" + POCtrl.CBTradeDiscount.ClientID + "');"
           + "$cbTradeDiscountType.onclick = "
           + "function ()"
           + "{"
           + "    if (cbTradeDiscountTypeState == false)"
           + "    {"
           + "        cbTradeDiscountTypeState = true;"
           + "        document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').disabled = false;"
           + "        document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').disabled = false;"
           + "    }"
           + "    else"
           + "    {"
           + "        cbTradeDiscountTypeState = false;"
           + "        document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').disabled = true;"
           + "        document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').disabled = true;"
           + "        document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').checked = false;"
           + "        document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').checked = false;"
           + "        document.getElementById('" + POCtrl.TBTradeDiscountPercent.ClientID + "').disabled = true;"
           + "        document.getElementById('" + POCtrl.TBTradeDiscountAmount.ClientID + "').disabled = true;"
           + "        document.getElementById('" + POCtrl.TBTradeDiscountPercent.ClientID + "').value = '';"
           + "        document.getElementById('" + POCtrl.TBTradeDiscountAmount.ClientID + "').value = '';"
           + "        EnableTradeDiscounts(false);"
           + "        CalculatePriceItem();"
           + "    }"
           + "};"

           + "function EnableTradeDiscounts(enabled)"
           + "{"
           + "    var $gvPurchase = document.getElementById('" + POCtrl.GVPurchase.ClientID + "');"
           + "    if (enabled)"
           + "    {"
           + "        for (row = 1; row < $gvPurchase.rows.length; ++row)"
           + "        {"
           + "            $gvPurchase.rows[row].cells[7].firstChild.value = '';"
           + "            $gvPurchase.rows[row].cells[8].firstChild.value = '';"
           + "            $gvPurchase.rows[row].cells[7].disabled = false;" // ส่วนลด %
           + "            $gvPurchase.rows[row].cells[8].disabled = false;" // ส่วนลด บาท
           + "        }"
           + "    }"
           + "    else"
           + "    {"
           + "        for (row = 1; row < $gvPurchase.rows.length; ++row)"
           + "        {"
           + "            $gvPurchase.rows[row].cells[7].firstChild.value = '';"
           + "            $gvPurchase.rows[row].cells[8].firstChild.value = '';"
           + "            $gvPurchase.rows[row].cells[7].disabled = true;" // ส่วนลด %
           + "            $gvPurchase.rows[row].cells[8].disabled = true;" // ส่วนลด บาท
           + "        }"
           + "    }"
           + "}"

           + "if ($cbTradeDiscountType.checked)"
           + "{"
           + "   document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').disabled = false;"
           + "   document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').disabled = false;"
           + "   if (document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').checked)"
           + "   {"
           + "        document.getElementById('" + POCtrl.TBTradeDiscountPercent.ClientID + "').disabled = false;"
           + "        document.getElementById('" + POCtrl.TBTradeDiscountAmount.ClientID + "').disabled = false;"
           + "   }"
           + "}"
           + "else"
           + "{"
           + "    document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').disabled = true;"
           + "    document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').disabled = true;"
           + "    document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_0').checked = false;"
           + "    document.getElementById('" + POCtrl.RBLTradeDiscountType.ClientID + "_1').checked = false;"
           + "    EnableTradeDiscounts(false);"
           + "}"

           + "if (document.getElementById('" + POCtrl.RBLVatType.ClientID + "_0').checked)"
           + "{"
           + "    document.getElementById('" + POCtrl.RBLVatType.ClientID + "_0').click();"
           + "}"
           + "if (document.getElementById('" + POCtrl.RBLVatType.ClientID + "_1').checked)"
           + "{"
           + "    document.getElementById('" + POCtrl.RBLVatType.ClientID + "_1').click();"
           + "}";
        }
        public override string GetPOType()
        {
            return "purchase";
        }

        #endregion
    }
}
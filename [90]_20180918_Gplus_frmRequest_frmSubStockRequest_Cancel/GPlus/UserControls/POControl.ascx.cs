using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GPlus.PRPO.PRPOHelper;
using GPlus.PRPO;
using System.Data;
using System.IO;

namespace GPlus.UserControls
{
    public partial class POControl : System.Web.UI.UserControl
    {
        #region ATTRIBUTE
        private POType poType;
        private DataTable dtPackage;
        private Pagebase pb = new Pagebase();

        #endregion

        #region CLICK

        protected void bDeletePurchaseItem_Click(object sender, EventArgs e)
        {
            poType.DeleteGridviewItems();

            if (gvPurchase.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock
                (
                    Page,
                    typeof(Page),
                    "clear",
                    "document.getElementById('" + bProductItem.ClientID + "').disabled = false;",
                    true
                );
            }

            SetUIsWhenGridViewHasNoRow(gvPurchase);
        }
        protected void bDeleteHireItem_Click(object sender, EventArgs e)
        {
            poType.DeleteGridviewItems();
            SetUIsWhenGridViewHasNoRow(gvHire);
        }
        protected void bSave_Click(object sender, EventArgs e)
        {
            if (PRPOSession.Action == PRPOAction.ADD_PO)
            {
                try
                {
                    string poID = poType.Save();
                    ScriptSavePopup(poID);
                }
                catch (Exception ex)
                {
                    ScriptSaveErrorPopup(ex.Message);
                }
            }
            else if (PRPOSession.Action == PRPOAction.VIEW_PO)
            {
                try
                {
                    poType.Update();
                    ScriptSavePopup(PRPOSession.PoID);
                }
                catch (Exception ex)
                {
                    ScriptSaveErrorPopup(ex.Message);
                }
            }
            
        }
        protected void bAddForm2_Click(object sender, EventArgs e)
        {
            if (gvHire.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock
                (
                    Page,
                    typeof(Page),
                    "Form2",
                    "alert('กรุณาเลือกรายการสินค้าก่อนเพิ่มบัญชี');",
                    true
                );

                btnRefreshI.Attributes.Add("style", "display:none");

                return;
            }

            PRPOForm2ActualTable pfat = new PRPOForm2ActualTable(PRPOSession.Form2Table);

            ((POHire) poType).SaveGridViewForm2(pfat);

            pfat.AddNewRow(PRPOType.PO);

            gvForm2.DataSource = pfat.Table;
            gvForm2.DataBind();
        }
        protected void bDeleteForm2_Click(object sender, EventArgs e)
        {
            PRPOForm2ActualTable pfat = new PRPOForm2ActualTable(PRPOSession.Form2Table);
            PRPOForm2DeleteTable pfdt = new PRPOForm2DeleteTable(PRPOSession.Form2DeleteTable);

            for (int i = 0; i < gvForm2.Rows.Count; ++i)
            {
                GridViewRow r = gvForm2.Rows[i];

                CheckBox cbDelete = r.FindControl("cbDelete") as CheckBox;

                if (cbDelete.Checked)
                {
                    HiddenField hfPoForm2ID = r.FindControl("hfPoForm2ID") as HiddenField;
                    HiddenField hfPrForm2ID = r.FindControl("hfPrForm2ID") as HiddenField;

                    pfdt.AddItem(hfPoForm2ID.Value);

                    pfat.DeleteItem(hfPoForm2ID.Value, hfPrForm2ID.Value);
                }
            }

            ((POHire) poType).SaveGridViewForm2(pfat);

            gvForm2.DataSource = pfat.Table;
            gvForm2.DataBind();
        }
        protected void Refresh_Click(object sender, EventArgs e)
        {
            if (rblPOType.SelectedIndex == 0)
            {
                if (reorderPoint_Date.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock
                    (
                        Page
                        , typeof(Page)
                        , "script"
                        , "document.getElementById('" + reorderPoint_Date.ClientID + "').value = '" + hfreorderPoint_Date.Value + "';", true
                     );
                }
            }

            poType.BindGridviewItems();

            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

            if (ppft.GetFormType() != "")
            {
                string js = "document.getElementById('" + bProductItem.ClientID + "').disabled = true;";

                ScriptManager.RegisterClientScriptBlock
                (
                    Page
                    , typeof(Page)
                    , "script"
                    , js
                    , true
                );
            }
        }
        protected void bCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PRPO/PoMgt.aspx");
        }
        // ไม่ดำเนินการสั่งซื้อ
        protected void bCancelPurchase_Click(object sender, EventArgs e)
        {
            if (PRPOSession.Action == PRPOAction.ADD_PO)
            {
                try
                {
                    string poID = poType.Save();
                    poType.CancelPO(poID);
                    ScriptSavePopup(poID);
                }
                catch (Exception ex)
                {
                    ScriptSaveErrorPopup(ex.Message);
                }
            }
            else
            {
                try
                {
                    poType.CancelPO();

                    ScriptManager.RegisterStartupScript
                    (
                        Page
                        , typeof(Page)
                        , "Success"
                        , "alert('ไม่ดำเนินการสั่งซื้อแล้ว'); window.location.href = '../PRPO/POMgt.aspx';"
                        , true
                    );
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript
                    (
                        Page
                        , typeof(Page)
                        , "CancelPO"
                        , "alert('เกิดข้อผิดพลาดระหว่างไม่ดำเนินการสั่งซื้อ'); window.location.href = '../PRPO/POMgt.aspx';"
                        , true
                    );
                }
            }
        }
        protected void bPrintPO_Click(object sender, EventArgs e)
        {

        }
        // ลบรายการ
        protected void bDeletePO_Click(object sender, EventArgs e)
        {
            try
            {
                poType.DeletePO();

                ScriptManager.RegisterStartupScript
                (
                    Page
                    , typeof(Page)
                    , "Success"
                    , "alert('ลบข้อมูลเรียบร้อย'); window.location.href = '../PRPO/POMgt.aspx';"
                    , true
                );
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript
                (
                    Page
                    , typeof(Page)
                    , "DeletePO"
                    , "alert('เกิดข้อผิดพลาดระหว่างการลบ'); window.location.href = '../PRPO/POMgt.aspx';"
                    , true
                );
            }
        }

        #endregion

        #region INITIALIZE

        private void InitializeUI()
        {
            //lCreateBy.Text = pb.UserName;                                   // ผู้ที่สร้าง
            //lUpdateBy.Text = pb.UserName;                                   // ผู้ที่แก้ไขล่าสุด
            lCreateBy.Text = pb.FirstName+' '+pb.LastName;                    // ผู้ที่สร้าง
            lUpdateBy.Text = pb.FirstName + ' ' + pb.LastName;                // ผู้ที่แก้ไขล่าสุด
            lCreateDate.Text = DateTime.Now.ToString(pb.DateTimeFormat);    // วันที่สร้าง
            lUpdateDate.Text = DateTime.Now.ToString(pb.DateTimeFormat);    // วันที่แก้ไขล่าสุด

            ccPODate.Text = DateTime.Now.ToString(pb.DateFormat);           // วันเวลาที่สั่งซื้อ

            tbDivPO.Text = pb.OrgName;                                      // หน่วยงานที่ออก PO

            // โครงการ
            ddlProject.DataSource = new DataAccess.ProjectDAO().GetProject("", "", "1", 1, 1000, "", "").Tables[0];
            ddlProject.DataTextField = "Project_Name";
            ddlProject.DataValueField = "Project_ID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("เลือกโครงการ", ""));

            ucDdlSupplier.RequireValidator = false;                         // ผู้ผลิด/ผู้จัดจำหน่าย
            tbContractName.Text = pb.FirstName + " " + pb.LastName;         // ผู้ติดต่อ MTL

            rblTradeDiscountType.Enabled = false;
            tbTradeDiscountPercent.Enabled = false;
            tbTradeDiscountAmount.Enabled = false;

            cbTradeDiscountType.Enabled = false;

            DDLProject.Width = new Unit(270);
            DDLSupplier.Width = new Unit(270);

            string js = "var $bCancelPurchase = document.getElementById('" + bCancelPurchase.ClientID + "');"
                      + "if ($bCancelPurchase != null) $bCancelPurchase.style.display = 'none';";

            ScriptManager.RegisterStartupScript(this, GetType(), "script", js, true);
        }
        private void InitializeControls()
        {
            tbObjective.Text = "";
            ddlProject.SelectedIndex = 0;
            cbPrintForm.Checked = false;

            tbQuotationCode1.Text = "";
            tbQuotationCode2.Text = "";
            ccQuotationDate1.Text = "";
            ccQuotationDate2.Text = "";

            cbIsPayCheque.Checked = true;
            cbIsPayCash.Checked = false;
            tbCreditTermDay.Text = "30";

            ccShippingDate.Text = "";
            tbShippingAt.Text = "";

            tbTradeDiscountPercent.Text = "";
            tbTradeDiscountAmount.Text = "";

            rblTradeDiscountType.Enabled = false;
            rblTradeDiscountType.SelectedIndex = -1;

            tbTradeDiscountPercent.Enabled = false;     // ส่วนลดการค้า %
            tbTradeDiscountAmount.Enabled = false;      // ส่วนลดการค้า บาท
            tbVat.Text = "7";

            cbTradeDiscountType.Checked = false;        // ส่วนลดการค้า
            cbTradeDiscountType.Enabled = false;

            rblVatType.SelectedValue = "0";
            rblVatUnitType.SelectedValue = "0";

            ucDdlSupplier.setFirstDropdownIndex();
            tbContractName.Text = pb.FirstName + " " + pb.LastName;         // ผู้ติดต่อ MTL

            tbTotal.Text = "";
            tbTotalDiscount.Text = "";
            tbTotalBeforeVat.Text = "";
            tbTotalVat.Text = "";
            tbGrandTotal.Text = "";
        }
        /// <summary>
        ///     กำหนดค่าเริ่มต้นตาราง Session โดยมีตารางดังนี้
        ///         - PRPOPurchaseActualTable เก็บค่าจริงที่ยังไม่มีการ SUM ของ จำนวนที่สั่ง (UnitQuantity)
        ///         - PRPOPurchaseVirtualTable เก็บค่าที่ได้จากการประมวลผลแล้วจากตาราง PRPOPurchaseActualTable
        ///         - PRPOPrintFormTable เก็บค่าแบบพิมพ์
        /// </summary>
        public void InitializeSessionTables()
        {
            new PRPOPurchaseActualTable();
            new PRPOPurchaseVirtualTable();
            new PRPOHireActualTable();
            new PRPOPrintFormTable();
            new PRPOForm2ActualTable();
        }

        #endregion

        #region OTHER

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeUI();
                bPrintForm.OnClientClick = GPlus.Util.CreatePopUp("pop_PrintForm2.aspx", null, null, "pop_PrintForm");
                ibPaymentNo.OnClientClick =
                    GPlus.Util.CreatePopUp
                    (
                        "pop_PaymentNo.aspx"
                        , new string[] { "tbPaymentNo" }
                        , new string[] { tbPaymentNo.ClientID }
                        , "popPaymentNo"
                    );
            }
            poType = POFactory.CreatePO(rblPOType.SelectedValue, this);
        }
        public void BindPO(string poId)
        {
            DataTable dtPO = new GPlus.DataAccess.PODAO().GetPOForm1(poId);

            if (dtPO.Rows.Count > 0)
            {
                DataRow row = dtPO.Rows[0];
                string potype = row["PO_Type"].ToString();
                string poStatus = row["Status"].ToString();

                poType = POFactory.CreatePO(potype, this);

                poType.BindPO(poId, dtPO);

                if (poStatus == "0")                            // ยกเลิก
                {
                    pDeletePO.Visible = true;
                    pDetail.Enabled = false;
                    pButtons.Visible = false;
                    pCancelPurchase.Visible = false;
                }
                else if (poStatus == "2" || poStatus == "3")    // อนุมัติ, ไม่อนุมัติ, รับเข้าคลังแล้ว
                {
                    // แสดง Print PO กับ ยกเลิก
                    bPrintPO.Visible = true;
                    bCancel.Visible = true;
                    // ไม่แสดงปุ่ม "ไม่ดำเนินการสั่งซื้อ", "บันทึก", "ลบข้อมูล"
                    bSave.Visible = false;
                    bDeletePO.Visible = false;
                    bCancelPurchase.Visible = false; 
    
                    pCancelPurchase.Visible = false;
                    pDeletePO.Visible = false;
                    pDetail.Enabled = true;
                    pButtons.Visible = true;
                }
                else if (poStatus == "4")                       // ตรวจสอบ PO
                {
                    bCancelPurchase.Visible = true;
                    bSave.Visible = true;
                    bDeletePO.Visible = true;
                    bPrintPO.Visible = true;
                    bCancel.Visible = true;

                    pDetail.Enabled = true;

                    pDeletePO.Visible = false;
                    pCancelPurchase.Visible = false;
                    pButtons.Visible = true;
                }
                else if (poStatus == "5")                       // ไม่ดำเนินการสั่งซื้อ
                {
                    pDeletePO.Visible = false;
                    pDetail.Enabled = false;
                    pCancelPurchase.Visible = true;

                    bCancelPurchase.Visible = false;
                    bSave.Visible = false;
                    bDeletePO.Visible = true;
                    bPrintPO.Visible = false;
                    pButtons.Visible = true;
                }
                else
                {
                    bCancelPurchase.Visible = true;
                    bSave.Visible = true;
                    bDeletePO.Visible = true;
                    bPrintPO.Visible = true;
                    bCancel.Visible = true;

                    pDeletePO.Visible = false;
                    pCancelPurchase.Visible = false;
                    pDetail.Enabled = true;
                    pButtons.Visible = true;
                }

                bPrintPO.OnClientClick = "open_popup('pop_PO.aspx?id=" + poId + "', 850, 450, 'pop', 'yes', 'yes', 'yes'); return false;";
                ibPaymentNo.OnClientClick =
                    GPlus.Util.CreatePopUp
                    (
                        "pop_PaymentNo.aspx"
                        , new string[] { "poId", "tbPaymentNo" }
                        , new string[] { poId, tbPaymentNo.ClientID }
                        , "popPaymentNo"
                    );

                ScriptManager.RegisterStartupScript
                (
                    this,
                    GetType(),
                    "script",
                    poType.GetScript(),
                    true
                );
            }
        }
        public void SetUIsWhenGridViewHasNoRow(GridView gv)
        {
            if (gv.Rows.Count == 0)
            {
                tbTradeDiscountAmount.Attributes.Remove("onblur");
                tbTradeDiscountPercent.Attributes.Remove("onblur");

                cbTradeDiscountType.Checked = false;
                cbTradeDiscountType.Enabled = false;

                rblTradeDiscountType.SelectedIndex = -1;
                tbTradeDiscountPercent.Text = "";
                tbTradeDiscountAmount.Text = "";

                rblTradeDiscountType.Enabled = false;
                tbTradeDiscountPercent.Enabled = false;
                tbTradeDiscountAmount.Enabled = false;

                // อยู่นอก update panel

                string js =
                    "document.getElementById('" + tbTotal.ClientID + "').value = '';"
                    + "document.getElementById('" + tbTotalDiscount.ClientID + "').value = '';"
                    + "document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = '';"
                    + "document.getElementById('" + tbTotalVat.ClientID + "').value = '';"
                    + "document.getElementById('" + tbGrandTotal.ClientID + "').value = '';"
                    + "document.getElementById('" + bProductItem.ClientID + "').disabled = false;"
                    + "document.getElementById('" + bPrintForm.ClientID + "').disabled = false;"
                    + "document.getElementById('" + cbPrintForm.ClientID + "').checked = false;"

                    + "document.getElementById('" + bCancelPurchase.ClientID + "').style.display = 'none';";

                ScriptManager.RegisterClientScriptBlock
                (
                    Page
                    , typeof(Page)
                    , "clear"
                    , js
                    , true
                );
            }
        }
        protected void rblPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeControls();
            poType.IndexChanged();
            poType.BindGridviewItems();
        }

        #endregion

        #region ROW_DATA_BOUND

        DataTable dtExpense = null;         // Use for gvForm2_RowDataBound
        DataTable dtAccExpense = null;      // Use for gvForm2_RowDataBound
        string prID = "";                   // Use for gvForm2_RowDataBound

        protected void gvPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Script = "function CalculatePriceItem()"
                       + "{"
                       + "    var total = 0;"
                       + "    var up = 0;"
                       + "    var qu = 0;"
                       + "    var disc = 0;"
                       + "    var vat = 0;"
                       + "    var prodtotal = 0;"
                       + "    var discTotal = 0;"
                       + "    var vatTotal = 0;"
                       + "    var disDiscount = document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked;" // ส่วนลดรวม
                       + "    var incVat = document.getElementById('" + rblVatType.ClientID + "_1').checked;"
                       + "    var vatExcItem = document.getElementById('" + rblVatUnitType.ClientID + "_1').checked;";

                ((Label)e.Row.FindControl("lblHeaderP")).Text = rblVatUnitType.SelectedValue == "1"
                                                               ? "ราคา/หน่วย (รวม Vat)"
                                                               : "ราคา/หน่วย";

                ((Label)e.Row.FindControl("lblHeaderP")).Text = rblVatUnitType.SelectedValue == "1" ? "ราคา/หน่วย (รวมVat)" : "ราคา/หน่วย";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = (e.Row.RowIndex + 1).ToString();      // ลำดับ

                DataRowView drv = e.Row.DataItem as DataRowView;

                CheckBox cbDelete = e.Row.FindControl("cbDelete") as CheckBox;

                HiddenField hfItemID = e.Row.FindControl("hfItemID") as HiddenField;
                HiddenField hfPackID = e.Row.FindControl("hfPackID") as HiddenField;
                HiddenField hfPopupType = e.Row.FindControl("hfPopupType") as HiddenField;
                HiddenField hfPrID = e.Row.FindControl("hfPrID") as HiddenField;
                HiddenField hfPrItemID = e.Row.FindControl("hfPrItemID") as HiddenField;
                HiddenField hfPOItemID = e.Row.FindControl("hfPOItemID") as HiddenField;
                HiddenField hfSpecPurchase = e.Row.FindControl("hfSpecPurchase") as HiddenField;

                TextBox tbUnitPrice = e.Row.FindControl("tbUnitPrice") as TextBox;
                TextBox tbUnitQuantity = e.Row.FindControl("tbUnitQuantity") as TextBox;
                TextBox tbTradeDiscountPercent = e.Row.FindControl("tbTradeDiscountPercent") as TextBox;
                TextBox tbTradeDiscountAmount = e.Row.FindControl("tbTradeDiscountAmount") as TextBox;
                TextBox tbTotalBeforeVat = e.Row.FindControl("tbTotalBeforeVat") as TextBox;
                TextBox tbVatPercent = e.Row.FindControl("tbVatPercent") as TextBox;
                TextBox tbVatAmount = e.Row.FindControl("tbVatAmount") as TextBox;
                TextBox tbNetAmount = e.Row.FindControl("tbNetAmount") as TextBox;

                #region Nin 06042014

                CheckBox chkClose = ((CheckBox)e.Row.FindControl("chkClose"));
                ImageButton CommentClose = ((ImageButton)e.Row.FindControl("CommentClose"));
                HiddenField hdRemarkClose = (HiddenField)e.Row.FindControl("hdRemarkClose");
                TextBox tbReceive_Quantity = e.Row.FindControl("tbReceive_Quantity") as TextBox;

                DataTable dt = new DataAccess.ReceiveStockDAO().GetPOItem_Close(drv["PoItemID"].ToString());
                string bool_Close = "f";

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DeliveryStop_flag"].ToString() == "1")
                    {
                        chkClose.Checked = true;
                        hdRemarkClose.Value = dt.Rows[0]["DeliveryStop_Remark"].ToString();
                    }
                    else
                    {
                        chkClose.Checked = false;
                        hdRemarkClose.Value = "";
                    }
                    tbReceive_Quantity.Text = ((Decimal)(dt.Rows[0]["Receive_Quantity"].ToString() == "" ? "0":dt.Rows[0]["Receive_Quantity"])).ToString("#,##0");
                }

                CommentClose.OnClientClick = "open_popup('../Stock/pop_Remark.aspx?ctl=" + hdRemarkClose.ClientID
                                          + "&POItem_ID=" + drv["PoItemID"].ToString() + "&chk=" + bool_Close
                                          + "', 500, 200, 'CloseRec', 'yes', 'yes', 'yes'); return false;";

                #endregion Nin 06042014

                e.Row.Cells[3].Text = "<span onmouseover='linkOver(this);' onmouseout='linkOut(this);'><a style='text-decoration: none' href=\"javascript:open_popup('pop_SpecPurchase.aspx?ctl="
                                    + hfSpecPurchase.ClientID + "', 550, 200, 'popSpecPurchase', 'yes', 'yes', 'yes');\">"
                                    + drv["InvItemName"].ToString() + "</a></span>";

                string invItemID = drv["InvItemID"].ToString();
                string packID = drv["PackID"].ToString();
                string prID = drv["PrID"].ToString();
                string prItemID = drv["PrItemID"].ToString();
                string popupType = drv["PopupType"].ToString();
                string unitPrice = drv["UnitPrice"].ToString();
                string unitQuantity = drv["UnitQuantity"].ToString();
                string tradeDiscountPercent = drv["TradeDiscountPercent"].ToString();
                string tradeDiscountAmount = drv["TradeDiscountAmount"].ToString();
                string totalBeforeVat = drv["TotalBeforeVat"].ToString();
                string vatPercent = drv["VatPercent"].ToString();
                string vatAmount = drv["VatAmount"].ToString();
                string netAmount = drv["NetAmount"].ToString();
                string poItemID = drv["PoItemID"].ToString();
                string specPurchase = drv["InvSpecPurchase"].ToString();

                hfItemID.Value = invItemID;
                hfPackID.Value = packID;
                hfPrID.Value = prID;
                hfPrItemID.Value = prItemID;
                hfPopupType.Value = popupType;
                hfPOItemID.Value = poItemID;
                hfSpecPurchase.Value = specPurchase;

                if (unitPrice.Trim().Length > 0)
                    tbUnitPrice.Text = Convert.ToDecimal(unitPrice).ToString("0.00");
                if (unitQuantity.Trim().Length > 0)
                    tbUnitQuantity.Text = Convert.ToDecimal(unitQuantity).ToString("0");
                if (tradeDiscountPercent.Trim().Length > 0)
                    tbTradeDiscountPercent.Text = Convert.ToDecimal(tradeDiscountPercent).ToString("0.00");
                if (tradeDiscountAmount.Trim().Length > 0)
                    tbTradeDiscountAmount.Text = Convert.ToDecimal(tradeDiscountAmount).ToString("0.00");
                if (totalBeforeVat.Trim().Length > 0)
                    tbTotalBeforeVat.Text = Convert.ToDecimal(totalBeforeVat).ToString(pb.CurrencyFormat);
                if (vatPercent.Trim().Length > 0)
                    tbVatPercent.Text = Convert.ToDecimal(vatPercent).ToString("0");
                if (vatAmount.Trim().Length > 0)
                    tbVatAmount.Text = Convert.ToDecimal(vatAmount).ToString(pb.CurrencyFormat);
                if (netAmount.Trim().Length > 0)
                    tbNetAmount.Text = Convert.ToDecimal(netAmount).ToString(pb.CurrencyFormat);

                tbUnitPrice.Attributes.Add("onblur", "CalculatePriceItem();");              // ราคาต่อหน่วย ของ GridView
                tbUnitQuantity.Attributes.Add("onblur", "CalculatePriceItem();");           // จำนวนที่สั่ง ของ GridView
                tbTradeDiscountPercent.Attributes.Add("onblur", "CalculatePriceItem();");   // ส่วนลด % ของ GridView
                tbTradeDiscountAmount.Attributes.Add("onblur", "CalculatePriceItem();");    // ส่วนลด บาท ของ GridView
                tbVatPercent.Attributes.Add("onblur", "CalculatePriceItem();");             // Vat % ของ GridView

                Script += "    up = document.getElementById('" + tbUnitPrice.ClientID + "').value;"
                        + "    if (up.length == 0)"
                        + "        up = 0;"
                        + "    else"
                        + "        up = up * 1;"
                        + "    qu = document.getElementById('" + tbUnitQuantity.ClientID + "').value;"
                        + "    if (qu.length == 0)"
                        + "        qu = 0;"
                        + "    else"
                        + "        qu = qu * 1;"
                        + "    total = up * qu; total = total.toFixed(4);"
                        + "    if (disDiscount)"
                        + "    {"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = ! document.getElementById('" + cbTradeDiscountType.ClientID + "').checked;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = ! document.getElementById('" + cbTradeDiscountType.ClientID + "').checked;"
                        + "        if (document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value.length > 0)"
                        + "        {"
                        + "            disc = (total * document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value) / 100;"
                        + "            document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = disc.toFixed(2);"
                        + "        }"
                        + "        else if (document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value.length > 0)"
                        + "        {"
                        + "            disc += document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value * 1;"
                        + "        }"
                        + "    }"
                        + "    discTotal += disc;"
                        + "    total = total - disc;"
                        + "    disc = 0;"
                        + "    document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(total);"
                        + "    if (incVat)"
                        + "    {"
                        + "        document.getElementById('" + tbVatPercent.ClientID + "').disabled = false;"
                        + "        vat = document.getElementById('" + tbVat.ClientID + "').value * 1;"
                        + "        if (vatExcItem)"
                        + "        {"
                        + "            if (document.getElementById('" + tbVatPercent.ClientID + "').value == '')"
                        + "                document.getElementById('" + tbVatPercent.ClientID + "').value = vat;"
                        + "            vat = document.getElementById('" + tbVatPercent.ClientID + "').value * 1;"
                        + "            vat = (vat * total) / 100;"
                        + "            total = total + vat;"
                        + "        }"
                        + "        else"
                        + "        {"
                        + "            if (document.getElementById('" + tbVatPercent.ClientID + "').value.length == 0)"
                        + "                document.getElementById('" + tbVatPercent.ClientID + "').value = vat;"
                        + "            vat = document.getElementById('" + tbVatPercent.ClientID + "').value * 1;"
                        + "            vat = (total / (vat + 100)) * vat;"
                        + "            document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(total - vat);"
                        + "        }"
                        + "        vatTotal += vat;"
                        + "        document.getElementById('" + tbVatAmount.ClientID + "').value = formatBaht(vat);"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        document.getElementById('" + tbVatPercent.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbVatPercent.ClientID + "').value = '0';"
                        + "        document.getElementById('" + tbVatAmount.ClientID + "').value = '0.00';"
                        + "    }"
                        + "    document.getElementById('" + tbNetAmount.ClientID + "').value = formatBaht(total);"
                        + "    prodtotal += total;";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Script += "    document.getElementById('" + tbTotal.ClientID + "').value = formatBaht(prodtotal);"
                        + "    if (disDiscount)"
                        + "    {"
                    // กรณีส่วนลดการค้าแบบ %
                        + "        if (document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value.length > 0)"
                        + "            disc = (prodtotal * document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value) / 100;"
                        + "        if (document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value.length > 0)"
                        + "        {"
                        + "            var discountAmount = document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value.replace(/,/g,'');"
                        + "            discountAmount = parseFloat(discountAmount);"
                        + "            disc += discountAmount;"
                        + "        }"
                        + "        document.getElementById('" + tbTotalDiscount.ClientID + "').value = formatBaht(disc);"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        disc = 0;"
                        + "        document.getElementById('" + tbTotalDiscount.ClientID + "').value = formatBaht(discTotal);"
                        + "    }"
                        + "    prodtotal -= disc;"
                        + "    document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(prodtotal);"
                        + "    if (incVat)"
                        + "    {"
                        + "        document.getElementById('" + tbTotalVat.ClientID + "').value = formatBaht(vatTotal);"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        vat = document.getElementById('" + tbVat.ClientID + "').value * 1;"
                        + "        if (vatExcItem)"
                        + "        {"
                        + "            vat = (vat * prodtotal) / 100;"
                        + "            document.getElementById('" + tbTotalVat.ClientID + "').value = formatBaht(vat);"
                        + "            prodtotal = prodtotal + vat;"
                        + "        }"
                        + "        else"
                        + "        {"
                        + "            vat = (prodtotal / (vat + 100)) * 7;"
                        + "            document.getElementById('" + tbTotalVat.ClientID + "').value = formatBaht(vat);"
                        + "            document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(prodtotal - vat);"
                        + "        }"
                        + "    }"
                        + "    prodtotal = prodtotal.toFixed(2);"
                        + "    prodtotal = Number(prodtotal);"
                        + "    document.getElementById('" + tbGrandTotal.ClientID + "').value = formatBaht(prodtotal);"
                        + "}"

                        + "CalculatePriceItem();"

                        // เมื่อมีการคลิกเลือก "ส่วนลดรวม"
                        + "document.getElementById('" + rblTradeDiscountType.ClientID + "_0').onclick = "
                        + "function ()"
                        + "{"
                        + "    document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = false;"
                        + "    document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = false;"
                        + "    CalculatePriceItem();"
                        + "};"
                    // เมื่อมีการคลิกเลือก "ส่วนลดแต่ละรายการ"
                        + "document.getElementById('" + rblTradeDiscountType.ClientID + "_1').onclick = "
                        + "function ()"
                        + "{"
                        + "    document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                        + "    document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;"
                        + "    document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                        + "    document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                        + "    if (cbTradeDiscountTypeState)"
                        + "    {"
                        + "        EnableTradeDiscounts(true);"
                        + "     }"
                        + "    CalculatePriceItem();"
                        + "};"
                        + "document.getElementById('" + rblVatType.ClientID + "_0').onclick = CalculatePriceItem;"
                        + "document.getElementById('" + rblVatType.ClientID + "_1').onclick = CalculatePriceItem;"
                        + "document.getElementById('" + rblVatUnitType.ClientID + "_0').onclick = "
                        + "function() { CalculatePriceItem(); if (this.checked) document.getElementById('lblHeaderP').innerHTML = 'ราคา/หน่วย (รวม Vat)'; };"
                        + "document.getElementById('" + rblVatUnitType.ClientID + "_1').onclick = "
                        + "function() { CalculatePriceItem(); if (this.checked) document.getElementById('lblHeaderP').innerHTML = 'ราคา/หน่วย';};"
                        + "document.getElementById('" + tbVat.ClientID + "').onclick = CalculatePriceItem;"

                        // เมื่อมีการ Click "ส่วนลดการค้า"
                        + "var cbTradeDiscountTypeState = document.getElementById('" + cbTradeDiscountType.ClientID + "').checked;"
                        + "var $cbTradeDiscountType = document.getElementById('" + cbTradeDiscountType.ClientID + "');"
                        + "$cbTradeDiscountType.onclick = "
                        + "function ()"
                        + "{"
                        + "    if (cbTradeDiscountTypeState == false)"
                        + "    {"
                        + "        cbTradeDiscountTypeState = true;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = false;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = false;"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        cbTradeDiscountTypeState = false;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = true;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = true;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked = false;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_1').checked = false;"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                        + "        EnableTradeDiscounts(false);"
                        + "        CalculatePriceItem();"
                        + "    }"
                        + "};"

                        + "function EnableTradeDiscounts(enabled)"
                        + "{"
                        + "    var $gvPurchase = document.getElementById('" + gvPurchase.ClientID + "');"
                        + "    if (enabled)"
                        + "    {"
                        + "        for (row = 1; row < $gvPurchase.rows.length; ++row)"
                        + "        {"
                        + "            $gvPurchase.rows[row].cells[7].getElementsByTagName('input')[0].value = '';"
                        + "            $gvPurchase.rows[row].cells[8].getElementsByTagName('input')[0].value = '';"
                        + "            $gvPurchase.rows[row].cells[7].disabled = false;" // ส่วนลด %
                        + "            $gvPurchase.rows[row].cells[8].disabled = false;" // ส่วนลด บาท
                        + "        }"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        for (row = 1; row < $gvPurchase.rows.length; ++row)"
                        + "        {"
                        + "            $gvPurchase.rows[row].cells[7].getElementsByTagName('input')[0].value = '';"
                        + "            $gvPurchase.rows[row].cells[8].getElementsByTagName('input')[0].value = '';"
                        + "            $gvPurchase.rows[row].cells[7].disabled = true;" // ส่วนลด %
                        + "            $gvPurchase.rows[row].cells[8].disabled = true;" // ส่วนลด บาท
                        + "        }"
                        + "    }"
                        + "}"

                        + "if ($cbTradeDiscountType.checked)"
                        + "{"
                        + "   document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = false;"
                        + "   document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = false;"
                        + "   if (document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked)"
                        + "   {"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = false;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = false;"
                        + "   }"
                        + "}"
                        + "else"
                        + "{"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = true;"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = true;"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked = false;"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_1').checked = false;"
                        + "}"

                        + "function linkOver($span) { $span.getElementsByTagName('a')[0].style.color = '#FF6699'; }"
		                + "function linkOut($span) { $span.getElementsByTagName('a')[0].style.color = 'blue'; }"

                        + ScriptSetEnterTAB();

                tbTradeDiscountPercent.Attributes.Add("onblur", "CalculatePriceItem();");
                tbTradeDiscountAmount.Attributes.Add("onblur", "CalculatePriceItem();");

                cbTradeDiscountType.Enabled = true;
                btnRefreshI.Attributes.Add("style", "display:none");

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Calculate", Script, true);
            }
        }

        protected void gvHire_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dtPackage = new DataAccess.PackageDAO().GetPackage("", "1", 1, 1000, "", "").Tables[0];

                Script = "function CalculatePriceItem2()"
                       + "{"
                       + "    var total = 0;"
                       + "    var up = 0;"
                       + "    var qu = 0;"
                       + "    var disc = 0;"
                       + "    var vat = 0;"
                       + "    var prodtotal = 0;"
                       + "    var discTotal = 0;"
                       + "    var vatTotal = 0;"
                       + "    var disDiscount = document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked;"
                       + "    var incVat = document.getElementById('" + rblVatType.ClientID + "_1').checked;"
                       + "    var vatExcItem = document.getElementById('" + rblVatUnitType.ClientID + "_1').checked;";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                HiddenField hfItemID = e.Row.FindControl("hfItemID") as HiddenField;
                HiddenField hfPackID = e.Row.FindControl("hfPackID") as HiddenField;
                HiddenField hfPrID = e.Row.FindControl("hfPrID") as HiddenField;
                HiddenField hfPrItemID = e.Row.FindControl("hfPrItemID") as HiddenField;
                HiddenField hfPoItemID = e.Row.FindControl("hfPoItemID") as HiddenField;

                TextBox tbProcureName = e.Row.FindControl("tbProcureName") as TextBox;
                TextBox tbQuantity = e.Row.FindControl("tbQuantity") as TextBox;
                TextBox tbUnitPrice = e.Row.FindControl("tbUnitPrice") as TextBox;
                TextBox tbTradeDiscountPercent = e.Row.FindControl("tbTradeDiscountPercent") as TextBox;
                TextBox tbTradeDiscountAmount = e.Row.FindControl("tbTradeDiscountAmount") as TextBox;
                TextBox tbTotalBeforeVat = e.Row.FindControl("tbTotalBeforeVat") as TextBox;
                TextBox tbVatPercent = e.Row.FindControl("tbVatPercent") as TextBox;
                TextBox tbVatAmount = e.Row.FindControl("tbVatAmount") as TextBox;
                TextBox tbNetAmount = e.Row.FindControl("tbNetAmount") as TextBox;
                TextBox tbSpecify = e.Row.FindControl("tbSpecify") as TextBox;

                DropDownList ddlPackage = e.Row.FindControl("ddlPackage") as DropDownList;

                string itemID = drv["InvItemID"].ToString();
                string packID = drv["PackID"].ToString();
                prID = drv["PrID"].ToString();
                string prItemID = drv["PrItemID"].ToString();
                string procureName = drv["ProcureName"].ToString();
                string unitQuantity = drv["UnitQuantity"].ToString();
                string unitPrice = drv["UnitPrice"].ToString();
                string tradeDiscountPercent = drv["TradeDiscountPercent"].ToString();
                string tradeDiscountAmount = drv["TradeDiscountAmount"].ToString();
                string totalBeforeVat = drv["TotalBeforeVat"].ToString();
                string vatPercent = drv["VatPercent"].ToString();
                string vatAmount = drv["VatAmount"].ToString();
                string specPurchase = drv["InvSpecPurchase"].ToString();
                string specify = drv["Specify"].ToString();
                string poItemID = drv["PoItemID"].ToString();

                hfItemID.Value = itemID;
                hfPackID.Value = packID;
                hfPrID.Value = prID;
                hfPrItemID.Value = prItemID;
                hfPoItemID.Value = poItemID;

                tbProcureName.Text = procureName;
                tbSpecify.Text = specify;

                if (unitQuantity.Trim().Length > 0)
                    tbQuantity.Text = Convert.ToDecimal(unitQuantity).ToString("0");
                if (unitPrice.Trim().Length > 0)
                    tbUnitPrice.Text = Convert.ToDecimal(unitPrice).ToString("0.00");
                if (tradeDiscountPercent.Trim().Length > 0)
                    tbTradeDiscountPercent.Text = Convert.ToDecimal(tradeDiscountPercent).ToString("0.00");
                if (tradeDiscountAmount.Trim().Length > 0)
                    tbTradeDiscountAmount.Text = Convert.ToDecimal(tradeDiscountAmount).ToString("0.00");
                if (totalBeforeVat.Trim().Length > 0)
                    tbTotalBeforeVat.Text = Convert.ToDecimal(totalBeforeVat).ToString(pb.CurrencyFormat);
                if (vatPercent.Trim().Length > 0)
                    tbVatPercent.Text = Convert.ToDecimal(vatPercent).ToString("0");
                if (vatAmount.Trim().Length > 0)
                    tbVatAmount.Text = Convert.ToDecimal(vatAmount).ToString(pb.CurrencyFormat);

                ddlPackage.DataSource = dtPackage;
                ddlPackage.DataBind();

                ddlPackage.Items.Insert(0, new ListItem("เลือก", ""));

                ListItem item = ddlPackage.Items.FindByValue(packID);
                if (item != null)
                    item.Selected = true;

                e.Row.Cells[11].ColumnSpan = 2;
                e.Row.Cells[11].BackColor = System.Drawing.ColorTranslator.FromHtml("white");
                e.Row.Cells[11].Text += "</td><tr style='background-color:white;'><td>";
                e.Row.Cells[12].ColumnSpan = 12;

                tbTradeDiscountPercent.Attributes.Add("onblur", "CalculatePriceItem2();");
                tbTradeDiscountAmount.Attributes.Add("onblur", "CalculatePriceItem2();");
                tbVatPercent.Attributes.Add("onblur", "CalculatePriceItem2();");
                tbUnitPrice.Attributes.Add("onblur", "CalculatePriceItem2();");
                tbQuantity.Attributes.Add("onblur", "CalculatePriceItem2();");

                Script += "    up = document.getElementById('" + tbUnitPrice.ClientID + "').value;"
                        + "    if (up.length == 0)"
                        + "        up = 0;"
                        + "    else"
                        + "        up = up * 1;"
                        + "    qu = document.getElementById('" + tbQuantity.ClientID + "').value;"
                        + "    if (qu.length == 0)"
                        + "        qu = 0;"
                        + "    else"
                        + "        qu = qu * 1;"
                        + "    total = up * qu;"
                        + "    total = total.toFixed(4);"
                        + "    if (disDiscount)"
                        + "    {"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = ! document.getElementById('" + cbTradeDiscountType.ClientID + "').checked;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = ! document.getElementById('" + cbTradeDiscountType.ClientID + "').checked;"
                        + "        if (document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value.length > 0)"
                        + "        {"
                        + "            disc = (total * document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value) / 100;"
                        + "            document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = disc.toFixed(2);"
                        + "        }"
                        + "        else if (document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value.length > 0)"
                        + "            disc += document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value * 1;"
                        + "    }"
                        + "    discTotal += disc;"
                        + "    total = total - disc;"
                        + "    disc = 0;"
                        + "    document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(total);"
                        + "    if (incVat)"
                        + "    {"
                        + "        document.getElementById('" + tbVatPercent.ClientID + "').disabled = false;"
                        + "        vat = document.getElementById('" + tbVat.ClientID + "').value * 1;"
                        + "        if (vatExcItem)"
                        + "        {"
                        + "            if (document.getElementById('" + tbVatPercent.ClientID + "').value == '')"
                        + "                document.getElementById('" + tbVatPercent.ClientID + "').value = vat;"
                        + "            vat = document.getElementById('" + tbVatPercent.ClientID + "').value * 1;"
                        + "            vat = (vat * total) / 100;"
                        + "            total = total + vat;"
                        + "        }"
                        + "        else"
                        + "        {"
                        + "            if (document.getElementById('" + tbVatPercent.ClientID + "').value.length == 0)"
                        + "                document.getElementById('" + tbVatPercent.ClientID + "').value = vat;"
                        + "            vat = document.getElementById('" + tbVatPercent.ClientID + "').value * 1;"
                        + "            vat = (total / (vat + 100)) * vat;"
                        + "            document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(total - vat);"
                        + "        }"
                        + "        vatTotal += vat;"
                        + "        document.getElementById('" + tbVatAmount.ClientID + "').value = formatBaht(vat);"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        document.getElementById('" + tbVatPercent.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbVatPercent.ClientID + "').value = '0';"
                        + "        document.getElementById('" + tbVatAmount.ClientID + "').value = '0.00';"
                        + "    }"
                        + "    document.getElementById('" + tbNetAmount.ClientID + "').value = formatBaht(total);"
                        + "    prodtotal += total;";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Script += "    document.getElementById('" + tbTotal.ClientID + "').value = formatBaht(prodtotal);"
                        + "    if (disDiscount)"
                        + "    {"
                        + "        if (document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value.length > 0)"
                        + "            disc = (prodtotal * document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value) / 100;"
                        + "        if (document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value.length > 0)"
                        + "            disc += document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value * 1;"
                        + "        document.getElementById('" + tbTotalDiscount.ClientID + "').value = formatBaht(disc);"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        disc = 0;"
                        + "        document.getElementById('" + tbTotalDiscount.ClientID + "').value = formatBaht(discTotal);"
                        + "    }"
                        + "    prodtotal -= disc;"
                        + "    document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(prodtotal);"
                        + "    if (incVat)"
                        + "    {"
                        + "        document.getElementById('" + tbTotalVat.ClientID + "').value = formatBaht(vatTotal);"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        vat = document.getElementById('" + tbVat.ClientID + "').value * 1;"
                        + "        if (vatExcItem)"
                        + "        {"
                        + "            vat = (vat * prodtotal) / 100;"
                        + "            document.getElementById('" + tbTotalVat.ClientID + "').value = formatBaht(vat);"
                        + "            prodtotal = prodtotal + vat;"
                        + "        }"
                        + "        else"
                        + "        {"
                        + "            vat = (prodtotal / (vat + 100)) * 7;"
                        + "            document.getElementById('" + tbTotalVat.ClientID + "').value = formatBaht(vat);"
                        + "            document.getElementById('" + tbTotalBeforeVat.ClientID + "').value = formatBaht(prodtotal - vat);"
                        + "        }"
                        + "    }"
                        + "    document.getElementById('" + tbGrandTotal.ClientID + "').value = formatBaht(prodtotal);"
                        + "}"
                        + "CalculatePriceItem2();"
                        + "document.getElementById('" + rblTradeDiscountType.ClientID + "_0').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + rblTradeDiscountType.ClientID + "_1').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + rblVatType.ClientID + "_0').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + rblVatType.ClientID + "_1').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + rblVatUnitType.ClientID + "_0').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + rblVatUnitType.ClientID + "_1').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + tbVat.ClientID + "').onclick = CalculatePriceItem2;"

                        // เมื่อมีการคลิกเลือก "ส่วนลดรวม"
                        + "document.getElementById('" + rblTradeDiscountType.ClientID + "_0').onclick = "
                        + "function ()"
                        + "{"
                        + "    document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = false;"
                        + "    document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = false;"
                        + "    CalculatePriceItem2();"
                        + "};"
                    // เมื่อมีการคลิกเลือก "ส่วนลดแต่ละรายการ"
                        + "document.getElementById('" + rblTradeDiscountType.ClientID + "_1').onclick = "
                        + "function ()"
                        + "{"
                        + "    document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                        + "    document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;"
                        + "    document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                        + "    document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                        + "    if (cbTradeDiscountTypeState)"
                        + "    {"
                        + "        EnableTradeDiscounts(true);"
                        + "     }"
                        + "    CalculatePriceItem2();"
                        + "};"
                        + "document.getElementById('" + rblVatType.ClientID + "_0').onclick = CalculatePriceItem2;"
                        + "document.getElementById('" + rblVatType.ClientID + "_1').onclick = CalculatePriceItem2;"

                        // เมื่อมีการ Click "ส่วนลดการค้า"
                        + "var cbTradeDiscountTypeState = document.getElementById('" + cbTradeDiscountType.ClientID + "').checked;"
                        + "var $cbTradeDiscountType = document.getElementById('" + cbTradeDiscountType.ClientID + "');"
                        + "$cbTradeDiscountType.onclick = "
                        + "function ()"
                        + "{"
                        + "    if (cbTradeDiscountTypeState == false)"
                        + "    {"
                        + "        cbTradeDiscountTypeState = true;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = false;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = false;"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        cbTradeDiscountTypeState = false;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = true;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = true;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked = false;"
                        + "        document.getElementById('" + rblTradeDiscountType.ClientID + "_1').checked = false;"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                        + "        EnableTradeDiscounts(false);"
                        + "        CalculatePriceItem2();"
                        + "    }"
                        + "};"

                        + "function EnableTradeDiscounts(enabled)"
                        + "{"
                        + "    var $gvHire = document.getElementById('" + gvHire.ClientID + "');"
                        + "    if (enabled)"
                        + "    {"
                        + "        for (row = 1; row < $gvHire.rows.length; ++row)"
                        + "        {"
                        + "            if (typeof $gvHire.rows[row].cells[5] != 'undefined')"
                        + "            {"
                        + "                $gvHire.rows[row].cells[5].getElementsByTagName('input')[0].value = '';"
                        + "                $gvHire.rows[row].cells[6].getElementsByTagName('input')[0].value = '';"
                        + "                $gvHire.rows[row].cells[5].disabled = false;" // ส่วนลด %
                        + "                $gvHire.rows[row].cells[6].disabled = false;" // ส่วนลด บาท
                        + "            }"
                        + "        }"
                        + "    }"
                        + "    else"
                        + "    {"
                        + "        for (row = 1; row < $gvHire.rows.length; ++row)"
                        + "        {"
                        + "            if (typeof $gvHire.rows[row].cells[5] != 'undefined')"
                        + "            {"
                        + "                $gvHire.rows[row].cells[5].getElementsByTagName('input')[0].value = '';"
                        + "                $gvHire.rows[row].cells[6].getElementsByTagName('input')[0].value = '';"
                        + "                $gvHire.rows[row].cells[5].disabled = true;" // ส่วนลด %
                        + "                $gvHire.rows[row].cells[6].disabled = true;" // ส่วนลด บาท
                        + "            }"
                        + "        }"
                        + "    }"
                        + "}"

                        + "if ($cbTradeDiscountType.checked)"
                        + "{"
                        + "   document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = false;"
                        + "   document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = false;"
                        + "   if (document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked)"
                        + "   {"
                        + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = false;"
                        + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = false;"
                        + "   }"
                        + "}"
                        + "else"
                        + "{"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_0').disabled = true;"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_1').disabled = true;"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_0').checked = false;"
                        + "    document.getElementById('" + rblTradeDiscountType.ClientID + "_1').checked = false;"
                        + "}"

                        + ScriptSetEnterTAB();

                if (gvForm2.Rows.Count > 0)
                    Script += "RecalculateGVForm2();";

                cbTradeDiscountType.Enabled = true;

                btnRefreshI.Attributes.Add("style", "display:none");

                ScriptManager.RegisterStartupScript
                (
                    this.Page,
                    typeof(Page),
                    "Calculate",
                    Script,
                    true
                );

                tbVat.Attributes.Add("onblur", "CalculatePriceItem2();");
                tbTradeDiscountPercent.Attributes.Add("onblur", "CalculatePriceItem2();");
                tbTradeDiscountAmount.Attributes.Add("onblur", "CalculatePriceItem2();");

                DataTable dtPRForm2 = new GPlus.DataAccess.PRDAO().GetPRForm2(prID);
                PRPOForm2ActualTable pfat = new PRPOForm2ActualTable(PRPOSession.Form2Table);

                if (gvForm2.Rows.Count == 0)
                    PRPOSession.PrPoForm2Binded = false;

                if (PRPOSession.PrPoForm2Binded == false)
                {
                    foreach (DataRow row in dtPRForm2.Rows)
                    {
                        string prForm2ID = row["PR_Form2_ID"].ToString();
                        string expenseID = row["Expense_ID"].ToString();
                        string accExpenseID = row["AccExpense_ID"].ToString();
                        string percentAllocate = string.Format("{0:0}", row["Percent_Allocate"]);
                        string amountAllocate = row["Amount_Allocate"].ToString();

                        pfat.AddItem("", prForm2ID, expenseID, accExpenseID, percentAllocate, amountAllocate);
                    }

                    PRPOSession.PrPoForm2Binded = true;
                }

                gvForm2.DataSource = pfat.Table;
                gvForm2.DataBind();
            }
        }
        protected void gvForm2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dtExpense = new DataAccess.ExpenseDAO().GetExpense("", "", "1", 1, 1000, "", "").Tables[0];
                dtAccExpense = new DataAccess.AccountExpenseDAO().GetAccountExpense("", "", "1", 1, 1000, "", "").Tables[0];
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                HiddenField hfPrForm2ID = e.Row.FindControl("hfPrForm2ID") as HiddenField;
                HiddenField hfPoForm2ID = e.Row.FindControl("hfPoForm2ID") as HiddenField;
                DropDownList ddlExpense = e.Row.FindControl("ddlExpense") as DropDownList;
                DropDownList ddlAccExpense = e.Row.FindControl("ddlAccExpense") as DropDownList;
                TextBox tbPercentAllocate = e.Row.FindControl("tbPercentAllocate") as TextBox;
                TextBox tbAmountAllocate = e.Row.FindControl("tbAmountAllocate") as TextBox;

                // กลุ่มค่าใช้จ่ายบัญชี
                ddlExpense.Items.Add(new ListItem("เลือกกลุ่มค่าใช้จ่าย", ""));
                for (int i = 0; i < dtExpense.Rows.Count; ++i)
                {
                    DataRow row = dtExpense.Rows[i];

                    ddlExpense.Items.Add
                    (
                        new ListItem
                        (
                            row["Expense_Code"].ToString() + " - " + row["Expense_Desc"].ToString(),
                            row["Expense_ID"].ToString()
                        )
                    );
                }

                // บัญชี
                ddlAccExpense.Items.Add(new ListItem("เลือกบัญชี", ""));
                for (int i = 0; i < dtAccExpense.Rows.Count; ++i)
                {
                    DataRow row = dtAccExpense.Rows[i];

                    ddlAccExpense.Items.Add
                    (
                        new ListItem
                        (
                            row["AccExpense_Code"].ToString() + " - " + row["AccExpense_Name"].ToString(),
                            row["AccExpense_ID"].ToString()
                        )
                    );
                }

                hfPrForm2ID.Value = drv["PrForm2ID"].ToString();
                hfPoForm2ID.Value = drv["PoForm2ID"].ToString();
                ddlExpense.SelectedValue = drv["ExpenseID"].ToString();
                ddlAccExpense.SelectedValue = drv["AccExpenseID"].ToString();
                tbPercentAllocate.Text = drv["PercentAllocate"].ToString();
                tbAmountAllocate.Text = drv["AmountAllocate"].ToString();

                tbPercentAllocate.Attributes.Add
                (
                    "onkeyup",
                    "CalculateForm2(document.getElementById('" + tbPercentAllocate.ClientID + "'), document.getElementById('" + tbAmountAllocate.ClientID + "'));"
                );
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox tbPercentAllocateSum = e.Row.FindControl("tbPercentAllocateSum") as TextBox;

                Script2 = "var gvForm2 = document.getElementById('" + gvForm2.ClientID + "');"
                        + "function CalculateForm2(percentAllocate, amountAllocate)"
                        + "{"
                        + "    CheckPercentAllocate(gvForm2);"
                        + "    var pAlloc = percentAllocate.value;"
                        + "    var grandTotal = Number(document.getElementById('" + tbGrandTotal.ClientID + "').value.replace(/,/g,''));"
                        + "    if (document.getElementById('" + tbGrandTotal.ClientID + "').value.length == 0)"
                        + "        grandTotal = 0;"
                        + "    if (percentAllocate.value.length == 0)"
                        + "        pAlloc = 0;"
                        + "    amountAllocate.value = formatBaht((pAlloc * grandTotal) / 100);"
                        + "    gvForm2.rows[gvForm2.rows.length - 1].cells[3].getElementsByTagName('input')[0].value = SumPercent(gvForm2);"
                        + "}"

                        + "function CheckPercentAllocate(gvForm2)"
                        + "{"
                        + "    var sumPercent = SumPercent(gvForm2);"
                        + "    if (sumPercent > 100.0)"
                        + "    {"
                        + "        alert('อัตราส่วนรวมต้องไม่เกิน 100%');"
                        + "        document.activeElement.value = '';"
                        + "    }"
                        + "}"

                        + "function SumPercent(gvForm2)"
                        + "{"
                        + "    var sumPercent = 0.0;"
                        + "    for (i = 1; i < gvForm2.rows.length - 1; ++i) {"
                        + "        sumPercent += Number(gvForm2.rows[i].cells[3].getElementsByTagName('input')[0].value);"
                        + "    }"
                        + "    return sumPercent;"
                        + "}"

                        + "gvForm2.rows[gvForm2.rows.length - 1].cells[3].getElementsByTagName('input')[0].value = SumPercent(gvForm2);"

                        + "function RecalculateGVForm2()"
                        + "{"
                        + "    var grandTotal = Number(document.getElementById('" + tbGrandTotal.ClientID + "').value.replace(/,/g,''));"
                        + "    var gv = document.getElementById('" + gvForm2.ClientID + "');"
                        + "    if (gv == null) return;"
                        + "    for (i = 1; i < gv.rows.length - 1; ++i)"
                        + "    {"
                        + "        var percentAllocate = Number(gv.rows[i].cells[3].getElementsByTagName('input')[0].value);"
                        + "        var result = Number((percentAllocate * grandTotal) / 100).toFixed(2);"
                        + "        gv.rows[i].cells[4].getElementsByTagName('input')[0].value = NumberWithCommas(result.toString());"
                        + "    }"
                        + "}"

                        + "function NumberWithCommas(x) { return x.toString().replace(/\\B(?=(\\d{3})+(?!\\d))/g, ','); }"

                        /* if there are more than 3 digits after the decimal point use this.
                         function numberWithCommas(x) {
                            var parts = x.toString().split(".");
                            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            return parts.join(".");
                         }
                         */

                        + "setInterval(function () { RecalculateGVForm2(); }, 100);";

                btnRefreshI.Attributes.Add("style", "display:none");

                ScriptManager.RegisterStartupScript
                (
                    Page,
                    typeof(Page),
                    "Form2",
                    Script2,
                    true
                );
            }
        }

        #endregion

        #region SCRIPT

        private void ScriptSaveErrorPopup(string errorMessage)
        {
            errorMessage = HttpUtility.JavaScriptStringEncode("เกิดข้อผิดพลาดระหว่างการบันทึก : " + errorMessage, true);

            ScriptManager.RegisterStartupScript
            (
                this,
                GetType(),
                "Save",
                "alert(" + errorMessage + "); window.location.href = '../PRPO/POMgt.aspx';",
                true
            );
        }
        private void ScriptSavePopup(string poID)
        {
            Response.Write
            (
                "<script>"
                + "    var screenWidth = (screen.width - 600) / 2;"
                + "    var screenHeight = (screen.height - 300) / 2;"
                + "    window.location.href = '../PRPO/POMgt.aspx';"
                + "    var popup = window.open('../PRPO/pop_PO.aspx?id=" + poID + "','pop_PO',\"location=no,menubar=no,scrollbars=no,resizable=yes,status=no,left=\" + screenWidth + \",top=\" + screenHeight + \",width=600,height=300\");"
                + "    if (popup) popup.focus();"
                + "</script>"
            );
        }
        private string ScriptSetEnterTAB()
        {
            // จัดการกับ TAB และ Enter
            tbTradeDiscountPercent.Attributes.Add("onkeyup", "return TradeDiscountPercentKeyup(event);");
            tbTradeDiscountPercent.Attributes.Add("onkeydown", "return TradeDiscountPercentKeydown(event);");
            tbTradeDiscountAmount.Attributes.Add("onkeyup", "return TradeDiscountAmountKeyup(event);");
            tbTradeDiscountAmount.Attributes.Add("onkeydown", "return TradeDiscountAmountKeydown(event);");

            return
                    "function TradeDiscountPercentKeyup(e)"
                + "{"
                + "    if (e.keyCode == '13' || e.keyCode == '9')"
                + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').focus();"
                + "    else"
                + "        document.getElementById('" + tbTradeDiscountAmount.ClientID + "').value = '';"
                + "}"

                + "function TradeDiscountPercentKeydown(e)"
                + "{"
                + "    if (e.keyCode == '13' || e.keyCode == '9')"
                + "       return false;"
                + "}"

                + "function TradeDiscountAmountKeyup(e)"
                + "{"
                + "    if (e.keyCode == '13' || e.keyCode == '9')"
                + "        document.getElementById('" + tbVat.ClientID + "').focus();"
                + "    else"
                + "        document.getElementById('" + tbTradeDiscountPercent.ClientID + "').value = '';"
                + "}"

                + "function TradeDiscountAmountKeydown(e)"
                + "{"
                + "    if (e.keyCode == '13' || e.keyCode == '9')"
                + "        return false;"
                + "}";
        }
        public string Script
        {
            get
            {
                if (ViewState["Script"] == null)
                    ViewState["Script"] = "";

                return ViewState["Script"].ToString();
            }
            set
            {
                ViewState["Script"] = value;
            }
        }
        public string Script2
        {
            get
            {
                if (ViewState["Script2"] == null)
                    ViewState["Script2"] = "";

                return ViewState["Script2"].ToString();
            }
            set
            {
                ViewState["Script2"] = value;
            }
        }

        #endregion

        #region UI

        public Button BPrintForm { get { return bPrintForm; } }
        public Button BProductItem { get { return bProductItem; } }
        public Button BCancelPurchase { get { return bCancelPurchase; } }
        public Button BPrintPO { get { return bPrintPO; } }
        public Button BDeletePO { get { return bDeletePO; } }

        public CheckBox CBPrintForm { get { return cbPrintForm; } }
        public CheckBox CBIsPayCheque { get { return cbIsPayCheque; } }
        public CheckBox CBIsPayCash { get { return cbIsPayCash; } }
        public CheckBox CBTradeDiscount { get { return cbTradeDiscountType; } }

        public GridView GVPurchase { get { return gvPurchase; } }
        public GridView GVHire { get { return gvHire; } }
        public GridView GVForm2 { get { return gvForm2; } }

        public Label LCreateDate { get { return lCreateDate; } }
        public Label LCreateBy { get { return lCreateBy; } }
        public Label LUpdateDate { get { return lUpdateDate; } }
        public Label LUpdateBy { get { return lUpdateBy; } }
        public Label LReorderPoint { get { return lReorderPoint; } }

        public TextBox TBTotal { get { return tbTotal; } }
        public TextBox TBTotalDiscount { get { return tbTotalDiscount; } }
        public TextBox TBTotalBeforeVat { get { return tbTotalBeforeVat; } }
        public TextBox TBTotalVat { get { return tbTotalVat; } }
        public TextBox TBGrandTotal { get { return tbGrandTotal; } }
        public TextBox TBObjective { get { return tbObjective; } }
        public TextBox TBQuotationCode1 { get { return tbQuotationCode1; } }
        public TextBox TBQuotationCode2 { get { return tbQuotationCode2; } }
        public TextBox TBCreditTermDay { get { return tbCreditTermDay; } }
        public TextBox TBShippingAt { get { return tbShippingAt; } }
        public TextBox TBTradeDiscountPercent { get { return tbTradeDiscountPercent; } }
        public TextBox TBTradeDiscountAmount { get { return tbTradeDiscountAmount; } }
        public TextBox TBVat { get { return tbVat; } }
        public TextBox TBContractName { get { return tbContractName; } }
        public TextBox TBPOCode { get { return tbPOCode; } }
        public TextBox TBRefPR { get { return tbRefPR; } }
        public TextBox TBPaymentNo { get { return tbPaymentNo; } }

        public DropDownList DDLProject { get { return ddlProject; } }
        public DropDownList DDLSupplier { get { return ucDdlSupplier.FindControl("ddlSupplier") as DropDownList; } }

        public Panel PHire { get { return pHire; } }
        public Panel PPurchase { get { return pPurchase; } }
        public Panel PDetail { get { return pDetail; } }

        public UpdatePanel UPSupplier { get { return upSupplier; } }

        public RadioButtonList RBLItem { get { return rblItem; } }
        public RadioButtonList RBLTypeAsset { get { return rblTypeAsset; } }
        public RadioButtonList RBLPOType { get { return rblPOType; } }
        public RadioButtonList RBLTradeDiscountType { get { return rblTradeDiscountType; } }
        public RadioButtonList RBLVatType { get { return rblVatType; } }
        public RadioButtonList RBLVatUnitType { get { return rblVatUnitType; } }

        public CalendarControl CCPODate { get { return ccPODate; } }
        public CalendarControl CCQuotationDate1 { get { return ccQuotationDate1; } }
        public CalendarControl CCQuotationDate2 { get { return ccQuotationDate2; } }
        public CalendarControl CCShippingDate { get { return ccShippingDate; } }
        public CalendarControl CCReorderPointDate { get { return reorderPoint_Date; } }


        public string OrgID { get { return pb.OrgID; } }
        public string UserName { get { return pb.UserName; } }

        public HttpRequest AttachRequest { get { return Request; } }

        #endregion UI  
  
        public void DisableDetail()
        {
            string js = "document.getElementById('" + rblTradeDiscountType.ClientID + "').disabled = true;"
                      + "document.getElementById('" + tbTradeDiscountPercent.ClientID + "').disabled = true;"
                      + "document.getElementById('" + tbTradeDiscountAmount.ClientID + "').disabled = true;";

            ScriptManager.RegisterStartupScript
            (
                Page
                , typeof(Page)
                , "disable"
                , js
                , true
            );
          
            pDetail.Enabled = false; 
        }

        public void DisableButtons()
        {
            pButtons.Visible = false;
        }
    }
}
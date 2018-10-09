using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using GPlus.PRPO.PRPOHelper;
using GPlus.DataAccess;
using System.IO;
using GPlus.PRPO;

namespace GPlus.UserControls
{
    public partial class PRControl : System.Web.UI.UserControl
    {
        #region ATTRIBUTE
        private PRType prType;
        private Pagebase pb = new Pagebase();
        private DataTable dtPackage;
        #endregion

        #region SCRIPT
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
                    + "document.getElementById('" + bProductSelect.ClientID + "').disabled = false;"
                    + "document.getElementById('" + bPrintForm.ClientID + "').disabled = false;"
                    + "document.getElementById('" + cbPrintForm.ClientID + "').checked = false;";

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
        private void ScriptSaveErrorPopup(string errorMessage)
        {
            errorMessage = HttpUtility.JavaScriptStringEncode("เกิดข้อผิดพลาดระหว่างการบันทึก : " + errorMessage, true);

            ScriptManager.RegisterStartupScript
            (
                this,
                GetType(),
                "Save",
                "alert(" + errorMessage + "); window.location.href = '../PRPO/PRMgt.aspx';",
                true
            );
        }

        private void ScriptSavePopup(string prID)
        {
            Response.Write
            (
                "<script>"
                + "    var screenWidth = (screen.width - 600) / 2;"
                + "    var screenHeight = (screen.height - 300) / 2;"
                + "    window.location.href = '../PRPO/PRMgt.aspx';"
                + "    var popup = window.open('../PRPO/pop_PR.aspx?id=" + prID + "&form=" + (cbPrintForm.Checked ? "1" : "0") + "','pop_PR',\"location=no,menubar=no,scrollbars=no,resizable=yes,status=no,left=\" + screenWidth + \",top=\" + screenHeight + \",width=600,height=300\");"
                + "    if (popup) popup.focus();"
                + "</script>"
            );
        }
        #endregion

        #region OTHER
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeUI();
                bProductSelect.OnClientClick = GPlus.Util.CreatePopUp("pop_ProductSelect2.aspx?main=pr&type=purchase", null, null, "pop_ProductSelect");
            }

            prType = PRFactory.CreatePR(rblPRType.SelectedValue, this);
        }

        public void BindPR(string prId)
        {
            DataTable dtPR = new PRDAO().GetPRForm1(prId);

            if (dtPR.Rows.Count > 0)
            {
                DataRow row = dtPR.Rows[0];
                string prtype = row["PR_Type"].ToString();
                string prStatus = row["Status"].ToString();

                prType = PRFactory.CreatePR(prtype, this);

                prType.BindPR(prId, dtPR);

                if (prStatus == "0")        // ลบข้อมูล
                {
                    pDeletePR.Visible = true;
                    pnlEdit.Enabled = false;
                }
                else if (prStatus == "1")   // รออนุมัติ, ส่งกลับไปแก้ไข
                {
                    bPrint.Visible = true;
                    bDelete.Visible = true;
                    bSave.Visible = true;

                    pDeletePR.Visible = false;
                    pnlEdit.Enabled = true;
                }
                else if (prStatus == "2")   // อนุมัติ
                {
                    bPrint.Visible = true;
                    bDelete.Visible = false;
                    bSave.Visible = false;

                    pDeletePR.Visible = false;
                    pnlEdit.Enabled = true;
                }
                else if (prStatus == "3")
                {
                    bPrint.Visible = true;
                    bSave.Visible = false;
                    bDelete.Visible = false;

                    pDeletePR.Visible = false;
                    pnlEdit.Enabled = true;
                }
                else if (prStatus == "5")   // ออก PO แล้ว
                {
                    bPrint.Visible = true;
                    bSave.Visible = false;
                    bDelete.Visible = false;
                    pDeletePR.Visible = false;
                    pnlEdit.Enabled = true;
                }

                bPrint.OnClientClick = "open_popup('pop_PR.aspx?id=" + prId + "',500, 270, 'pop', 'yes', 'yes', 'yes'); return false;";
            }
        }

        protected void rblPRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeControls();
            prType.IndexChanged();
            //prType.BindGridViewItems();
        }
        #endregion

        #region INITIALIZE
        private void InitializeUI()
        {
            //lCreateBy.Text = pb.UserName;                                   // ผู้ที่สร้าง
            //lUpdateBy.Text = pb.UserName;                                   // ผู้ที่แก้ไขล่าสุด
            lCreateBy.Text = pb.FirstName+' '+ pb.LastName;                  // ผู้ที่สร้าง
            lUpdateBy.Text = pb.FirstName + ' ' + pb.LastName;               // ผู้ที่แก้ไขล่าสุด
            lCreateDate.Text = DateTime.Now.ToString(pb.DateTimeFormat);    // วันที่สร้าง
            lUpdateDate.Text = DateTime.Now.ToString(pb.DateTimeFormat);    // วันที่แก้ไขล่าสุด

            ccRequestDate.Text = DateTime.Now.ToString(pb.DateFormat);      // วันเวลาที่ขอซื้อ

            tbDivPR.Text = pb.OrgName;                                      // หน่วยงานที่ขอซื้อ

            //// โครงการ
            ddlProject.DataSource = new DataAccess.ProjectDAO().GetProject("", "", "1", 1, 1000, "", "").Tables[0];
            ddlProject.DataTextField = "Project_Name";
            ddlProject.DataValueField = "Project_ID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("เลือกโครงการ", ""));
            ddlProject.Width = new Unit(300);

            ddlSupplier.DataSource = new SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
            ddlSupplier.DataTextField = "Supplier_Name";
            ddlSupplier.DataValueField = "Supplier_ID";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));
            ddlSupplier.Width = new Unit(300);

            rblTradeDiscountType.Enabled = false;
            tbTradeDiscountPercent.Enabled = false;
            tbTradeDiscountAmount.Enabled = false;

            cbTradeDiscountType.Enabled = false;
        }
        private void InitializeControls()
        {
            tbObjective.Text = "";
            ddlProject.SelectedIndex = 0;
            cbPrintForm.Checked = false;

            tbQuotationCode.Text = "";
            ccQuotationDate.Text = "";

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

            tbTotal.Text = "";
            tbTotalDiscount.Text = "";
            tbTotalBeforeVat.Text = "";
            tbTotalVat.Text = "";
            tbGrandTotal.Text = "";
        }
        #endregion

        #region CLICK
        protected void bDeletePurchaseItem_Click(object sender, EventArgs e)
        {
            prType.DeleteGridViewItems();
            SetUIsWhenGridViewHasNoRow(gvPurchase);
        }
        protected void bDeleteHireItem_Click(object sender, EventArgs e)
        {
            prType.DeleteGridViewItems();
        }
        protected void Refresh_Click(object sender, EventArgs e)
        {
            prType.BindGridViewItems();
        }
        protected void bSave_Click(object sender, EventArgs e)
        {
            if (PRPOSession.Action == PRPOAction.ADD_PR)
            {
                try
                {
                    string prID = prType.Save(Request);
                    ScriptSavePopup(prID);
                }
                catch (Exception ex)
                {
                    ScriptSaveErrorPopup(ex.Message);
                }
            }
            else if (PRPOSession.Action == PRPOAction.VIEW_PR)
            {
                try
                {
                    prType.Update(Request);
                    ScriptSavePopup(PRPOSession.PrID);
                }
                catch (Exception ex)
                {
                    ScriptSaveErrorPopup(ex.Message);
                }
            }
        }
        protected void bDelete_Click(object sender, EventArgs e)
        {
            PRDAO2 prDao = new PRDAO2();
            try
            {
                prDao.BeginTransaction();
                prDao.DeletePR(PRPOSession.PrID);
                prDao.CommitTransaction();

                ScriptManager.RegisterStartupScript
                (
                    Page
                    , typeof(Page)
                    , "deletePR"
                    , "alert('ลบข้อมูลเรียบร้อย'); window.location.href = '../PRPO/PRMgt.aspx';"
                    , true
                );
            }
            catch (Exception)
            {
                prDao.RollbackTransaction();

                ScriptManager.RegisterStartupScript
                (
                    Page
                    , typeof(Page)
                    , "deletePR"
                    , "alert('เกิดข้อผิดพลาดระหว่างการลบ'); window.location.href = '../PRPO/PRMgt.aspx';"
                    , true
                );
            }
        }
        protected void bClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PRPO/PRMgt.aspx");
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

            ((PRHire)prType).SaveGridViewForm2(pfat);

            pfat.AddPrForm2Row();

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
                    HiddenField hfPrForm2ID = r.FindControl("hfPrForm2ID") as HiddenField;

                    pfdt.AddItem(hfPrForm2ID.Value);

                    pfat.DeleteItem("", hfPrForm2ID.Value);
                }
            }

            ((PRHire)prType).SaveGridViewForm2(pfat);

            gvForm2.DataSource = pfat.Table;
            gvForm2.DataBind();
        }
        protected void bAddHireItem_Click(object sender, EventArgs e)
        {
            PRPOHireActualTable phat = new PRPOHireActualTable(PRPOSession.HireActualTable);

            //prType.SaveGridViewItems();
            phat.AddItem();
            prType.BindGridViewItems();
        }

        #endregion

        #region ROW_DATA_BOUND
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

        DataTable dtExpense = null;         // Use for gvForm2_RowDataBound
        DataTable dtAccExpense = null;      // Use for gvForm2_RowDataBound
        string prID = "";                   // Use for gvForm2_RowDataBound
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
                HiddenField hfInvSpecPurchase = e.Row.FindControl("hfInvSpecPurchase") as HiddenField;

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

                ((Literal)e.Row.FindControl("lbInvSpecPurchase")).Text = 
                    "<a href=\"javascript:open_popup('pop_SpecPurchase.aspx?ctl=" 
                    + hfInvSpecPurchase.ClientID
                    + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">...</a>";

                hfItemID.Value = itemID;
                hfPackID.Value = packID;
                hfPrID.Value = prID;
                hfPrItemID.Value = prItemID;
                hfPoItemID.Value = poItemID;

                tbProcureName.Text = procureName;
                tbSpecify.Text = specify;
                hfInvSpecPurchase.Value = specPurchase;

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

        #region UIs

        public Button BPrintForm { get { return bPrintForm; } }
        public Button BProductSelect { get { return bProductSelect; } }

        public CheckBox CBTradeDiscountType { get { return cbTradeDiscountType; } }
        public CheckBox CBPrintForm { get { return cbPrintForm; } }

        public CalendarControl CCRequestDate { get { return ccRequestDate; } }
        public CalendarControl CCQuotationDate { get { return ccQuotationDate; } }

        public DropDownList DDLProject { get { return ddlProject; } }
        public DropDownList DDLSupplier { get { return ddlSupplier; } }

        public GridView GVForm2 { get { return gvForm2; } }
        public GridView GVHire { get { return gvHire; } }
        public GridView GVPurchase { get { return gvPurchase; } }

        public Label LCreateDate { get { return lCreateDate; } }
        public Label LCreateBy { get { return lCreateBy; } }
        public Label LUpdateDate { get { return lUpdateDate; } }
        public Label LUpdateBy { get { return lUpdateBy; } }

        public Panel PPurchase { get { return pPurchase; } }
        public Panel PHire { get { return pHire; } }

        public RadioButtonList RBLPRType { get { return rblPRType; } }
        public RadioButtonList RBLTradeDiscountType { get { return rblTradeDiscountType; } }
        public RadioButtonList RBLVatType { get { return rblVatType; } }
        public RadioButtonList RBLVatUnitType { get { return rblVatUnitType; } }

        public TextBox TBPRCode { get { return tbPRCode; } }
        public TextBox TBObjective { get { return tbObjective; } }
        public TextBox TBQuotationCode { get { return tbQuotationCode; } }
        public TextBox TBTradeDiscountPercent { get { return tbTradeDiscountPercent; } }
        public TextBox TBTradeDiscountAmount { get { return tbTradeDiscountAmount; } }
        public TextBox TBVat { get { return tbVat; } }
        public TextBox TBTotal { get { return tbTotal; } }
        public TextBox TBTotalDiscount { get { return tbTotalDiscount; } }
        public TextBox TBTotalBeforeVat { get { return tbTotalBeforeVat; } }
        public TextBox TBTotalVat { get { return tbTotalVat; } }
        public TextBox TBGrandTotal { get { return tbGrandTotal; } }
        public TextBox TBRefPO { get { return tbRefPO; } }

        #endregion

        public void Disable()
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

            pnlEdit.Enabled = false;
        }
    }
}
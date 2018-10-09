using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.PRPO.PRPOHelper;

using GPlus.DataAccess;
using GPlus.UserControls;

namespace GPlus.PRPO
{
    public partial class pop_PrintForm2 : Pagebase
    {
        private PrintFormType FormType;

        protected void Page_Load(object sender, EventArgs e)
        {
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

            if (!IsPostBack)
            {
                TbOldDiv.Text = OrgName;
                TbNewDiv.Text = OrgName;
            }

            if (ppft.GetFormType() != "" && !IsPostBack)
            {
                FormType = PrintFormFactory.CreatePrintForm(this, ppft.GetFormType());
                FormType.IndexChanged();
                FormType.LoadForm(ppft);
            }
            else
                FormType = PrintFormFactory.CreatePrintForm(this, RBLFormType.SelectedValue);

            SubscribeButtonSelect();
            ScriptUpdateQuantity();

            ScriptManager.RegisterStartupScript
            (
                Page
                , typeof(Page)
                , "popPrintForm"
                , Script()
                , true
            );

            ppft.Table.Rows[0]["TradeDiscountPercent"] = "";
            ppft.Table.Rows[0]["TradeDiscountAmount"] = "";
            ppft.Table.Rows[0]["TotalBeforeVat"] = "";
            ppft.Table.Rows[0]["VatPercent"] = "";
            ppft.Table.Rows[0]["VatAmount"] = "";
            ppft.Table.Rows[0]["NetAmount"] = "";

            ppft.Table.AcceptChanges();
        }

        private void ScriptUpdateQuantity()
        {
            string js = "if (window.opener)"
                      + "{"
                      + "    var gvPurchase = window.opener.document.getElementById('ContentPlaceHolder1_POControl1_gvPurchase');"
                      + "    if (gvPurchase != null)"
                      + "    {"
                      + "        var unitQuantity = gvPurchase.rows[1].cells[6].getElementsByTagName('input')[0].value;"
                      + "        if (document.getElementById('" + RblFormType.ClientID + "_0').checked)"
                      + "        {"
                      + "            if (document.getElementById('" + RbNewFormBorrowType1.ClientID + "').checked)"
                      + "            {"
                      + "                document.getElementById('" + TbNewBorrowQuantity.ClientID + "').value = unitQuantity;"
                      + "            }"
                      + "            if (document.getElementById('" + RbNewFormBorrowType2.ClientID + "').checked)"
                      + "            {"
                      + "                document.getElementById('" + TbNewUnitQuantity.ClientID + "').value = unitQuantity;"
                      + "            }"
                      + "        }"
                      + "        else if (document.getElementById('" + RblFormType.ClientID + "_1').checked)"
                      + "        {"
                      + "            if (document.getElementById('" + RbOldFormBorrowType1.ClientID + "').checked)"
                      + "            {"
                      + "                document.getElementById('" + TbOldBorrowQuantity.ClientID + "').value = unitQuantity;"
                      + "            }"
                      + "            if (document.getElementById('" + RbOldFormBorrowType2.ClientID + "').checked)"
                      + "            {"
                      + "                document.getElementById('" + TbOldUnitQuantity.ClientID + "').value = unitQuantity;"
                      + "            }"
                      + "        }"
                      + "    }"
                      + "}";


            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "script"
                , js
                , true
            );
        }

        private void SubscribeButtonSelect()
        {
            BtnNewSelect.OnClientClick = Util.CreatePopUp
            (
                "pop_ProductPrintForm.aspx",
                new string[] { "ItemID", "PackID", "ItemCode", "ItemName" },
                new string[] { HfNewItemID.ClientID, HfNewPackID.ClientID, TbNewFormPrintCode.ClientID, TbNewFormPrintName.ClientID },
                "pop_ProductPrintForm"
            );

            BtnOldSelect.OnClientClick = Util.CreatePopUp
            (
                "pop_ProductPrintForm.aspx",
                new string[] { "ItemID", "PackID", "ItemCode", "ItemName" },
                new string[] { HfOldItemID.ClientID, HfOldPackID.ClientID, TbOldFormPrintCode.ClientID, TbOldFormPrintName.ClientID },
                "pop_ProductPrintForm"
            );
        }

        protected void BtnOK_Click(object sender, EventArgs e)
        {
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);

            FormType.Save(ppft);

            ScriptManager.RegisterStartupScript
            (
                this,
                GetType(),
                "PrintForm",
                "if (window.opener) { " +
                "    window.opener.document.getElementById('btnRefreshI').click();" +
                "}" + 
                "window.close();"
                , true
            );
        }

        protected void btnRefreshSelect_Click(object sender, EventArgs e)
        {
            btnRefreshSelect.Attributes.Add("style", "display:none");

            string scriptSharedUIs = FormType.Refresh();

            ScriptManager.RegisterClientScriptBlock
            (
                Page
                , typeof(Page)
                , "PrintForm"
                , Script() + scriptSharedUIs
                , true
            );
        }

        private string Script()
        {
            string js = "";

            if (RblFormType.SelectedValue == "0")
            {
                js = "function RbNewFormBorrowType1_Click()"
                   + "{"
                   + "    document.getElementById('" + TbNewBorrowMonthQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbNewBorrowFirstQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbNewUnitQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbNewBorrowMonthQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbNewBorrowFirstQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbNewUnitQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + DdlNewBorrowMonthUnit.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbNewBorrowQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + DdlNewBorrowUnit.ClientID + "').disabled = false;"
                   + "}"

                   + "function RbNewFormBorrowType2_Click()"
                   + "{"
                   + "    document.getElementById('" + TbNewBorrowQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbNewBorrowQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + DdlNewBorrowUnit.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbNewBorrowMonthQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + TbNewBorrowFirstQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + TbNewUnitQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + DdlNewBorrowMonthUnit.ClientID + "').disabled = false;"
                   + "}"

                   + "document.getElementById('" + DdlNewBorrowMonthUnitDisabled.ClientID + "').selectedIndex = document.getElementById('" + DDLNewBorrowMonthUnit.ClientID + "').selectedIndex;"
                   + "document.getElementById('" + DdlNewBorrowMonthUnit.ClientID + "').onchange = "
                   + "function ()"
                   + "{"
                   + "    document.getElementById('" + DdlNewBorrowMonthUnitDisabled.ClientID + "').selectedIndex = this.selectedIndex;"
                   + "};"

                   + "document.getElementById('" + RbNewFormBorrowType1.ClientID + "').onclick = RbNewFormBorrowType1_Click;"
                   + "document.getElementById('" + RbNewFormBorrowType2.ClientID + "').onclick = RbNewFormBorrowType2_Click;"

                   + "if (document.getElementById('" + RbNewFormBorrowType1.ClientID + "').checked)"
                   + "    RbNewFormBorrowType1_Click();"
                   + "if (document.getElementById('" + RbNewFormBorrowType2.ClientID + "').checked)"
                   + "    RbNewFormBorrowType2_Click();";

            }
            else if (RblFormType.SelectedValue == "1")
            {
                js = "function RbOldFormBorrowType1_Click()"
                   + "{"
                   + "    document.getElementById('" + TbOldBorrowFirstQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbOldBorrowMonthQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbOldUnitQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbOldBorrowFirstQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbOldBorrowMonthQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbOldUnitQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + DdlOldBorrowMonthUnit.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbOldBorrowQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + DdlOldBorrowUnit.ClientID + "').disabled = false;"
                   + "}"

                   + "function RbOldFormBorrowType2_Click()"
                   + "{"
                   + "    document.getElementById('" + TbOldBorrowQuantity.ClientID + "').value = '';"
                   + "    document.getElementById('" + TbOldBorrowQuantity.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + DdlOldBorrowUnit.ClientID + "').disabled = true;"
                   + "    document.getElementById('" + TbOldBorrowMonthQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + TbOldBorrowFirstQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + TbOldUnitQuantity.ClientID + "').disabled = false;"
                   + "    document.getElementById('" + DdlOldBorrowMonthUnit.ClientID + "').disabled = false;"
                   + "}"

                   + "document.getElementById('" + DdlOldBorrowMonthUnitDisabled.ClientID + "').selectedIndex = document.getElementById('" + DDLOldBorrowMonthUnit.ClientID + "').selectedIndex;"
                   + "document.getElementById('" + DdlOldBorrowMonthUnit.ClientID + "').onchange = "
                   + "function ()"
                   + "{"
                   + "    document.getElementById('" + DdlOldBorrowMonthUnitDisabled.ClientID + "').selectedIndex = this.selectedIndex;"
                   + "};"

                   + "document.getElementById('" + RbOldFormBorrowType1.ClientID + "').onclick = RbOldFormBorrowType1_Click;"
                   + "document.getElementById('" + RbOldFormBorrowType2.ClientID + "').onclick = RbOldFormBorrowType2_Click;"

                   + "if (document.getElementById('" + RbOldFormBorrowType1.ClientID + "').checked)"
                   + "    RbOldFormBorrowType1_Click();"
                   + "if (document.getElementById('" + RbOldFormBorrowType2.ClientID + "').checked)"
                   + "    RbOldFormBorrowType2_Click();";
            }
            return js;
        }

        protected void RblFormType_IndexChanged(object sender, EventArgs e)
        {
            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);
            FormType = PrintFormFactory.CreatePrintForm(this, RBLFormType.SelectedValue);

            ccBorrowDate.Text = "";
            TbRemark2.Text = "";
            RbBorrowType0.Checked = false;
            RbBorrowType1.Checked = false;
            RbBorrowType2.Checked = false;
            RbBorrowType3.Checked = false;

            FormType.IndexChanged();

            FormType.LoadForm(ppft);
        }

        #region share type elements
        public RadioButtonList RBLFormType { get { return RblFormType; } }

        public RadioButton RBBorrowType0 { get { return RbBorrowType0; } }
        public RadioButton RBBorrowType1 { get { return RbBorrowType1; } }
        public RadioButton RBBorrowType2 { get { return RbBorrowType2; } }
        public RadioButton RBBorrowType3 { get { return RbBorrowType3; } }
        public TextBox TBRemark2 { get { return TbRemark2; } }
        public CalendarControl CCBorrowDate { get { return ccBorrowDate; } }

        #endregion share type elements

        #region new type elements
        public DropDownList DDLNewBorrowUnit { get { return DdlNewBorrowUnit; } }
        public DropDownList DDLNewBorrowMonthUnit { get { return DdlNewBorrowMonthUnit; } }
        public DropDownList DDLNewBorrowMonthUnitDisabled { get { return DdlNewBorrowMonthUnitDisabled; } }

        public TextBox TBNewFormPrintCode { get { return TbNewFormPrintCode; } }
        public TextBox TBNewFormPrintName { get { return TbNewFormPrintName; } }
        public TextBox TBNewPaperType { get { return TbNewPaperType; } }
        public TextBox TBNewPaperColor { get { return TbNewPaperColor; } }
        public TextBox TBNewPaperGram { get { return TbNewPaperGram; } }
        public TextBox TBNewFontColor { get { return TbNewFontColor; } }
        public TextBox TBNewBorrowQuantity { get { return TbNewBorrowQuantity; } }
        public TextBox TBNewBorrowMonthQuantity { get { return TbNewBorrowMonthQuantity; } }
        public TextBox TBNewBorrowFirstQuantity { get { return TbNewBorrowFirstQuantity; } }
        public TextBox TBNewSizeDetail { get { return TbNewSizeDetail; } }
        public TextBox TBNewRemark { get { return TbNewRemark; } }
        public TextBox TBNewUnitQuantity { get { return TbNewUnitQuantity; } }

        public RadioButtonList RBLNewFormat { get { return RblNewFormat; } }
        public RadioButtonList RBLNewPrintType { get { return RblNewPrintType; } }

        public RadioButton RBNewFormBorrowType1 { get { return RbNewFormBorrowType1; } }
        public RadioButton RBNewFormBorrowType2 { get { return RbNewFormBorrowType2; } }

        public Panel PNLNew { get { return PnlNew; } }

        public HiddenField HFNewItemID { get { return HfNewItemID; } }
        public HiddenField HFNewPackID { get { return HfNewPackID; } }

        public UpdatePanel UPPnlNew { get { return upPnlNew; } }

        #endregion new type elements

        # region old type elements
        public DropDownList DDLOldBorrowUnit { get { return DdlOldBorrowUnit; } }
        public DropDownList DDLOldBorrowMonthUnit { get { return DdlOldBorrowMonthUnit; } }
        public DropDownList DDLOldBorrowMonthUnitDisabled { get { return DdlOldBorrowMonthUnitDisabled; } }

        public TextBox TBOldFormPrintCode { get { return TbOldFormPrintCode; } }
        public TextBox TBOldFormPrintName { get { return TbOldFormPrintName; } }
        public TextBox TBOldPaperType { get { return TbOldPaperType; } }
        public TextBox TBOldPaperColor { get { return TbOldPaperColor; } }
        public TextBox TBOldPaperGram { get { return TbOldPaperGram; } }
        public TextBox TBOldFontColor { get { return TbOldFontColor; } }
        public TextBox TBOldBorrowQuantity { get { return TbOldBorrowQuantity; } }
        public TextBox TBOldBorrowMonthQuantity { get { return TbOldBorrowMonthQuantity; } }
        public TextBox TBOldBorrowFirstQuantity { get { return TbOldBorrowFirstQuantity; } }
        public TextBox TBOldSizeDetail { get { return TbOldSizeDetail; } }
        public TextBox TBOldRequestModifyDesc { get { return TbOldRequestModifyDesc; } }
        public TextBox TBOldRemark { get { return TbOldRemark; } }
        public TextBox TBOldUnitQuantity { get { return TbOldUnitQuantity; } }

        public Panel PNLOld { get { return PnlOld; } }

        public RadioButton RBOldFormBorrowType1 { get { return RbOldFormBorrowType1; } }
        public RadioButton RBOldFormBorrowType2 { get { return RbOldFormBorrowType2; } }

        public RadioButtonList RBLOldPrintType { get { return RblOldPrintType; } }

        public CheckBox CBOldIsRequestModify { get { return CbOldIsRequestModify; } }
        public CheckBox CBOldIsFixedContent { get { return CbOldIsFixedContent; } }
        public CheckBox CBOldIsPaper { get { return CbOldIsPaper; } }
        public CheckBox CBOldIsFont { get { return CbOldIsFont; } }

        public HiddenField HFOldItemID { get { return HfOldItemID; } }
        public HiddenField HFOldPackID { get { return HfOldPackID; } }

        public UpdatePanel UPPnlOld { get { return upPnlOld; } }

        #endregion old type elements
    }
}
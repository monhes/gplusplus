using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GPlus.UserControls;
using System.Data;
using GPlus.DataAccess;

namespace GPlus.PRPO.PRPOHelper
{
    public abstract class PRType : Pagebase
    {
        protected PRControl PRCtrl;

        protected PRType(PRControl control) 
        { 
            PRCtrl = control;  
        }

        public abstract string Save(HttpRequest request);
        public abstract void Update(HttpRequest request);

        public abstract void BindGridViewItems();
        public abstract void SaveGridViewItems();
        public abstract void DeleteGridViewItems();
        public abstract string GetPRType();
        public abstract void IndexChanged();

        public virtual void BindPR(string prId, DataTable dtPR)
        {
            if (dtPR.Rows.Count < 1)
                return;

            DataRow row = dtPR.Rows[0];

            PRCtrl.TBPRCode.Text = row["PR_Code"].ToString();
            PRCtrl.CCRequestDate.Text = row["Request_Date"].ToString();
            PRCtrl.DDLSupplier.SelectedValue = row["Supplier_ID"].ToString();
            PRCtrl.TBQuotationCode.Text = row["Quotation_code"].ToString();
            PRCtrl.CCQuotationDate.Text = row["Quotation_Date"].ToString();
            PRCtrl.CBPrintForm.Checked = row["Have_PrintForm"].ToString() == "1" ? true : false;
            PRCtrl.CBTradeDiscountType.Checked = row["HaveDiscount"].ToString() == "1" ? true : false;

            if (row["TradeDiscount_Type"].ToString().Trim() == "")
                PRCtrl.RBLTradeDiscountType.SelectedIndex = -1;
            else
                PRCtrl.RBLTradeDiscountType.SelectedValue = row["TradeDiscount_Type"].ToString();

            PRCtrl.TBTradeDiscountPercent.Text = PRPOUtility.ToIntegerString(row["TradeDiscount_Percent"].ToString());
            PRCtrl.TBTradeDiscountAmount.Text = PRPOUtility.To2PointString(row["TradeDiscount_Amount"].ToString());
            PRCtrl.RBLVatType.SelectedValue = row["Vat_Type"].ToString();
            PRCtrl.TBVat.Text = PRPOUtility.ToIntegerString(row["Vat"].ToString());
            PRCtrl.RBLVatUnitType.SelectedValue = row["VatUnit_Type"].ToString();
            PRCtrl.LCreateDate.Text = row["Create_Date"].ToString();
            //PRCtrl.LCreateBy.Text = row["Create_By"].ToString();
            PRCtrl.LCreateBy.Text = row["Create_By_FullName"].ToString();
            PRCtrl.LUpdateDate.Text = row["Update_Date"].ToString();
            //PRCtrl.LUpdateBy.Text = row["Update_By"].ToString();
            PRCtrl.LUpdateBy.Text = row["Update_By_FullName"].ToString();

            PRCtrl.TBTotal.Text = PRPOUtility.To2PointString(row["Total_Price"].ToString());
            PRCtrl.TBTotalDiscount.Text = PRPOUtility.To2PointString(row["Total_Discount"].ToString());
            PRCtrl.TBTotalBeforeVat.Text = PRPOUtility.To2PointString(row["Total_Before_Vat"].ToString());
            PRCtrl.TBTotalVat.Text = PRPOUtility.To2PointString(row["Vat_Amount"].ToString());
            PRCtrl.TBGrandTotal.Text = PRPOUtility.To2PointString(row["Net_Amonut"].ToString());

            PRCtrl.RBLPRType.Enabled = false;
            PRCtrl.TBRefPO.Text = "";
            PRCtrl.TBRefPO.Text = GetRefPO(prId);
        }

        private static string GetRefPO(string prID)
        {
            DataTable dt = new PRDAO2().GetRefPO(Convert.ToInt32(prID));

            string refPo = "";
            foreach (DataRow row in dt.Rows)
                refPo += row["PO_Code"].ToString() + ", ";

            if (!string.IsNullOrEmpty(refPo))
                refPo = refPo.Remove(refPo.Length - 2);

            return refPo;
        }

        public virtual string GetPRCode()
        {
            string prCode = "PR-";
            Pagebase pb = new Pagebase();
            if (pb.OrgID == pb.PurchaseDivID)
                prCode += "SS-";
            else
                prCode += "SD-";
            return prCode;
        }
    }

    public static class PRFactory
    {
        public static PRType CreatePR(string type, PRControl ctrl)
        {
            if (type == "1")                    // ขอซื้อ
                return new PRPurchase(ctrl);
            else if (type == "2")               // ขอจ้าง
                return new PRHire(ctrl);
            return null;
        }
    }
}
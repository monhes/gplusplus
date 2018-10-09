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
    public abstract class POType : Pagebase
    {
        protected POControl POCtrl;

        public abstract void IndexChanged();
        public abstract void BindGridviewItems();
        public abstract void DeleteGridviewItems();
        public abstract string Save();
        public abstract void Update();
        //public abstract void DeletePO();
        public abstract string GetPOType();
        //public abstract void CancelPO();

        protected virtual void UpdatePrForm1Status(PODAO2 poDao2, int poID, Action<DataRow> UpdatePrItemStatusAction = null)
        {
            DataTable dtPoPrItem = poDao2.GetPoPrByPoID(poID);

            if (UpdatePrItemStatusAction != null)
            {
                DataRow[] rows = dtPoPrItem.Select(string.Format("PO_ID = '{0}'", poID));

                foreach (DataRow r in rows)
                {
                    string prItemID = r["PRItem_ID"].ToString();
                    UpdatePrItemStatusAction(r);
                }
            }

            var result = from row in dtPoPrItem.AsEnumerable()
                         group row by row["PR_ID"] into grp
                         select new { grp.Key, itemStatus = grp.Select(Col => Col["Item_Status"]) };

            string[] prItemStatus = { "1", "2" };
            foreach (var group in result)
            {
                bool found = false;
                string prId = group.Key.ToString();

                foreach (var itemStat in group.itemStatus)
                {
                    if (prItemStatus.Contains(itemStat.ToString()))
                    {
                        poDao2.UpdatePRForm1(prId, "5");
                        found = true;
                        break;
                    }
                }
                // ไม่พบสถานะเป็น "1" หรือ "2"
                if (!found)
                    poDao2.UpdatePRForm1(prId, "2");
            }
        }

        public virtual void DeletePO()
        {
            PODAO2 poDao2 = new PODAO2();

            try
            {
                poDao2.BeginTransaction();

                // กำหนดค่าสถานะของ PO เป็น "0"
                poDao2.DeletePO(PRPOSession.PoID);

                UpdatePrForm1Status
                (
                    poDao2
                    , Convert.ToInt32(PRPOSession.PoID)
                    , (DataRow row) =>
                    {
                        string prItemID = row["PRItem_ID"].ToString();
                        poDao2.UpdatePrItemStatus(Convert.ToInt32(prItemID), null);
                        row["Item_Status"] = DBNull.Value;
                        row.AcceptChanges();
                    }
                );

                poDao2.CommitTransaction();
            }
            catch (Exception ex)
            {
                poDao2.RollbackTransaction();
                throw ex;
            }
        }

        public virtual void CancelPO(string poId = "")
        {
            PODAO2 poDao2 = new PODAO2();

            if (string.IsNullOrEmpty(poId))
                poId = PRPOSession.PoID;

            try
            {
                poDao2.BeginTransaction();

                poDao2.UpdatePOForm1Status(Convert.ToInt32(poId), "5");

                UpdatePrForm1Status
                (
                    poDao2
                    , Convert.ToInt32(poId)
                    , (DataRow row) =>
                    {
                        string prItemID = row["PRItem_ID"].ToString();
                        poDao2.UpdatePrItemStatus(Convert.ToInt32(prItemID), "2");   // ไม่ดำเนินการสั่งซื้อ
                        row["Item_Status"] = "2";
                        row.AcceptChanges();
                    }
                );

                poDao2.CommitTransaction();
            }
            catch (Exception ex)
            {
                poDao2.RollbackTransaction();
                throw ex;
            }
        }

        public virtual string GetPOCode()
        {
            return "PO-" + PRPOUtility.MapAssetType(POCtrl.RBLTypeAsset.SelectedValue) + "-";
        }

        public virtual void BindPO(string poID, DataTable dtPO)
        {
            if (dtPO.Rows.Count < 1)
                return;

            DataRow row = dtPO.Rows[0];

            POCtrl.TBPOCode.Text                        = row["PO_Code"].ToString();
            POCtrl.RBLTypeAsset.SelectedValue           = row["Type_Inv_Asset"].ToString();
            POCtrl.CCPODate.Text                        = row["PO_Date"].ToString();
            POCtrl.CBPrintForm.Checked                  = row["Have_PrintForm"].ToString() == "1" ? true : false;
            POCtrl.DDLSupplier.SelectedValue            = row["Supplier_ID"].ToString();
            POCtrl.TBQuotationCode1.Text                = row["Quotation_Code1"].ToString();
            POCtrl.CCQuotationDate1.Text                = row["Quotation_Date1"].ToString();
            POCtrl.TBQuotationCode2.Text                = row["Quotation_Code2"].ToString();
            POCtrl.CCQuotationDate2.Text                = row["Quotation_Date2"].ToString();
            POCtrl.CBIsPayCheque.Checked                = row["Is_PayCheque"].ToString() == "1" ? true : false;
            POCtrl.CBIsPayCash.Checked                  = row["Is_PayCash"].ToString() == "1" ? true : false;
            POCtrl.TBCreditTermDay.Text                 = row["CreditTerm_Day"].ToString();
            POCtrl.CCShippingDate.Text                  = row["Shipping_Date"].ToString();
            POCtrl.TBShippingAt.Text                    = row["Shipping_At"].ToString();
            POCtrl.CBTradeDiscount.Checked              = row["HaveDiscount"].ToString() == "1" ? true : false;
            

            if (row["TradeDiscount_Type"].ToString().Trim() == "")
                POCtrl.RBLTradeDiscountType.SelectedIndex = -1;
            else
                POCtrl.RBLTradeDiscountType.SelectedValue = row["TradeDiscount_Type"].ToString();
            
            POCtrl.TBTradeDiscountPercent.Text          = PRPOUtility.ToIntegerString(row["TradeDiscount__Percent"].ToString());
            POCtrl.TBTradeDiscountAmount.Text           = PRPOUtility.To2PointString(row["TradeDiscount__Amount"].ToString());
            POCtrl.RBLVatType.SelectedValue             = row["Vat_Type"].ToString();
            POCtrl.TBVat.Text                           = PRPOUtility.ToIntegerString(row["Vat"].ToString());
            POCtrl.RBLVatUnitType.SelectedValue         = row["VatUnit_Type"].ToString();
            POCtrl.LCreateDate.Text                     = row["Create_Date"].ToString();
            //POCtrl.LCreateBy.Text                       = row["Create_By"].ToString();
            POCtrl.LCreateBy.Text                       = row["Create_By_FullName"].ToString();
            POCtrl.LUpdateDate.Text                     = row["Update_Date"].ToString();
            //POCtrl.LUpdateBy.Text                       = row["Update_By"].ToString();
            POCtrl.LUpdateBy.Text                       = row["Update_By_FullName"].ToString();
            POCtrl.TBTotal.Text                         = PRPOUtility.To2PointString(row["Total_Price"].ToString());
            POCtrl.TBTotalDiscount.Text                 = PRPOUtility.To2PointString(row["Total_Discount"].ToString());
            POCtrl.TBTotalBeforeVat.Text                = PRPOUtility.To2PointString(row["Total_Before_Vat"].ToString());
            POCtrl.TBTotalVat.Text                      = PRPOUtility.To2PointString(row["Vat_Amount"].ToString());
            POCtrl.TBGrandTotal.Text                    = PRPOUtility.To2PointString(row["Net_Amonut"].ToString());
            POCtrl.TBContractName.Text                  = row["Contract_Name"].ToString();

            POCtrl.RBLTypeAsset.Enabled = false;

            POCtrl.RBLPOType.Enabled = false;
            POCtrl.BCancelPurchase.Visible = true;
            POCtrl.BPrintPO.Visible = true;

            POCtrl.TBRefPR.Text = "";
            POCtrl.TBRefPR.Text = GetRefPR(poID);
            POCtrl.TBPaymentNo.Text = "";
            POCtrl.TBPaymentNo.Text = row["Payment_No"].ToString();

            POCtrl.CCReorderPointDate.Text = row["ReorderPoint_Date"].ToString();
        }

        private static string GetRefPR(string poID)
        {
            DataTable dt = new PODAO2().GetRefPR(Convert.ToInt32(poID));

            string refPr = "";
            foreach (DataRow row in dt.Rows)
                refPr += row["PR_Code"].ToString() + ", ";

            if (!string.IsNullOrEmpty(refPr))
                refPr = refPr.Remove(refPr.Length - 2);

            return refPr;
        }

        protected POType(POControl control) { 
            POCtrl = control;  
        }

        public abstract string GetScript();
    }

    public static class POFactory
    {
        public static POType CreatePO(string type, POControl ctrl)
        {
            if (type == "1")                    // สั่งซื้อ
                return new POPurchase(ctrl);
            else if (type == "2")               // สั่งจ้าง
                return new POHire(ctrl);
            return null;
        }
    }
}
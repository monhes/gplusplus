using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.ModelClass;

namespace GPlus.PRPO
{
    public partial class pop_POPrintFormReport : Pagebase
    {
        /// <summary>
        /// ราคารวมสุทธิของ Invoice ทั้งใบ
        /// </summary>
        public decimal Net_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// [Output] ราคารวมของทั้ง Invoice คิดจากทุกรายการของ UnitPrice * จำนวนสั่ง
        /// </summary>
        public decimal Total_Price
        {
            get;
            set;
        }

        /// <summary>
        /// [Output] ส่วนลดรวมของ Invoice ทั้งใบ
        /// </summary>
        public decimal Total_Discount
        {
            get;
            set;
        }

        /// <summary>
        /// [Output] ราคารวมของทั้ง Invoice ก่อน vat
        /// </summary>
        public decimal Total_Before_Vat
        {
            get;
            set;
        }

        /// <summary>
        ///  [Output] ราคา vat รวมของ Invoice ทั้งใบ
        /// </summary>
        public decimal Vat_Amount
        {
            get;
            set;
        }

        public List<ReceiveStkItemModel> ReceiveStkItemList
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null)
                {
                    DataSet ds = new DataAccess.PODAO().GetPOPrintFormReport(Request["id"]);
                    calculatePrice(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string printType = ds.Tables[0].Rows[0]["Print_Type"].ToString();
                        //0 = ฟอร์มคอมพิวเตอร์,  1 = เข้าเล่ม, 2 = เข้าชุด 3 = แผ่น, 4 = โบรชัว
                        switch (printType)
                        {
                            case "0": ds.Tables[0].Rows[0]["Print_Type"] = "ฟอร์มคอมพิวเตอร์"; break;
                            case "1": ds.Tables[0].Rows[0]["Print_Type"] = "เข้าเล่ม"; break;
                            case "2": ds.Tables[0].Rows[0]["Print_Type"] = "เข้าชุด"; break;
                            case "3": ds.Tables[0].Rows[0]["Print_Type"] = "แผ่น"; break;
                            case "4": ds.Tables[0].Rows[0]["Print_Type"] = "แผ่นพับ"; break;
                        }
                        ds.Tables[0].Columns.Add("Max_Receive_Date_TH");
                        ds.Tables[0].Columns.Add("Shipping_Date_TH");
                        ds.Tables[0].Columns.Add("Create_Date_TH");
                        //ds.Tables[0].Columns.Add("Receive_Date_TH");
                        ds.Tables[0].Columns.Add("Form1_PO_Date_TH");
                        ds.Tables[0].Columns.Add("Form1_Print_PaperDetail");
                        ds.Tables[0].Columns.Add("ReorderPoint_Date_TH");

                        if (ds.Tables[0].Rows[0]["Max_Receive_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Max_Receive_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Max_Receive_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Max_Receive_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Max_Receive_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }
                        if (ds.Tables[0].Rows[0]["Shipping_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Shipping_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }
                        if (ds.Tables[0].Rows[0]["Create_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Create_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Create_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Create_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Create_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        //if (ds.Tables[0].Rows[0]["Receive_Date"].ToString().Length > 0)
                        //{
                        //    int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Receive_Date"]).Year;
                        //    if (dateYear < 2500)
                        //        dateYear += 543;
                        //    ds.Tables[0].Rows[0]["Receive_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Receive_Date"]).Day.ToString() + "/" +
                        //        ((DateTime)ds.Tables[0].Rows[0]["Receive_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        //}

                        if (ds.Tables[0].Rows[0]["Form1_PO_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Form1_PO_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Form1_PO_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Form1_PO_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Form1_PO_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        if (ds.Tables[0].Rows[0]["ReorderPoint_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["ReorderPoint_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["ReorderPoint_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["ReorderPoint_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["ReorderPoint_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        string paper_detail = "";

                        if (ds.Tables[0].Rows[0]["Paper_Type"].ToString().Length > 0)
                        {
                            paper_detail = paper_detail + ds.Tables[0].Rows[0]["Paper_Type"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["Paper_Color"].ToString().Length > 0)
                        {
                            paper_detail = paper_detail + " สี" + ds.Tables[0].Rows[0]["Paper_Color"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["Paper_Gram"].ToString().Length > 0)
                        {
                            paper_detail = paper_detail + " หนา " + ds.Tables[0].Rows[0]["Paper_Gram"].ToString() + " แกรม";
                        }

                        ds.Tables[0].Rows[0]["Form1_Print_PaperDetail"] = paper_detail;

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MainDataSet", ds.Tables[0]));

                        ReportViewer1.Visible = true;
                    }
                }
            }
        }

        private void calculatePrice(DataSet ds)
        {
            InvoiceModel inv = new InvoiceModel()
            {
                TradeDiscount_Percent = Convert.ToDecimal(ds.Tables[0].Rows[0]["TradeDiscount_PercentPO"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["TradeDiscount_PercentPO"].ToString()) // Util.ToDecimal( Request["POTradeDiscount_Percent"].ToString())
              ,
                TradeDiscount_Price = Convert.ToDecimal(ds.Tables[0].Rows[0]["TradeDiscount_PricePO"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["TradeDiscount_PricePO"].ToString()) // Util.ToDecimal(Request["POTradeDiscount_Price"].ToString())
              ,
                TradeDiscount_Type = ds.Tables[0].Rows[0]["TradeDiscount_Type"].ToString() // Request["TradeDiscount_Type"].ToString()
              ,
                Vat = Convert.ToDecimal(ds.Tables[0].Rows[0]["PO_Vat"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["PO_Vat"].ToString())  // Util.ToDecimal(Request["PO_Vat"].ToString())
              ,
                Vat_Type = ds.Tables[0].Rows[0]["Vat_Type"].ToString() //  Request["Vat_Type"].ToString()
              ,
                VatUnit_Type = ds.Tables[0].Rows[0]["VatUnit_Type"].ToString() //  Request["VatUnit_Type"].ToString()

            };



            foreach (DataRow r in ds.Tables[0].Rows)
            {

                InvoiceItemModel mo = new InvoiceItemModel();
                //mo.Receive_Quantity = Convert.ToDecimal(r["POItem_Receive_Quantity"].ToString() == "" ? "0" : r["POItem_Receive_Quantity"].ToString()); // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                /* ในกรณี ของการคำนวณ PO ให้ใช้ Receive_Quantity = Unit_Quantity*/
                mo.Receive_Quantity = Convert.ToDecimal(r["poItems_Unit_Qty"].ToString() == "" ? "0" : r["poItems_Unit_Qty"].ToString()); // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                mo.TradeDiscount_Percent = Convert.ToDecimal(r["POItem_TradeDiscount_Percent"].ToString() == "" ? "0" : r["POItem_TradeDiscount_Percent"].ToString()); // Util.ToDecimal(Request["POItemTradeDiscount_Percent"].ToString());
                mo.TradeDiscount_Price = Convert.ToDecimal(r["POItem_TradeDiscount_Price"].ToString() == "" ? "0" : r["POItem_TradeDiscount_Price"].ToString());// Util.ToDecimal(Request["POItemTradeDiscount_Price"].ToString());
                mo.Unit_Price = Convert.ToDecimal(r["poItems_Unit_Price"].ToString() == "" ? "0" : r["poItems_Unit_Price"].ToString()); // Util.ToDecimal(Request["Unit_Price"].ToString());
                mo.Unit_Quantity = Convert.ToDecimal(r["poItems_Unit_Qty"].ToString() == "" ? "0" : r["poItems_Unit_Qty"].ToString()); // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                mo.Vat = Convert.ToDecimal(r["POItem_Vat"].ToString() == "" ? "0" : r["POItem_Vat"].ToString()); // Util.ToDecimal(Request["PO_Vat"].ToString());
                inv.InvoiceItem.Add(mo);

            }




            inv.CalculatePrice();


            // calculate price
            this.Net_Amount = inv.Net_Amount;
            this.Total_Price = inv.Total_Price;
            this.Total_Discount = inv.Total_Discount;
            this.Total_Before_Vat = inv.Total_Before_Vat;
            this.Vat_Amount = inv.VAT_Amount;

            //for (int ii = 0; ii < this.ReceiveStkItemList.Count; ii++)
            //{

            //        this.ReceiveStkItemList[ii].Total_Before_Vat = inv.InvoiceItem[ii].Total_before_Vat;
            //        this.ReceiveStkItemList[ii].Vat_Amount = inv.InvoiceItem[ii].Vat_Amount;
            //        this.ReceiveStkItemList[ii].Net_Amount = inv.InvoiceItem[ii].Net_Amount;
            //        this.ReceiveStkItemList[ii].Total_Discount = inv.InvoiceItem[ii].Discount_Price;



            //}

            //ds.Tables[0].Columns.Add("Total_Before_Vat");
            //ds.Tables[0].Columns.Add("Vat_Amount");
            ds.Tables[0].Columns.Add("Unit_Price_VatInc");
            ds.Tables[0].Columns.Add("Cal_Net_Amount");
            //ds.Tables[0].Columns.Add("Total_Discount");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //ds.Tables[0].Rows[i]["Total_Before_Vat"] = inv.InvoiceItem[i].Total_before_Vat.ToString("#,###.0000"); ;
                //ds.Tables[0].Rows[i]["Vat_Amount"] = inv.InvoiceItem[i].Vat_Amount.ToString("#,###.0000");
                ds.Tables[0].Rows[i]["Cal_Net_Amount"] = inv.InvoiceItem[i].Net_Amount.ToString("#,##.00");
                if (Convert.ToDecimal(ds.Tables[0].Rows[i]["poItems_Unit_Qty"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["poItems_Unit_Qty"].ToString()) != 0)
                {
                    ds.Tables[0].Rows[i]["Unit_Price_VatInc"] = (inv.InvoiceItem[i].Net_Amount / Convert.ToDecimal(ds.Tables[0].Rows[i]["poItems_Unit_Qty"].ToString())).ToString("#,##0.0000");
                }
                else
                {
                    ds.Tables[0].Rows[i]["Unit_Price_VatInc"] = "0.0000";
                }
                //ds.Tables[0].Rows[i]["Total_Discount"] = inv.InvoiceItem[i].Discount_Price.ToString("#,###.0000"); ;
            }
        }
    }
}
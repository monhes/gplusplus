﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.ModelClass;

namespace GPlus.PRPO
{
    public partial class pop_POPrintAttachReport : Pagebase
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
                    DataSet ds = new DataAccess.PODAO().GetPOReport2(Request["id"]);
                    calculatePrice(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("PO_Date_TH");
                        if (ds.Tables[0].Rows[0]["PO_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["PO_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        ds.Tables[0].Columns.Add("Shipping_Date_TH");
                        if (ds.Tables[0].Rows[0]["Shipping_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Shipping_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        ds.Tables[0].Columns.Add("Purchase_Date_TH");
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["Purchase_Date"].ToString().Length > 0)
                            {
                                int dateYear = ((DateTime)ds.Tables[1].Rows[i]["Purchase_Date"]).Year;
                                if (dateYear < 2500)
                                    dateYear += 543;
                                ds.Tables[0].Rows[0]["Purchase_Date_TH"] = ((DateTime)ds.Tables[1].Rows[i]["Purchase_Date"]).Day.ToString() + "/" +
                                    ((DateTime)ds.Tables[1].Rows[i]["Purchase_Date"]).Month.ToString() + "/" + dateYear.ToString();
                            }
                        }

                        ds.Tables[0].Columns.Add("ReorderPoint_Date_TH");
                        if (ds.Tables[0].Rows[0]["ReorderPoint_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["ReorderPoint_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["ReorderPoint_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["ReorderPoint_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["ReorderPoint_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }


                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MainDataSet2", ds.Tables[0]));
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemDataSet2", ds.Tables[1]));

                        ReportViewer1.Visible = true;
                    }
                }

            }
        }

        private void calculatePrice(DataSet ds)
        {
            InvoiceModel inv = new InvoiceModel()
            {
                TradeDiscount_Percent = Convert.ToDecimal(ds.Tables[0].Rows[0]["TradeDiscount_PercentPO"].ToString()==""?"0":ds.Tables[0].Rows[0]["TradeDiscount_PercentPO"].ToString()) // Util.ToDecimal( Request["POTradeDiscount_Percent"].ToString())
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



            foreach (DataRow r in ds.Tables[1].Rows)
            {
        
                    InvoiceItemModel mo = new InvoiceItemModel();
                    //mo.Receive_Quantity = Convert.ToDecimal(r["POItem_Receive_Quantity"].ToString() == "" ? "0" : r["POItem_Receive_Quantity"].ToString()); // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                    /* ในกรณี ของการคำนวณ PO ให้ใช้ Receive_Quantity = Unit_Quantity*/
                    mo.Receive_Quantity = Convert.ToDecimal(r["Unit_Quantity"].ToString() == "" ? "0" : r["Unit_Quantity"].ToString()); // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                    mo.TradeDiscount_Percent = Convert.ToDecimal(r["POItem_TradeDiscount_Percent"].ToString() == "" ? "0" : r["POItem_TradeDiscount_Percent"].ToString()); // Util.ToDecimal(Request["POItemTradeDiscount_Percent"].ToString());
                    mo.TradeDiscount_Price = Convert.ToDecimal(r["POItem_TradeDiscount_Price"].ToString() == "" ? "0" : r["POItem_TradeDiscount_Price"].ToString());// Util.ToDecimal(Request["POItemTradeDiscount_Price"].ToString());
                    mo.Unit_Price = Convert.ToDecimal(r["Unit_Price"].ToString() == "" ? "0" : r["Unit_Price"].ToString()); // Util.ToDecimal(Request["Unit_Price"].ToString());
                    mo.Unit_Quantity = Convert.ToDecimal(r["Unit_Quantity"].ToString() == "" ? "0" : r["Unit_Quantity"].ToString()); // Util.ToDecimal(Request["Unit_Quantity"].ToString());
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

            ds.Tables[1].Columns.Add("Total_Before_Vat");
            ds.Tables[1].Columns.Add("Vat_Amount");
            ds.Tables[1].Columns.Add("Cal_Net_Amount");
            ds.Tables[1].Columns.Add("Total_Discount");

            for (int i=0; i < ds.Tables[1].Rows.Count;i++)
            {
                ds.Tables[1].Rows[i]["Total_Before_Vat"] = inv.InvoiceItem[i].Total_before_Vat.ToString("#,###.0000"); ;
                ds.Tables[1].Rows[i]["Vat_Amount"] = inv.InvoiceItem[i].Vat_Amount.ToString("#,###.0000");
                ds.Tables[1].Rows[i]["Cal_Net_Amount"] = inv.InvoiceItem[i].Net_Amount.ToString("#,###.00");
                ds.Tables[1].Rows[i]["Total_Discount"] = inv.InvoiceItem[i].Discount_Price.ToString("#,###.0000"); ;
            }
        }
    
        }


    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus.ModelClass
{
    public class InvoiceModel
    {
        /// <summary>
        /// [Input] Vat_Type = [0 คือ Vat รวม]  [1 คือ Vat แยก]
        /// </summary>
        public string Vat_Type
        {
            get;
            set;
        }

        /// <summary>
        /// [Input] Vat ของ Invoice (%) ใช้ในกรณีเป็น vat รวม
        /// </summary>
        public decimal Vat
        {
            get;
            set;
        }

        /// <summary>
        /// [Input] VatUnit_Type = [0 คือ Exclude]  [1 คือ Include]
        /// </summary>
        public string VatUnit_Type
        {
            get;
            set;
        }

        /// <summary>
        /// [Input] TradeDiscount_Type = [ 0 ส่วนลดรวม ]  [ 1 ส่วนลดแยก ]
        /// </summary>
        public string TradeDiscount_Type
        {
            get;
            set;
        }

        /// <summary>
        /// [Input] ส่วนลดรวมของ Invoice เป็น % 
        /// </summary>
        public decimal TradeDiscount_Percent
        {
            get;
            set;
        }

        /// <summary>
        /// [Input] ส่วนลดรวมของ Invoice เป็นราคา
        /// </summary>
        public decimal TradeDiscount_Price
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
        public decimal VAT_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// [Output] ราคารวมสุทธิของ Invoice ทั้งใบ
        /// </summary>
        public decimal Net_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// รายการใน Invoice 
        /// </summary>

        public List<InvoiceItemModel> InvoiceItem
        {
            get;
            set;
        }


        public InvoiceModel()
        {
            this.InvoiceItem = new List<InvoiceItemModel>();
        }

        /// <summary>
        /// Function คำนวนราคาทั้งหมด
        /// </summary>
        public void CalculatePrice()
        {

            this.Net_Amount = 0;
            this.VAT_Amount = 0;
            this.Total_Before_Vat = 0;
            this.Total_Price = 0;
            this.Total_Discount = 0;

            decimal totalUnitQty = 0;
            decimal totalPriceSum = 0;

            foreach (var a in this.InvoiceItem)
            {
                totalPriceSum += a.Unit_Price * a.Unit_Quantity;
                totalUnitQty += a.Unit_Quantity;
            }

            // แต่ละ po item
            foreach (var a in this.InvoiceItem)
            {
               // decimal vatPer = a.Vat;


              
                if (this.TradeDiscount_Type.Trim() == "0" || this.TradeDiscount_Type.Trim() == "")  // ส่วนลดรวม หรือ ไม่มีส่วนลด
                {

                    decimal unitP = a.Unit_Price;
                    decimal recv = a.Receive_Quantity;

                    decimal dis = 0;
                    decimal disc = 0;

                    if (this.TradeDiscount_Percent != 0)
                    {

                        disc = unitP * recv * (this.TradeDiscount_Percent / 100);
                    }
                    else
                    {
                        if (this.TradeDiscount_Price != 0)
                        {

                            disc = this.TradeDiscount_Price * (recv * unitP) / totalPriceSum;

                        }
                        else
                        {
                            dis = 0;
                            disc = 0;
                        }

                    }

                    a.TradeDiscount_Price = disc;

                    if (this.VatUnit_Type.Trim() == "0")
                    {  // exclude vat

                        if (this.Vat_Type.Trim() == "0")
                        { // คิด รวม vat

                            decimal total_beforevat = ((unitP * recv) - disc);
                           // decimal vat_price = (vatPer / 100) * total_beforevat;
                            decimal vat_price = (this.Vat / 100) * total_beforevat;

                            decimal net_amount = total_beforevat + vat_price;
                            a.Total_before_Vat = total_beforevat;
                            a.Discount_Price = disc;
                            a.Vat_Amount = vat_price;

                            a.Net_Amount = net_amount;

                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;

                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;

                        }
                        else
                        {   // คิด แต่ ได้ ละรายการ

                            decimal total_beforevat = ((unitP * recv) - disc);
                          //  decimal vat_price = (vatPer / 100) * total_beforevat;
                            decimal vat_price = (a.Vat / 100) * total_beforevat;
                            a.Total_before_Vat = total_beforevat;
                            a.Vat_Amount = vat_price;


                            decimal net_amount = total_beforevat + vat_price;

                            a.Net_Amount = net_amount;
                            a.Discount_Price = disc;
                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;

                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;

                        }

                    }
                    else    // include vat
                    {
                        if (this.Vat_Type.Trim() == "0")
                        { // คิด รวม vat

                            decimal net_amount = ((unitP * recv) - disc);


                          //  decimal total_beforevat = net_amount / (1 + vatPer / 100);
                            decimal total_beforevat = net_amount / (1 + this.Vat / 100);

                            decimal vat_price = net_amount - total_beforevat;

                            a.Total_before_Vat = total_beforevat; //.ToString();
                            a.Discount_Price = disc;
                            a.Vat_Amount = vat_price;
                            a.Net_Amount = net_amount;


                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;

                            this.VAT_Amount += vat_price;
                            this.Net_Amount += net_amount;

                        }
                        else
                        {   // คิด vat แต่ ได้ ละรายการ

                            decimal net_amount = ((unitP * recv) - disc);
                            //decimal total_beforevat = net_amount / (1 + vatPer / 100);
                            decimal total_beforevat = net_amount / (1 + a.Vat / 100);

                            decimal vat_price = net_amount - total_beforevat;

                            a.Discount_Price = disc;
                            a.Total_before_Vat = total_beforevat;

                            a.Vat_Amount = vat_price;


                            a.Net_Amount = net_amount;


                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;

                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;


                        }
                    }


                    /////////////////////////////////////////////////////////////////////////////////////////////////////

                }
                else
                { // ส่วนลดแยก



                    decimal unitP = a.Unit_Price;
                    decimal recv = a.Receive_Quantity;


                    decimal dis = 0;
                    decimal disc = 0;

                    this.TradeDiscount_Percent = a.TradeDiscount_Percent;

                    this.TradeDiscount_Price = a.TradeDiscount_Price;


                    if (this.TradeDiscount_Percent > 0)
                    {

                        disc = unitP * recv * (this.TradeDiscount_Percent / 100);
                    }
                    else
                    {
                        if (this.TradeDiscount_Price > 0)
                        {

                            disc = this.TradeDiscount_Price;

                        }
                        else
                        {
                            dis = 0;
                            disc = 0;
                        }

                    }


                    this.TradeDiscount_Price = disc;

                    if (this.VatUnit_Type.Trim() == "0")
                    {  // exclude vat

                        if (this.Vat_Type.Trim() == "0")
                        { // คิด รวม vat

                            decimal total_beforevat = ((unitP * recv) - disc);
                           // decimal vat_price = (vatPer / 100) * total_beforevat;
                            decimal vat_price = (this.Vat / 100) * total_beforevat;

                            decimal net_amount = total_beforevat + vat_price;
                            a.Total_before_Vat = total_beforevat;

                            a.Vat_Amount = vat_price;

                            a.Net_Amount = net_amount;
                            a.Discount_Price = disc;

                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;
                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;

                        }
                        else
                        {   // คิด แต่ ได้ ละรายการ

                            decimal total_beforevat = ((unitP * recv) - disc);
                          //  decimal vat_price = (vatPer / 100) * total_beforevat;
                            decimal vat_price = (a.Vat / 100) * total_beforevat;

                            a.Total_before_Vat = total_beforevat;

                            a.Discount_Price = disc;
                            a.Vat_Amount = vat_price;


                            decimal net_amount = total_beforevat + vat_price;

                            a.Net_Amount = net_amount;


                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;
                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;

                        }

                    }
                    else    // include vat
                    {
                        if (this.Vat_Type.Trim() == "0")
                        { // คิด รวม vat

                            decimal net_amount = ((unitP * recv) - disc);


                            //decimal total_beforevat = net_amount / (1 + vatPer / 100);
                            decimal total_beforevat = net_amount / (1 + this.Vat / 100);

                            decimal vat_price = net_amount - total_beforevat;

                            a.Discount_Price = disc;
                            a.Total_before_Vat = total_beforevat;

                            a.Vat_Amount = vat_price;

                            a.Net_Amount = net_amount;


                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;

                            this.Total_Before_Vat += total_beforevat;

                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;

                        }
                        else
                        {   // คิด vat แต่ ได้ ละรายการ

                            decimal net_amount = ((unitP * recv) - disc);
                          //  decimal total_beforevat = net_amount / (1 + vatPer / 100);
                            decimal total_beforevat = net_amount / (1 + a.Vat / 100);

                            decimal vat_price = net_amount - total_beforevat;
                            a.Total_before_Vat = total_beforevat;

                            a.Vat_Amount = vat_price;


                            a.Net_Amount = net_amount;
                            a.Discount_Price = disc;

                            this.Total_Price += unitP * recv;

                            this.Total_Discount += disc;
                           

                            this.Total_Before_Vat += total_beforevat;


                            this.VAT_Amount += vat_price;

                            this.Net_Amount += net_amount;



                        }
                    }


                    ///////////////////////////////////////////////////////////////////////////////////////////////

                }

            }


        }


    }
    public class InvoiceItemModel
    {
        /// <summary>
        /// [Input] ราคาต่อหน่วยในรายการ
        /// </summary>
        public decimal Unit_Price { get; set; }
        /// <summary>
        /// [Input] จำนวนรับในแต่ละรายการของ Invoice
        /// </summary>
        public decimal Receive_Quantity { get; set; }

        /// <summary>
        /// [Input] ส่วนลดเป็น % ของแต่ละรายการกรณีส่วนลดแยก
        /// </summary>
        public decimal TradeDiscount_Percent { get; set; }

        /// <summary>
        /// [Input] ส่วนลดเป็นราคา ของแต่ละรายการกรณีส่วนลดแยก
        /// </summary>
        public decimal TradeDiscount_Price { get; set; }

        /// <summary>
        /// [Output] ราคาของรายการ Invoice ก่อนรวม vat
        /// </summary>
        public decimal Total_before_Vat { get; set; }
        /// <summary>
        /// [Input] Vat (%) ในแต่ละ invoice กรณี vat แยก
        /// </summary>
        public decimal Vat { get; set; }

        /// <summary>
        /// [Output] ราคา vat รวมในแต่ละ invoice
        /// </summary>
        public decimal Vat_Amount { get; set; }

        /// <summary>
        /// [Output] ราคาสุทธิรวมในแต่ละ invoice
        /// </summary>
        public decimal Net_Amount { get; set; }

        /// <summary>
        /// [Input] จำนวนรับรวมของแต่ละรายการใน invoice
        /// </summary>
        public decimal Unit_Quantity { get; set; }


        /// <summary>
        /// [Output] ส่วนลดเป็นราคา ของแต่ละรายการกรณีส่วนลดแยก
        /// </summary>
        public decimal Discount_Price { get; set; }
     
    }
}
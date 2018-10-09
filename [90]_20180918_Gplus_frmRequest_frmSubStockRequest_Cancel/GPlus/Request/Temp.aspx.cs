using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.images
{
    public partial class Temp : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "610";
            }
           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtConfirm.Text == "confirm")
            {
                String result = new DataAccess.TempDAO().UpdateStockPayManual(txtPay_ID.Text, txtPay_ID2.Text, txtPay_ID3.Text, txtPay_ID4.Text, txtPay_ID5.Text, txtInv_ItemID.Text, txtPack_ID.Text, txtUnit_Price.Text);
                if (result == "1")
                {
                    ShowMessageBox("ทำการบันทึกข้อมูลเรียบร้อย");
                }
                else
                {
                    ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้");
                }
            }
            else
            {
                ShowMessageBox("Confirm Text ไม่ถูกต้อง");
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPay_ID.Text = "";
            txtPay_ID2.Text = "";
            txtPay_ID3.Text = "";
            txtPay_ID4.Text = "";
            txtPay_ID5.Text = "";
            txtInv_ItemID.Text = "";
            txtPack_ID.Text = "";
            txtUnit_Price.Text = "";
            txtConfirm.Text = "";

        }

       
    }
}
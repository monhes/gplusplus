using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.Stock.Commons;

namespace GPlus.Stock
{
    public partial class SetStockItemUserControl : System.Web.UI.UserControl
    {
        private static StockReceiveNotSame _data;
        public StockReceiveNotSame Data 
        {
            get 
            {
                if (_data != null)
                {
                    _data.ItemCount = Convert.ToInt32(string.IsNullOrEmpty(this.txtItemCount.Text) ? "0" : this.txtItemCount.Text);
                }
                return _data; 
            }
            set 
            { 
                _data = value; 
            } 
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _data = new StockReceiveNotSame();
                _data.ItemCount = Int32.Parse(txtItemCount.Text == string.Empty ? "0" : txtItemCount.Text);
                _data.ItemID = 0;

                //ทำการ Enable ปุ่ม search และ dropdown หน่วย สินค้า หลังจากที่ กด AddLot แล้ว
                btnSelect1.Enabled = true;
                ddPack.Enabled = true;
            }
            else
            {
                if (Session["ITEM_ID_SetStk"] != null)
                {
                    this.LoadData((int)Session["ITEM_ID_SetStk"]);
                    Session["ITEM_ID_SetStk"] = null;
                }
                else
                {
                    if (Data != null)
                    {

                    }
                }
            }
        }

        private void LoadData(int itemId)
        {
            DataTable dt = new DataAccess.ReceiveStockDAO().GetItemDetailWithPackage(itemId);
            if (dt.Rows.Count > 0)
            {
                this.txtItemId.Value = "" + itemId;
                this.txtItemName.Text = dt.Rows[0]["Inv_ItemName"].ToString();
                this.txtItemCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();

                this.ddPack.DataSource = dt;
                this.ddPack.DataBind();

                DataRow[] drRows = dt.Select("Pack_ID ='" + this.ddPack.SelectedValue + "'");
                txtBarcode.Text = drRows[0]["Barcode_From_Supplier"].ToString();

                if (drRows[0]["Avg_Cost"].ToString() == "")
                {
                    Session["AvgCost_SetStk"] = "0.0000";
                }
                else
                {
                    Session["AvgCost_SetStk"] = drRows[0]["Avg_Cost"].ToString();
                }

                _data = new StockReceiveNotSame();
                _data.ItemCount = Int32.Parse(txtItemCount.Text == string.Empty ? "0" : txtItemCount.Text);
                _data.ItemID = itemId;
                _data.ItemName = dt.Rows[0]["Inv_ItemName"].ToString();
                if (!string.IsNullOrEmpty(this.ddPack.SelectedValue) && (Int32.Parse(this.ddPack.SelectedValue) >= 0))
                {
                    Data.PackId = Int32.Parse(this.ddPack.SelectedValue);
                }
                _data.ItemName = this.ddPack.SelectedItem.Text;
            }
            else
            {
                this.txtItemId.Value = "";
                this.txtItemName.Text = "";

                this.ddPack.Items.Clear();
            }
        }

        protected void ChangeUnit(object sender, EventArgs e)
        {
            DataTable dt = new DataAccess.ItemDAO().GetItemPack(txtItemId.Value,ddPack.SelectedValue);
            txtBarcode.Text = dt.Rows[0]["Barcode_From_Supplier"].ToString();
            if (dt.Rows[0]["Avg_Cost"].ToString() == "")
            {
                Session["AvgCost_SetStk"] = "0.0000";
            }
            else
            {
                Session["AvgCost_SetStk"] = dt.Rows[0]["Avg_Cost"].ToString();
            }
        }

        public String ItemCode
        {
            get
            {
                return txtItemCode.Text;
            }

        }

        public String PackID
        {
            get
            {
                return ddPack.SelectedValue;
            }

        }

        public String SupplierBarcode
        {
            get
            {
                return txtBarcode.Text;
            }

        }

        public int ItemCount
        {
            get
            {
                return Int32.Parse(txtItemCount.Text == string.Empty ? "0" : txtItemCount.Text);
            }

        }

        public string setDisableSearch
        {
            set
            {
                btnSelect1.Enabled = false;
                ddPack.Enabled = false;
            }

        }



    }
}
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
    public partial class StockReceiverStockItemUserControl : System.Web.UI.UserControl
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
            }
            else
            {
                if (Session["ITEM_ID"] != null)
                {
                    this.LoadData((int)Session["ITEM_ID"]);
                    Session["ITEM_ID"] = null;
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

                this.ddPack.DataSource = dt;
                this.ddPack.DataBind();

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
    }
}
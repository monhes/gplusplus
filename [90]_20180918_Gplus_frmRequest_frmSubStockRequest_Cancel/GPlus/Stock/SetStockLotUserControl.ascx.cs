using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace GPlus.Stock
{
    public partial class SetStockLotUserControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Get or set lot id
        /// </summary>
        public int LotId
        {
            get
            {
                if (ViewState["LotId" + this.ID] == null) ViewState["LotId" + this.ID] = 1;
                return (int)ViewState["LotId" + this.ID];
            }
            set 
            {
                ViewState["LotId" + this.ID] = value;
            }
        }

        private DataTable _lots;
        /// <summary>
        /// Get or set lot item for this lot
        /// </summary>
        public DataTable Lot
        {
            get
            {
                CollectDataRow();
                return _lots;
            }
            set
            {
                _lots = value;
                if (_lots != null && (_lots.Rows.Count > 0))
                {
                    this.LoadData();
                }
            }
        }

        public string LotName
        {
            get
            {
                return txtLotNo.Text;
            }
        }
        
        /// <summary>
        /// This constructor use to
        /// </summary>
        public SetStockLotUserControl() { }

        public void DisableDelButton()
        {
            btnDelete.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
        }

        /// <summary>
        /// This method use to add new row
        /// </summary>

        public int LocationItemCount = 0;
        public int LotItemCount = 0;

        private void CollectDataRow()
        {
            if(txtReceiveNumber.Text.Trim().Length > 0)
               LotItemCount = int.Parse(txtReceiveNumber.Text);
            DataTable dt = Session["Data_SetStk"] as DataTable;
            
            DataRow[] drRows = dt.Select("Lot_ID='" + this.LotId + "'");
            for (int i = 0; i < drRows.Length; i++)
            {
                drRows[i]["rownumber"] = i + 1;
                TextBox txtEachUnitNumber = (TextBox)gvStk.Rows[i].Cells[1].FindControl("txtEachUnitNumber");
                drRows[i]["Qty_Location"] = txtEachUnitNumber.Text;
                if (txtEachUnitNumber.Text.Trim().Length > 0)
                    LocationItemCount += int.Parse(txtEachUnitNumber.Text);
                if(((DropDownList)gvStk.Rows[i].Cells[2].FindControl("ddLocationList")).SelectedValue.Trim().Length > 0)
                    drRows[i]["Location_ID"] = int.Parse(((DropDownList)gvStk.Rows[i].Cells[2].FindControl("ddLocationList")).SelectedValue);
                drRows[i]["Expire_Date"] = txtExpireDate.Text;
                drRows[i]["Barcode_No"] = txtSupBarcode.Text;
                drRows[i]["Barcode_PrintQty"] = string.IsNullOrEmpty(txtPrintCount.Text) ? 0 : Convert.ToInt32(txtPrintCount.Text);
                drRows[i]["Lot_Qty"] = Convert.ToInt32(txtReceiveNumber.Text);
                drRows[i]["Lot_No"] = txtLotNo.Text;
                drRows[i]["Lot_Item_ID"] = dt.Rows[i]["Inv_ItemID"];
                drRows[i].AcceptChanges();
            }
            Session["Data_SetStk"] = dt;
            _lots = dt;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DesignMode == true)
            {
                this.EnsureChildControls();
            }
            this.Page.RegisterRequiresControlState(this);
        } 

        protected void BtnAddRowClick(object sender, EventArgs e)
        {
            AddNewRow();
        }

        private void AddNewRow()
        {
            //if (Session["LotId"] != null) 
            //    _lotId = Convert.ToInt32(Session["LotId"].ToString());
            this.CollectDataRow();
            DataTable dt = Session["Data_SetStk"] as DataTable;
            DataView drv = new DataView(dt);
            drv.RowFilter = "Lot_ID='" + this.LotId + "'";
            DataRow drRow = dt.NewRow();
            drRow.ItemArray = dt.Select("Lot_ID='" + this.LotId + "'").LastOrDefault().ItemArray;
            drRow["Location_ID"] = 1;
            drRow["IsNewLot"] = true;
            drRow["rownumber"] = dt.Select("Lot_ID='" + this.LotId + "'").Length + 1;
            drRow["Lot_ID"] = this.LotId;
            drRow["Lot_Item_ID"] = drRow["Inv_ItemID"];
            dt.Rows.Add(drRow);

            gvStk.DataSource = drv;
            gvStk.DataBind();

            Session["Data_SetStk"] = dt;
        }

        /// <summary>
        /// This method use to load datat into lot
        /// </summary>
        private void LoadData()
        {
            //if (!_lots.Columns.Contains("rownumber"))
            //{
            //    _lots.Columns.Add("rownumber", typeof(int));
            //    _lots.AcceptChanges();
            //}

            DataRow[] drs = _lots.Select("Lot_ID='" + this.LotId + "'");
            
            if (drs.Length == 0)
            {
                if (string.IsNullOrEmpty(_lots.Rows[0]["Lot_ID"].ToString()))
                {
                    _lots.Rows[0]["Lot_ID"] = this.LotId;
                    _lots.Rows[0]["IsNewLot"] = true;
                }
                else
                {
                    _lots.Rows[0]["IsNewLot"] = false;
                }
                drs = _lots.Select("Lot_ID='" + this.LotId + "'");
            }

            for (int i = 0; i < drs.Length; i++)
            {
                drs[i]["rownumber"] = i + 1;
            }

            if (!string.IsNullOrEmpty(drs[0]["Expire_Date"].ToString()) && Convert.ToDateTime(drs[0]["Expire_Date"]) != DateTime.MinValue)
            {
                this.txtExpireDate.Value = Convert.ToDateTime(drs[0]["Expire_Date"]);
            }
            this.txtLotNo.Text = drs[0]["Lot_No"].ToString();
            this.txtPrintCount.Text = drs[0]["Barcode_PrintQty"].ToString();
            //int receiveNum = 0;
            //foreach (DataRow dr in _lots.Rows)
            //{
            //    receiveNum += Convert.ToInt32(dr["Qty_Location"].ToString());
            //}

            if(_lots.Rows.Count > 0)
                this.txtReceiveNumber.Text = Convert.ToInt32(drs[0]["Lot_Qty"]).ToString();
            else
                this.txtReceiveNumber.Text = "1"; // (Convert.ToInt32(drs[0]["Unit_Quantity"]) - receiveNum).ToString();
                this.txtSupBarcode.Text = drs[0]["Barcode_No"].ToString();
                this.txtAvgCost.Text = Session["AvgCost_SetStk"].ToString();
                this.txtPrice.Text = (Convert.ToDouble(txtReceiveNumber.Text) * Convert.ToDouble(txtAvgCost.Text)).ToString("#,##0.0000");
                //DataTable dtt = _lots.Clone();
            DataView drv = new DataView(_lots);
            drv.RowFilter = "Lot_ID='" + this.LotId + "'";
            gvStk.DataSource = drv;
            gvStk.DataBind();

            this.Session["Data_SetStk"] = _lots;
        }

        public void GvStkRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                //GetLoacation()
                DataTable dtLocation = new DataAccess.ReceiveStockDAO().GetLoacation(Request["stockID"]);
                //DataTable dtLocation = new DataAccess.ReceiveStockDAO().GetLoacation("1");

                DropDownList ddl = (DropDownList)e.Row.FindControl("ddLocationList");
                ddl.DataSource = dtLocation;
                ddl.DataBind();
                ddl.SelectedValue = drv["Location_ID"].ToString();
                ((HiddenField)e.Row.FindControl("hdRowID")).Value = drv["rownumber"].ToString();

                ((LinkButton)e.Row.FindControl("btnDelete")).CommandArgument = e.Row.RowIndex.ToString();

                if (e.Row.RowIndex == 0)
                    ((LinkButton)e.Row.FindControl("btnDelete")).Visible = false;

                ((TextBox)e.Row.FindControl("txtEachUnitNumber")).Text = drv["Qty_Location"].ToString();
            }
        }

        public void GvStkRowCommand(object sender, GridViewCommandEventArgs e) 
        {
            if (e.CommandName == "Del")
            {
                DataTable dt = Session["Data_SetStk"] as DataTable;
                DataRow[] drRows = dt.Select("Lot_ID='" + this.LotId + "' AND rownumber = " +
                    ((HiddenField)gvStk.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("hdRowID")).Value);
                for (int i = 0; i < drRows.Length; i++)
                {
                    drRows[i].Delete();
                }
                dt.AcceptChanges();
                Session["Data_SetStk"] = dt;
                gvStk.Rows[int.Parse(e.CommandArgument.ToString())].Visible = false;
            }
        }
        /// <summary>
        /// This method use to submit all data
        /// </summary>
        public void SubmitData()
        {
            this.CollectDataRow();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["Data_SetStk"] as DataTable;
            DataRow[] drRows = dt.Select("Lot_ID='" + this.LotId + "'");
            for (int i = 0; i < drRows.Length; i++)
            {
                drRows[i].Delete();
            }
            dt.AcceptChanges();
            Session["Data_SetStk"] = dt;
            this.Visible = false;
        }

        protected void ChangeLotNo(object sender, EventArgs e)
        {
            Genbarcode Gen = new Genbarcode();
            //txtSupBarcode.Text = Gen.Genearetebarcode(hdInvItemID.Value, hdInvPackID.Value, txtLotNo.Text);
            txtSupBarcode.Text = Gen.Genearetebarcode(Session["Inv_ItemCode_SetStk"].ToString(), Session["PackID_SetStk"].ToString(), txtLotNo.Text);
        }

        protected void ChangeReceiveNumber(object sender, EventArgs e)
        {
            txtPrice.Text = (Convert.ToDouble(txtReceiveNumber.Text) * Convert.ToDouble(txtAvgCost.Text)).ToString("#,##0.0000");    
        }
    }
}
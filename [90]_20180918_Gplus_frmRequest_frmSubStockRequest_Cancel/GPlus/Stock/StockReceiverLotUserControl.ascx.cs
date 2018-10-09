using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using GPlus.ModelClass;

namespace GPlus.Stock
{
    public partial class StockReceiverLotUserControl : System.Web.UI.UserControl
    {

        private ReceiveStkModel ReceiveStkModel
        {
            get
            {
                ReceiveStkModel rcvModel = (ReceiveStkModel)Session["ReceiveStkModel"];
                return rcvModel;
            }
            set
            {
                Session["ReceiveStkModel"] = value;
            }
        }

        private ReceiveStkItemModel ReceiveStkItemModel
        {
            get
            {
                int itemId = Util.ToInt(Request["Inv_ItemID"].ToString());
                int packId = Util.ToInt(Request["Pack_ID"].ToString());
                return this.ReceiveStkModel.ReceiveStkItemList.Where(m => m.Inv_ItemID == itemId && m.Pack_ID == packId).FirstOrDefault();

            }
        }
      
        
        /// <summary>
        /// Get or set lot id
        /// </summary>
        public string LotID
        {
            get
            {
                return this.hdLotId.Value;
            }

            set
            {
                this.hdLotId.Value = value;
            }

        }

    

       
        public string LotNo
        {
            get
            {
                return txtLotNo.Text;
            }
            set
            {
                this.txtLotNo.Text = value;
            }
        }

        public string ReceiveNumber
        {
            get
            {
                return txtReceiveNumber.Text;
            }
            set
            {
                this.txtReceiveNumber.Text = value;
            }
        }


        public string SupplierBarCode
        {
            get
            {
                return this.txtSupBarcode.Text.Trim();
            }
            set
            {
                this.txtSupBarcode.Text = value;
            }
        }

        public string PrintCounnt
        {
            set
            {
                this.txtPrintCount.Text = value;
            }
            get
            {
               return this.txtPrintCount.Text.Trim();
            }
        }

        public string ExpireDate
        {
            get
            {
                return this.txtExpireDate.Text.Trim();
            }
            set
            {
                this.txtExpireDate.Text = value;
            }
        }
        
        /// <summary>
        /// This constructor use to
        /// </summary>
        public StockReceiverLotUserControl() { }

        public void DisableDelButton()
        {
           // btnDelete.Visible = false;
        }

        private void SetView(){
            if (Session["LotAction"] as string == "View")
            {
                btnAddRow.Visible = false;
                btnDeleteRow.Visible = false;
                gvStk.Columns[1].Visible = true;
                gvStk.Columns[2].Visible = false;

                gvStk.Columns[3].Visible = false;
                gvStk.Columns[4].Visible = true;
            }
            else
            {
                btnAddRow.Visible = true;
                btnDeleteRow.Visible = true;
                gvStk.Columns[1].Visible = false;
                gvStk.Columns[2].Visible = true;

                gvStk.Columns[3]. Visible = true;
                gvStk.Columns[4].Visible = false;


            }
        }
        protected void Page_Load(object sender, EventArgs e) 
        {
  
        //  if(!Page.IsPostBack){
               SetView();
  

         //  }
   
        }

        /// <summary>
        /// This method use to add new row
        /// </summary>

        public int LocationItemCount = 0;
        public int LotItemCount = 0;

      
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
            SetView();

            List<string> unitNumLst = new List<string>();
            List<string> selStkLst = new List<string>();
            foreach(GridViewRow r in gvStk.Rows){

                    DropDownList ddl = r.FindControl("ddLocationList") as DropDownList; 
                    TextBox txt = (TextBox)r.FindControl("txtEachUnitNumber");
                    unitNumLst.Add(txt.Text.Trim());
                    selStkLst.Add(ddl.SelectedItem.Value);
            }
              ReceiveStkItemModel rcv =  this.ReceiveStkItemModel;
              var a =   rcv.StockLotList.Where(m => m.LotID == int.Parse(this.LotID)).FirstOrDefault();
              a.AddLotLocation();
              gvStk.DataSource =  a.GetLotLocationView();
              gvStk.DataBind();

              int i = 0;
              foreach (GridViewRow r in gvStk.Rows)
              {
                  if (i < unitNumLst.Count)
                  {
                      TextBox txt = (TextBox)r.FindControl("txtEachUnitNumber");
                      DropDownList ddl = r.FindControl("ddLocationList") as DropDownList;
                      txt.Text = unitNumLst[i];
                      ddl.SelectedValue = selStkLst[i];
                      i++;
                  }
                 
              }


         
       
        }

     
        public void GvStkRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                DataTable dtLocation = new DataAccess.ReceiveStockDAO().GetLoacation(this.ReceiveStkModel.Stock_ID.ToString());

                DropDownList ddl = (DropDownList)e.Row.FindControl("ddLocationList");
                ddl.DataSource = dtLocation;
                ddl.DataBind();
            }
        }

        public void GvStkRowCommand(object sender, GridViewCommandEventArgs e) 
        {
         

          
        }

        protected void BtnDeleteRowClick(object sender, EventArgs e)
        {
            SetView();

            List<string> unitNumLst = new List<string>();
            List<string> selStkLst = new List<string>();
            
            foreach (GridViewRow r in gvStk.Rows)
            {


                DropDownList ddl = r.FindControl("ddLocationList") as DropDownList;
                TextBox txt = (TextBox)r.FindControl("txtEachUnitNumber");
                unitNumLst.Add(txt.Text.Trim());
                selStkLst.Add(ddl.SelectedItem.Value);
            }

            ReceiveStkItemModel rcv = this.ReceiveStkItemModel;
            var a = rcv.StockLotList.Where(m => m.LotID == int.Parse(this.LotID)).FirstOrDefault();
            a.DeleteLotLocation();
            gvStk.DataSource = a.GetLotLocationView();
            gvStk.DataBind();


            int i = 0;
            foreach (GridViewRow r in gvStk.Rows)
            {

                TextBox txt = (TextBox)r.FindControl("txtEachUnitNumber");
                DropDownList ddl = r.FindControl("ddLocationList") as DropDownList;
                txt.Text = unitNumLst[i];
                ddl.SelectedValue = selStkLst[i];
                i++;

            }
        
        }
  

      
    }
}
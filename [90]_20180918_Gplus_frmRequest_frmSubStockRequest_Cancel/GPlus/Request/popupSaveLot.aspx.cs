using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.ModelClass;
using GPlus.Stock;

namespace GPlus.Request
{
    public partial class popupSaveLot : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                ReceiveStkModel rcvModel = this.ReceiveStkModel; // (ReceiveStkModel)Session["ReceiveStkModel"];
                ReceiveStkItemModel rcvItem = this.ReceiveStkItemModel; //rcvModel.ReceiveStkItemList.Where(m => m.Inv_ItemID == itemId && m.Pack_ID == packId).FirstOrDefault();


                txtItemName.Text = rcvItem.Inv_ItemName;
                txtItemCode.Text = rcvItem.Inv_ItemCode;
                txtUnit.Text = rcvItem.Package_Name;

                if (rcvItem.Temp_Receive_Quantity > 0)
                {
                    txtTotalUnit.Text = rcvItem.Temp_Receive_Quantity.ToString();
                }
                else
                {
                    txtTotalUnit.Text = Request["MaxRcv"].ToString();
                }
              
                txtTotalUnit.Focus();

                rcvItem.GetLot();
                if (rcvItem.StockLotList.Count == 0)
                {
                    rcvItem.CreateLot();
                    rcvItem.StockLotList[0].LotQty = int.Parse(Request["MaxRcv"].ToString());
                    rcvItem.StockLotList[0].StockLotLocationList[0].Qty_Location = int.Parse(Request["MaxRcv"].ToString());
                }
            


                dlLot.DataSource = rcvItem.GetLotView();

                dlLot.DataBind();

                for (int i = 0; i < dlLot.Items.Count; i++)
                {
              

                    StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                    slu.ReceiveNumber =  rcvItem.StockLotList[i].LotQty.ToString();
                    slu.LotNo = rcvItem.StockLotList[i].LotNo;
                    slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
                    slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
                    slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
                    slu.LotID = rcvItem.StockLotList[i].LotID.ToString();



                    slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
                    slu.gvStk.DataBind();
                    int ii = 0;
                    foreach (GridViewRow gr in slu.gvStk.Rows)
                    {
                        TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                        DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                        ddl.SelectedValue = rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID.ToString();
                        tb.Text =  rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location.ToString();
                        ii++;
                    }

                }

        
        
            }
        }

        protected void btnAddLot_Click(object sender, EventArgs e)
        {
           btnDeleteLot.Enabled = true;

           ReceiveStkItemModel rcvItem = this.ReceiveStkItemModel;
           for (int i = 0; i < dlLot.Items.Count; i++)
           {
               StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
               rcvItem.StockLotList[i].LotQty = Util.ToInt(slu.ReceiveNumber);
               rcvItem.StockLotList[i].LotNo = slu.LotNo;
               rcvItem.StockLotList[i].BarcodeNo = slu.SupplierBarCode;
               rcvItem.StockLotList[i].BarcodePrintQty = Util.ToInt(slu.PrintCounnt);
               rcvItem.StockLotList[i].ExpireDate = (slu.ExpireDate == "") ? null : Util.ToDateTime(slu.ExpireDate);
               rcvItem.StockLotList[i].LotID = int.Parse(slu.LotID);


               int ii = 0;
               foreach (GridViewRow gr in slu.gvStk.Rows)
               {
                   TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                   DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                   rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID = Util.ToInt(ddl.SelectedValue);
                   rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location = Util.ToInt(tb.Text);

                   ii++;
               }
           }

            rcvItem.AddLot();

            //----------------  DISPLAY LOT ------------------------------

            dlLot.DataSource = rcvItem.GetLotView();
            dlLot.DataBind();

            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                slu.ReceiveNumber = rcvItem.StockLotList[i].LotQty.ToString();
                slu.LotNo = rcvItem.StockLotList[i].LotNo;
                slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
                slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
                slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
                slu.LotID = rcvItem.StockLotList[i].LotID.ToString();
                slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
                slu.gvStk.DataBind();

                int ii = 0;
                foreach (GridViewRow gr in slu.gvStk.Rows)
                {
                    TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                    DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                    ddl.SelectedValue = rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID.ToString();
                    tb.Text = rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location.ToString();

                    ii++;
                }
            }
        }

        protected void btnDeleteLot_Click(object sender, EventArgs e)
        {
            ReceiveStkItemModel rcvItem = this.ReceiveStkItemModel;
            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                rcvItem.StockLotList[i].LotQty = Util.ToInt(slu.ReceiveNumber);
                rcvItem.StockLotList[i].LotNo = slu.LotNo;
                rcvItem.StockLotList[i].BarcodeNo = slu.SupplierBarCode;
                rcvItem.StockLotList[i].BarcodePrintQty = Util.ToInt(slu.PrintCounnt);
                rcvItem.StockLotList[i].ExpireDate = (slu.ExpireDate == "") ? null : Util.ToDateTime(slu.ExpireDate);
                rcvItem.StockLotList[i].LotID = int.Parse(slu.LotID);


                int ii = 0;
                foreach (GridViewRow gr in slu.gvStk.Rows)
                {
                    TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                    DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                    rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID = Util.ToInt(ddl.SelectedValue);
                    rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location = Util.ToInt(tb.Text);

                    ii++;
                }
            }
            int count = rcvItem.GetLotView().Rows.Count;
            if (count > 1)
            {

                rcvItem.DeleteLot();


                if (!(count > 1)) btnDeleteLot.Enabled = false;


                dlLot.DataSource = rcvItem.GetLotView();
                dlLot.DataBind();
              

             
                for (int i = 0; i < rcvItem.GetLotView().Rows.Count; i++)
                {
                    StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                    slu.ReceiveNumber = rcvItem.StockLotList[i].LotQty.ToString();
                    slu.LotNo = rcvItem.StockLotList[i].LotNo;
                    slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
                    slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
                    slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
                    slu.LotID = rcvItem.StockLotList[i].LotID.ToString();


                    slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
                    slu.gvStk.DataBind();

                    int ii = 0;
                    foreach (GridViewRow gr in slu.gvStk.Rows)
                    {
                        TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                        DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                        ddl.SelectedValue = rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID.ToString();
                        tb.Text = rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location.ToString();


                        ii++;
                    }
                }
            }
            else
            {
                btnDeleteLot.Enabled = false;
            } 
        }

        private ExcecutionResult GetLotData()
        {

            int lotNumAll = 0;
            int getLot = 0;
            int freeUnit = 0;
            int totalUnit = 0;
     

            if (txtTotalUnit.Text.Trim() != "")
            {
                if (!(int.TryParse(txtTotalUnit.Text.Trim(), out totalUnit)))
                {

                    return new ExcecutionResult()
                    {
                        result = false,
                        message = "ท่านกรอกจำนวนรับไม่ถูกต้อง"
                    };
                }
                else
                {
                    int max = int.Parse(Request["MaxRcv"].ToString());
                   
               

                    if (totalUnit > max)
                    {

                        return new ExcecutionResult()
                        {
                            result = false,
                            message = "ท่านกรอกจำนวนรับไม่ถูกต้อง (เกินจากจำนวนรับสูงสุดคือ " + max.ToString() + " )"
                        };
                    }
                }
            }

            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");

                if (int.TryParse(slu.ReceiveNumber, out getLot))
                {
                    lotNumAll += Util.ToInt(slu.ReceiveNumber);
                }
                else
                {
                    return new ExcecutionResult()
                    {
                        message = "ท่านกรอกจำนวน Lot ไม่ถุกต้อง"
                      ,
                        result = false
                    };
                }


                int locAcc = 0;

                //----------  Check Quantity of Lot Location --------------------------------

                GridView gvStk = (GridView)slu.FindControl("gvStk");
                foreach (GridViewRow gf in gvStk.Rows)
                {
                    DropDownList ddl = gf.FindControl("ddLocationList") as DropDownList;


                    TextBox txt = (TextBox)gf.FindControl("txtEachUnitNumber");

                    int locQtyItem = 0;

                    if (int.TryParse(txt.Text, out locQtyItem))
                    {
                        locAcc += locQtyItem;


                    }
                    else
                    {
                        return new ExcecutionResult()
                        {
                            message = "ท่านกรอกจำนวน ใน Location ไม่ถุกต้อง"
                            ,
                            result = false
                        };
                    }

                }

                if (locAcc != getLot)
                {
                    return new ExcecutionResult()
                    {
                        message = "ท่านกรอกจำนวน ใน Location ไม่ถุกต้อง"
                        ,
                        result = false
                    };
                }
                //---------------------------------------------------------------------------

            }



       

            if (lotNumAll != freeUnit + totalUnit)
            {

                return new ExcecutionResult()
                {
                    result = false,
                    message = "ท่านกรอกจำนวนรับไม่ถูกต้อง"
                };

            }
            else
            {
                //  get lot qty
                for (int i = 0; i < dlLot.Items.Count; i++)
                {
                    StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");


                    this.ReceiveStkItemModel.StockLotList[i].LotQty = Util.ToInt(slu.ReceiveNumber);
                    this.ReceiveStkItemModel.StockLotList[i].LotNo = slu.LotNo;
                    this.ReceiveStkItemModel.StockLotList[i].BarcodeNo = slu.SupplierBarCode;
                    this.ReceiveStkItemModel.StockLotList[i].BarcodePrintQty = Util.ToInt(slu.PrintCounnt);
                    this.ReceiveStkItemModel.StockLotList[i].ExpireDate = Util.ToDateTime(slu.ExpireDate);


                    //------------------------       --------------------------------------
                    List<string> locationLst = new List<string>();
                    GridView gvStk = (GridView)slu.FindControl("gvStk");
                    int ii = 0;
                    foreach (GridViewRow gf in gvStk.Rows)
                    {

                        DropDownList ddl = gf.FindControl("ddLocationList") as DropDownList;

                        TextBox txt = (TextBox)gf.FindControl("txtEachUnitNumber");

                        if (chkDup(locationLst, ddl.SelectedItem.Value))
                        {
                            return new ExcecutionResult()
                            {
                                result = false,
                                message = "ท่านกรอก Location ไม่ถูกต้อง"
                            };
                        }
                        this.ReceiveStkItemModel.StockLotList[i].StockLotLocationList[ii].Qty_Location = int.Parse(txt.Text.Trim());
                        this.ReceiveStkItemModel.StockLotList[i].StockLotLocationList[ii].Qty_Out = 0;
                        this.ReceiveStkItemModel.StockLotList[i].StockLotLocationList[ii].LocationID = int.Parse(ddl.SelectedItem.Value);
                        ii++;
                        locationLst.Add(ddl.SelectedItem.Value);
                    }

                    //----------------------------------------------------------------------

                }
                //  get give away

                this.ReceiveStkItemModel.GiveAway_Quantity = freeUnit;
                this.ReceiveStkItemModel.Temp_Receive_Quantity = totalUnit;

                return new ExcecutionResult()
                {
                    result = true,
                    message = ""
                };

            }


        }



        private bool chkDup(List<string> chk, string val)
        {
            foreach (var a in chk)
            {
                if (a == val)
                {
                    return true;
                }
            }
            return false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExcecutionResult exe = GetLotData();


            // success
            if (exe.result)
            {

                string scriptStr = "if(window.opener)window.opener.document.getElementById('ContentPlaceHolder1_btnRefreshItem').click(); window.close();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closing", scriptStr, true);

            }
            else
            {

                ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('" + exe.message + "');</script>");

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
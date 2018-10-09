using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.Data;

namespace GPlus.Request
{
    public partial class popupReorderPoint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Response.Expires = 0;

            if (!Page.IsPostBack)
            {
                Session.Remove("SearchData");
                Session.Remove("ResultData");

                ddlMaterialType.DataSource = new StockDAO().RepInvGetAllCatagory();
                ddlMaterialType.DataBind();
                ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", "0"));
                panel0.Visible = false;
                panel1.Visible = false;

            }


          pagingControlReqList.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }


        private void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = Session["SearchData"] as DataTable;//new ReceiveStockDAO().GetReceiveStkNotCompleteSearch(poCode, stkNo, dateStPO, dateEnPO, dateStStk, dateEnStk);
            DataTable dtt = new DataTable();
            foreach (DataColumn dc in dt.Columns)
            {
                dtt.Columns.Add(dc.ColumnName, dc.DataType);
            }

            var rows = dt.AsEnumerable().Skip((pagingControlReqList.CurrentPageIndex-1) * pagingControlReqList.PageSize).Take(pagingControlReqList.PageSize);
            rows.CopyToDataTable(dtt, LoadOption.OverwriteChanges);
            pagingControlReqList.RecordCount = dt.Rows.Count;
            gvRequestItem.DataSource = dtt;
            gvRequestItem.DataBind();
          
        }
        protected void ddlSubMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
     
             
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = new StockDAO().RepGetSubCateByCateID(int.Parse(ddlMaterialType.SelectedValue));

                ddlSubMaterialType.DataSource = dt;
                ddlSubMaterialType.DataBind();
                pagingControlReqList.RecordCount = dt.Rows.Count;
                pagingControlReqList.PageSize = 10;
                if (dt.Rows.Count > 0)
                {
                    ddlSubMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", "0"));
                }

            }
            catch (Exception ex)
            {
                Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                Response.Redirect("../Stock/Error.aspx");
                //  ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('Error " + ex.ToString() + "');</script>");
            }
  
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            DataTable dtFilter = new DataTable();
            dt = new RequestDAO().ReqInvItemReorderPoint(Util.ToInt(ddlMaterialType.SelectedValue),Util.ToInt(ddlSubMaterialType.SelectedValue), int.Parse(Request["Stock_ID"]));
            foreach (DataColumn dc in dt.Columns)
            {
                dtFilter.Columns.Add(dc.ColumnName, dc.DataType);
            }
           EnumerableRowCollection<DataRow> result = dt.AsEnumerable();

            // filter by condition  -----------------------------------------------
           if (int.Parse(ddlMaterialType.SelectedValue) > 0)
           {
               result = dt.AsEnumerable().Where(r => r["Cate_ID"].ToString() == ddlMaterialType.SelectedValue);
              
               if (int.Parse(ddlSubMaterialType.SelectedValue) > 0)
               {
                   result = result.Where(r => r["SubCate_ID"].ToString() == ddlSubMaterialType.SelectedValue);
               }
          
           }
           result = result.Where(r => r["Inv_ItemCode"].ToString().Contains(txtItemCode.Text.Trim()) && r["Inv_ItemName"].ToString().Contains(txtItemName.Text.Trim()));

            //  end filter   -------------------------------------------------------

           result.CopyToDataTable(dtFilter, LoadOption.OverwriteChanges);
            
          
            Session["SearchData"] = dtFilter;
            if (dt.Rows.Count > 0)
            {
                DataTable dtt = new DataTable();
                foreach (DataColumn dc in dtFilter.Columns)
                {
                    dtt.Columns.Add(dc.ColumnName, dc.DataType);
                }

                var rows = dtFilter.AsEnumerable().Skip((pagingControlReqList.CurrentPageIndex - 1) * pagingControlReqList.PageSize).Take(pagingControlReqList.PageSize);
                rows.CopyToDataTable(dtt, LoadOption.OverwriteChanges);
                pagingControlReqList.RecordCount = dtFilter.Rows.Count;
                gvRequestItem.DataSource = dtt;
                gvRequestItem.DataBind();

                panel0.Visible = true;
                panel1.Visible = true;
                gvRequestItem.Visible = true;
                lbNotfound.Visible = false;
            }
            else
            {
                panel0.Visible = true;
                panel1.Visible = true;
                gvRequestItem.Visible = false;
                lbNotfound.Visible = true;
                pagingControlReqList.RecordCount = 0;
            }
      

        }


        protected void GvRequestItemResultRowDeleting(object sender,GridViewDeleteEventArgs e)
        {

        }

        protected void GvRequestListResultRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string[] s = e.CommandArgument.ToString().Split('/');
                string itemId = s[0];
                string packId = s[1];
                DataTable dt =  Session["ResultData"] as DataTable;
                int i = 0;
                int rowDel=0;
              
                foreach(DataRow r in dt.Rows){
                    if (r["Inv_ItemID"].ToString() == itemId && r["Pack_ID"].ToString() == packId)
                    {
                        rowDel = i;
                        break;
                    }
                    i++;
                }
                dt.Rows.RemoveAt(rowDel);
                gvRequestItemResult.DataSource = dt;
                gvRequestItemResult.DataBind();
            }
        }


        protected void GvRequestItemResultRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btn = ((LinkButton)e.Row.FindControl("lbDelete"));
                btn.CommandArgument = drv["Inv_ItemID"].ToString() + "/" + drv["Pack_ID"].ToString();

                TextBox tb = (TextBox)e.Row.FindControl("txt_OrderQty");
                tb.Text = drv["Order_Quantity"].ToString();
            }
        }


        protected void GvRequestListRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string[] s = e.CommandArgument.ToString().Split('/');
                string itemId = s[0];
                string packId = s[1];
                string rIndex = s[2];
                string sQty = (gvRequestItem.Rows[int.Parse(rIndex)].FindControl("txt_OrderQty") as TextBox).Text.Trim();

                 int rQty = 0;
                 if (int.TryParse(sQty, out rQty))
                 {
                
                     DataTable dt = Session["SearchData"] as DataTable;
                     DataTable dtr = Session["ResultData"] as DataTable;
                     if (dtr == null)
                     {
                         dtr = new DataTable();
                         foreach (DataColumn dc in dt.Columns)
                         {
                             dtr.Columns.Add(dc.ColumnName, dc.DataType);
                         }
                     }
                     var rows = dt.AsEnumerable().Where(r => r["Inv_ItemID"].ToString() == itemId && r["Pack_ID"].ToString() == packId).FirstOrDefault();
                     rows["Order_Quantity"] = rQty;
                     bool dup = false;
                     foreach (DataRow r in dtr.Rows)
                     {
                         if (r["Inv_ItemID"].ToString() == rows["Inv_ItemID"].ToString() && r["Pack_ID"].ToString() == rows["Pack_ID"].ToString())
                         {
                             dup = true;
                         }
                     }
                     if (!dup)
                     {
                         dtr.ImportRow(rows);
                     }
                     else
                     {
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('ท่านได้เลือกรายการนี้แล้ว');", true);
                         return;
                     }



                     gvRequestItemResult.DataSource = dtr;
                     gvRequestItemResult.DataBind();
                     Session["ResultData"] = dtr;
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('กรอกจำนวนให้ถูกต้อง');", true);
                 return;
             }

            }
        }


        protected void GvRequestItemRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btn = ((LinkButton)e.Row.FindControl("lbSelect"));
                btn.CommandArgument = drv["Inv_ItemID"].ToString() + "/" + drv["Pack_ID"].ToString() + "/" + e.Row.RowIndex; ;
               
             
            }
        }


        protected void btCancel_Click(object sender, EventArgs e)
        {
            Session.Remove("SearchData");
            Session.Remove("ResultData");
        }

        protected void btClose_Click(object sender, EventArgs e)
        {
            string scriptStr = "if(window.opener) window.close();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closing", scriptStr, true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            // check valid number

            if (gvRequestItemResult.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('ท่านยังไม่ได้เลือกรายการ');", true);
                return;
            }
            foreach (GridViewRow gr in gvRequestItemResult.Rows)
            {
              int rQty = 0;
              TextBox tb =  gr.FindControl("txt_OrderQty") as TextBox;
              if (!int.TryParse(tb.Text.Trim(), out rQty))
              {
                  ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('กรอกจำนวนให้ถูกต้อง');", true);
                  return;
              }
              else
              {
                  if (rQty <= 0)
                  {
                      ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('กรอกจำนวนให้ถูกต้อง');", true);
                      return;
                  }

                  //------------  ALL NUMBER IS VALID   -----------------------

              }
            
            }

            string scriptStr = "if(window.opener)window.opener.document.getElementById('ContentPlaceHolder1_btnRefreshItem3').click(); window.close();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closing", scriptStr, true);
        }
    }
}
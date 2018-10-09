using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using GPlus.DataAccess;

namespace GPlus.PRPO
{
    public partial class pop_ProductHelp : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["st_id"] != null)
            {
                hid_Stock_id.Value = Request.QueryString["st_id"];
                
                PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            div_item.Visible = true;
            
            PagingControl1.CurrentPageIndex = 1;
            PagingControl1.PageSize = 10;

            BindItemHelp();
        }


        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindItemHelp();
        }


        private void BindItemHelp()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Stock_Id_From", hid_Stock_id.Value));

            if (txtProductCode.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Itemcode", txtProductCode.Text.Trim()));
            }

            if (txtProductName.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@ItemDescription", txtProductName.Text.Trim()));
            }


            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_ItemHelp", param);
            PagingControl1.RecordCount = (int)dt.Rows.Count;
            int page = (PagingControl1.CurrentPageIndex - 1) * PagingControl1.PageSize;

            if (dt.Rows.Count > 0)
                gvItem.DataSource = dt.Select().Skip(page).Take(PagingControl1.PageSize).AsEnumerable().CopyToDataTable();
            else
                gvItem.DataSource = dt;
            
            gvItem.DataBind();

            //gvItem.DataSource = dt;
            //gvItem.DataBind();





            
        }



        protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Choose")
            {
                string inv_Itemcode = e.CommandArgument.ToString().Split(',')[0].ToString();
                string Item_Search_Desc = e.CommandArgument.ToString().Split(',')[1].ToString();
                string Pack_Description = e.CommandArgument.ToString().Split(',')[2].ToString();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", @" 
                
                window.opener.document.getElementById('ContentPlaceHolder1_txt_No').value='" + inv_Itemcode + @"';
                window.opener.document.getElementById('ContentPlaceHolder1_txt_item_name').value='" + Item_Search_Desc + @"';
                window.opener.document.getElementById('ContentPlaceHolder1_txt_pack').value='" + Pack_Description + @"';
                window.close();
                ", true);


//                Response.Write(@"<script type='text/javascript'>
//                                window.opener.document.getElementById('#ContentPlaceHolder1_txt_No').val(" + inv_Itemcode + @");
//                                window.close();
//                                </script>");

                


                

            }
            
        }
    }
}
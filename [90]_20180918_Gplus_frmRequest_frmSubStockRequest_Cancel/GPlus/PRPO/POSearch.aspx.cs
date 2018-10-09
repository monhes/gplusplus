using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace GPlus.PRPO
{
    public partial class POSearch : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "305";

                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
            //POControl1.SavePO += new EventHandler(POControl1_SavePO);
            //POControl1.CancelPO += new EventHandler(POControl1_CancelPO);
            //POControl1.DeletePO += new EventHandler(POControl1_DeletePO);
        }

        void POControl1_DeletePO(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }

        void POControl1_CancelPO(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }

        void POControl1_SavePO(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            BindData();
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPOCodeSearch.Text = "";
            ccFrom.Text = "";
            ccTo.Text = "";
            rblStockType.SelectedIndex = 2;
            ddlPOType.SelectedIndex = 0;
        }

        protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                ((ImageButton)e.Row.FindControl("btnPrint")).OnClientClick = "open_popup('pop_PO.aspx?id=" + drv["PO_ID"].ToString()
                + "', 850, 450, 'PRReport', 'yes', 'yes', 'yes'); return false;";

                if (drv["PO_Type"].ToString() == "1")
                    e.Row.Cells[3].Text = "สั่งซื้อ";
                else if (drv["PO_Type"].ToString() == "2")
                    e.Row.Cells[3].Text = "สั่งจ้าง";

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateFormat);

                if (drv["Net_Amonut"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((decimal)drv["Net_Amonut"]).ToString(this.CurrencyFormat);


                switch (drv["Status"].ToString().ToUpper())
                {
                    case "0": e.Row.Cells[8].Text = "ยกเลิก"; break;
                    case "1": e.Row.Cells[8].Text = "รออนุมัติ"; break;
                    case "2": e.Row.Cells[8].Text = "อนุมัติ";  break;
                    case "3": e.Row.Cells[8].Text = "ไม่อนุมัติ"; break;
                    case "4": e.Row.Cells[8].Text = "ตรวจสอบ PO"; break;
                    case "5": e.Row.Cells[8].Text = "ไม่ดำเนินการสั่งซื้อ"; break;
                    case "U": e.Row.Cells[8].Text = "Upload PO";  break;
                    case "P": e.Row.Cells[8].Text = "Download PO"; break;
                }

                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                btnDetail.CommandArgument = drv["PO_ID"].ToString();
                ImageButton btnPrint = (ImageButton)e.Row.FindControl("btnPrint");
            }
        }

        protected void gvPO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                pnlDetail.Visible = true;
                POControl1.BindPO(e.CommandArgument.ToString());
                //POControl1.Enable = false;
                POControl1.DisableDetail();
            }
        }



        protected void gvPO_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvPO);
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.PODAO().GetPOForm1(
                            txtPOCodeSearch.Text, 
                            ddlPOType.SelectedValue, 
                            rblStockType.SelectedValue,
                            ccFrom.Text,
                            ccTo.Text,
                            "", 
                            "", 
                            chkIsUpload.Checked ? "1" : "0", 
                            "", 
                            "", 
                            "", 
                            PagingControl1.CurrentPageIndex, 
                            PagingControl1.PageSize,
                            this.SortColumn, 
                            this.SortOrder
                        );

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPO.DataSource = ds.Tables[0];
            gvPO.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace GPlus.Stock
{
    public partial class InvEditListingReport : Pagebase
    {
        public DataTable InvEditListingReportPackageTable
        {
            get
            {
                return (DataTable)Session["InvEditListingReport"];
            }
            set
            {
                Session["InvEditListingReport"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                this.PageID = "422";
                BindDropdown();
                ReportViewer1.Visible = false;
            }
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            DataTable dtCategory = new DataAccess.CategoryDAO().GetCategoryAll();

            ddlCategory.DataSource = dtCategory;
            ddlCategory.DataTextField = "Cat_Name";
            ddlCategory.DataValueField = "Cate_Id";
            ddlCategory.DataBind();

            //ddlCategory.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            ddlCategory.SelectedValue = "8"; //default เครื่องเขียน

            dtCreateStart.Text = DateTime.Now.ToString(this.DateFormat);

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlStock.SelectedIndex == 0)
            {
                ShowMessageBox("กรุณาเลือกคลังสินค้า");
                return;
            }
            if (dtCreateStart.Text == "")
            {
                ShowMessageBox("กรุณาระบุวันที่");
                return;
            }
            //ShowMessageBox("Code : " + ItemControl2.ItemCode + " Name : " + ItemControl2.ItemName + " Pack : " + ItemControl2.PackName);
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlStock.SelectedIndex = 0;
            ddlCategory.SelectedValue = "8"; //default เครื่องเขียน
            dtCreateStart.Text = "";
            ItemControl2.Clear();
            ReportViewer1.Visible = false;
        }

        private void BindData()
        {

            DataSet ds = new DataAccess.StockDAO().StockInvEditListing(ddlStock.SelectedValue, dtCreateStart.Text,
               ItemControl2.ItemID, ItemControl2.PackID, ddlCategory.SelectedValue);

            if (ds.Tables[0].Rows.Count == 0)
            {
                ShowMessageBox("ไม่พบข้อมูล");
                ReportViewer1.Visible = false;
            }
            else
            {

                DataTable dt = ds.Tables[0];

                dt.Columns.Add("StartDate");
                string[] str_date = dtCreateStart.Text.Split('/');
                dt.Rows[0]["StartDate"] = str_date[0] + "/" + str_date[1] + "/" + (Convert.ToInt32(str_date[2] == "" ? "0" : str_date[2]) + 543).ToString();

                /* คำนวณค่าคงเหลือของแต่ละแถว และแต่ละ Item จาก คงเหลือ (แถวก่อนหน้า) + รับเข้า - จ่ายออก */
                int temptotal = 0;
                dt.Columns.Add("Total");
                for(int i=0;i < dt.Rows.Count ; i++)
                {
                    if(dt.Rows[i]["Income_Balance"].ToString() != "" && dt.Rows[i]["Transaction_Date"].ToString() == "") //แสดงว่าเป็นแถวแรก ของแต่ละ item
                    {
                        temptotal = Convert.ToInt32(dt.Rows[i]["Income_Balance"].ToString() == ""?"0":dt.Rows[i]["Income_Balance"].ToString());
                        dt.Rows[i]["Total"] = temptotal.ToString("#,###");
                    }
                    else
                    {
                        temptotal = temptotal+Convert.ToInt32(dt.Rows[i]["Recieve_Qty"].ToString() == ""?"0":dt.Rows[i]["Recieve_Qty"].ToString())-Convert.ToInt32(dt.Rows[i]["Pay_Qty"].ToString() == ""?"0":dt.Rows[i]["Pay_Qty"].ToString());
                        dt.Rows[i]["Total"] = temptotal.ToString("#,###");
                    }
                
                }

                ReportViewer1.Visible = true;

                ReportDataSource rds1 = new ReportDataSource("InvEditListing", ds.Tables[0]);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds1);


                this.ReportViewer1.LocalReport.Refresh();
                
            }

        }

    }
}
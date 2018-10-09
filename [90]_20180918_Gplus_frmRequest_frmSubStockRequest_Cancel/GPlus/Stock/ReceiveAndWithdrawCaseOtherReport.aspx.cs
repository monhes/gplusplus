using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace GPlus.PRPO
{
    public partial class ReceiveAndWithdrawCaseOtherReport : Pagebase
    {
        public DataTable ReceiveAndWithdrawCaseOtherReportPackageTable
        {
            get
            {
                return (DataTable)Session["ReceiveAndWithdrawCaseOtherReport"];
            }
            set
            {
                Session["ReceiveAndWithdrawCaseOtherReport"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "417";
                BindDropdown();
                ReportViewer1.Visible = false;
                supplierDdlCtrl.EnableValidator = false;

            }

        }


        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            //ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            ddlMaterialType.DataSource = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
            ddlMaterialType.DataTextField = "Cat_Name";
            ddlMaterialType.DataValueField = "Cate_ID";
            ddlMaterialType.DataBind();
            ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
        }

        private void BindSubMaterialType()
        {
            if (ddlMaterialType.SelectedIndex > 0)
            {
                ddlSubMaterialType.DataSource = new DataAccess.CategoryDAO().GetSubCate("", ddlMaterialType.SelectedValue, "",
                    "1", 1, 1000, "", "").Tables[0];
                ddlSubMaterialType.DataTextField = "SubCate_Name";
                ddlSubMaterialType.DataValueField = "SubCate_ID";
                ddlSubMaterialType.DataBind();
                ddlSubMaterialType.Items.Insert(0, new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
            }
            else
                ddlSubMaterialType.Items.Clear();
        }

       

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            //if (dtStart.Text == "")
            //{
            //    ShowMessageBox("กรูณาระบุวันที่เริ่มต้น");
            //    return;
            //}
            //else if (dtStop.Text == "")
            //{
            //    ShowMessageBox("กรูณาระบุวันที่สิ้นสุด");
            //    return;
            //}
            BindData();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {
           
            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            dtStart.Text = "";
            dtStop.Text = "";
            txtItemCode.Text = "";
            txtItemName.Text = "";
            rdbType.SelectedIndex = 0;
            
        }

        private void BindData()
        {
            DropDownList ddlSupplier = supplierDdlCtrl.FindControl("ddlSupplier") as DropDownList;

            DataTable dt = new DataAccess.ReceiveStockDAO().GetReceiveAndWithdrawOtherReport(rdbType.SelectedValue, ddlStock.SelectedValue, dtStart.Text, dtStop.Text , ddlSupplier.SelectedValue, ddlMaterialType.SelectedValue, ddlSubMaterialType.SelectedValue, txtItemCode.Text, txtItemName.Text);
            ReportDataSource rds1 = new ReportDataSource("ReceiveAndWithdrawCaseOther", dt);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds1);

            if (dt.Rows.Count == 0)
            {
                ReportViewer1.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            else if (dt.Rows.Count > 0)
            {
                ReportViewer1.Visible = true;

                dt.Columns.Add("StartDate");
                if (dtStart.Text != "")
                {
                    string[] dtStartTemp = dtStart.Text.Split('/');
                    dt.Rows[0]["StartDate"] = dtStartTemp[0] + "/" + dtStartTemp[1] + "/" + (Int32.Parse(dtStartTemp[2]) + 543).ToString();
                }

                dt.Columns.Add("EndDate");
                if (dtStop.Text != "")
                {
                    string[] dtEndTemp = dtStop.Text.Split('/');
                    dt.Rows[0]["EndDate"] = dtEndTemp[0] + "/" + dtEndTemp[1] + "/" + (Int32.Parse(dtEndTemp[2]) + 543).ToString();
                }

                dt.Columns.Add("CateName");
                if (ddlMaterialType.SelectedIndex != 0)
                {
                    dt.Rows[0]["CateName"] = ddlMaterialType.SelectedItem.Text;
                }

                dt.Columns.Add("FormType");
                dt.Columns.Add("FormDesc");
                if (rdbType.SelectedIndex == 0)
                {
                    dt.Rows[0]["FormType"] = "รายการรับสินค้าเข้ากรณีอื่นๆ " + ddlStock.SelectedItem.Text;
                    dt.Rows[0]["FormDesc"] = "รับ";
                }
                else if (rdbType.SelectedIndex == 1)
                {
                    dt.Rows[0]["FormType"] = "รายการจ่ายออกสินค้ากรณีอื่นๆ " + ddlStock.SelectedItem.Text;
                    dt.Rows[0]["FormDesc"] = "จ่าย";
                }
                else
                {
                    dt.Rows[0]["FormType"] = "รายการ Adjust Stock " + ddlStock.SelectedItem.Text;
                    dt.Rows[0]["FormDesc"] = " Adjust Stock";
                }

                dt.Columns.Add("RowColor");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Row"].ToString() != "")
                    {
                        dt.Rows[i]["RowColor"] = "Plum";
                    }
                    else
                    {
                        dt.Rows[i]["RowColor"] = "Transparent";
                    }
                }

            }
            this.ReportViewer1.LocalReport.Refresh();
        }


        
    }
}
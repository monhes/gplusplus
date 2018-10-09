using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class SupplierMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "110";
                BindProvince();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtSupplierCodeSearch.Text = "";
            txtSupplierNameSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = true;
            ClearData();
            btnPopAccount.Visible = false;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
        }

        protected void gvSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["Supplier_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Supplier_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvSupplier_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                hdID.Value = e.CommandArgument.ToString();
                DataTable dt = new DataAccess.SupplierDAO().GetSupplier(hdID.Value);
                btnPopAccount.OnClientClick = "open_popup('pop_SupplierAccount.aspx?sid=" + hdID.Value
                    +"', 840, 400, 'popAcc', 'yes', 'yes', 'yes'); return false;";
                btnPopAccount.Visible = true;
                if (dt.Rows.Count > 0)
                {
                    txtSupplierCode.Text = dt.Rows[0]["Supplier_Code"].ToString();
                    txtSupplierName.Text = dt.Rows[0]["Supplier_Name"].ToString();
                    txtAddress.Text = dt.Rows[0]["Supplier_Address"].ToString();
                    if (dt.Rows[0]["Province_Code"].ToString().Trim().Length > 0)
                    {
                        if (ddlProvince.Items.FindByValue(dt.Rows[0]["Province_Code"].ToString()) != null)
                        {
                            ddlProvince.SelectedValue = dt.Rows[0]["Province_Code"].ToString();
                            BindAmphur();
                        }
                    }

                    if (dt.Rows[0]["Amphur_Code"].ToString().Trim().Length > 0)
                    {
                        if (ddlAmphur.Items.FindByValue(dt.Rows[0]["Amphur_Code"].ToString()) != null)
                        {
                            ddlAmphur.SelectedValue = dt.Rows[0]["Amphur_Code"].ToString();
                            BindTumbon();
                        }
                    }

                    if (dt.Rows[0]["Tumbol_Code"].ToString().Trim().Length > 0)
                    {
                        if (ddlTumbon.Items.FindByValue(dt.Rows[0]["Tumbol_Code"].ToString()) != null)
                            ddlTumbon.SelectedValue = dt.Rows[0]["Tumbol_Code"].ToString();
                    }
                    txtPostCode.Text = dt.Rows[0]["Postal_Code"].ToString();
                    txtTel.Text = dt.Rows[0]["Telephone_Number"].ToString();
                    txtEmail.Text = dt.Rows[0]["e-mail"].ToString();
                    chkEmailPO.Checked = dt.Rows[0]["MailPo_Flag"].ToString() == "1";
                    txtFax.Text = dt.Rows[0]["Fax_Number"].ToString();
                    if (ddlBillSupplier.Items.FindByValue(dt.Rows[0]["Billing_Address_Sup_Id"].ToString()) != null)
                        ddlBillSupplier.SelectedValue = dt.Rows[0]["Billing_Address_Sup_Id"].ToString();

                    chkCash.Checked = dt.Rows[0]["Payment_Type"].ToString() == "1";
                    chkCheque.Checked = dt.Rows[0]["Payment_Type"].ToString() == "2";
                    txtCreditTerm.Text = dt.Rows[0]["Credit_Term"].ToString();

                    chkIncludeVat.Checked = dt.Rows[0]["IncludeVat_Flag"].ToString() == "1";

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Supplier_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Supplier_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);

                    //rdbSupplierType.Items[0].Selected = dt.Rows[0]["Supplier_Type"].ToString() == "E";
                    //rdbSupplierType.Items[1].Selected = dt.Rows[0]["Supplier_Type"].ToString() == "I";
                    chkSupplier_Type.Checked = dt.Rows[0]["Supplier_Type"].ToString() == "I";
                }
                pnlDetail.Visible = true;
            }
        }

        protected void gvSupplier_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvSupplier);
        }


        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAmphur();
        }

        protected void ddlAmphur_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTumbon();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retVal = "";
            //string supplier_type = rdbSupplierType.SelectedIndex == 0 ? "E" : "I";
            string supplier_type = chkSupplier_Type.Checked == true ? "I" : "E";
            if (hdID.Value.Trim().Length == 0)
            {
               retVal = new DataAccess.SupplierDAO().AddSupplier(txtSupplierCode.Text, txtSupplierName.Text, txtAddress.Text, ddlProvince.SelectedValue,
                    ddlAmphur.SelectedValue, ddlTumbon.SelectedValue, txtPostCode.Text, txtTel.Text, txtEmail.Text, chkEmailPO.Checked ? "1" : "0",
                    txtFax.Text, ddlBillSupplier.SelectedValue, chkCheque.Checked ? "2" : "1", chkIncludeVat.Checked ? "1" : "0", txtCreditTerm.Text,
                     status, this.UserName, supplier_type);
               if (retVal != "0") hdID.Value = retVal;

               this.ShowMessageBox("Supplier "+txtSupplierName.Text+" รหัส "+txtSupplierCode.Text+" สร้างสำเร็จ");
            }
            else
            {
                retVal = new DataAccess.SupplierDAO().UpdateSupplier(hdID.Value, txtSupplierCode.Text, txtSupplierName.Text, txtAddress.Text, ddlProvince.SelectedValue,
                    ddlAmphur.SelectedValue, ddlTumbon.SelectedValue, txtPostCode.Text, txtTel.Text, txtEmail.Text, chkEmailPO.Checked ? "1" : "0",
                    txtFax.Text, ddlBillSupplier.SelectedValue, chkCheque.Checked ? "2" : "1", chkIncludeVat.Checked ? "1" : "0", txtCreditTerm.Text,
                     status, this.UserName, supplier_type);
            }
            if (retVal == "0")
            {
                this.ShowMessageBox("มีรหัส "+txtSupplierCode.Text+" นี้อยู่ในระบบแล้ว");
                txtSupplierCode.Focus();
                return;
            }

            BindData();

            ddlBillSupplier.DataSource = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
            ddlBillSupplier.DataBind();
            ddlBillSupplier.Items.Insert(0, new ListItem("เลือกสถานที่วางบิล", ""));

            pnlDetail.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }


        private void BindProvince()
        {
            ddlBillSupplier.DataSource = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
            ddlBillSupplier.DataBind();
            ddlBillSupplier.Items.Insert(0, new ListItem("เลือกสถานที่วางบิล", ""));

            ddlProvince.DataSource = new DataAccess.ProvinceDAO().GetProvince();
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("เลือกจังหวัด", ""));
        }

        private void BindAmphur()
        {
            if (ddlProvince.SelectedIndex > 0)
            {
                ddlAmphur.DataSource = new DataAccess.ProvinceDAO().GetAmphur(ddlProvince.SelectedValue);
                ddlAmphur.DataBind();
                ddlAmphur.Items.Insert(0, new ListItem("เลือกอำเภอ", ""));
            }
            else
            {
                ddlAmphur.Items.Clear();
            }
        }

        private void BindTumbon()
        {
            if (ddlAmphur.SelectedIndex > 0)
            {
                ddlTumbon.DataSource = new DataAccess.ProvinceDAO().GetTumbon(ddlProvince.SelectedValue, ddlAmphur.SelectedValue);
                ddlTumbon.DataBind();
                ddlTumbon.Items.Insert(0, new ListItem("เลือกตำบล", ""));
            }
            else
            {
                ddlTumbon.Items.Clear();
            }
        }



        private void BindData()
        {
            DataSet ds = new DataAccess.SupplierDAO().GetSupplier(txtSupplierCodeSearch.Text, txtSupplierNameSearch.Text, ddlStatus.SelectedValue,
            PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvSupplier.DataSource = ds.Tables[0];
            gvSupplier.DataBind();

        }

        private void ClearData()
        {
            hdID.Value = "";
            txtSupplierCode.Text = "";
            txtSupplierName.Text = "";
            txtAddress.Text = "";
            ddlProvince.SelectedIndex = 0;
            BindAmphur();
            BindTumbon();
            txtPostCode.Text = "";
            txtTel.Text = "";
            txtEmail.Text = "";
            chkEmailPO.Checked = false;
            txtFax.Text = "";
            ddlBillSupplier.SelectedIndex = 0;
            chkCheque.Checked = false;
            chkCash.Checked = false;
            txtCreditTerm.Text = "";
            chkIncludeVat.Checked = false;
            rdbStatus.SelectedIndex = 0;
            //rdbSupplierType.SelectedIndex = 0;
            chkSupplier_Type.Checked = false;
            btnPopAccount.Visible = false;
            
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}
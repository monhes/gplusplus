using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace GPlus.MasterData
{
    public partial class TypeRequisitionMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "125";
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        public DataTable dtOrgFile
        {
            get { return (ViewState["dtOrgFile"] == null) ? null : (DataTable)ViewState["dtOrgFile"]; }
            set { ViewState["dtOrgFile"] = value; }
        }

        public DataTable dtOrgFileDeleted
        {
            get { return (ViewState["dtOrgFileDeleted"] == null) ? null : (DataTable)ViewState["dtOrgFileDeleted"]; }
            set { ViewState["dtOrgFileDeleted"] = value; }
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
            ddlMaterialTypeSearch.SelectedIndex = 0;
            ddlUserGroupSearch.Items.Clear();
            orgCtrlSearch.Clear();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            BindOrgGridview();
            ddlMaterialType.Enabled = true;
            ddlUserGroup.Enabled = true;
            pnlDetail.Visible = true;
            //lblCreateBy.Text = this.FirstName + " " + this.LastName;
            //lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            //lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            //lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
        
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];

            ddlMaterialTypeSearch.Items.Clear();
            ddlMaterialTypeSearch.Items.Add(new ListItem("เลือกประเภท", ""));

            ddlMaterialType.Items.Clear();
            ddlMaterialType.Items.Add(new ListItem("เลือกประเภท", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlMaterialTypeSearch.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));

                ddlMaterialType.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));
            }

        }

        protected void ddlMaterialTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUserGroupSearch();
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUserGroup();
        }

        private void BindUserGroupSearch()
        {
            if (ddlMaterialTypeSearch.SelectedIndex > 0)
            {
                DataTable dt = new DataAccess.TypeDAO().GetTypeByCateID(ddlMaterialTypeSearch.SelectedValue);
                ddlUserGroupSearch.Items.Clear();
                ddlUserGroupSearch.Items.Insert(0, new ListItem("กลุ่มผู้ใช้งาน", ""));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlUserGroupSearch.Items.Add(new ListItem(dt.Rows[i]["Type_Code"].ToString() + " - " + dt.Rows[i]["Type_Name"].ToString(),
                        dt.Rows[i]["Type_ID"].ToString()));
                }
            }
            else
                ddlUserGroupSearch.Items.Clear();
        }

        private void BindUserGroup()
        {
            if (ddlMaterialType.SelectedIndex > 0)
            {
                DataTable dt = new DataAccess.TypeDAO().GetTypeByCateID(ddlMaterialType.SelectedValue);
                ddlUserGroup.Items.Clear();
                ddlUserGroup.Items.Insert(0, new ListItem("กลุ่มผู้ใช้งาน", ""));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlUserGroup.Items.Add(new ListItem(dt.Rows[i]["Type_Code"].ToString() + " - " + dt.Rows[i]["Type_Name"].ToString(),
                        dt.Rows[i]["Type_ID"].ToString()));
                }
            }
            else
                ddlUserGroup.Items.Clear();
        }

        protected void gvTypeRequisition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Cate_Id"].ToString() + "&" + drv["Type_Id"].ToString();
            }
        }

        protected void gvTypeRequisition_Sorting(object sender, GridViewSortEventArgs e)
        {
            //SetSortGridView(e.SortExpression);
            //BindData();
            //this.GridViewSort(gvPackage);
        }

        protected void gvTypeRequisition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                string[] cmd = e.CommandArgument.ToString().Split('&');

                if (dtOrgFileDeleted != null)
                {
                    dtOrgFileDeleted.Clear();
                }

                dtOrgFile = new DataAccess.OrgStructureDAO().GetTypeRequisitionByID(cmd[0], cmd[1]);
                if (dtOrgFile.Rows.Count > 0)
                {
                     BindDropdownDetail(dtOrgFile.Rows[0]["Cate_Id"].ToString(), dtOrgFile.Rows[0]["Type_Id"].ToString());
                     BindOrgGridview();
                }
                ddlMaterialType.Enabled = false;
                ddlUserGroup.Enabled = false;
                pnlDetail.Visible = true;

            }
        }


        private void BindDropdownDetail(string CateId, string TypeId)
        {
            ddlMaterialType.Enabled = false;
            ddlUserGroup.Enabled = false;
            ddlMaterialType.SelectedValue = CateId;
            BindUserGroup();
            ddlUserGroup.SelectedValue = TypeId;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlMaterialType.SelectedValue == "")
            {
                ShowMessageBox("กรุณาเลือกประเภทวัสดุอุปกรณ์");
                return;
            }
            if (ddlUserGroup.SelectedValue == "")
            {
                ShowMessageBox("กรุณาเลือกกลุ่มผู้ใช้งาน");
                return;
            }

            bool result =  new DataAccess.OrgStructureDAO().TypeRequisition_InsertUpdate(dtOrgFile, dtOrgFileDeleted, ddlMaterialType.SelectedValue, ddlUserGroup.SelectedValue, this.UserID);

            if (result)
            {
                ShowMessageBox("บันทึกข้อมูลเรียบร้อย");
            }
            else
            {
                ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้");
                return;
            }

            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = false;
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.OrgStructureDAO().GetTypeRequisition(ddlMaterialTypeSearch.SelectedValue, ddlUserGroupSearch.SelectedValue, orgCtrlSearch.OrgStructID, PagingControl1.CurrentPageIndex,
               PagingControl1.PageSize, this.SortColumn, this.SortOrder);
            
            
            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvTypeRequisition.DataSource = ds.Tables[0];
            gvTypeRequisition.DataBind();
            pnlDetail.Visible = false;
        }

        private void BindOrgGridview(string show = null)
        {
            if (dtOrgFile != null)
            {
                if (dtOrgFile.Rows.Count > 0)
                {
                    gvOrg.DataSource = dtOrgFile;
                    gvOrg.DataBind();
                    gvOrg.Visible = true;
                    pnlDetail.Visible = true;
                }
                else if (dtOrgFile.Rows.Count <= 0 || show == "T")
                {
                    gvOrg.Visible = false;
                    pnlDetail.Visible = true;
                }
                else
                {
                    gvOrg.Visible = false;
                    pnlDetail.Visible = false;
                }
            }
            else
            {
                gvOrg.Visible = false;
                pnlDetail.Visible = false;
            }
           
        }

        public void ClearData()
        {
            BindDropdown();
            ddlUserGroup.Items.Clear();
            if (dtOrgFile != null)
            {
                dtOrgFile.Clear();
            }

            if (dtOrgFileDeleted != null)
            {
                dtOrgFileDeleted.Clear();
            }

            orgCtrl.Clear();
            //lblCreateBy.Text = "";
            //lblCreateDate.Text = "";
            //lblUpdateBy.Text = "";
            //lblUpdatedate.Text = "";
        }

        protected void btnAddOrgClick(object sender, EventArgs e)
        {
            if (orgCtrl.OrgStructID == "")
            {
                ShowMessageBox("กรุณาเลือก ฝ่าย/ทีม");
                return;
            }

            if (dtOrgFile != null)
            {
                /* ทำการตรวจสอบว่าฝ่าย/ทีมนี้ เคยถูกเลือกหรือยัง */
                DataRow[] drs = dtOrgFile.Select("OrgStruc_Id = " + orgCtrl.OrgStructID);

                if (drs.Length > 0)
                {
                    ShowMessageBox("ฝ่าย/ทีมนี้ ถูกเลือกแล้ว");
                    return;
                }
            }
            else
            {
                dtOrgFile = new DataTable();

                dtOrgFile.Columns.Add("TrypeReq_ID", System.Type.GetType("System.Int32"));
                dtOrgFile.Columns.Add("Cate_Id", System.Type.GetType("System.Int32"));
                dtOrgFile.Columns.Add("Type_Id", System.Type.GetType("System.Int32"));
                dtOrgFile.Columns.Add("Cat_Name", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("Type_Name", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("OrgStruc_Id", System.Type.GetType("System.Int32"));
                dtOrgFile.Columns.Add("Div_Code", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("Div_Name", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("Dep_Code", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("Dep_Name", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("Create_Date", System.Type.GetType("System.DateTime"));
                dtOrgFile.Columns.Add("Create_By", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("FullName_Create_By", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("Update_Date", System.Type.GetType("System.DateTime"));
                dtOrgFile.Columns.Add("Update_By", System.Type.GetType("System.String"));
                dtOrgFile.Columns.Add("FullName_Update_By", System.Type.GetType("System.String"));
            }

            int tmp_TrypeReq_ID = GetMinTrypeReq_ID();

            DataRow drRow = dtOrgFile.NewRow();

            drRow["TrypeReq_ID"] = tmp_TrypeReq_ID;
            drRow["Cate_Id"] = DBNull.Value;
            drRow["Type_Id"] = DBNull.Value;
            drRow["Cat_Name"] = DBNull.Value;
            drRow["Type_Name"] = DBNull.Value;
            drRow["OrgStruc_Id"] = Convert.ToInt32(orgCtrl.OrgStructID == "" ? "0" : orgCtrl.OrgStructID);
            drRow["Div_Code"] = orgCtrl.DivCode;
            drRow["Div_Name"] = orgCtrl.DivName;
            drRow["Dep_Code"] = orgCtrl.DepCode;
            drRow["Dep_Name"] = orgCtrl.DepName;
            drRow["Create_Date"] = DBNull.Value;
            drRow["Create_By"] = DBNull.Value;
            drRow["FullName_Create_By"] = DBNull.Value;
            drRow["Update_Date"] = DBNull.Value;
            drRow["Update_By"] = DBNull.Value;
            drRow["FullName_Update_By"] = DBNull.Value;
            dtOrgFile.Rows.Add(drRow);

            BindOrgGridview();

        }


        private int GetMinTrypeReq_ID()
        {
            int minID = 0;
            int resultID = 0;

            if (dtOrgFile.Rows.Count > 0)
            {
                minID = Convert.ToInt32(dtOrgFile.Compute(" MIN(TrypeReq_ID)", string.Empty));
            }
            else
            {
                resultID = -1;
            }

            if (minID > 0 || minID == 0)
            {
                resultID = -1;
            }
            else
            {

                resultID = minID - 1;
            }

            return resultID;
        }

        protected void btnDeleteOrgClick(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvOrg.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                HiddenField hdOrgStuct = (HiddenField)row.FindControl("hdOrgStuct");
                HiddenField hdTrypeReq_ID = (HiddenField)row.FindControl("hdTrypeReq_ID");

                if (chkSelect.Checked == true) //ติ๊กเลือก เพื่อทำการลบ
                {
                    int TrypeReq_ID = Convert.ToInt32(hdTrypeReq_ID.Value == "" ? "0" : hdTrypeReq_ID.Value);
                    if (TrypeReq_ID < 0) //ไฟล์ที่เพิ่มเข้ามาใหม่อยู่ใน tmp
                    {

                        DataRow[] drs = dtOrgFile.Select(string.Format("TrypeReq_ID = {0}", TrypeReq_ID));
                        if (drs != null)
                        {
                            dtOrgFile.Rows.Remove(drs[0]);
                            dtOrgFile.AcceptChanges();
                        }
                    }
                    else //ไฟล์ที่อยู่ใน db ให้ลบออกจาก dt แต่ยังไม่ต้องลบไฟล์จริง
                    {
                        DataRow[] drs = dtOrgFile.Select(string.Format("TrypeReq_ID = {0}", TrypeReq_ID));
                        if (drs != null)
                        {
                            AddDeletedItem(TrypeReq_ID);
                            dtOrgFile.Rows.Remove(drs[0]);
                            dtOrgFile.AcceptChanges();
                        }
                    }
                    
                }
            }

            BindOrgGridview("T");
        }

        private void AddDeletedItem(int TrypeReq_ID)
        {

            if (dtOrgFileDeleted == null)
            {
                dtOrgFileDeleted = new DataTable();
                dtOrgFileDeleted.Columns.Add("TrypeReq_ID", typeof(int));
            }

            DataRow dr = dtOrgFileDeleted.NewRow();
            dr["TrypeReq_ID"] = TrypeReq_ID;

            dtOrgFileDeleted.Rows.Add(dr);

        }

        protected void gvOrg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HiddenField hdOrgStuct = (HiddenField)e.Row.FindControl("hdOrgStuct");
                HiddenField hdTrypeReq_ID = (HiddenField)e.Row.FindControl("hdTrypeReq_ID");

                hdOrgStuct.Value = drv["OrgStruc_Id"].ToString();
                hdTrypeReq_ID.Value = drv["TrypeReq_ID"].ToString();

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateTimeFormat);
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[7].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvOrg_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindOrgGridview();
            this.GridViewSort(gvOrg);
        }


    }
}
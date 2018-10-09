using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class ProductCodeMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "201";
                BindDropdown();
                BindMainData();

                //DataSet dsApprover = new DataAccess.ApproverDAO().GetApproverAndTemp(this.OrgID, "3");
                //if (dsApprover.Tables[0].Select("Approve_ID = " + this.UserID).Length > 0 ||dsApprover.Tables[1].Select("Account_ID = " + this.UserID).Length > 0)
                
                // Begin Green Edit
                DataTable dt = new DataAccess.UserDAO().GetUserGroupUser(UserID);

                DataRow[] rows = dt.Select("UserGroup_ID IN (2,3)");
                if (rows.Length > 0)
                    pnlRequest.Visible = false;
                else
                    pnlRequest.Visible = true;
                // End Green Edit

                /*
                DataTable dtOrg = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(this.OrgID == "" ? "0" : this.OrgID));

                if (dtOrg.Rows.Count > 0)
                {
                    if (dtOrg.Rows[0]["Div_Code"].ToString() == "3050")
                    {
                        pnlRequest.Visible = false;
                    }
                    else
                    {
                        pnlRequest.Visible = true;
                    }
                }
                */
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindMainData();
        }

        protected void btnSearchMain_Click(object sender, EventArgs e)
        {
            BindMainData();
        }

        protected void btnCancelMain_Click(object sender, EventArgs e)
        {
            txtRequestBySearch.Text = "";
            cblStatus.Items[0].Selected = true;
            cblStatus.Items[1].Selected = true;
            ccRequestFrom.Text  = "";
            ccRequestTo.Text = "";
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                btnDetail.CommandArgument = drv["Req_ItemCode_ID"].ToString();

                if (drv["Request_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[1].Text = ((DateTime)drv["Request_Date"]).ToString(this.DateTimeFormat);

                e.Row.Cells[4].Text = "รหัสสินค้า";
                if (drv["Pack_Desc"].ToString().Trim().Length > 0) e.Row.Cells[4].Text += "และหน่วยนับ";

                switch (drv["Status"].ToString())
                {
                    case "0": e.Row.Cells[8].Text = "ขอสร้างรหัสสินค้า" + (drv["Pack_Desc"].ToString().Trim().Length > 0 ? "และหน่วยนับ" : ""); break;
                    case "1": e.Row.Cells[8].Text = "ให้รหัสสินค้า" + (drv["Pack_Desc"].ToString().Trim().Length > 0 ? "และหน่วยนับ" : ""); break;
                }
            }
        }

        protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                pnlDetail.Visible = true;
                hdID.Value = e.CommandArgument.ToString();
                DataTable dt = new DataAccess.RequestItemDAO().GetRequestItem(hdID.Value);
                if(dt.Rows.Count > 0)
                {
                    txtItemName.Text = dt.Rows[0]["Inv_ItemName"].ToString();
                    txtAttribute.Text = dt.Rows[0]["Inv_Attribute"].ToString();

                    if (ddlCategory.Items.FindByValue(dt.Rows[0]["Cate_ID"].ToString()) != null)
                        ddlCategory.SelectedValue = dt.Rows[0]["Cate_ID"].ToString();

                    if (ddlForm.Items.FindByValue(dt.Rows[0]["Form_Id"].ToString()) != null)
                        ddlForm.SelectedValue = dt.Rows[0]["Form_Id"].ToString();

                    if (ddlType.Items.FindByValue(dt.Rows[0]["Type_ID"].ToString()) != null)
                        ddlType.SelectedValue = dt.Rows[0]["Type_ID"].ToString();

                    BindSubMaterialType();

                    if (ddlSubCate.Items.FindByValue(dt.Rows[0]["SubCate_ID"].ToString()) != null)
                        ddlSubCate.SelectedValue = dt.Rows[0]["SubCate_ID"].ToString();

                    txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();

                    if (ddlPackage.Items.FindByValue(dt.Rows[0]["Pack_ID"].ToString()) != null)
                        ddlPackage.SelectedValue = dt.Rows[0]["Pack_ID"].ToString();

                    txtPackDesc.Text = dt.Rows[0]["Pack_Desc"].ToString();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString();

                    //txtApproveItemCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();
                    ItemControl.ItemCode = dt.Rows[0]["Inv_ItemCode"].ToString();
                    hdItemCode.Value = dt.Rows[0]["Inv_ItemCode"].ToString();
                    //txtApproveItemName.Text = dt.Rows[0]["Inv_ItemName"].ToString();

                    
                    //ถ้ามี Code ค่อยค้นหา ชื่อสินค้า จากรหัสสินค้า
                    if (hdItemCode.Value != "")
                    {
                        DataSet ds = new DataAccess.ItemDAO().GetItemSearch(hdItemCode.Value, "", 1, 1000, "", "");


                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ItemControl.ItemName = ds.Tables[0].Rows[0]["Inv_ItemName"].ToString();
                        }
                        else
                        {
                            ItemControl.ItemName = "";
                        }
                    }
                    else
                    {
                        ItemControl.ItemName = "";
                    }

                    // Begin Green Edit
                    ImageButton imgBtn = ItemControl.FindControl("btnSelect1") as ImageButton;
                    Button btnSaveApprove = pnlApproveCode.FindControl("btnSaveApprove") as Button;
                    Button btnCancelApprove = pnlApproveCode.FindControl("btnCancelApprove") as Button;
                    // End Green Edit

                    if (pnlRequest.Visible)
                    {
                        btnDelete.Visible = true;
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        // Begin Green Edit
                        //pnlApproveCode.Visible = false;
                        imgBtn.Enabled = false;
                        btnSaveApprove.Visible = false;
                        btnCancelApprove.Visible = false;
                        // End Green Edit
                    }
                    else
                    {
                        btnDelete.Visible = false;
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        pnlApproveCode.Visible = true;
                        // Begin Green Edit
                        imgBtn.Enabled = true;
                        btnSaveApprove.Visible = true;
                        btnCancelApprove.Visible = true;
                        // End Green Edit
                        btnRefresh_Click(this, new EventArgs());
                    }

                    // ให้รหัสสินค้าแล้ว
                    if (dt.Rows[0]["Status"].ToString() == "1")
                    {
                        txtApproveBy.Text = dt.Rows[0]["Approve_By"].ToString();
                        if (dt.Rows[0]["Approve_Date"].ToString().Trim().Length > 0)
                            txtApproveDate.Text = ((DateTime)dt.Rows[0]["Approve_Date"]).ToString(this.DateTimeFormat);
                        btnDelete.Enabled = false;
                        btnSave.Enabled = false;
                        //btnSaveApprove.Enabled = false;
                    }
                    else
                    {
                        txtApproveBy.Text = this.UserName;
                        txtApproveDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
                        btnDelete.Enabled = true;
                        btnSave.Enabled = true;
                        //btnSaveApprove.Enabled = true;
                    }
                }

            }

        }

        protected void gvItem_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindMainData();
            this.GridViewSort(gvItem);
        }

        public void BindMainData()
        {
            string status = "";
            if (cblStatus.Items[0].Selected) status = "'0'";
            if (cblStatus.Items[1].Selected) status += (status.Trim().Length > 0?",":"") +"'1'";

            // Begin Green Edit
            DataTable dt = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt32(OrgID));
            DataSet ds = null;

            // ดึงกลุ่มของผู้ใช้
            DataTable dtUsrGrpUsr = new DataAccess.UserDAO().GetUserGroupUser(UserID);

            DataRow[] rows = dtUsrGrpUsr.Select("UserGroup_ID IN (2,3)");
            
            // ผู้ใช้คนนี้อยู่กลุ่ม "ฝ่ายธุรการ จัดซื้อ" หรือ "เจ้าหน้าที่ฝ่ายธุรการจัดซื้อ" 
            // สามารถดูการขอรหัสได้ทั้งหมด
            if (rows.Length > 0)
            {
                ds = new DataAccess.RequestItemDAO().GetRequestItem(status, txtRequestBySearch.Text, ccRequestFrom.Text, ccRequestTo.Text,
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder, "");
            }
            // ผู้ใช้คนนี้ดูการขอรหัสสินค้าได้เฉพาะ Div_Code เดียวกัน
            else
            {
                ds = new DataAccess.RequestItemDAO().GetRequestItem(status, txtRequestBySearch.Text, ccRequestFrom.Text, ccRequestTo.Text,
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder, dt.Rows[0]["Div_Code"].ToString());
            }
            // End Green Edit

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvItem.DataSource = ds.Tables[0];
            gvItem.DataBind();
        }

        public void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("เลือกประเภท", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlCategory.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));

            }

            dt = new DataAccess.TypeDAO().GetType("", "", "1", 1, 1000, "Type_Code", "").Tables[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(new ListItem("เลือกกลุ่มผู้ใช้งาน", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlType.Items.Add(new ListItem(dt.Rows[i]["Type_Code"].ToString() + " - " + dt.Rows[i]["Type_Name"].ToString(),
                    dt.Rows[i]["Type_ID"].ToString()));
            }

            dt = new DataAccess.FormDAO().GetForm("", "", "1", 1, 1000, "Form_Code", "").Tables[0];
            ddlForm.Items.Clear();
            ddlForm.Items.Add(new ListItem("เลือกชนิด", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlForm.Items.Add(new ListItem(dt.Rows[i]["Form_Code"].ToString() + " - " + dt.Rows[i]["Form_Name"].ToString(),
                    dt.Rows[i]["Form_Id"].ToString()));
            }

            ddlPackage.DataSource = new DataAccess.PackageDAO().GetPackage("", "1", 1, 1000, "", "").Tables[0];
            ddlPackage.DataBind();
            ddlPackage.Items.Insert(0, new ListItem("เลือกหน่วยนับ", ""));
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
        }

        private void BindSubMaterialType()
        {
            if (ddlCategory.SelectedIndex > 0)
            {
                DataTable dt = new DataAccess.CategoryDAO().GetSubCate("", ddlCategory.SelectedValue, "",
                    "1", 1, 1000, "SubCate_Code", "").Tables[0];
                ddlSubCate.Items.Clear();
                ddlSubCate.Items.Add(new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSubCate.Items.Add(new ListItem(dt.Rows[i]["SubCate_Code"].ToString() + " - " + dt.Rows[i]["SubCate_Name"].ToString(),
                        dt.Rows[i]["SubCate_ID"].ToString()));
                }
            }
            else
                ddlSubCate.Items.Clear();
        }



        protected void btnSearchProduct_Click(object sender, EventArgs e)
        {
            gvItemName.DataSource = new DataAccess.RequestItemDAO().GetRequestItemByName(txtProductNameSearch.Text);
            gvItemName.DataBind();
            if (gvItemName.Rows.Count == 0)
                btnNewRequest.Visible = true;
            else
                btnNewRequest.Visible = false;
        }

        protected void btnNewRequest_Click(object sender, EventArgs e)
        {
            ClearRequestData();
            pnlRequest.Visible = true;
            btnDelete.Enabled = false;
            pnlDetail.Visible = true;
            txtItemName.Text = txtProductNameSearch.Text;
            pnlApproveCode.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hdID.Value.Trim().Length == 0)
            {
                new DataAccess.RequestItemDAO().AddRequestItem(
                    txtItemName.Text, 
                    ddlPackage.SelectedValue, 
                    txtPackDesc.Text,
                    txtAttribute.Text, 
                    ddlCategory.SelectedValue, 
                    ddlType.SelectedValue, 
                    ddlForm.SelectedValue, 
                    ddlSubCate.SelectedValue,
                    txtQuantity.Text, 
                    txtRemark.Text, 
                    this.UserName, 
                    // Begin Green Edit 
                    Convert.ToInt32(OrgID)
                    // End Green Edit
                    );
            }
            else
            {
                new DataAccess.RequestItemDAO().UpdateRequestItem(hdID.Value, txtItemName.Text, ddlPackage.SelectedValue, txtPackDesc.Text,
                    txtAttribute.Text, ddlCategory.SelectedValue, ddlType.SelectedValue, ddlForm.SelectedValue, ddlSubCate.SelectedValue,
                    txtQuantity.Text, txtRemark.Text, this.UserName);
            }
            BindMainData();
            ClearRequestData();
            pnlDetail.Visible = false;
            // Begin Green Edit
            txtProductNameSearch.Text = "";
            // End Green Edit
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            new DataAccess.RequestItemDAO().DeleteRequestItem(hdID.Value);
            ClearRequestData();
            pnlDetail.Visible = false;
            BindMainData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearRequestData();
        }

        private void ClearRequestData()
        {
            hdID.Value = "";
            txtItemName.Text = "";
            txtAttribute.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlForm.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            BindSubMaterialType();
            txtQuantity.Text = "";
            ddlPackage.Text = "";
            txtPackDesc.Text = "";
            txtRemark.Text = "";

            txtApproveBy.Text = "";
            txtApproveDate.Text = "";
            //txtApproveItemCode.Text = "";
            //txtApproveItemName.Text = "";
            ItemControl.Clear();
            gvPackage.DataSource = null;
            gvPackage.DataBind();
        }


        string basePackID = "";
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DataAccess.ItemDAO db = new DataAccess.ItemDAO();
            //DataTable dt = db.GetItemID(txtApproveItemCode.Text);
            DataTable dt = db.GetItemID(ItemControl.ItemCode);
            if (dt.Rows.Count > 0)
            {
                basePackID = dt.Rows[0]["BaseUnit_Pack_ID"].ToString();

                string itemID = dt.Rows[0]["Inv_ItemID"].ToString();

                gvPackage.DataSource = db.GetItemPack(itemID);
                gvPackage.DataBind();
            }
            else
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "at", "alert('ไม่พบสินค้ารหัส "+txtApproveItemCode.Text+"');", true);
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "at", "alert('ไม่พบสินค้ารหัส " + ItemControl.ItemCode + "');", true);
            }
        }

        protected void gvPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (basePackID == drv["Pack_Id"].ToString())
                    e.Row.Cells[5].Text = "Yes";
            }
        }

        protected void btnSaveApprove_Click(object sender, EventArgs e)
        {
            //new DataAccess.RequestItemDAO().UpdateRequestItem(hdID.Value, txtApproveItemCode.Text, txtApproveItemName.Text, this.UserName);
            //new DataAccess.RequestItemDAO().UpdateRequestItem(hdID.Value, ItemControl.ItemCode, ItemControl.ItemName, this.UserName);
            new DataAccess.RequestItemDAO().UpdateRequestItem(hdID.Value, ItemControl.ItemCode, this.UserName);
            pnlDetail.Visible = false;
            ClearRequestData();
            BindMainData();
        }

        protected void btnCancelApprove_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearRequestData();
        }


    }
}
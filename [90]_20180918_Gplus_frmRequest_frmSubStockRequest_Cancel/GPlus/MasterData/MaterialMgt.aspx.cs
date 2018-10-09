using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace GPlus.MasterData
{
    public partial class MaterialMgt : Pagebase
    {
        public DataTable MaterialPackageTable
        {
            get
            {
                return (DataTable)Session["MaterialPackageTable"];
            }
            set
            {
                Session["MaterialPackageTable"] = value;
            }
        }

        #region LPA 10022014
        public DataTable dtFileDeletedPack
        {
            get { return (ViewState["dtFileDeletedPack"] == null) ? null : (DataTable)ViewState["dtFileDeletedPack"]; }
            set { ViewState["dtFileDeletedPack"] = value; }
        }

        //public DataTable dtTempDeletedPack
        //{
        //    get { return (ViewState["dtTempDeletedPack"] == null) ? null : (DataTable)ViewState["dtTempDeletedPack"]; }
        //    set { ViewState["dtTempDeletedPack"] = value; }
        //}

        public DataTable dtTempDeletedPack
        {
            get
            {
                return (DataTable)Session["dtTempDeletedPack"];
            }
            set
            {
                Session["dtTempDeletedPack"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "101";
                SetAccessMenu();
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        protected void SetAccessMenu()
        {
            DataTable dt = this.GetAccessMenu(this.PageID,this.UserID);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Can_Add"].ToString() == "0")
                {
                    btnAdd.Visible = false;
                }

                if (dt.Rows[0]["Can_Update"].ToString() == "0")
                {
                    btnAdd.Visible = false;
                    btnSave.Visible = false;
                }
            }
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
            txtMaterialCodeSearch.Text = "";
            txtMaterialNameSearch.Text = "";
            ddlMaterialTypeSearch.SelectedIndex = 0;
            txtOldCodeSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            txtKeyCode.Text = "";
            txtKeyCode.Enabled = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            MaterialPackageTable = new DataAccess.ItemDAO().GetItemPack("-1");
            int rand = new System.Random().Next(1000000) * -1;
            ImageListControl1.BindFile(rand.ToString());

            ddlMaterialType.Enabled = true;
            ddlUserGroup.Enabled = true;
            ddlSubMaterialType.Enabled = true;
            txtMaterialName.Focus();
        }

        protected void gvMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[8].Text = drv["Asset_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Inv_ItemID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[9].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                dtFileDeletedPack = null;
                DataTable dt = new DataAccess.ItemDAO().GetItem(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                ImageListControl1.BindFile(hdID.Value);
                if (dt.Rows.Count > 0)
                {
                    txtMaterialCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();
                    txtMaterialName.Text = dt.Rows[0]["Inv_ItemName"].ToString();
                    txtMaterialProperty.Text = dt.Rows[0]["Inv_Attrbute"].ToString();

                    if (ddlMaterialType.Items.FindByValue(dt.Rows[0]["Cate_ID"].ToString()) != null)
                    {
                        ddlMaterialType.SelectedValue = dt.Rows[0]["Cate_ID"].ToString();
                        BindSubMaterialType();
                        BindUserGroup();
                    }

                    if (ddlUserGroup.Items.FindByValue(dt.Rows[0]["Type_ID"].ToString()) != null)
                        ddlUserGroup.SelectedValue = dt.Rows[0]["Type_ID"].ToString();

                    txtOldMaterialCode.Text = dt.Rows[0]["Inv_AS400"].ToString();

                    if (ddlType.Items.FindByValue(dt.Rows[0]["Form_ID"].ToString()) != null)
                        ddlType.SelectedValue = dt.Rows[0]["Form_ID"].ToString();

                    if (ddlSubMaterialType.Items.FindByValue(dt.Rows[0]["SubCate_ID"].ToString()) != null)
                        ddlSubMaterialType.SelectedValue = dt.Rows[0]["SubCate_ID"].ToString();

                    isBaseUnitID = dt.Rows[0]["BaseUnit_Pack_ID"].ToString();

                    txtOrderDetail.Text = dt.Rows[0]["Order_Detail"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Asset_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Asset_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }
                MaterialPackageTable = new DataAccess.ItemDAO().GetItemPack(hdID.Value);
                gvPackage.DataSource = MaterialPackageTable;
                gvPackage.DataBind();

                txtMaterialName.Focus();
                pnlDetail.Visible = true;
                txtKeyCode.Text = "";
                txtKeyCode.Enabled = false;
                ddlMaterialType.Enabled = false;
                ddlUserGroup.Enabled = false;
                ddlSubMaterialType.Enabled = false;
            }
        }
        string isBaseUnitID = "";

        protected void gvMaterial_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvMaterial);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> prePack = new List<string>();
            for (int i = 0; i < gvPackage.Rows.Count; i++)
            {
                HiddenField hdDetailID = (HiddenField)gvPackage.Rows[i].FindControl("hdDetailID");
                DropDownList ddlPackageUnit = (DropDownList)gvPackage.Rows[i].FindControl("ddlPackageUnit");
                DropDownList ddlPackageBase = (DropDownList)gvPackage.Rows[i].FindControl("ddlPackageBase");

                prePack.Add(ddlPackageUnit.SelectedValue);
                bool isCorrect = false;
                for (int j = 0; j < prePack.Count; j++)
                {
                    if (prePack[j] == ddlPackageBase.SelectedValue)
                    {
                        isCorrect = true;
                        break;
                    }
                }

                if (!isCorrect)
                {
                    ddlPackageBase.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Err1", "alert('คุณเลือกหน่วยนับไม่ถูกต้อง');", true);
                    return;
                }
                for (int j = 0; j < i; j++)
                {
                    DropDownList ddlPackageUnitPre = (DropDownList)gvPackage.Rows[j].FindControl("ddlPackageUnit");
                    if (ddlPackageUnit.SelectedValue == ddlPackageUnitPre.SelectedValue)
                    {
                        ddlPackageUnit.Focus();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Err2", "alert('คุณเลือก Package ซ้ำกัน');", true);
                        return;
                    }
                }
            }
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            DataAccess.ItemDAO db = new DataAccess.ItemDAO();
            if (hdID.Value == "")
            {
                if (txtKeyCode.Text != "")
                {
                    if (txtKeyCode.Text.Length != 4)
                    {
                        ShowMessageBox("กรุณาใส่รหัสรายการ 4 หลัก");
                        return;
                    }
                }
                string itemCode = ddlMaterialType.SelectedItem.Text.Split('-')[0].Trim() + "-";
                itemCode += ddlUserGroup.SelectedItem.Text.Split('-')[0].PadLeft(2,'0').Trim() + "-";
                itemCode += ddlSubMaterialType.SelectedItem.Text.Split('-')[0].PadLeft(2, '0').Trim() + "-";

                hdID.Value = db.AddItem(itemCode, txtMaterialName.Text, txtMaterialProperty.Text,
                   ddlMaterialType.SelectedValue, ddlUserGroup.SelectedValue, ddlSubMaterialType.SelectedValue, ddlType.SelectedValue, txtOldMaterialCode.Text
                   , txtOrderDetail.Text, status, this.UserName, txtKeyCode.Text);// ddlPackage.SelectedValue,

                ImageListControl1.UpdateReference(hdID.Value);

                DataTable dt = db.GetItem(hdID.Value);
                if (dt.Rows.Count > 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "succ",
                        "alert('รหัสวัสดุ -  อุปกรณ์ " + dt.Rows[0]["Inv_ItemCode"].ToString() + " ชื่อ " + txtMaterialName.Text + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error",
                       "alert('ระบบไม่สามารถสร้างวัสดุอุปกรณ์ได้ กรุณากดบันทึกอีกครั้ง');", true);
            }
            else
            {
                db.UpdateItem(hdID.Value, txtMaterialCode.Text, txtMaterialName.Text, txtMaterialProperty.Text, ddlMaterialType.SelectedValue,
                    ddlUserGroup.SelectedValue, ddlSubMaterialType.SelectedValue, ddlType.SelectedValue, txtOldMaterialCode.Text, txtOrderDetail.Text, status, this.UserName);
            }

            //Save Package
            bool saveSuccess = true;

            if (dtFileDeletedPack != null)
            {
                string result_del = db.Delete_ItemPack(dtFileDeletedPack);
                if (result_del != "True")
                {
                    saveSuccess = false;
                    if (result_del != "False")
                    {
                        ShowMessageBox("ไม่สามารถลบ Package ที่เลือกได้ เนื่องจาก Package ( " + result_del + " ) ได้ถูกนำไปใช้งานแล้ว");
                        //DataTable dtMerge = null;
                        //dtMerge.Merge(MaterialPackageTable);
                        //dtMerge.Merge(dtTempDeletedPack);
                        //gvPackage.DataSource = dtMerge;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Err3", "alert('ไม่สามารถลบ Package ที่เลือกได้');", true);
                    }

                    DataTable dt = new DataAccess.ItemDAO().GetItem(hdID.Value);
                    isBaseUnitID = dt.Rows[0]["BaseUnit_Pack_ID"].ToString();
                    MaterialPackageTable = new DataAccess.ItemDAO().GetItemPack(hdID.Value);
                    gvPackage.DataSource = MaterialPackageTable;
                    gvPackage.DataBind();
                    dtFileDeletedPack = null;
                    
                    return;
                }
            }

            for (int i = 0; i < gvPackage.Rows.Count; i++)
            {
                HiddenField hdDetailID = (HiddenField)gvPackage.Rows[i].FindControl("hdDetailID");
                DropDownList ddlPackageUnit = (DropDownList)gvPackage.Rows[i].FindControl("ddlPackageUnit");
                TextBox txtQuantity = (TextBox)gvPackage.Rows[i].FindControl("txtQuantity");
                DropDownList ddlPackageBase = (DropDownList)gvPackage.Rows[i].FindControl("ddlPackageBase");
                TextBox txtPackageDetail = (TextBox)gvPackage.Rows[i].FindControl("txtPackageDetail");
                Label lblSequence = (Label)gvPackage.Rows[i].FindControl("lblSequence");

                CheckBox chkIsBase = ((CheckBox)gvPackage.Rows[i].FindControl("chkIsBase"));
                if (chkIsBase.Checked)
                {
                    if (Convert.ToInt32(hdDetailID.Value == "" ? "0" : hdDetailID.Value) <= 0)
                    {
                        hdDetailID.Value = ddlPackageUnit.SelectedValue;
                    }
                    db.UpdateItem(hdID.Value, hdDetailID.Value, this.UserName);
                }

                try
                {
                    db.UpdateItemPack(hdID.Value, ddlPackageUnit.SelectedValue, lblSequence.Text, txtQuantity.Text, ddlPackageBase.SelectedValue,
                           txtPackageDetail.Text, this.UserName);
                }
                catch
                {
                    ddlPackageUnit.Focus();
                    saveSuccess = false;
                }
            }

            if (!saveSuccess)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Err3", "alert('คุณเลือก Package ซ้ำกัน');", true);

            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }



        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItem(txtMaterialCodeSearch.Text, txtMaterialNameSearch.Text, ddlMaterialTypeSearch.SelectedValue,
                txtOldCodeSearch.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvMaterial.DataSource = ds.Tables[0];
            gvMaterial.DataBind();
        }

        private void ClearData()
        {
            hdID.Value = "";
            txtMaterialCode.Text = "";
            txtMaterialName.Text = "";
            txtMaterialProperty.Text = "";
            txtOldMaterialCode.Text = "";
            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            //ddlPackage.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            ddlType.SelectedIndex = 0;
            txtOrderDetail.Text = "";
            gvPackage.DataSource = null;
            gvPackage.DataBind();
            rdbStatus.SelectedIndex = 0;
        }


        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];
            ddlMaterialType.Items.Clear();
            ddlMaterialType.Items.Add(new ListItem("เลือกประเภท", ""));
            ddlMaterialTypeSearch.Items.Clear();
            ddlMaterialTypeSearch.Items.Add(new ListItem("เลือกประเภท", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlMaterialType.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));

                ddlMaterialTypeSearch.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));
            }


            //dt = new DataAccess.TypeDAO().GetType("", "", "1", 1, 1000, "Type_Code", "").Tables[0];
            //ddlUserGroup.Items.Clear();
            //ddlUserGroup.Items.Add(new ListItem("เลือกกลุ่มผู้ใช้งาน", ""));
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    ddlUserGroup.Items.Add(new ListItem(dt.Rows[i]["Type_Code"].ToString() + " - " + dt.Rows[i]["Type_Name"].ToString(),
            //        dt.Rows[i]["Type_ID"].ToString()));
            //}

            dt = new DataAccess.FormDAO().GetForm("", "", "1", 1, 1000, "Form_Code", "").Tables[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(new ListItem("เลือกชนิด", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlType.Items.Add(new ListItem(dt.Rows[i]["Form_Code"].ToString() + " - " + dt.Rows[i]["Form_Name"].ToString(),
                    dt.Rows[i]["Form_Id"].ToString()));
            }
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
            BindUserGroup();
        }

        private void BindSubMaterialType()
        {
            if (ddlMaterialType.SelectedIndex > 0)
            {
                DataTable dt = new DataAccess.CategoryDAO().GetSubCate("", ddlMaterialType.SelectedValue, "",
                    "1", 1, 1000, "SubCate_Code", "").Tables[0];
                ddlSubMaterialType.Items.Clear();
                ddlSubMaterialType.Items.Add(new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSubMaterialType.Items.Add(new ListItem(dt.Rows[i]["SubCate_Code"].ToString() + " - " + dt.Rows[i]["SubCate_Name"].ToString(),
                        dt.Rows[i]["SubCate_ID"].ToString()));
                }
            }
            else
                ddlSubMaterialType.Items.Clear();
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

        protected void btnAddPackage_Click(object sender, EventArgs e)
        {
            SavePackageState();
            DataRow dr = MaterialPackageTable.NewRow();
            dr["Pack_Id"] = (gvPackage.Rows.Count + 1) * -1;
            dr["Pack_Seq"] = gvPackage.Rows.Count + 1;

            if (gvPackage.Rows.Count > 0)
            {
                DropDownList ddlPackageUnit = (DropDownList)gvPackage.Rows[gvPackage.Rows.Count - 1].FindControl("ddlPackageUnit");
                try
                {
                    dr["Pack_Id_Base"] = ddlPackageUnit.SelectedValue;
                }
                catch { }
            }

            MaterialPackageTable.Rows.Add(dr);
            gvPackage.DataSource = MaterialPackageTable;
            gvPackage.DataBind();
        }

        DataTable dtPackage = null;
        StringBuilder sb = new StringBuilder();
        protected void gvPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dtPackage = new DataAccess.PackageDAO().GetPackage("", "1", 1, 1000, "", "").Tables[0];
                sb.Append("function BaseCheck(sender){");
                sb.Append("if(sender.checked){");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnDel = (LinkButton)e.Row.FindControl("btnDel");
                HiddenField hdDetailID = (HiddenField)e.Row.FindControl("hdDetailID");
                Label lblSequence = (Label)e.Row.FindControl("lblSequence");
                DropDownList ddlPackageUnit = (DropDownList)e.Row.FindControl("ddlPackageUnit");
                TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                DropDownList ddlPackageBase = (DropDownList)e.Row.FindControl("ddlPackageBase");
                TextBox txtPackageDetail = (TextBox)e.Row.FindControl("txtPackageDetail");
                CheckBox chkIsBase = (CheckBox)e.Row.FindControl("chkIsBase");

                TextBox stats = (TextBox)e.Row.FindControl("stats");

                btnDel.OnClientClick = "return confirm('คุณต้องการลบรายการนี้หรือไม่');";

                btnDel.CommandArgument = e.Row.RowIndex.ToString();
                hdDetailID.Value = drv["Pack_Id"].ToString();
                ((ImageButton)e.Row.FindControl("btnUp")).CommandArgument = e.Row.RowIndex.ToString();
                ((ImageButton)e.Row.FindControl("btnDown")).CommandArgument = e.Row.RowIndex.ToString();
                lblSequence.Text = (e.Row.RowIndex + 1).ToString();

                ddlPackageUnit.DataSource = dtPackage;
                ddlPackageUnit.DataBind();
                ddlPackageUnit.Items.Insert(0, new ListItem("เลือกหน่วยนับ", ""));
                if (ddlPackageUnit.Items.FindByValue(drv["Pack_ID"].ToString()) != null)
                    ddlPackageUnit.SelectedValue = drv["Pack_ID"].ToString();
                else if (drv["Package_Name"].ToString().Trim().Length > 0)
                {
                    ddlPackageUnit.Items.Add(new ListItem(drv["Package_Name"].ToString(), drv["Pack_ID"].ToString()));
                    ddlPackageUnit.SelectedValue = drv["Pack_ID"].ToString();
                }

                //1 Select from Previous
                ddlPackageBase.DataSource = dtPackage;
                ddlPackageBase.DataBind();
                ddlPackageBase.Items.Insert(0, new ListItem("เลือกหน่วยนับ", ""));
                if (ddlPackageBase.Items.FindByValue(drv["Pack_Id_Base"].ToString()) != null)
                { ddlPackageBase.SelectedValue = drv["Pack_Id_Base"].ToString();
                    if (drv["ItemPack_status"].ToString() == "1")
                    {
                        stats.Text = "ใช้งาน";
                    }
                    else stats.Text = "ยกเลิก";
                }

                else if (drv["Pack_Name_Base"].ToString().Trim().Length > 0)
                {
                    ddlPackageBase.Items.Add(new ListItem(drv["Pack_Name_Base"].ToString(), drv["Pack_Id_Base"].ToString()));
                    ddlPackageBase.SelectedValue = drv["Pack_Id_Base"].ToString();
                }

                if (drv["Pack_Content"].ToString().Trim().Length > 0)
                    txtQuantity.Text = ((decimal)drv["Pack_Content"]).ToString("0");
                else if (e.Row.RowIndex == 0)
                {
                    txtQuantity.Text = "1";
                    txtQuantity.Enabled = false;
                }

                if (e.Row.RowIndex > 0)
                {
                    ddlPackageUnit.Attributes.Add("onchange", "document.getElementById('" + txtPackageDetail.ClientID + "').value = " +
                        "document.getElementById('" + ddlPackageUnit.ClientID + "').options[document.getElementById('" +
                        ddlPackageUnit.ClientID + "').selectedIndex].text + '('+document.getElementById('" + txtQuantity.ClientID + "').value+' '+" +
                        "document.getElementById('" + ddlPackageBase.ClientID + "').options[document.getElementById('" +
                        ddlPackageBase.ClientID + "').selectedIndex].text+ ')' ;");

                    txtQuantity.Attributes.Add("onblur", "document.getElementById('" + txtPackageDetail.ClientID + "').value = " +
                        "document.getElementById('" + ddlPackageUnit.ClientID + "').options[document.getElementById('" +
                        ddlPackageUnit.ClientID + "').selectedIndex].text + '('+document.getElementById('" + txtQuantity.ClientID + "').value+' '+" +
                        "document.getElementById('" + ddlPackageBase.ClientID + "').options[document.getElementById('" +
                        ddlPackageBase.ClientID + "').selectedIndex].text+ ')' ;");

                    ddlPackageBase.Attributes.Add("onchange", "document.getElementById('" + txtPackageDetail.ClientID + "').value = " +
                        "document.getElementById('" + ddlPackageUnit.ClientID + "').options[document.getElementById('" +
                        ddlPackageUnit.ClientID + "').selectedIndex].text + '('+document.getElementById('" + txtQuantity.ClientID + "').value+' '+" +
                        "document.getElementById('" + ddlPackageBase.ClientID + "').options[document.getElementById('" +
                        ddlPackageBase.ClientID + "').selectedIndex].text+ ')' ;");
                }
                else
                {
                    ddlPackageUnit.Attributes.Add("onchange", "document.getElementById('" + txtPackageDetail.ClientID + "').value = " +
                       "document.getElementById('" + ddlPackageUnit.ClientID + "').options[document.getElementById('" +
                       ddlPackageUnit.ClientID + "').selectedIndex].text ;");

                    txtQuantity.Attributes.Add("onblur", "document.getElementById('" + txtPackageDetail.ClientID + "').value = " +
                        "document.getElementById('" + ddlPackageUnit.ClientID + "').options[document.getElementById('" +
                        ddlPackageUnit.ClientID + "').selectedIndex].text ;");

                    ddlPackageBase.Attributes.Add("onchange", "document.getElementById('" + txtPackageDetail.ClientID + "').value = " +
                        "document.getElementById('" + ddlPackageUnit.ClientID + "').options[document.getElementById('" +
                        ddlPackageUnit.ClientID + "').selectedIndex].text ;");
                }

                txtPackageDetail.Text = drv["Description"].ToString();
                if (drv["Pack_Id"].ToString() == isBaseUnitID)
                    chkIsBase.Checked = true;

                sb.Append("document.getElementById('" + chkIsBase.ClientID + "').checked = false;");
                chkIsBase.Attributes.Add("onclick", "BaseCheck(document.getElementById('" + chkIsBase.ClientID + "'));");

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                sb.Append("sender.checked = true;}");
                sb.Append("}");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "BasePackage", sb.ToString(), true);
            }
        }

        protected void gvPackage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = gvPackage.Rows[int.Parse(e.CommandArgument.ToString())];
            HiddenField hdDetailID = (HiddenField)row.FindControl("hdDetailID");
            if (e.CommandName == "Del")
            {
                if (int.Parse(hdDetailID.Value) > 0 && hdID.Value.Trim().Length > 0)
                    //new DataAccess.ItemDAO().DeleteItemPack(hdID.Value, hdDetailID.Value);
                    AddDeletedItem(hdID.Value, hdDetailID.Value);
                DataRow[] drs = MaterialPackageTable.Select("Pack_Id = " + hdDetailID.Value);
                //AddToTempDeleted(drs);
                if (drs.Length > 0)
                    drs[0].Delete();
            }
            else if (e.CommandName == "Up")
            {
                if (e.CommandArgument.ToString() != "0")
                {
                    ((Label)gvPackage.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblSequence")).Text = e.CommandArgument.ToString();

                    ((Label)gvPackage.Rows[int.Parse(e.CommandArgument.ToString()) - 1].FindControl("lblSequence")).Text =
                        (int.Parse(e.CommandArgument.ToString()) + 1).ToString();
                }
            }
            else if (e.CommandName == "Down")
            {
                if (e.CommandArgument.ToString() != (gvPackage.Rows.Count - 1).ToString())
                {
                    ((Label)gvPackage.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblSequence")).Text =
                        (int.Parse(e.CommandArgument.ToString()) + 1).ToString();

                    ((Label)gvPackage.Rows[int.Parse(e.CommandArgument.ToString()) + 1].FindControl("lblSequence")).Text =
                        e.CommandArgument.ToString();
                }
            }
            SavePackageState();
            DataView dv = MaterialPackageTable.DefaultView;
            dv.Sort = "Pack_Seq";
            gvPackage.DataSource = dv;
            gvPackage.DataBind();
        }

        private void SavePackageState()
        {
            DataTable dt = MaterialPackageTable;
            for (int i = 0; i < gvPackage.Rows.Count; i++)
            {
                DataRow[] drs = dt.Select("Pack_Id = " + ((HiddenField)gvPackage.Rows[i].FindControl("hdDetailID")).Value);
                if (drs.Length > 0)
                {
                    DropDownList ddlPackageUnit = (DropDownList)gvPackage.Rows[i].FindControl("ddlPackageUnit");
                    TextBox txtQuantity = (TextBox)gvPackage.Rows[i].FindControl("txtQuantity");
                    DropDownList ddlPackageBase = (DropDownList)gvPackage.Rows[i].FindControl("ddlPackageBase");
                    TextBox txtPackageDetail = (TextBox)gvPackage.Rows[i].FindControl("txtPackageDetail");

                    drs[0]["Pack_Seq"] = Int16.Parse(((Label)gvPackage.Rows[i].FindControl("lblSequence")).Text);
                    if (ddlPackageUnit.SelectedIndex > 0)
                        drs[0]["Pack_Id"] = int.Parse(ddlPackageUnit.SelectedValue);
                    if (txtQuantity.Text.Trim().Length > 0)
                        drs[0]["Pack_Content"] = decimal.Parse(txtQuantity.Text);
                    if (ddlPackageBase.SelectedIndex > 0)
                        drs[0]["Pack_Id_Base"] = int.Parse(ddlPackageBase.SelectedValue);
                    drs[0]["Description"] = ((TextBox)gvPackage.Rows[i].FindControl("txtPackageDetail")).Text;
                    CheckBox chkIsBase = (CheckBox)gvPackage.Rows[i].FindControl("chkIsBase");
                    if (chkIsBase.Checked)
                        isBaseUnitID = ((HiddenField)gvPackage.Rows[i].FindControl("hdDetailID")).Value;
                }
            }

            MaterialPackageTable = dt;
        }

        #region LPA 10022014
        private void AddDeletedItem(string item_id, string pack_id)
        {
            if (dtFileDeletedPack == null)
            {
                dtFileDeletedPack = new DataTable();
                dtFileDeletedPack.Columns.Add("Inv_ItemID", typeof(string));
                dtFileDeletedPack.Columns.Add("Pack_Id", typeof(string));
            }

            DataRow dr = dtFileDeletedPack.NewRow();
            dr["Inv_ItemID"] = item_id;
            dr["Pack_Id"] = pack_id;

            dtFileDeletedPack.Rows.Add(dr);
        }

        //private void AddToTempDeleted(DataRow[] drs)
        //{
        //    if (dtTempDeletedPack == null)
        //    {
        //        dtTempDeletedPack = MaterialPackageTable.Clone();
        //    }
        //    dtTempDeletedPack.Rows.Add(drs);
        //}
        #endregion

    }
}
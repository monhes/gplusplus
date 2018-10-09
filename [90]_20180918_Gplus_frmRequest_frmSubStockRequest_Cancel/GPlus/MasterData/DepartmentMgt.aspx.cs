using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace GPlus.MasterData
{
    public partial class DepartmentMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "113";
                BindDropdown();
                BindData();


                btnDep.OnClientClick = Util.CreatePopUp("../UserControls/pop_OrgSelect_New.aspx",
                                    new string[] { "divName", "depName", "orgID", "divCode", "depCode" },
                                    new string[] { ddlDiv.ClientID, TxtItemDepName.ClientID, hdOrgId.ClientID, HdDiv.ClientID, HdDep.ClientID },
                                    "popDivDepSelect"
                                );
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
            txtDepartmentCodeSearch.Text = "";
            txtDivNameSearch.Text = "";
            txtDepNameSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

            // Begin Green Edit //
            rblType.Enabled = true;
            BindBuildingFloorDropDownListWhenAddClick();
            PrepareOrgType();
            // End Green Edit
        }

        protected void gvDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[1].Text = drv["Div_Code"].ToString();
                if (drv["Dep_Code"].ToString().Trim().Length > 0)
                    e.Row.Cells[1].Text += "-" + drv["Dep_Code"].ToString();

                if (drv["Dep_Code"].ToString().ToString().Trim().Length == 0)
                    e.Row.Cells[2].Text = drv["Description"].ToString();
                else
                    e.Row.Cells[3].Text = drv["Description"].ToString();

                e.Row.Cells[5].Text = drv["OrgStruc_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["OrgStruc_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                ddlDiv.Enabled = true;
                btnDep.Enabled = true;
                rblType.Enabled = false;

                DataTable dt = new DataAccess.OrgStructureDAO().GetOrgStructure(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Dep_Code"].ToString().Trim().Length > 0)
                    {
                        rblType.SelectedIndex = 1;
                        txtDepCode.Text = dt.Rows[0]["Dep_Code"].ToString();
                        txtDepName.Text = dt.Rows[0]["Description"].ToString();
                    }
                    else
                    {
                        rblType.SelectedIndex = 0;
                        txtDivCode.Text = dt.Rows[0]["Div_Code"].ToString();
                        txtDivName.Text = dt.Rows[0]["Description"].ToString();
                        // Begin Green Edit
                        txtDepCode.Text = "";
                        txtDepName.Text = "";
                        // End Green Edit
                    }

                    PrepareOrgType();

                    // Begin Green Edit
                    DataTable dtDivDep = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt32(e.CommandArgument.ToString()));
                    ddlDiv.Text = dt.Rows[0]["Div_Code"] + "/" + dtDivDep.Rows[0]["DivName"].ToString();
                    HdDiv.Value = dt.Rows[0]["Div_Code"].ToString();
                    //ListItem lstItm = ddlDiv.Items.FindByText(dt.Rows[0]["Div_Code"] + "/" + dtDivDep.Rows[0]["DivName"].ToString());
                    //int index = ddlDiv.Items.IndexOf(lstItm);
                    //if (lstItm != null)
                    //    ddlDiv.SelectedIndex = index;

                    txtDivCode.Text = dtDivDep.Rows[0]["Div_Code"].ToString();
                    txtDivName.Text = dtDivDep.Rows[0]["DivName"].ToString();
                    // End Green Edit

                    if (ddlFromMainStock.Items.FindByValue(dt.Rows[0]["Stock_ID"].ToString()) != null)
                        ddlFromMainStock.SelectedValue = dt.Rows[0]["Stock_ID"].ToString();

                    // Green Edit //
                    BindBuildingFloorDropDownListWhenEditClick(e);
                    // End Green Edit //

                    chkNotApprove.Checked = dt.Rows[0]["NotApprove_Flag"].ToString() == "1";

                    rdbStatus.Items[0].Selected = dt.Rows[0]["OrgStruc_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["OrgStruc_Status"].ToString() == "0";

                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }

                pnlDetail.Visible = true;
            }
        }

        protected void gvDepartment_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvDepartment);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retID = "";
            string divCode = "";
            string depCode = "";
            string desc = "";
            if (rblType.SelectedIndex == 0)
            {
                divCode = txtDivCode.Text;
                desc = txtDivName.Text;
            }
            else
            {
                //divCode = ddlDiv.SelectedValue;
                divCode = HdDiv.Value.Trim();
                //divCode = ddlDiv.Text;
                depCode = txtDepCode.Text;
                desc = txtDepName.Text;
            }
            // กรณีเพิ่ม
            if (hdID.Value == "")
            {
                retID = new DataAccess.OrgStructureDAO().AddOrgStructure(
                            divCode,
                            depCode,
                            desc,
                            chkNotApprove.Checked ? "1" : "0",
                            status,
                            "",
                            this.UserName);

                if (retID != "0")
                {
                    hdID.Value = retID;
                    if (rblType.SelectedIndex == 0) // ฝ่าย
                    {
                        new DataAccess.DatabaseHelper().ExecuteQuery("UPDATE Inv_OrgStructure SET OrgStruc_Id_Division = " + hdID.Value + " WHERE OrgStruc_Id = " + hdID.Value);
                    }
                    else if (rblType.SelectedIndex == 1) // ทีม
                    {
                        DataTable dt = new DataAccess.OrgStructureDAO().GetOrgStructureByDivCode(divCode);
                        
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = " [Dep_Code] = '' ";
                        DataRowView drv = dv[0];
                        string orgStrucId = drv["OrgStruc_Id"].ToString();

                        new DataAccess.DatabaseHelper().ExecuteQuery("UPDATE Inv_OrgStructure SET OrgStruc_Id_Division = " + orgStrucId + " WHERE OrgStruc_Id = " + hdID.Value);
                    }
                }

                // Green Edit

                if (ValidateBuildingFloor())
                    InsertOrgLocation(retID);
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "Validate",
                        "alert('ตีกและชั้นต้องไม่ซ้ำกัน')",
                        true
                    );

                    return;
                }
                
                // End Green Edit
            }
            // กรณีอัพเดต
            else
            {
                // Green Edit

                retID = hdID.Value;
                new DataAccess.OrgStructureDAO().UpdateOrgStructure(hdID.Value, divCode, depCode, desc,
                 chkNotApprove.Checked ? "1" : "0", status, "", this.UserName);

                if (rblType.SelectedIndex == 1)
                {
                    DataTable dt = new DataAccess.OrgStructureDAO().GetOrgStructureByDivCode(divCode);

                    DataView dv = dt.DefaultView;
                    dv.RowFilter = " [Dep_Code] = '' ";
                    DataRowView drv = dv[0];
                    string orgStrucId = drv["OrgStruc_Id"].ToString();

                    new DataAccess.DatabaseHelper().ExecuteQuery("UPDATE Inv_OrgStructure SET OrgStruc_Id_Division = " + orgStrucId + " WHERE OrgStruc_Id = " + hdID.Value);
                }

                if (ValidateBuildingFloor())
                {
                    new DataAccess.DatabaseHelper().ExecuteQuery("DELETE FROM INV_ORGLOCATION WHERE OrgStruc_Id = " + hdID.Value);
                    InsertOrgLocation(hdID.Value);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "Validate",
                        "alert('ตีกและชั้นต้องไม่ซ้ำกัน')",
                        true
                    );

                    return;
                }

                // End Green Edit
            }
           
            if (ddlFromMainStock.SelectedIndex > 0)
            {
                new DataAccess.OrgStructureDAO().UpdateOrgStk(retID, ddlFromMainStock.SelectedValue, status, this.UserName);
            }

            // เมื่อเพิ่มฝ่ายหรือทีมแล้วให้รีเซ็ทหน้า เนื่องจาก BindDropDown ทำเพียงครั้งเดียว
            this.Response.Redirect("../MasterData/DepartMentMgt.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }

        /// <summary>
        ///     กำหนดคลังสินค้าให้กับ dropdownlist
        ///     กำหนดรหัสฝ่าย/ชื่อฝ่ายให้กับ dropdownlist
        /// </summary>
        private void BindDropdown()
        {
            ddlFromMainStock.DataSource = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlFromMainStock.DataBind();
            ddlFromMainStock.Items.Insert(0, new ListItem("--เลือกคลังสินค้า--", ""));

            DataAccess.OrgStructureDAO db = new DataAccess.OrgStructureDAO();
            DataTable dt = db.GetOrgStructure("", "", "", "", "", 1, 9000, "Div_Code", "").Tables[0];

            //ddlDiv.Items.Insert(0, new ListItem("--เลือกรหัสฝ่าย/เลือกฝ่าย--", ""));
            DataView dv = dt.DefaultView;
            
            dv.RowFilter = "Dep_Code IS NULL OR Dep_Code = ''";
            //for (int i = 0; i < dv.Count; i++)
            //    ddlDiv.Items.Add(new ListItem(dv[i]["Div_Code"].ToString() + "/" + dv[i]["Description"].ToString(), dv[i]["Div_Code"].ToString()));
        }

        private void BindData()
        {
            DataSet ds = 
                new DataAccess.OrgStructureDAO().GetOrgStructure
                (
                    txtDepartmentCodeSearch.Text, 
                    txtDepartmentCodeSearch.Text,
                    txtDivNameSearch.Text, 
                    txtDepNameSearch.Text, 
                    ddlStatus.SelectedValue, 
                    PagingControl1.CurrentPageIndex, 
                    PagingControl1.PageSize, 
                    this.SortColumn, 
                    this.SortOrder
                );

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvDepartment.DataSource = ds.Tables[0];
            gvDepartment.DataBind();
        }

        private void ClearData()
        {
            hdID.Value = "";
            txtDivCode.Text = "";
            txtDivName.Text = "";
            //ddlDiv.SelectedIndex = 0;
            ddlDiv.Text = "";
            txtDepCode.Text = "";
            txtDepName.Text = "";
            ddlFromMainStock.SelectedIndex = 0;
            chkNotApprove.Checked = false;
            rdbStatus.SelectedIndex = 0;
            rblType.SelectedIndex = 0;
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareOrgType();
        }

        private void PrepareOrgType()
        {
            if (rblType.SelectedValue == "1")
            {
                //Div Enable
                txtDivCode.Enabled = true;
                RequiredFieldValidator1.Enabled = true;
                txtDivName.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                ddlDiv.Enabled = false;
                btnDep.Enabled = false;
                RequiredFieldValidator3.Enabled = false;
                txtDepCode.Enabled = false;
                RequiredFieldValidator4.Enabled = false;
                txtDepName.Enabled = false;
                RequiredFieldValidator5.Enabled = false;
            }
            else
            {
                //Dep Enable
                txtDivCode.Enabled = false;
                RequiredFieldValidator1.Enabled = false;
                txtDivName.Enabled = false;
                RequiredFieldValidator2.Enabled = false;
                ddlDiv.Enabled = true;
                btnDep.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                txtDepCode.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                txtDepName.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
            }
        }

        #region Green 
        // -------------------------------- Green Edit -------------------------------------
        /// <summary>
        ///     โค๊ดส่วนนี้จัดการกับ dropdownlist ของตึกและชั้น
        /// </summary>

        List<Tuple<string, string>> buildingFloors = new List<Tuple<string, string>>();
        
        /// <summary>
        ///     บันทึกสถานะของ dropdownlist ของตึกและชั้นในกรณีที่มีการโหลดหน้าเพจใหม่
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            Object[] states = new Object[2];

            foreach (TableRow tableRow in TblBuildingFloor.Rows)
            {
                DropDownList ddlBuilding    = tableRow.Cells[0].Controls[0] as DropDownList;
                DropDownList ddlFloor       = tableRow.Cells[1].Controls[0] as DropDownList;

                string buildingValue        = ddlBuilding.SelectedValue;
                string floorValue           = ddlFloor.SelectedValue;

                buildingFloors.Add(new Tuple<string, string>(buildingValue, floorValue));
            }

            states[0] = buildingFloors;

            return states;
        }

        /// <summary>
        ///     คืนค่าสถานะของ dropdownlist ของตึกและชั้นในกรณีที่มีการโหลดหน้าเพจใหม่
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            Object[] states = savedState as Object[];

            List<Tuple<string, string>> values = states[0] as List<Tuple<string, string>>;

            foreach (Tuple<string, string> keyValue in values)
            {
                DropDownList ddlBuilding    = CreateBuildingDropDownList();
                DropDownList ddlFloor       = CreateFloorDropDownList();

                ddlBuilding.SelectedValue = keyValue.Item1;
                ddlFloor.SelectedValue = keyValue.Item2;

                // กรณีที่ ddlFloor ไม่สามารถจับคู่กับค่า keyValue.Item2
                if (ddlFloor.SelectedValue == "")
                {
                    DataTable dt = new DataAccess.OrgStructureDAO().GetBuildingFloor(ddlBuilding.SelectedValue);

                    foreach (DataRow drow in dt.Rows)
                    {
                        string buildingFloor = drow["Building_Floor_Desc"].ToString();
                        string buildingId = drow["Building_FloorId"].ToString();

                        ddlFloor.Items.Add(new ListItem(buildingFloor, buildingId, true));
                    }

                    ddlFloor.SelectedValue = keyValue.Item2;
                }

                AddDropDownListToTable(TblBuildingFloor, ddlBuilding, ddlFloor);
            }
        }

        private DropDownList CreateBuildingDropDownList()
        {
            DataAccess.OrgStructureDAO db = new DataAccess.OrgStructureDAO();

            DropDownList ddlBuilding    = new DropDownList();

            ddlBuilding.DataTextField = "Building_Name";
            ddlBuilding.DataValueField = "Building_ID";
            ddlBuilding.DataSource = db.GetBuilding();
            ddlBuilding.DataBind();
            ddlBuilding.Items.Insert(0, new ListItem("--ตึก--", ""));

            ddlBuilding.AutoPostBack = true;
            ddlBuilding.SelectedIndexChanged += new EventHandler(ddlBuilding_SelectedIndexChanged);

            return ddlBuilding;
        }

        private DropDownList CreateFloorDropDownList()
        {
            DropDownList ddlFloor = new DropDownList();

            ddlFloor.Items.Insert(0, new ListItem("--เลือกชั้น--", ""));

            return ddlFloor;
        }

        protected void BtnAddBuildingFloor_Click(object sender, EventArgs e)
        {
            DropDownList ddlBuilding = CreateBuildingDropDownList();
            DropDownList ddlFloor = CreateFloorDropDownList();

            AddDropDownListToTable(TblBuildingFloor, ddlBuilding, ddlFloor);
        }

        protected void BtnDelBuildingFloor_Click(object sender, EventArgs e)
        {
            if (TblBuildingFloor.Rows.Count > 0)
            {
                TblBuildingFloor.Rows.RemoveAt(TblBuildingFloor.Rows.Count - 1);
            }
        }

        private void AddDropDownListToTable(Table table, DropDownList ddlBuilding, DropDownList ddlFloor)
        {
            TableRow    tableRow    = new TableRow();
            TableCell   tableCell1  = new TableCell();
            TableCell   tableCell2  = new TableCell();

            tableCell1.Controls.Add(ddlBuilding);
            tableCell2.Controls.Add(ddlFloor);

            tableRow.Cells.AddRange(new TableCell[] { tableCell1, tableCell2 });

            table.Rows.Add(tableRow);
        }

        protected void ddlBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBuilding = sender as DropDownList;

            TableCell tableCell = ddlBuilding.Parent as TableCell;
            TableRow tableRow = tableCell.Parent as TableRow;

            DropDownList ddlFloor = tableRow.Cells[1].Controls[0] as DropDownList;

            ddlFloor.Items.Clear();

            if (ddlBuilding.SelectedIndex > 0)
            {
                DataTable dt = new DataAccess.OrgStructureDAO().GetBuildingFloor(ddlBuilding.SelectedValue);

                foreach (DataRow drow in dt.Rows)
                {
                    string buildingFloor = drow["Building_Floor_Desc"].ToString();
                    string buildingId = drow["Building_FloorId"].ToString();

                    ddlFloor.Items.Add(new ListItem(buildingFloor, buildingId, true));
                }
            }

            ddlFloor.Items.Insert(0, new ListItem("--เลือกชั้น--", ""));
        }

        protected void InsertOrgLocation(string retID)
        {
            DataAccess.OrgLocationDAO orgLocation = new DataAccess.OrgLocationDAO();

            foreach (TableRow row in TblBuildingFloor.Rows)
            {
                if (row.Enabled || row.Visible)
                {
                    DropDownList ddlBuilding = row.Cells[0].Controls[0] as DropDownList;
                    DropDownList ddlFloor = row.Cells[1].Controls[0] as DropDownList;

                    string building = ddlBuilding.SelectedValue;
                    string floor = ddlFloor.SelectedValue;

                    // เพิ่มลงตาราง OrgLocation ก็ต่อเมื่อ Building และ floor มีค่า
                    if (building != "" && floor != "")
                    {
                        int intBuilding = Convert.ToInt32(building);
                        int intFloor = Convert.ToInt32(floor);

                        orgLocation.InsertOrgLocation(Convert.ToInt32(retID), intBuilding, intFloor);
                    }
                }
            }
        }

        private bool ValidateBuildingFloor()
        {
            List<Tuple<string, string>> buildingFloors = new List<Tuple<string, string>>();

            foreach (TableRow row in TblBuildingFloor.Rows)
            {
                if (row.Enabled || row.Visible)
                {
                    DropDownList ddlBuilding = row.Cells[0].Controls[0] as DropDownList;
                    DropDownList ddlFloor = row.Cells[1].Controls[0] as DropDownList;

                    buildingFloors.Add(new Tuple<string, string>(ddlBuilding.SelectedValue, ddlFloor.SelectedValue));
                }
            }

            for (int i = 0; i < buildingFloors.Count; ++i)
            {
                for (int j = i + 1; j < buildingFloors.Count; ++j)
                {
                    if (buildingFloors[i].Equals(buildingFloors[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void BindBuildingFloorDropDownListWhenEditClick(GridViewCommandEventArgs e)
        {
            DataTable table = new DataAccess.OrgLocationDAO().GetBuildingFloorByOrgID(Convert.ToInt32(e.CommandArgument.ToString()));

            int numrowBeforeAdd = TblBuildingFloor.Rows.Count;
            foreach (DataRow row in table.Rows)
            {
                string buildingID = row["Building_ID"].ToString();
                string buildingFloor = row["Building_FloorID"].ToString();
                string buildingName = row["Building_Name"].ToString();
                string buildingFloorDesc = row["Building_Floor_Desc"].ToString();

                DropDownList ddlBuilding = CreateBuildingDropDownList();
                DropDownList ddlFloor = CreateFloorDropDownList();

                ddlBuilding.SelectedValue = buildingID;

                DataTable tb = new DataAccess.OrgStructureDAO().GetBuildingFloor(buildingID);
                foreach (DataRow drow in tb.Rows)
                {
                    string floorId = drow["Building_FloorId"].ToString();

                    ddlFloor.Items.Add(new ListItem(drow["Building_Floor_Desc"].ToString(), drow["Building_FloorId"].ToString(), true));
                }

                ddlFloor.SelectedValue = buildingFloor;

                AddDropDownListToTable(TblBuildingFloor, ddlBuilding, ddlFloor);
            }

            while (numrowBeforeAdd > 0)
            {
                TblBuildingFloor.Rows[numrowBeforeAdd - 1].Enabled = false;
                TblBuildingFloor.Rows[numrowBeforeAdd - 1].Visible = false;
                numrowBeforeAdd--;
            }
        }

        private void BindBuildingFloorDropDownListWhenAddClick()
        {
            if (TblBuildingFloor.Rows.Count == 0)
            {
                DropDownList ddlBuilding = CreateBuildingDropDownList();
                DropDownList ddlFloor = CreateFloorDropDownList();

                TableRow tableRow = new TableRow();
                TableCell tableCell1 = new TableCell();
                TableCell tableCell2 = new TableCell();

                tableCell1.Controls.Add(ddlBuilding);
                tableCell2.Controls.Add(ddlFloor);

                tableRow.Cells.AddRange(new TableCell[] { tableCell1, tableCell2 });

                TblBuildingFloor.Rows.Add(tableRow);
            }
            else
            {
                while (TblBuildingFloor.Rows.Count > 1)
                {
                    TblBuildingFloor.Rows.RemoveAt(TblBuildingFloor.Rows.Count - 1);
                }

                DropDownList ddlBuilding = TblBuildingFloor.Rows[0].Cells[0].Controls[0] as DropDownList;
                DropDownList ddlFloor = TblBuildingFloor.Rows[0].Cells[1].Controls[0] as DropDownList;

                ddlBuilding.SelectedValue = "";
                ddlFloor.SelectedValue = "";

                if (TblBuildingFloor.Rows.Count == 1)
                {
                    TblBuildingFloor.Rows[0].Enabled = true;
                    TblBuildingFloor.Rows[0].Visible = true;
                }
            }
        }

        // -------------------------------- Green Edit -------------------------------------
        #endregion Green
    }
}
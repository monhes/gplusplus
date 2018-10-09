using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.MasterData
{
    public partial class BulingFloorMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "127";
                BindBuildingDropdownList();
                BindBuildingDropdownList1();

                DataTable dt = new DataTable();
                dt.Columns.Add("Building_Id");
                dt.Columns.Add("Building_FloorId");
                dt.Columns.Add("Building_Floor_Desc");
                dt.Columns.Add("Created_By");
                dt.Columns.Add("Created_Date");
                dt.Columns.Add("Updated_By");
                dt.Columns.Add("Updated_Date");
                dt.Columns.Add("Status");

                ViewState["buildingFloorTable"] = dt;
                ViewState["lastest_buildingFloor"] = -1;
            }
        }

        private void BindBuildingDropdownList()
        {
            DataTable dtBuilding = new OrgStructureDAO().GetBuilding();

            ddlBuilding.DataSource = dtBuilding;
            ddlBuilding.DataBind();
            ddlBuilding.Items.Insert(0, new ListItem("เลือกตึก/อาคาร", ""));
            ClearData();
            if (Request["tid"] != null)
            {
                if (ddlBuilding.Items.FindByValue(Request["tid"]) != null)
                    ddlBuilding.SelectedValue = Request["tid"];
            }
        }

        private void BindBuildingDropdownList1()
        {

            DataTable dtBuilding = new OrgStructureDAO().GetBuilding();

            ddlBuilding1.DataSource = dtBuilding;
            ddlBuilding1.DataBind();
            ddlBuilding1.Items.Insert(0, new ListItem("เลือกตึก/อาคาร", ""));
            ClearData();
           
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlBuilding.SelectedValue == "")
            {
                ShowMessageBox("กรุณาเลือกตึก/อาคาร");
                return;
            }
            else
            {
                pnlDetail.Visible = true;
                BindData();
            }
        }

        protected void gvBuilding_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                 DataRowView drv = (DataRowView)e.Row.DataItem;
                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Building_ID"].ToString();

            }

        }
        protected void gvBuilding_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {

                DataAccess.BuildingDAO db = new DataAccess.BuildingDAO();
                DataTable dt = db.GetBuildingByCodeAndName(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();

                if (dt.Rows.Count > 0)
                {
                    ViewState["buildingFloorTable"] = dt;
                    ddlBuilding1.Text = dt.Rows[0]["Building_Id"].ToString();
                    ddlBuilding1_SelectedIndexChanged(this, new EventArgs());
                    gvBuildingFloor.DataSource = dt;
                    gvBuildingFloor.DataBind();
                }
            }
            ddlBuilding1.Enabled = false;
            pnlDetail.Visible = true;
        }


        protected void gvBuildingFloor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdBuildingFloor = e.Row.FindControl("hdBuildingFloor") as HiddenField;
                DataRowView drv = (DataRowView)e.Row.DataItem;

                TextBox txtFloor = e.Row.Cells[0].FindControl("txtFloor") as TextBox;
                RadioButtonList rblStatus = e.Row.Cells[1].FindControl("rblStatus") as RadioButtonList;

                txtFloor.Text = drv["Building_Floor_Desc"].ToString();
                //rblStatus.Text = drv["Status"].ToString();
                string status = drv["Status"].ToString();
                if (status.Trim() == "0")
                {
                    rblStatus.SelectedIndex = 1;
                }
                else
                {
                    rblStatus.SelectedIndex = 0;
                }

                e.Row.Cells[2].Text = drv["Updated_Date"].ToString();
                e.Row.Cells[3].Text = drv["Updated_By"].ToString();
                e.Row.Cells[4].Text = drv["Created_Date"].ToString();
                e.Row.Cells[5].Text = drv["Created_By"].ToString();

                hdBuildingFloor.Value = drv["Building_FloorId"].ToString();

                //DataRowView drv = (DataRowView) e.Row.DataItem;

                //string buildingCode = drv["Building_Code"].ToString();
                //string buildingName = drv["Building_Name"].ToString();
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            ddlBuilding1.Enabled = true;
            
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {

            txtFloor.Text = "";
            ddlBuilding.SelectedIndex = 0;
        }

        protected void btnPlus_Click(object sender, EventArgs e)
        {


            DataTable dtBuildingFloor = ViewState["buildingFloorTable"] as DataTable;
            for (int i = 0; i < gvBuildingFloor.Rows.Count; ++i)
            {
                GridViewRow row = gvBuildingFloor.Rows[i];

                HiddenField hdBuildingFloor = row.FindControl("hdBuildingFloor") as HiddenField;
                TextBox txtFloor = row.FindControl("txtFloor") as TextBox;

                RadioButtonList rblStatus = row.FindControl("rblStatus") as RadioButtonList;

                DataRow[] rows = dtBuildingFloor.Select(String.Format("Building_FloorId = '{0}'", hdBuildingFloor.Value));
                if (rows.Length > 0)
                {
                    rows[0]["Building_Floor_Desc"] = txtFloor.Text;
                    rows[0]["Status"] = rblStatus.SelectedValue;
                    rows[0]["Created_By"] = row.Cells[5].Text;
                    if (row.Cells[4].Text != null && !row.Cells[4].Text.Equals(""))
                    {
                        rows[0]["Created_Date"] = row.Cells[4].Text;
                    }
                    
                    rows[0]["Updated_By"] = row.Cells[3].Text;
                    if (row.Cells[2].Text != null && !row.Cells[2].Text.Equals(""))
                    {
                        rows[0]["Updated_Date"] = row.Cells[2].Text;
                    }
                    
                }
                else
                {
                    DataRow drNew = dtBuildingFloor.NewRow();
                    drNew["Building_Id"] = ddlBuilding1.SelectedValue;
                    drNew["Building_FloorId"] = hdBuildingFloor.Value;
                    drNew["Building_Floor_Desc"] = txtFloor.Text;
                    drNew["Status"] = rblStatus.SelectedValue;
                    drNew["Created_By"] = row.Cells[5].Text;
                    drNew["Created_Date"] = row.Cells[4].Text;
                    drNew["Updated_By"] = row.Cells[3].Text;
                    drNew["Updated_Date"] = row.Cells[2].Text;

                    dtBuildingFloor.Rows.Add(drNew);
                    
                }
           
            }

            DataRow dr = dtBuildingFloor.NewRow();

            int latestBuildingFloor = (int)ViewState["lastest_buildingFloor"];
            dr["Building_Id"] = 0;
            dr["Building_FloorId"] = latestBuildingFloor;
            ViewState["lastest_buildingFloor"] = --latestBuildingFloor;

            dr["Building_Floor_Desc"] = "ชั้น ";

            dtBuildingFloor.Rows.Add(dr);
            dtBuildingFloor.AcceptChanges();

            gvBuildingFloor.DataSource = dtBuildingFloor;
            gvBuildingFloor.DataBind();

        }

        protected void ddlBuilding1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void btnAdd1_Click(object sender, EventArgs e)
        {
            if (ddlBuilding1.SelectedValue == "")
            {
                this.ShowMessageBox("กรุณาเลือกอาคาร");
                ddlBuilding1.Focus();
                return;
            }
            
            foreach (GridViewRow gvr in gvBuildingFloor.Rows)
            {
      
                HiddenField hdBuildingFloor = (HiddenField)gvr.FindControl("hdBuildingFloor");
             
                TextBox txtFloor = (TextBox)gvr.FindControl("txtFloor");

                RadioButtonList rblStatus = (RadioButtonList)gvr.FindControl("rblStatus");

                string status = rblStatus.SelectedIndex == 0 ? "1" : "0";
               

                if (hdID.Value == "")
                {
                    if (Convert.ToInt32(hdBuildingFloor.Value == "" ? "0" : hdBuildingFloor.Value) < 0)
                    {
                        DataTable dtFloor = new DataAccess.BuildingDAO().GetBuildingByFloorDesc(ddlBuilding1.SelectedValue, txtFloor.Text, status, this.UserID);

                        if (dtFloor.Rows[0]["result"].ToString().Equals("0"))
                        {
                            ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้ เนื่องจากมีข้อมูลนี้อยู่แล้ว กรูณากรอกข้อมูลใหม่");
                            return;
                        } 
                    }
                    else
                    {
                        DataTable dtFloor = new DataAccess.BuildingDAO().UpdateBuildingFloor(hdID.Value, hdBuildingFloor.Value, txtFloor.Text, status, this.UserID);
                        if (dtFloor.Rows[0]["result"].ToString().Equals("0"))
                        {
                            ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้ เนื่องจากมีข้อมูลนี้อยู่แล้ว กรูณากรอกข้อมูลใหม่");
                            return;
                        } 
                    }
                }
                else
                {
                    if (Convert.ToInt32(hdBuildingFloor.Value == "" ? "0" : hdBuildingFloor.Value) < 0)
                    {
                        DataTable dtFloor = new DataAccess.BuildingDAO().GetBuildingByFloorDesc(ddlBuilding1.SelectedValue, txtFloor.Text, status, this.UserID);
                        if (dtFloor.Rows[0]["result"].ToString().Equals("0"))
                        {
                            ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้ เนื่องจากมีข้อมูลนี้อยู่แล้ว กรูณากรอกข้อมูลใหม่");
                            return;
                        } 
                    }
                    else
                    {
                        DataTable dtFloor = new DataAccess.BuildingDAO().UpdateBuildingFloor(hdID.Value, hdBuildingFloor.Value, txtFloor.Text, status, this.UserID);
                        if (dtFloor.Rows[0]["result"].ToString().Equals("0"))
                        {
                            ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้ เนื่องจากมีข้อมูลนี้อยู่แล้ว กรูณากรอกข้อมูลใหม่");
                            return;  
                        }                       
                    }
              
                }
               
              
            }
           
            DataTable dt = ViewState["buildingFloorTable"] as DataTable;
            dt.Rows[dt.Rows.Count - 1].Delete();
            dt.AcceptChanges();
            ////dt.Rows.Add(hdID.Value, hdBuildingFloor.Value, txtFloor.Text, "", "", "", "", status); //
            ViewState["buildingFloorTable"] = dt;
            ClearData();
            pnlDetail.Visible = false;
            BindData();
            
        }
        private void BindData()
        {
            string floorDescription = txtFloor.Text;
            string buildingId = ddlBuilding.SelectedValue;

            DataTable dt = new BuildingDAO().GetBuildingByCodeAndName(buildingId, floorDescription);

            gvBuilding.DataSource = dt;
            gvBuilding.DataBind();
        }
        private void ClearData()
        {
            hdID.Value = "";
            ddlBuilding1.SelectedIndex = 0;           
            gvBuildingFloor.DataSource = null;
            gvBuildingFloor.DataBind();
        }

        protected void btnDelete1_Click(object sender, EventArgs e)
        {
           DataTable dtBuildingFloor = ViewState["buildingFloorTable"] as DataTable;

            foreach (GridViewRow row in gvBuildingFloor.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                HiddenField hdBdFloor = (HiddenField)row.FindControl("hdBdFloor");
                HiddenField hdBuildingFloor = (HiddenField)row.FindControl("hdBuildingFloor");

                if (chkSelect.Checked == true) //ติ๊กเลือก เพื่อทำการลบ
                {
                    int Building_FloorId = Convert.ToInt32(hdBuildingFloor.Value == "" ? "0" : hdBuildingFloor.Value);
                 
                    if (Building_FloorId < 0) 
                    {                     
                        DataRow[] drs = dtBuildingFloor.Select("Building_FloorId = '" + Building_FloorId + "'");
                        if (drs != null && drs.Length > 0)
                        {
                            
                            dtBuildingFloor.Rows.Remove(drs[0]);
                            dtBuildingFloor.AcceptChanges();

                            for (int i = 0; i < gvBuildingFloor.Rows.Count; ++i)
                            {
                                GridViewRow row1 = gvBuildingFloor.Rows[i];
                               
                                HiddenField hdBuildingFloor1 = row1.FindControl("hdBuildingFloor") as HiddenField;
                                TextBox txtFloor = row1.FindControl("txtFloor") as TextBox;
                                RadioButtonList rblStatus = row1.FindControl("rblStatus") as RadioButtonList;
                                DataRow[] rows = dtBuildingFloor.Select(String.Format("Building_FloorId = '{0}'", hdBuildingFloor1.Value));
                                if (rows.Length > 0)
                                {
                                    rows[0]["Building_Floor_Desc"] = txtFloor.Text;
                                    rows[0]["Status"] = rblStatus.SelectedValue;
                                    rows[0]["Created_By"] = row1.Cells[5].Text;

                                    if (row.Cells[4].Text != null && !row.Cells[4].Text.Equals(""))
                                    {
                                        rows[0]["Created_Date"] = row1.Cells[4].Text;
                                    }

                                    rows[0]["Updated_By"] = row1.Cells[3].Text;

                                    if (row.Cells[2].Text != null && !row.Cells[2].Text.Equals(""))
                                    {
                                        rows[0]["Updated_Date"] = row1.Cells[2].Text;
                                    }
                                }
                            }

                            gvBuildingFloor.DataSource = dtBuildingFloor;
                            gvBuildingFloor.DataBind();
                        }
                    }
                    else 
                    {
                        
                            ShowMessageBox("ไม่สามารถลบได้ เนื่องจากมีข้อมูลอยู่ในฐานข้อมูลแล้ว ทำการแก้ไขสถานะได้เท่านั้น");
                        
                    }

                }
            }
        }
       
        protected void btnDelete2_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }


}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.Diagnostics;

namespace GPlus.Stock
{
    public partial class frmDistribute : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "412";
                //ConfrimationSave
                this.BindingMandatoryData();
                this.BindingScheduleGrid();
                //this.btnSave.Attributes["onclick"] = "return ConfrimationSave();";
                //this.rdNoLimitToDate.Attributes["onchange"] = "javascript:OnLimitDateSelectionChange();";
                //this.rdLimitToDate.Attributes["onchange"] = "javascript:OnLimitDateSelectionChange();";
            }
            else
            {
                if (Session["InsertOrUpdate"] != null)
                {
                    Save();
                    Session["InsertOrUpdate"] = null;
                }
            }

            //Page.MaintainScrollPositionOnPostBack = true;
        }
        #region [ Binding ]
        /// <summary>
        /// This method use for binding schedule grid
        /// </summary>
        private void BindingScheduleGrid(bool isSearch=false)
        {
            DataView dv = null;
            if (!isSearch)
            {
                dv = new DistributeDAO().GetScheduleList();
            }
            else
            {
                dv = new DistributeDAO().GetScheduleList(
                    Convert.ToInt32(this.ddStock.SelectedValue)
                    , Convert.ToInt32(this.ddSearchDSStatus.SelectedValue)
                    , this.ccSearchFrom.Value == DateTime.MinValue ? string.Empty : this.ccSearchFrom.Value.ToString(this.DateFormat)
                    , this.ccSearchTo.Value == DateTime.MinValue ? string.Empty : this.ccSearchTo.Value.ToString(this.DateFormat));
            }
            this.gvSchedule.DataSource = dv;
            this.gvSchedule.DataBind();

            this.Session["ScheduleList"] = dv;
        }
        /// <summary>
        /// This method use to binding mandatory datasource to every controls
        /// </summary>
        private void BindingMandatoryData()
        {
            DistributeDAO dDao = new DistributeDAO();
            //Stock Dropdown
            this.ddStock.DataTextField = "Stock_Name";
            this.ddStock.DataValueField = "Stock_Id";
            this.ddStock.DataSource = dDao.GetActiveStock();
            this.ddStock.DataBind();
            //Search Status dropdown
            this.ddSearchDSStatus.DataTextField = "Seacrh_DS_Desc";
            this.ddSearchDSStatus.DataValueField = "Seacrh_DS_Id";
            this.ddSearchDSStatus.DataSource = dDao.GetSearchDistributeStatus();
            this.ddSearchDSStatus.DataBind();
            //Building Combobox
            this.ddBuilding.DataTextField = "Building_Name";
            this.ddBuilding.DataValueField = "Building_ID";
            this.ddBuilding.DataSource = dDao.GetActiveBuilding();
            this.ddBuilding.DataBind();
            this.ddBuilding.Items.Insert(0, new ListItem("ทั้งหมด",""));
            this.ddBuilding.SelectedIndex = 0;
            //Building Floor Combobox
            this.BindingBuildingFloor();
        }
        /// <summary>
        /// This method use to binding building floor
        /// </summary>
        private void BindingBuildingFloor()
        {
            DistributeDAO dDao = new DistributeDAO();
            if (this.ddBuilding.SelectedIndex > 0)
            {
                // Green Edit
                DataTable dt = dDao.GetBuildingFloor2(ddBuilding.SelectedItem.Value);
                ddBuildinfFloor.DataTextField = dt.Columns["Building_Floor_Desc"].Caption;
                ddBuildinfFloor.DataValueField = dt.Columns["Building_FloorId"].Caption;
                ddBuildinfFloor.DataSource = dt;
                ddBuildinfFloor.DataBind();
                ddBuildinfFloor.Items.Insert(0, new ListItem("ทั้งหมด", ""));
                // End green Edit
                
                //this.ddBuildinfFloor.DataTextField = "Building_Floor_Desc";
                //this.ddBuildinfFloor.DataValueField = "Building_Floor_Desc";
                ////this.ddBuildinfFloor.DataSource = dDao.GetBuildingFloor(this.ddBuilding.SelectedItem.Value);
                //this.ddBuildinfFloor.DataSource = dDao.GetBuildingFloor2(this.ddBuilding.SelectedItem.Value);
                //this.ddBuildinfFloor.DataBind();
                //this.ddBuildinfFloor.Items.Insert(0, new ListItem("ทั้งหมด", ""));
            }
        }
        /// <summary>
        /// This method use to set binding distribute form with new / old data
        /// </summary>
        /// <param name="scheduleId">scheduleId id</param>
        private void BindingDistributeForm(int scheduleId = 0, string dayNumber = null, bool isNewScheduleDay = false)
        {
            this.pnDSDetail.Visible = true; //Show detail panel

            // เมื่อผู้ใช้คลิกเลือก "สร้างตารางการจ่าย"
            if (scheduleId == 0)
            {
                //New form
                //Set default value for new form
                this.rdScheduleDayStatus.SelectedValue = "1";
                this.ccDSFrom.Value = DateTime.Now.Date;
                this.rdNoLimitToDate.Checked = true;
                //this.ccLimitDate.Enabled = false;
                this.rdSearchNameType.SelectedValue = "1";
                this.rdDateOfWeek.SelectedValue = this.GetDayOfWeek();
                this.tbCreateBy.Text = string.Empty;
                this.tbUpdateBy.Text = string.Empty;
                this.tbCreateDate.Text = string.Empty;
                this.tbUpdateDate.Text = string.Empty;
                this.SetReadOnlyEditor(false);
                //Get default schema
                DataSet ds = new DistributeDAO().GetSchedule();

                this.Session["Schedule"] = ds;
            }
            else
            {
                DataSet ds = new DistributeDAO().GetSchedule(scheduleId, isNewScheduleDay);
                if (dayNumber != null && !isNewScheduleDay)
                {
                    DataTable dt = new DistributeDAO().GetScheduleDay("" + scheduleId, dayNumber);
                    ds.Tables.RemoveAt(1);
                    ds.Tables.Add(dt);
                }
                DataRow dr = ds.Tables[0].Rows[0];
                this.rdScheduleDayStatus.SelectedValue = dr["Cancel_Status"].ToString() == "True" ? "1" : "0";
                this.ccDSFrom.Value = Convert.ToDateTime(dr["Start_Date"].ToString());
                if (!string.IsNullOrEmpty(dr["Finish_Date"].ToString()))
                {
                    this.rdLimitToDate.Checked = true;
                    this.ccLimitDate.Value = Convert.ToDateTime(dr["Finish_Date"].ToString());
                }
                else
                {
                    this.rdNoLimitToDate.Checked = true;
                }
                this.rdSearchNameType.SelectedValue = "1";
                if (dayNumber != null)
                {
                    this.rdDateOfWeek.SelectedValue = dayNumber;
                }
                else 
                {
                    this.rdDateOfWeek.SelectedValue = this.GetDayOfWeek();
                }
                this.tbCreateBy.Text = dr["Create_By_Name"].ToString();
                this.tbUpdateBy.Text = dr["Update_By_Name"].ToString();
                this.tbCreateDate.Text = Convert.ToDateTime(dr["Create_Date"]).ToString(this.DateTimeFormat);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        this.ddBuilding.SelectedValue = ds.Tables[1].Rows[0]["Building_Id"].ToString();
                        this.BindingBuildingFloor();
                        if(ds.Tables[1].Rows[0]["Building_FloorId"].ToString().Trim().Length > 0)
                            this.ddBuildinfFloor.SelectedValue = ds.Tables[1].Rows[0]["Building_FloorId"].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(dr["Update_Date"].ToString()))
                {
                    this.tbUpdateDate.Text = Convert.ToDateTime(dr["Update_Date"]).ToString(this.DateTimeFormat);
                }
                this.Session["Schedule"] = ds;
                if (!isNewScheduleDay)
                {
                    this.SetReadOnlyEditor(false);      // change "true" to "false"
                    this.BindingScheduleDayData("" + scheduleId, dayNumber);
                }
                else
                {
                    this.SetReadOnlyEditor(false);
                    this.BindingScheduleDayData("" + scheduleId, dayNumber, false);
                }
                
            }
        }
       
        /// <summary>
        /// This method use for binding schedule day data
        /// </summary>
        private void BindingScheduleDayData(string scheduleId = null, string dayNumber = null, bool isVisible = true)
        {
            this.pnDaySelector.Visible = isVisible;
            string mainStockId = string.Empty;
            string buidingId = string.Empty;
            string buildingFloor = string.Empty;
            string searchName = this.tbSearchName.Text;
            string searchBy = this.rdSearchNameType.SelectedValue;

            // เพิ่มการค้นหาด้วย "รหัสฝ่าย" และ "รหัสทีม"
            string searchDiv = tbDivCode.Text;
            string searchDep = tbDepCode.Text;

            if (scheduleId != null && dayNumber != null)
            {
                if (Session["ScheduleList"] != null)
                {
                    DataTable dt = ((DataView)Session["ScheduleList"]).Table;
                    if (dt != null)
                    {
                        DataRow dr = dt.Select("[Schedule_ID]='" + scheduleId + "' AND [DayNumber]='" + dayNumber + "'").FirstOrDefault();
                        if (dr != null)
                        {
                            mainStockId = dr["Stock_ID"].ToString();
                            if (dr["Building_ID"].ToString().Trim().Length > 0)
                                buidingId = dr["Building_ID"].ToString();
                            if(dr["Building_FloorID"].ToString().Trim().Length > 0)
                                buildingFloor = dr["Building_FloorID"].ToString();
                        }
                    }
                }
            }
            else
            {
                mainStockId = this.ddStock.SelectedValue;
                buidingId = this.ddBuilding.SelectedValue;

                buildingFloor = ddBuildinfFloor.SelectedValue;
                //buildingFloor = this.ddBuildinfFloor.SelectedItem.Value;
            }
            //Set day label
            this.lbDisplayDayOfWeek.Text = this.GetDayOfWeekString();
            DataSet dsResult = null;
            if (Session["ScheduleDayData"] == null)
            {
                dsResult = new DistributeDAO().GetScheduleDayData(buidingId, buildingFloor, mainStockId, searchName, searchBy, searchDiv, searchDep);
            }
            else
            {
                dsResult = (DataSet)Session["ScheduleDayData"];
            }

            if (dsResult != null)
            {
                this.Session["ScheduleDayData"] = dsResult;
                this.BindingGridResult(dsResult);

                // ========================== Begin Green Edit ============================

                DataRow[] rows = dsResult.Tables[0].Select("[Is_Selected] = 'Y'");
                string orgidstr = "";
                foreach (DataRow row in rows)
                {
                    orgidstr += row["OrgStruc_Id"].ToString() + ",";
                }
                if (orgidstr != "")
                {
                    orgidstr = orgidstr.Substring(0, orgidstr.Length - 1);

                    rows = dsResult.Tables[0].Select("[OrgStruc_Id] IN (" + orgidstr + ")");

                    foreach (DataRow row in rows)
                    {
                        row["Is_Selected"] = "Y";
                        row.AcceptChanges();
                    }
                }

                // =========================== End Green edit ============================= 

                DataView drvOrg = new DataView(dsResult.Tables[0]);

                drvOrg.RowFilter = "[Is_Selected] = 'N'";

                
                // ========================== Begin Green Edit ============================

                if (tbDivCode.Text.Trim().Length > 0)
                    drvOrg.RowFilter += " AND [Div_Code] LIKE '%" + tbDivCode.Text + "%'";
                if (tbDepCode.Text.Trim().Length > 0)
                    drvOrg.RowFilter += " AND [Dep_Code] LIKE '%" + tbDepCode.Text + "%'";

                DataTable table = drvOrg.ToTable(true, new string[] { "OrgStruc_Id", "Div_Code", "Dep_Code", "Description" });
                this.gvOrg.DataSource = table;

                // =========================== End Green edit =============================

                DataView drvStock = new DataView(dsResult.Tables[1]);
                drvStock.RowFilter = "[Is_Selected] = 'N'";
                this.gvStock.DataSource = drvStock;
                this.gvOrg.DataBind();
                this.gvStock.DataBind();
            }
        }

        /// <summary>
        /// This method use for set result grid
        /// </summary>
        /// <param name="dsResult"></param>
        private void BindingGridResult(DataSet dsResult)
        {
            if (Session["Schedule"] != null)
            {
                DataSet ds = (DataSet)Session["Schedule"];
                if (ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        this.SyncDataSource();
                    }

                    // Green Edit
                    // this.CreateDummyRow(dsResult, ds);
                    DataRow[] rows = ds.Tables[1].Select("[IsDummy] = 'Y'");
                    foreach (DataRow row in rows)
                    {
                        row.Delete();
                    }
                    // End Green Edit
                }

                DataView dv = new DataView(ds.Tables[1]);
                dv.RowFilter = "[Is_Delete] = 'N'";
                this.gvResult.DataSource = dv;
                this.gvResult.DataBind();

                Session["Schedule"] = ds;
            }
        }

        #endregion


        #region [ Ultility ]
        /// <summary>
        /// This method use for get day of week
        /// </summary>
        /// <returns></returns>
        private string GetDayOfWeek()
        {
            return (Convert.ToInt32(DateTime.Now.DayOfWeek)).ToString();
        }
        /// <summary>
        /// This method use to get old or new daynumber
        /// </summary>
        /// <returns></returns>
        private string GetOldOrNewDayOfWeek()
        {
            DataSet dsSchdule = (DataSet)Session["Schedule"];
            DataRow drOldDataFromSchedule = dsSchdule.Tables[1].Select("[Is_New] = 'N'").FirstOrDefault();
            if (drOldDataFromSchedule != null)
                return drOldDataFromSchedule["DayNumber"].ToString();
            else
                return this.rdDateOfWeek.SelectedValue;
        }
        /// <summary>
        /// This method use for get day of week string
        /// </summary>
        /// <returns>Day name</returns>
        private string GetDayOfWeekString(string dayNumber = null)
        {
            string[] dayNames = { "วันอาทิตย์", "วันจันทร์", "วันอังคาร", "วันพุธ", "วันพฤหัสบดี", "วันศุกร์", "วันเสาร์" };
            if (dayNumber != null)
            {
                return dayNames[Convert.ToInt32(dayNumber)];
            }
            return "เลือก " + dayNames[Convert.ToInt32(this.rdDateOfWeek.SelectedValue)];
        }
        /// <summary>
        /// This method use for created dummy row
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="ds"></param>
        private void CreateDummyRow(DataSet dsResult, DataSet ds)
        {
            this.RemoveDummyRow(ds.Tables[1]); //create all dummy row
            //Created dummy row
            int maxDummyRow = dsResult.Tables[0].Rows.Count + dsResult.Tables[1].Rows.Count - ds.Tables[1].Rows.Count;
            for (int i = 0; i < maxDummyRow; i++)
            {
                DataRow dr = ds.Tables[1].NewRow();
                dr["IsDummy"] = "Y";
                dr["Is_New"] = "Y";
                dr["Is_Delete"] = "N";
                dr["Distribute_Freq"] = 0;
                ds.Tables[1].Rows.Add(dr);
            }
        }
        /// <summary>
        /// This method use for remove dummy row
        /// </summary>
        /// <param name="dataTable"></param>
        private void RemoveDummyRow(DataTable dataTable)
        {
            DataRow[] drs = dataTable.Select("[IsDummy]='Y' AND [Schedule_DayID] IS NULL");
            foreach (DataRow dr in drs)
            {
                dataTable.Rows.Remove(dr);
            }
        }
        /// <summary>
        /// This method use to push selected data row to result grid
        /// </summary>
        private void PushIn()
        {
            this.CollectScheduleDayResultData();

            DataSet dsScheduleDay = (DataSet)Session["ScheduleDayData"];
            DataSet dsSchdule = (DataSet)Session["Schedule"];
            // Org
            foreach (GridViewRow gvdr in this.gvOrg.Rows)
            {
                if ((gvdr.Cells[3].FindControl("chkSelectOrg") as CheckBox).Checked)
                {
                    string orgId = gvdr.Cells[0].Text;
                    DataRow dr = dsScheduleDay.Tables[0].Select("[OrgStruc_Id] = '" + orgId + "'").FirstOrDefault();
                    if (dr != null)
                    {
                        dr["Is_Selected"] = "Y";
                        dr.AcceptChanges();
                    }
                    DataRow drOldDataFromSchedule = dsSchdule.Tables[1].Select("[Is_New] = 'N' AND [Is_Delete] = 'Y' AND [OrgStruc_Id] = '" + orgId + "'").FirstOrDefault();
                    if (drOldDataFromSchedule != null)
                    {
                        //active old item
                        drOldDataFromSchedule["Is_Delete"] = 'N';
                        drOldDataFromSchedule.AcceptChanges();
                    }
                    else
                    {
                        DataRow row = dsSchdule.Tables[1].NewRow();
                        row["Org_Desc"] = dr["Description"];
                        row["OrgStruc_Id"] = orgId;
                        row["IsDummy"] = "N";
                        row["DayNumber"] = this.GetOldOrNewDayOfWeek();
                        row["Cancel_Status"] = true;
                        row["Div_Code"] = dr["Div_Code"];
                        row["Dep_Code"] = dr["Dep_Code"];

                        // according to CreateDummyRow()
                        row["Is_New"] = "Y";
                        row["Is_Delete"] = "N";
                        row["Distribute_Freq"] = "0";
                        // 

                        if (this.ddBuildinfFloor.SelectedIndex > 0)
                            row["Building_FloorId"] = this.ddBuildinfFloor.SelectedValue;
                        if (this.ddBuilding.SelectedIndex > 0)
                            row["Building_Id"] = this.ddBuilding.SelectedValue;

                        dsSchdule.Tables[1].Rows.Add(row);

                        row.AcceptChanges();
                        /*
                        //Add new item
                        DataRow drFromSchedule = dsSchdule.Tables[1].Select("[IsDummy] = 'Y'").FirstOrDefault();
                        drFromSchedule["Org_Desc"] = dr["Description"];
                        drFromSchedule["OrgStruc_Id"] = orgId;
                        drFromSchedule["IsDummy"] = "N";
                        drFromSchedule["DayNumber"] = this.GetOldOrNewDayOfWeek();
                        drFromSchedule["Cancel_Status"] = true;
                        // Green
                        drFromSchedule["Div_Code"] = dr["Div_Code"];
                        drFromSchedule["Dep_Code"] = dr["Dep_Code"];
                        // Green
                        if(this.ddBuildinfFloor.SelectedIndex > 0)
                            drFromSchedule["Building_FloorId"] = this.ddBuildinfFloor.SelectedValue;
                        if (this.ddBuilding.SelectedIndex > 0)
                            drFromSchedule["Building_Id"] = this.ddBuilding.SelectedValue;
                        drFromSchedule.AcceptChanges();
                        */
                    }
                }
            }
            //Stock
            foreach (GridViewRow gvdr in this.gvStock.Rows)
            {
                if ((gvdr.Cells[1].FindControl("chkSelectStock") as CheckBox).Checked)
                {
                    string stockId = gvdr.Cells[0].Text;
                    DataRow dr = dsScheduleDay.Tables[1].Select("[Stock_ID] = '" + stockId + "'").FirstOrDefault();
                    if (dr != null)
                    {
                        dr["Is_Selected"] = "Y";
                        dr.AcceptChanges();
                    }
                    DataRow drOldDataFromSchedule = dsSchdule.Tables[1].Select("[Is_New] = 'N' AND [Is_Delete] = 'Y' AND [Stock_ID] = '" + stockId + "'").FirstOrDefault();
                    if (drOldDataFromSchedule != null)
                    {
                        //active old item
                        drOldDataFromSchedule["Is_Delete"] = 'N';
                        drOldDataFromSchedule.AcceptChanges();
                    }
                    else
                    {
                        DataRow drFromSchedule = dsSchdule.Tables[1].Select("[IsDummy] = 'Y'").FirstOrDefault();
                        drFromSchedule["Stock_Name"] = dr["Stock_Name"];
                        drFromSchedule["Stock_ID"] = stockId;
                        drFromSchedule["IsDummy"] = "N";
                        drFromSchedule["DayNumber"] = this.GetOldOrNewDayOfWeek();
                        drFromSchedule["Cancel_Status"] = true;
                        if (this.ddBuildinfFloor.SelectedIndex > 0)
                            drFromSchedule["Building_FloorId"] = this.ddBuildinfFloor.SelectedValue;
                        if (this.ddBuilding.SelectedIndex > 0)
                            drFromSchedule["Building_Id"] = this.ddBuilding.SelectedValue;
                        drFromSchedule.AcceptChanges();
                    }
                }
            }
            this.Session["Schedule"] = dsSchdule;
            this.Session["ScheduleDayData"] = dsScheduleDay;
            this.BindingScheduleDayData();

            ScrollToElement(gvResult.ClientID);
        }
       
        /// <summary>
        /// This method use to push data out from result grid
        /// </summary>
        private void PushOut()
        {
            this.CollectScheduleDayResultData();

            DataSet dsScheduleDay = (DataSet)Session["ScheduleDayData"];
            DataSet dsSchdule = (DataSet)Session["Schedule"];
            foreach (GridViewRow gvdr in this.gvResult.Rows)
            {
                if ((gvdr.Cells[2].FindControl("chkSelect") as CheckBox).Checked)
                {
                    string stockId = gvdr.Cells[0].Text;
                    string orgId = gvdr.Cells[1].Text;
                    if (!string.IsNullOrEmpty(stockId) && stockId != "&nbsp;")
                    {
                        DataRow drResult = dsSchdule.Tables[1].Select("[Stock_ID]='" + stockId + "'").FirstOrDefault();
                        DataRow drStock = dsScheduleDay.Tables[1].Select("[Stock_ID]='" + stockId + "'").FirstOrDefault();
                        if (drResult != null && drStock != null)
                        {
                            if (drResult["Is_New"].ToString() == "N" && drResult["Is_Delete"].ToString() == "N")
                            {
                                drResult["Is_Delete"] = "Y";
                            }
                            else
                            {
                                drResult["IsDummy"] = "Y";
                            }
                            drStock["Is_Selected"] = "N";
                        }
                    }
                    else if (!string.IsNullOrEmpty(orgId) && orgId != "&nbsp;")
                    {
                        DataRow drResult = dsSchdule.Tables[1].Select("[OrgStruc_Id]='" + orgId + "'").FirstOrDefault();
                        DataRow drOrg= dsScheduleDay.Tables[0].Select("[OrgStruc_Id]='" + orgId + "'").FirstOrDefault();
                        if (drResult != null && drOrg != null)
                        {
                            if (drResult["Is_New"].ToString() == "N" && drResult["Is_Delete"].ToString() == "N")
                            {
                                drResult["Is_Delete"] = "Y";
                            }
                            else
                            {
                                drResult["IsDummy"] = "Y";
                            }

                            // Green Edit 
  
                            DataRow[] rows = dsScheduleDay.Tables[0].Select("[OrgStruc_Id] = " + orgId);
                            foreach (DataRow row in rows)
                            {
                                row["Is_Selected"] = "N";
                                row.AcceptChanges();
                            }

                            // End Green Edit

                            drOrg["Is_Selected"] = "N";
                        }
                    }
                }
            }

            this.Session["Schedule"] = dsSchdule;
            this.Session["ScheduleDayData"] = dsScheduleDay;
            this.BindingScheduleDayData();

            ScrollToElement(gvResult.ClientID);
        }
        /// <summary>
        /// This method use for collect scheduly day result
        /// </summary>
        private void CollectScheduleDayResultData()
        {
            if (Session["Schedule"] != null)
            {
                DataTable dt = ((DataSet)Session["Schedule"]).Tables[1];
                foreach (GridViewRow dvr in this.gvResult.Rows)
                {
                    string stockId = dvr.Cells[0].Text;
                    string orgId = dvr.Cells[1].Text;
                    if (!string.IsNullOrEmpty(stockId) && stockId != "&nbsp;")
                    {
                        DataRow dr = dt.Select("[Stock_ID]='" + stockId + "'").FirstOrDefault();
                        dr["Distribute_Freq"] = (dvr.Cells[7].FindControl("ddFreq") as DropDownList).SelectedValue;
                        dr.AcceptChanges();
                    }
                    else if (!string.IsNullOrEmpty(orgId) && orgId != "&nbsp;")
                    {
                        DataRow dr = dt.Select("[OrgStruc_Id]='" + orgId + "'").FirstOrDefault();
                        dr["Distribute_Freq"] = (dvr.Cells[7].FindControl("ddFreq") as DropDownList).SelectedValue;
                        dr.AcceptChanges();
                    }
                }

            }
        }
        /// <summary>
        /// This method use to collect schedule result
        /// </summary>
        private void CollectScheduleResultData()
        {
            if (Session["Schedule"] != null)
            {
                DataTable dt = ((DataSet)Session["Schedule"]).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(dt.NewRow());
                }

                DataRow dr = dt.Rows[0];
                dr["Stock_ID"] = this.ddStock.SelectedValue;
                dr["Start_Date"] = this.ccDSFrom.Value;
                if (this.rdLimitToDate.Checked)
                {
                    dr["Finish_Date"] = this.ccLimitDate.Value;
                }
                else if (this.rdNoLimitToDate.Checked)
                {
                    dr["Finish_Date"] = DBNull.Value;
                }
                dr["Cancel_Status"] = this.rdScheduleDayStatus.SelectedValue == "0" ? false : true;
                dr["Update_By"] = this.UserID;
                dr["Create_By"] = this.UserID;
                dr.AcceptChanges();
            }
        }
        /// <summary>
        /// This method use for clear last data
        /// </summary>
        private void ClearLastData()
        {
            if (Session["Schedule"] != null)
            {
                DataTable dt = ((DataSet)Session["Schedule"]).Tables[1];
                DataRow[] drs = dt.Select("[Is_New]='Y'");
                foreach (DataRow dr in drs)
                {
                    dt.Rows.Remove(dr);
                }
            }
        }
        /// <summary>
        /// This method use to clear form
        /// </summary>
        private void ClearForm()
        {
            this.pnDaySelector.Visible = false;
            this.pnDSDetail.Visible = false;
            Session["Schedule"] = null;
            Session["ScheduleDayData"] = null;
            foreach (ListItem li in this.rdDateOfWeek.Items)
            {
                li.Enabled = true;
            }
            this.btnAddScheduleDay.Visible = false;
            this.lbScheduleDayText.Visible = false;
            this.gvScheduleWithBuilding.DataSource = null;
            this.gvScheduleWithBuilding.DataBind();

            // ========= Begin Green Edit =========
            tbDivCode.Text = "";
            tbDepCode.Text = "";
            tbSearchName.Text = "";
            // ========= End Green Edit =========

            Page.MaintainScrollPositionOnPostBack = false;
        }
        /// <summary>
        /// This method use for sync datasource
        /// </summary>
        private void SyncDataSource()
        {
            if (Session["ScheduleDayData"] != null)
            {
                DataSet ds = Session["ScheduleDayData"] as DataSet;
                DataTable dt = ((DataSet)Session["Schedule"]).Tables[1];

                foreach (DataRow dr in dt.Select("[IsDummy] = 'N' AND [Is_Delete]='N' AND [Is_New]='N'"))
                {
                    string orgId = dr["OrgStruc_Id"].ToString();
                    string stockId = dr["Stock_ID"].ToString();
                    if (!string.IsNullOrEmpty(orgId))
                    {
                        DataRow drOrg = ds.Tables[0].Select("[OrgStruc_Id]='" + orgId + "'").FirstOrDefault();
                        // green
                        if (drOrg == null) break;
                        // green
                        drOrg["Is_Selected"] = "Y";
                        drOrg.AcceptChanges();
                    }
                    else if (!string.IsNullOrEmpty(stockId))
                    {
                        DataRow drStock = ds.Tables[1].Select("[Stock_ID]='" + stockId + "'").FirstOrDefault();
                        // green
                        if (drStock == null) break;
                        // green
                        drStock["Is_Selected"] = "Y";
                        drStock.AcceptChanges();
                    }
                }
            }
        }
        /// <summary>
        /// This method use for set read only form
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetReadOnlyEditor(bool isReadOnly)
        {
            this.ddBuilding.Enabled = !isReadOnly;
            this.ddBuildinfFloor.Enabled = !isReadOnly;
            this.rdSearchNameType.Enabled = !isReadOnly;
            this.tbSearchName.Enabled = !isReadOnly;
            this.btnSearchDS.Enabled = !isReadOnly;
            this.rdDateOfWeek.Enabled = !isReadOnly;
        }
        /// <summary>
        /// This method use for save data
        /// </summary>
        private void Save()
        {
            try
            {
                string confirmResult = (this.Request["__EVENTARGUMENT"] == null) ? string.Empty : this.Request["__EVENTARGUMENT"];
                if (confirmResult == "true")
                {
                    DistributeDAO dDao = new DistributeDAO();
                    if (Session["Schedule"] != null)
                    {
                        this.CollectScheduleResultData();
                        this.CollectScheduleDayResultData();

                        DataSet ds = (DataSet)Session["Schedule"];
                        int saveStatus = 0;
                        if (Session["Temp_ScheduleID"] != null)
                        {
                            saveStatus = dDao.InsertOrUpdate(ds, Session["Temp_ScheduleID"].ToString(), Session["Temp_ScheduleDayID"].ToString(), Session["Temp_CreatedDate"].ToString());
                            Session["Temp_ScheduleID"] = null;
                            Session["Temp_ScheduleDayID"] = null;
                            Session["Temp_CreatedDate"] = null;
                        }
                        else
                        {
                            saveStatus = dDao.InsertOrUpdate(ds);
                        }
                        if (saveStatus == 1)
                        {
                            this.ClearForm();
                            this.BindingScheduleGrid();
                            ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('บันทึกเรียบร้อย')</script>;");
                        }
                        else if (saveStatus == -1)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('เกิดข้อผิดพลาดบางอย่างกรุณาลองใหม่อีกครัง')</script>;");
                        }
                    }
                }
                else
                {
                    Page.MaintainScrollPositionOnPostBack = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// This method use to check exits schedule day
        /// </summary>
        /// <param name="p1"></param>
        /// <returns></returns>
        private bool CheckExistScheduleDay(out string scheduleId, out string startDate)
        {
            scheduleId = string.Empty;
            startDate = string.Empty;
            if (Session["ScheduleList"] != null)
            {
                DataRow[] drs = ((DataSet)Session["Schedule"]).Tables[1].Select("[IsDummy]='N' AND [Is_Delete]='N'");
                foreach (DataRow eachRow in drs)
                {
                    DataRow drOrg = null;
                    DataRow drStock = null;
                    if (!string.IsNullOrEmpty(eachRow["OrgStruc_Id"].ToString()))
                    {
                        drOrg = ((DataView)Session["ScheduleList"])
                            .Table.Select("[Start_Date] < '#" + this.ccDSFrom.Value.ToString("dd/MM/yyyy") + "#' AND ISNULL([OrgStruc_Id], 0) = '" + eachRow["OrgStruc_Id"].ToString() + "' AND [Cancel_Status_Day] = 'True'").LastOrDefault();
                    }
                    else if (!string.IsNullOrEmpty(eachRow["Stock_ID"].ToString()))
                    {
                        drStock = ((DataView)Session["ScheduleList"])
                            .Table.Select("[Start_Date] < '#" + this.ccDSFrom.Value.ToString("dd/MM/yyyy") + "#' AND ISNULL([Stock_ID_Day], 0) = '" + eachRow["Stock_ID"].ToString() + "' AND [Cancel_Status_Day] = 'True'").LastOrDefault();
                    }


                    if (drOrg != null)
                    {
                        scheduleId = drOrg["Schedule_ID"].ToString();
                        startDate = Convert.ToDateTime(drOrg["Start_Date"]).ToString(this.DateFormat);
                        Session["Temp_ScheduleDayID"] = drOrg["Schedule_DayID"].ToString();
                        Session["Temp_ScheduleID"] = drOrg["Schedule_ID"].ToString();
                        Session["Temp_CreatedDate"] = drOrg["Create_Date"].ToString();
                        return true;
                    }
                    else if (drStock != null)
                    {
                        scheduleId = drStock["Schedule_ID"].ToString();
                        startDate = Convert.ToDateTime(drStock["Start_Date"]).ToString(this.DateFormat);
                        Session["Temp_ScheduleDayID"] = drStock["Schedule_DayID"].ToString();
                        Session["Temp_ScheduleID"] = drStock["Schedule_ID"].ToString();
                        Session["Temp_CreatedDate"] = drStock["Create_Date"].ToString();
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region [ Dropdown Selection Changed ]
        /// <summary>
        /// Building dropdown selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdBuildingSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddBuilding.SelectedIndex > -1)
            {
                this.BindingBuildingFloor();
                if (this.pnDaySelector.Visible)
                {
                    this.Session["ScheduleDayData"] = null;
                    this.ClearLastData();
                    this.BindingScheduleDayData();
                   
                }
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }
        #endregion

        #region [ Button Events ]
        /// <summary>
        /// Search Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearchClick(object sender, EventArgs e)
        {
            this.BindingScheduleGrid(true);
        }
        /// <summary>
        /// Close form button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSeachCancelClick(object sender, EventArgs e)
        {
            this.Response.Redirect("../Home/Home.aspx");
        }
        /// <summary>
        /// Create new distribute schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCreateClick(object sender, EventArgs e)
        {
            this.ClearForm();
            this.ClearLastData();
            this.BindingMandatoryData();
            this.BindingDistributeForm();
            //Page.MaintainScrollPositionOnPostBack = true;
            ScrollToElement(pnDSDetail.ClientID);
        }
    
        /// <summary>
        /// Search Schedule day
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearchDSClick(object sender, EventArgs e)
        {
            this.Session["ScheduleDayData"] = null;
            //this.ClearLastData();
            this.BindingScheduleDayData();
            //Page.MaintainScrollPositionOnPostBack = true;
            ScrollToElement(pnDSDetail.ClientID);
        }
       
        /// <summary>
        /// Push data in to result grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnInClick(object sender, EventArgs e)
        {
            this.PushIn();
        }
        /// <summary>
        /// Push data out from result grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnOutClick(object sender, EventArgs e)
        {
            this.PushOut();
        }
        /// <summary>
        /// Save Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            DataRow[] drs = ((DataSet)Session["Schedule"]).Tables[1].Select("IsDummy='N' AND [Is_Delete]='N'");
            if (drs.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('กรุณาเลือกหน่วยงาน/คลัง ที่ต้องการด้วย.')", true);
                Page.MaintainScrollPositionOnPostBack = true;
                return;
            }
            else
            {
                Session["InsertOrUpdate"] = true;
                string scheduleId = string.Empty;
                string startDate = string.Empty;
                bool isExits = this.CheckExistScheduleDay(out scheduleId, out startDate);
                string message = "คุณต้องการบันทึกหรือไม่?";
                if (isExits)
                {
                    message = "ทำการปิดจากอัตโนมัติช่วงวันที่ " + startDate + " - " + this.ccDSFrom.Value.AddDays(-1).ToString(this.DateFormat)
                        + "\\nและเปิดตารางการจ่ายใหม่สำหรับหน่วยงานที่ระบุ\\nโดยเริ่มตั้งแต่วันที่ " + this.ccDSFrom.Value.ToString(this.DateFormat) + " เป็นต้นไป";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "ConfrimationSave('" + message + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "ConfrimationSave('" + message + "');", true);
                }
            }

        }
        
        /// <summary>
        /// Cancel Schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDeleteClick(object sender, EventArgs e)
        {
            DistributeDAO dDao = new DistributeDAO();
            if (Session["Schedule"] != null)
            {
                DataSet ds = (DataSet)Session["Schedule"];
                if (ds.Tables[0].Rows.Count == 0)
                    return;

                string scheduleId = this.hfScheduleId.Value;
                int saveStatus = dDao.CancelDistributeSchedule(scheduleId);
                if (saveStatus == 1)
                {
                    this.ClearForm();
                    this.BindingScheduleGrid();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('ยกเลิกตารางจ่ายนี้เรียบร้อย')", true);
                }
                else if (saveStatus == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('เกิดข้อผิดพลาดบางอย่างกรุณาลองใหม่อีกครัง')</script>;");
                }
            }
        }
        /// <summary>
        /// Clear form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancelClick(object sender, EventArgs e)
        {
            this.ClearForm();            
        }
        /// <summary>
        /// Add schedule day Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddScheduleDayClick(object sender, EventArgs e)
        {
            if (Session["ScheduleList"] != null)
            {
                DataView dv = (DataView)Session["ScheduleList"];
                dv.RowFilter = "[Schedule_ID]='" + this.hfScheduleId.Value + "'";
                DataTable dt = dv.ToTable("tt", true, "DayNumber");

                // --------- Disable exits day number ----------
                if (dt.Rows.Count > 0)
                {
                    string[] exitsDayNumber = new string[dt.Rows.Count];
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        exitsDayNumber[i] = dt.Rows[i]["DayNumber"].ToString();
                    }

                    foreach (ListItem li in this.rdDateOfWeek.Items)
                    {
                        if (exitsDayNumber.Contains(li.Value))
                        {
                            li.Enabled = false;
                        }
                        else
                        {
                            li.Enabled = true;
                        }
                    }
                }

                this.BindingDistributeForm(Convert.ToInt32(this.hfScheduleId.Value), null, true);
            }
        }

        #endregion

        #region [ Grid Event ]
        /// <summary>
        /// Grid result data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvResultRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                bool isdummyRow = drv["IsDummy"] == "Y" ? true : false;
                DropDownList ddl = e.Row.Cells[5].FindControl("ddFreq") as DropDownList;
                ddl.DataTextField = "Freq_Desc";
                ddl.DataValueField = "Freq_Id";
                ddl.DataSource = new DistributeDAO().GetFrequencyDistribute();
                if (!isdummyRow)
                {
                    ddl.DataBind();
                    ddl.SelectedValue = drv["Distribute_Freq"].ToString();
                }
                ddl.Enabled = !isdummyRow;

                CheckBox chkSelect = e.Row.Cells[5].FindControl("chkSelect") as CheckBox;
                chkSelect.Enabled = !isdummyRow;
            }
        }
        private int _rowNumber = 1;
        private string _scheduleId = "0";
        /// <summary>
        /// Grid schedule data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvScheduleRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                switch (drv["Cancel_Status_Day"].ToString().ToLower())
                {
                    case "false": e.Row.Cells[5].Text = "<span style='color:red'>" + drv["Status_Text"] + "</span>"; break;
                    case "true": e.Row.Cells[5].Text = "<span style='color:blue'>" + drv["Status_Text"] + "</span>"; break;
                }

                if (!string.IsNullOrEmpty(drv["Org_Name"].ToString()))
                {
                    e.Row.Cells[3].Text = drv["Org_Name"].ToString();
                }
                else if (!string.IsNullOrEmpty(drv["Stock_Name"].ToString()))
                {
                    e.Row.Cells[3].Text = drv["Stock_Name"].ToString();
                }

                if (_scheduleId != drv["Schedule_Id"].ToString())
                {
                    e.Row.Cells[1].Text = "" + _rowNumber++;
                    string dateRangeText = Convert.ToDateTime(drv["Start_Date"]).ToString(this.DateFormat) + " - ";
                    if (!string.IsNullOrEmpty(drv["Finish_Date"].ToString()))
                    {
                        dateRangeText += Convert.ToDateTime(drv["Finish_Date"]).ToString(this.DateFormat);
                    }
                    else
                    {
                        dateRangeText += "ไม่มีกำหนด";
                    }
                    e.Row.Cells[2].Text = dateRangeText;
                    LinkButton btnSelect = e.Row.Cells[1].FindControl("btnSelectSchedule") as LinkButton;
                    btnSelect.CommandArgument = drv["Schedule_Id"].ToString();
                    btnSelect.Visible = true;
                    _scheduleId = drv["Schedule_Id"].ToString();
                }
                
            }
        }
        /// <summary>
        /// Grid schedule row command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvScheduleRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                this.hfScheduleId.Value = e.CommandArgument.ToString();

                if (Session["ScheduleList"] != null)
                {
                    this.pnDSDetail.Visible = false;
                    this.pnDaySelector.Visible = false;
                    DataView dv = (DataView)Session["ScheduleList"];
                    dv.RowFilter = "[Schedule_ID] = '" + e.CommandArgument + "'";
                    dv.Sort = "[DayNumber] ASC";
                    DataTable dt = dv.Table;
                    DataRow dr = dt.Select("[Schedule_ID]='" + e.CommandArgument + "'").FirstOrDefault();
                    this.btnAddScheduleDay.Visible = true;
                    this.lbScheduleDayText.Visible = true;
                    string dateRangeText = "ตารางการจ่ายช่วงวันที่ " +  Convert.ToDateTime(dr["Start_Date"]).ToString(this.DateFormat) + " - ";
                    if (!string.IsNullOrEmpty(dr["Finish_Date"].ToString()))
                    {
                        dateRangeText += Convert.ToDateTime(dr["Finish_Date"]).ToString(this.DateFormat);
                    }
                    else
                    {
                        dateRangeText += "ไม่มีกำหนด";
                    }
                    this.lbScheduleDayText.Text = dateRangeText;
                    this.gvScheduleWithBuilding.DataSource = dv;
                    this.gvScheduleWithBuilding.DataBind();

                    ScrollToElement(gvScheduleWithBuilding.ClientID);
                }

                //Page.MaintainScrollPositionOnPostBack = true;
            }
        }
        private string _dayNumber = string.Empty;
        /// <summary>
        /// Grid schedule with building data row bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvScheduleWithBuildingRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (!string.IsNullOrEmpty(drv["Org_Name"].ToString()))
                {
                    e.Row.Cells[3].Text = drv["Org_Name"].ToString();
                }
                else if (!string.IsNullOrEmpty(drv["Stock_Name"].ToString()))
                {
                    e.Row.Cells[3].Text = drv["Stock_Name"].ToString();
                }

                if (_dayNumber != drv["DayNumber"].ToString())
                {
                    e.Row.Cells[1].Text = "" + _rowNumber++;
                    e.Row.Cells[2].Text = this.GetDayOfWeekString(drv["DayNumber"].ToString());
                    LinkButton btnSelect = e.Row.Cells[1].FindControl("btnSelectScheduleDetail") as LinkButton;
                    btnSelect.CommandArgument = drv["DayNumber"].ToString();
                    btnSelect.Visible = true;
                    _dayNumber = drv["DayNumber"].ToString();
                }
            }
        }
        /// <summary>
        /// Grid schedule with building data row command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvScheduleWithBuildingRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                this.Session["Schedule"] = null;
                this.Session["ScheduleDayData"] = null;
                // hffDayNumber เก็บค่า "ลำดับ"
                this.hffDayNumber.Value = e.CommandArgument.ToString();
                this.BindingDistributeForm(
                    Convert.ToInt32(this.hfScheduleId.Value),
                    e.CommandArgument.ToString()
                );

                ScrollToElement(pnDSDetail.ClientID);
            }
        }

        protected void ScrollToElement(string elementId)
        {
            string js   = "var $element         = document.getElementById('" + elementId + "');"
                        + "var bodyRect         = document.body.getBoundingClientRect();"
                        + "var elementRect      = $element.getBoundingClientRect();"
                        + "var offset           = elementRect.top - bodyRect.top;"
                        + "window.scrollTo(0, offset);";

            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "script"
                , js
                , true
            );
        }

        #endregion
    }
}
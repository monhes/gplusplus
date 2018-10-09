using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GPlus.DataAccess
{
    public class DistributeDAO : DbConnectionBase
    {
        /// <summary>
        /// This method use to get active building
        /// </summary>
        /// <returns>building list</returns>
        public DataTable GetActiveBuilding()
        {
            return this.ExecuteDataSet("sp_Inv_Building_SelectActive").Tables[0];
        }
        /// <summary>
        /// This method use to get building floor
        /// </summary>
        /// <param name="buildingId">building id</param>
        /// <returns>building floor list</returns>
        public DataTable GetBuildingFloor(string buildingId, bool isDistinct = true)
        {
            DataTable dt = this.ExecuteDataSet("sp_Inv_Building_SelectActive").Tables[1];
            DataRow[] drs;
            if(buildingId.Trim().Length > 0)
                drs = dt.Select("Building_Id = '" + buildingId + "'");
            else
                drs = dt.Select("1 = 1");
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in drs)
            {
                newdt.ImportRow(dr);
            }
            if (isDistinct)
            {
                return newdt.DefaultView.ToTable("distinctTb", true, "Building_FloorId");
            }
            else
            {
                return newdt;
            }
        }

        public DataTable GetBuildingFloor2(string buildingId)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Building_Id", buildingId.ToString()));
            return this.ExecuteDataTable("sp_Inv_Building_Floor_Select", this.Parameters);
        }

        /// <summary>
        /// This method use to get active stock
        /// </summary>
        /// <returns></returns>
        public DataView GetActiveStock()
        {
            //LevelStk_IdReq
            DataTable dt = this.ExecuteDataTable("sp_Inv_Stock_SelectActive");
            DataView dv = new DataView(dt);
            dv.RowFilter = "[LevelStk_IdReq] IS NULL";
            return dv;
        }
        /// <summary>
        /// This method use to get search distribute status
        /// </summary>
        /// <returns>status table</returns>
        public DataTable GetSearchDistributeStatus()
        {
            string[] descs = { "ทั้งหมด", "ยกเลิก" };
            DataTable dt = new DataTable();
            dt.Columns.Add("Seacrh_DS_Id", typeof(int));
            dt.Columns.Add("Seacrh_DS_Desc", typeof(string));
            dt.AcceptChanges();
            //Fill data
            for (int i = 0; i < descs.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Seacrh_DS_Id"] = i;
                dr["Seacrh_DS_Desc"] = descs[i];
                dt.Rows.Add(dr);
            }

            return dt;
        }
        /// <summary>
        /// This method use to get frequency distribute
        /// </summary>
        /// <returns>frequency table</returns>
        public DataTable GetFrequencyDistribute()
        {
            string[] descs = { "ทุกสัปดาห์", "สัปดาห์ที่ 1 ของเดือน", "สัปดาห์ที่ 2 ของเดือน", "สัปดาห์ที่ 3 ของเดือน", "สัปดาห์ที่ 4 ของเดือน" };
            DataTable dt = new DataTable();
            dt.Columns.Add("Freq_Id", typeof(int));
            dt.Columns.Add("Freq_Desc", typeof(string));
            dt.AcceptChanges();
            //Fill data
            for (int i = 0; i < descs.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Freq_Id"] = i;
                dr["Freq_Desc"] = descs[i];
                dt.Rows.Add(dr);
            }

            return dt;
        }
        /// <summary>
        /// This method use to get schedule 
        /// </summary>
        /// <param name="scheduleId">schedule Id</param>
        /// <returns>schedule dataset</returns>
        public DataSet GetSchedule(int scheduleId = 0, bool isNewSchduleDay = false)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Schedule_ID", scheduleId));
            if (isNewSchduleDay)
            {
                this.Parameters.Add(Parameter("@Is_NewScheduleDay", "Y"));
            }
            else
            {
                this.Parameters.Add(Parameter("@Is_NewScheduleDay", "N"));
            }
            return this.ExecuteDataSet("sp_Inv_Schedule_Stock_Select", this.Parameters);
        }
        /// <summary>
        /// This method use to get schedule by day
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="dayNumber"></param>
        /// <returns></returns>
        public DataTable GetScheduleDay(string scheduleId, string dayNumber)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Schedule_ID", scheduleId));
            this.Parameters.Add(Parameter("@DayNumber", dayNumber));
            return this.ExecuteDataTable("sp_Inv_Schedule_Day_Select", this.Parameters);
        }
        /// <summary>
        /// This method use to get schedule day result data
        /// </summary>
        /// <param name="buidingId">building id</param>
        /// <param name="buildingFloor">building floor</param>
        /// <param name="mainStockId">Main stock id</param>
        /// <param name="searchName">Search name</param>
        /// <returns>schedule day result</returns>
        public DataSet GetScheduleDayData(
            string buidingId, 
            string buildingFloor, 
            string mainStockId, 
            string searchName = null, 
            string searchType = null,
            string searchDiv = null,
            string searchDep = null)
        {
            DataSet ds = new DataSet();
            DataTable dt = GetBuildingFloor(buidingId, false);
            dt.Columns.Add("Is_Selected", typeof(string));
            dt.AcceptChanges();

            //DataRow[] drs = dt.Select(" 1 = 1 ");
            //if(buildingFloor.Trim().Length > 0)
            //    dt.Select("[Building_FloorId] = '" + buildingFloor + "'");
                

            // Nin Edit
            String rowFilter = " 1 = 1 ";

            if (buildingFloor.Trim().Length > 0)
                rowFilter += " AND [Building_FloorId] = '" + buildingFloor + "'";

            if (searchName.Trim().Length > 0)
                rowFilter += " AND [Description] like '%" + searchName + "%'";

            DataRow[] drs = dt.Select(rowFilter);

            // End Nin Edit

            DataTable newDt1 = dt.Clone();

            foreach (DataRow dr in drs)
            {
                dr["Is_Selected"] = "N";
                dr.AcceptChanges();
                newDt1.ImportRow(dr);
            }

            DataTable dtStock = this.ExecuteDataTable("sp_Inv_Stock_SelectActive");
            dtStock.Columns.Add("Is_Selected", typeof(string));
            dtStock.AcceptChanges();

            DataRow[] drsStock = dtStock.Select("[LevelStk_IdReq] = '" + mainStockId + "'");
            DataTable newDt2 = dtStock.Clone();

            foreach (DataRow dr in drsStock)
            {
                dr["Is_Selected"] = "N";
                dr.AcceptChanges();
                newDt2.ImportRow(dr);
            }

            ds.Tables.Add(newDt1);
            ds.Tables.Add(newDt2);
            return ds;
        }
        /// <summary>
        /// This method use to save data or update data to database
        /// </summary>
        /// <param name="ds">data set result</param>
        /// <returns>save status</returns>
        public int InsertOrUpdate(DataSet ds, string updateScheduleId = null, string updateScheduleDayId = null, string createddate = null)
        {
            try
            {
                if (ds.Tables[0].Rows.Count == 0 || ds.Tables[1].Select("[Is_Delete]='N'").Length == 0) 
                    return -1;
                this.UseTransaction = true;
                this.BeginParameter();
                DataRow drSchedule = ds.Tables[0].Rows[0];
                int scheduleId = 0;
                if (!string.IsNullOrEmpty(drSchedule["Schedule_ID"].ToString()))
                {
                    scheduleId = Convert.ToInt32(drSchedule["Schedule_ID"]);
                }
                this.Parameters.Add(Parameter("@Schedule_ID", scheduleId));
                this.Parameters.Add(Parameter("@Stock_ID", drSchedule["Stock_ID"]));
                this.Parameters.Add(Parameter("@Start_Date", drSchedule["Start_Date"]));
                this.Parameters.Add(Parameter("@Finish_Date", drSchedule["Finish_Date"]));
                this.Parameters.Add(Parameter("@Cancel_Status", drSchedule["Cancel_Status"]));
                if (updateScheduleId != null)
                {
                    DateTime datetime = Convert.ToDateTime(createddate);
                    this.Parameters.Add(Parameter("@Create_Date", datetime));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Create_Date", DateTime.Now));
                }
                this.Parameters.Add(Parameter("@Create_By", drSchedule["Create_By"]));
                this.Parameters.Add(Parameter("@Update_By", drSchedule["Update_By"]));
                DataTable dtResult = this.ExecuteDataTable("sp_Inv_Schedule_Stock_InsertOrUpdate", this.Parameters);
                scheduleId = Convert.ToInt32(dtResult.Rows[0]["Schedule_ID"].ToString());

                DataRow[] drs = ds.Tables[1].Select("[IsDummy] = 'N'");
                foreach (DataRow dr in drs)
                {
                    this.BeginParameter();
                    this.Parameters.Add(Parameter("@Schedule_DayID", string.IsNullOrEmpty(dr["Schedule_DayID"].ToString()) ? 0 : Convert.ToInt32(dr["Schedule_DayID"])));
                    this.Parameters.Add(Parameter("@Schedule_ID", scheduleId));
                    this.Parameters.Add(Parameter("@DayNumber", dr["DayNumber"]));
                    this.Parameters.Add(Parameter("@OrgStruc_Id", dr["OrgStruc_Id"]));
                    this.Parameters.Add(Parameter("@Stock_ID", dr["Stock_ID"]));
                    this.Parameters.Add(Parameter("@Distribute_Freq", dr["Distribute_Freq"]));
                    this.Parameters.Add(Parameter("@Cancel_Status", dr["Cancel_Status"]));
                    this.Parameters.Add(Parameter("@Is_Delete", dr["Is_Delete"]));
                    this.Parameters.Add(Parameter("@Building_Id", dr["Building_Id"]));
                    this.Parameters.Add(Parameter("@Building_FloorId", dr["Building_FloorId"]));
                    this.ExecuteNonQuery("sp_Inv_Schedule_Day_InsertOrUpdate", this.Parameters);
                }

                if (updateScheduleId != null)
                {
                    this.BeginParameter();
                    
                    this.Parameters.Add(Parameter("@Schedule_ID", updateScheduleId));
                    this.Parameters.Add(Parameter("@Schedule_ID2", scheduleId));
                    this.Parameters.Add(Parameter("@Schedule_DayID", updateScheduleDayId));
                    //this.Parameters.Add(Parameter("@Stock_ID", updateStockId));
                    this.Parameters.Add(Parameter("@End_Date", Convert.ToDateTime(drSchedule["Start_Date"]).AddDays(-1)));
                    this.ExecuteNonQuery("sp_Inv_Schedule_Day_ChangeDay", this.Parameters);
                }

                this.CommitTransaction();// Request commit transaction

                return 1;
            }
            catch (Exception)
            {
                this.CommitTransaction();
                return -1;
            }
            
        }
        /// <summary>
        /// This method use to cancel schedule
        /// </summary>
        /// <param name="scheduleId">Schedule Id</param>
        /// <returns></returns>
        public int CancelDistributeSchedule(string scheduleId)
        {
            try
            {
                if (string.IsNullOrEmpty(scheduleId)) return -1;
                this.UseTransaction = true;
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Schedule_Id", Convert.ToInt32(scheduleId)));
                this.ExecuteNonQuery("sp_Inv_Schedule_Stock_Cancel", this.Parameters);
                this.CommitTransaction();
                return 1;
            }
            catch (Exception)
            {
                this.CommitTransaction();
                return -1;
            }
        }
        /// <summary>
        /// This method use for get schedule search list
        /// </summary>
        /// <param name="stockId">Master Stock Id</param>
        /// <param name="status">Schedule Status</param>
        /// <param name="from">Schedule date from</param>
        /// <param name="to">Schedule date to</param>
        /// <returns></returns>
        public DataView GetScheduleList(int stockId = 0, int status = 0, string from = null, string to = null)
        {
            DataTable dt = this.ExecuteDataTable("sp_Inv_Schedule_Stock_SelectList");
            DataView dv = new DataView(dt);
            string filter = string.Empty;
            if (stockId != 0)
            {
                filter += "[Stock_ID] = '" + stockId + "'";
            }

            //Request No
            if (status != 0)
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " AND ";
                }
                filter += "[Cancel_Status_Day] = 'False'";
            }

            if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(to))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " AND ";
                }

                if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                {
                    DateTime dtfrom = Convert.ToDateTime(from);
                    dtfrom = dtfrom.AddYears(543);
                    filter += "[Start_Date] >= '#" + dtfrom.ToString("dd/MM/yyyy") + "#'";
                }
                else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    DateTime dtTo = Convert.ToDateTime(to);
                    dtTo = dtTo.AddYears(543);
                    filter += "[Start_Date] <= '#" + dtTo.ToString("dd/MM/yyyy") + "#'";
                }
                else
                {
                    DateTime dtfrom = Convert.ToDateTime(from);
                    dtfrom = dtfrom.AddYears(543);
                    DateTime dtTo = Convert.ToDateTime(to);
                    dtTo = dtTo.AddYears(543);
                    filter += "[Start_Date] >= '#" + dtfrom.ToString("dd/MM/yyyy") + "#' AND " + "[Start_Date] <= '#" + dtTo.ToString("dd/MM/yyyy") + "#'";
                }
            }
            dv.RowFilter = filter;
            return dv;
        }
    }
}
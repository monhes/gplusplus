using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.Web.Configuration;
using System.Diagnostics;
using System.IO;
using System.Data.OleDb;


namespace GPlus.Request
{
    public partial class frmRequest : Pagebase
    {

        private RequestDAO _reqeustDAO;
        protected void Page_Load(object sender, EventArgs e)
        {

            //tbQty.Attributes.Add("onKeyUp", "if (window.event.keyCode == 13){document.getElementById('" + btnAddItem.ClientID + "').click();}return NumberBoxKeyUp(event, 0, 46, false);");
            //tbItemCode.Attributes.Add("onKeyUp", "if (window.event.keyCode == 13){if(this.value.length > 0)document.getElementById('" + btnFileItem.ClientID + "').click();else { alert('กรุณาระบุรหัสสินค้า');this.focus(); }}");
            //tbQty.Attributes.Add("onKeyUp", "if (event.keyCode == 13){document.getElementById('" + btnAddItem.ClientID + "').click();}return NumberBoxKeyUp(event, 0, 46, false);");
            //tbItemCode.Attributes.Add("onKeyUp", "if (event.keyCode == 13){if(this.value.length > 0)document.getElementById('" + btnFileItem.ClientID + "').click();else { alert('กรุณาระบุรหัสสินค้า');this.focus(); }}");
            if (!IsPostBack)
            {
                if (Session["Item"] != null)
                {
                    Session["Item"] = null;
                }
                if (Session["RequestItem"] != null)
                {
                    Session["RequestItem"] = null;
                }


                Session["Show_receive"] = "false";
                Session["Request_More3"] = "false";
                Session["request_id"] = "";
                Session["pay_id"] = "";
                Session["Can_Save"] = "false";
                bool isNotApprove = false;
                DataTable dt = new DataAccess.OrgStructureDAO().GetOrgStructure(this.OrgID);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["NotApprove_Flag"].ToString().Trim().Length > 0)
                        isNotApprove = dt.Rows[0]["NotApprove_Flag"].ToString() == "1";
                }

                Session["isNotApprove"] = isNotApprove;
                if (this.Request["approv"] != null)
                {
                    rdbIsWait.Visible = true;
                    ddlStatus.Visible = true;
                    lbConsider.Visible = true;
                    this.PrepareToSetApproveForm();
                    lb.Visible = true;
                }
                else
                {
                    lbConsider.Visible = false;
                    rdbIsWait.Visible = false;
                    ddlStatus.Visible = false;
                }
                this.PageID = "501";
                this._reqeustDAO = new RequestDAO();
                this.BindingMandatoryData();
                this.BindingGridRequestList();
                divNotApprove.Visible = false;
                //Nin Add หากเป็นฝ่ายธุรการ ให้ Enable ให้ คีย์วันที่เบิกได้
                //DataTable dtOrg = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(this.OrgID));
                //if (dtOrg.Rows.Count > 0)
                //{
                //    if (dtOrg.Rows[0]["Div_Code"].ToString() == "3050")
                //    {
                //        this.tbRequestDate.Enabled = true;
                //        orgCtrl.setSelectBtnEnable(); //ถ้าเป็นฝ่ายธุรการสามารถเลือกฝ่ายอื่นๆได้
                //        divNotApprove.Visible = true;
                //    }
                //    else
                //    {
                //        orgCtrl.setSelectBtnDisable();
                //        divNotApprove.Visible = false;
                //    }
                //}

                //Nin Add หากเป็น User Groupid = 4,7 คีย์วันที่เบิกได้
                DataTable dtGroupUser = new DataAccess.UserDAO().GetUserGroupUser(UserID);

                DataRow[] rows = dtGroupUser.Select("UserGroup_ID IN (4,7)");
                if (rows.Length > 0)
                {
                    this.tbRequestDate.Enabled = true;
                    orgCtrl.setSelectBtnEnable();
                    divNotApprove.Visible = true;
                }
                else
                {
                    this.tbRequestDate.Enabled = false;
                    orgCtrl.setSelectBtnDisable();
                    divNotApprove.Visible = false;
                }
            }
            else
            {
                this.SetNewReqeustEmp();
                this.SetNewItem();

                // Green Edit 
                if (rdbIsWait.SelectedValue == "0")
                {
                    ddlStatus.Enabled = true;
                }

                else if (rdbIsWait.SelectedValue == "1")
                {
                    ddlStatus.SelectedValue = "";
                    ddlStatus.Enabled = false;
                }
                else if (rdbIsWait.SelectedValue == "2")
                {
                    ddlStatus.SelectedValue = "";
                    ddlStatus.Enabled = false;
                }
                // Green Edit


            }
            pagingControlReqList.CurrentPageIndexChanged += new EventHandler(pagingControlReqList_CurrentPageIndexChanged);
            Page.MaintainScrollPositionOnPostBack = true;
        }

        void pagingControlReqList_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            this.BindingGridRequestList();
        }
        /// <summary>
        /// This method use to set approve form
        /// </summary>
        private void PrepareToSetApproveForm()
        {
            this.btnAddItem.Visible = false;
            this.btnAddNewRequest.Visible = false;
            this.btnCancelForm.Visible = false;
            //this.btnDelete.Visible = false;
            this.btnDelItem.Visible = false;
            this.btnFindItem.Visible = false;
            this.btnFindRequestName.Visible = false;
            //this.btnPrint.Visible = false;
            //this.btnReset.Visible = false;
            this.btnSave.Visible = false;
            this.gvRequestItem.Enabled = false;
            this.rblDeptType.Enabled = false;
            this.rdlRequestType.Enabled = false;
        }
        /// <summary>
        /// This method use to binding request list
        /// </summary>
        private void BindingGridRequestList()
        {
            string status = "";

            if (this.Request["approv"] != null)
            {
                if (rdbIsWait.SelectedValue == "1")
                {
                    status = "NULL";
                    lb.Text = "วันที่เบิก";
                }
                else if (rdbIsWait.SelectedValue == "2")
                {
                    status = "P";
                    lb.Text = "วันที่จ่าย";
                }
                else
                {
                    status = ddlStatus.SelectedValue;
                    lb.Text = "วันที่เบิก";
                }

            }
            else
            {
                status = "A";
                lb.Text = "วันที่เบิก";
            }

            //if (this.Request["approv"] != null)
            //{
            //    if (rdbIsWait.SelectedIndex == 0)
            //        status = "'1'";
            //    else
            //        status = "'0','2','3','4','5','6'";
            //}
            //else if (ddlStatus.SelectedIndex > 0)
            //    status = "'" + ddlStatus.SelectedValue + "'";

            //DataSet ds = new RequestDAO().GetRequestList(this.OrgID
            //    , status
            //    , this.txtRequestNo.Text
            //    , this.txtName.Text
            //    , this.ccForm.Text
            //    , this.ccTo.Text, pagingControlReqList.CurrentPageIndex, pagingControlReqList.PageSize, this.SortColumn, this.SortOrder);

            //Nin EDIT 
            DataSet ds = new RequestDAO().GetRequestList(
                orgCtrl.OrgStructID
                , status
                , this.txtRequestNo.Text
                , this.txtName.Text
                , this.ccForm.Text
                , this.ccTo.Text, pagingControlReqList.CurrentPageIndex, pagingControlReqList.PageSize, this.SortColumn, this.SortOrder);
            //End Nin EDIT
            pagingControlReqList.RecordCount = (int)ds.Tables[1].Rows[0][0];

            Session["RequestList"] = ds.Tables[0];
            this.gvRequestList.DataSource = ds.Tables[0];
            this.gvRequestList.DataBind();
        }

        /// <summary>
        /// This method use to binding first time
        /// </summary>
        private void BindingMandatoryData()
        {
            //DataTable dtt2 = this._reqeustDAO.GetDepartmentDetail(this.OrgID);
            DataTable dtt = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(this.OrgID));
            if (dtt != null)
            {
                if (dtt.Rows.Count > 0)
                {
                    //this.txtDeptName.Text = dtt.Rows[0]["Description"].ToString();
                    orgCtrl.OrgStructID = this.OrgID;
                    orgCtrl.DivName = dtt.Rows[0]["DivName"].ToString();
                    orgCtrl.DepName = dtt.Rows[0]["DepName"].ToString();
                    orgCtrl.DivCode = dtt.Rows[0]["Div_Code"].ToString();
                    orgCtrl.DepCode = dtt.Rows[0]["Dep_Code"].ToString();
                }
                Session["Department"] = dtt;
            }

            //this.ddlStatus.DataSource = this._reqeustDAO.GetRequestStatus();
            //this.ddlStatus.DataBind();
        }
        /// <summary>
        /// This method use to binding request form
        /// </summary>
        /// <param name="requestId"></param>
        private void BindingRequestForm(int requestId, bool can_receive)
        {
            divConsiderReason.Visible = false; //Default ไม่ต้องแสดงส่วนเหตุผลการพิจารณา

            this.pnDetail.Visible = true; // Show detail panel

            //หาก request_status > 3 (can_receive = true) สามารถทำการรับของได้ 
            if (can_receive)
            {
                //ค้นหาใน StockPay ว่ามี RequestId นี้หรือไม่ ถ้ามี ให้ซ่อนส่วน AddItem
                DataTable dtStkPay = new RequestDAO().GetStockPay(requestId.ToString());

                if (dtStkPay.Rows.Count > 0)
                {
                    //เลือก row ที่เป็น สถานะรอรับมา 1 อัน เพื่อ get ค่า pay_id เอามาเป็น ครั้งที่ทำการรับ default
                    DataRow drPayWaitRec = dtStkPay.Select("[Rec_Status]='รอรับ'").FirstOrDefault();
                    if (drPayWaitRec != null)
                    {
                        default_payId = drPayWaitRec["Pay_Id"].ToString();
                        receive_no = drPayWaitRec["row_num"].ToString();
                        have_Waitreceive = true;
                    }

                    this.gvStockPay.DataSource = dtStkPay;
                    this.gvStockPay.DataBind();
                    this.gvStockPay.Visible = true;
                    pnlEnt.Visible = false; // Hide Panel Add Item
                    Session["Show_receive"] = "true";
                }
                else
                {
                    this.gvStockPay.Visible = false;
                    pnlEnt.Visible = true; // Show Panel Add Item
                    Session["Show_receive"] = "false";
                }
            }
            else
            {
                this.gvStockPay.Visible = false;
                pnlEnt.Visible = true;
            }

            DataTable dtRequest = null;
            DataTable dtRequestItem = null;
            DataRow dr = null;
            if (requestId == 0) // ในกรณีที่ทำการเพิ่มวัสดุ-อุปกรณ์
            {
                btnPrint.Visible = false;
                //Create temp table for request uer
                dtRequest = new RequestDAO().GetRequest();
                dtRequestItem = new RequestDAO().GetRequestItem(requestId, default_payId);
                this.hdRequestId.Value = null;
                this.rblDeptType.SelectedValue = "0"; //selected ฝ่าย
                this.rdlRequestType.SelectedValue = "0"; //selected รอบ
                this.tbRequestDate.Text = DateTime.Now.ToString(this.DateFormat);
                this.tbRequestNo.Text = string.Empty;
                this.tbCreatedBy.Text = string.Empty;
                this.tbCreatedDate.Text = string.Empty;
                this.tbUpdatedBy.Text = string.Empty;
                this.tbUpdatedDate.Text = string.Empty;

                dr = dtRequest.NewRow();
                dr["Request_Type"] = "0";
                dr["Account_Id"] = this.UserID;
                dr["Account_Fname"] = this.FirstName;
                dr["Account_Lname"] = this.LastName;
                DataTable dtDept = null;
                if (Session["Department"] != null)
                {
                    //dtDept = (DataTable)Session["Department"];
                    //dr["Div_Code"] = dtDept.Rows[0]["Div_Code"];
                    //dr["Dep_Code"] = dtDept.Rows[0]["Dep_Code"];
                    //dr["Description"] = dtDept.Rows[0]["Description"];

                    //if (!string.IsNullOrEmpty(dr["Dep_Code"].ToString()))
                    //{
                    //    this.txtDivName.Text = dr["Description"].ToString();
                    //}
                    //else
                    //{
                    //    this.txtDepName.Text = dr["Description"].ToString();
                    //}

                    #region Nin Edit 15082013

                    dr["Div_Code"] = orgCtrl.DivCode;
                    dr["Dep_Code"] = orgCtrl.DepCode;

                    if (!string.IsNullOrEmpty(orgCtrl.DepName.ToString()))
                    {
                        dr["Description"] = orgCtrl.DepName;
                    }
                    else
                    {
                        dr["Description"] = orgCtrl.DivName;
                    }

                    if (!string.IsNullOrEmpty(orgCtrl.DepCode.ToString()))
                    {
                        this.txtDepName.Text = orgCtrl.DepName;
                    }
                    else
                    {
                        this.txtDepName.Text = "";
                    }
                    if (!string.IsNullOrEmpty(orgCtrl.DivCode.ToString()))
                    {
                        this.txtDivName.Text = orgCtrl.DivName;
                    }
                    else
                    {
                        this.txtDivName.Text = "";
                    }

                    #endregion
                }
                DataRow drEmployee = new RequestDAO().GetEmployeeDetail(this.UserID, this.OrgID);
                if (drEmployee != null)
                {

                    //this.tbStockName.Text = drEmployee["Stock_Name"].ToString();
                    //dr["Stock_Id_From"] = drEmployee["Stock_Id"];


                    //Nin Edit 18082013
                    DataTable dtStockFrom = new OrgStructureDAO().GetOrgStk(orgCtrl.OrgStructID);
                    DataTable dtStockName; //ถ้าในกรณีที่ OrgID ที่เลือกมาไม่ได้กำหนด Stock_ID ใน Inv_OrgStk จะทำการ default stock_id = 1 ให้
                    if (dtStockFrom.Rows.Count > 0)
                    {
                        dtStockName = new StockDAO().GetStock(dtStockFrom.Rows[0]["Stock_ID"].ToString());
                    }
                    else
                    {
                        dtStockName = new StockDAO().GetStock("1");
                    }
                    this.tbStockName.Text = dtStockName.Rows[0]["Stock_Name"].ToString();
                    dr["Stock_Id_From"] = dtStockFrom.Rows[0]["Stock_ID"].ToString();
                    dr["Stock_Id_Req"] = drEmployee["Stock_Id"];
                }

                //if((bool)Session["isNotApprove"])
                //    dr["Request_Status"] = 2;
                //else
                //    dr["Request_Status"] = 1;

                bool getIsNotApprove = false;

                DataTable dt = new DataAccess.OrgStructureDAO().GetOrgStructure(orgCtrl.OrgStructID);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["NotApprove_Flag"].ToString().Trim().Length > 0)
                        getIsNotApprove = dt.Rows[0]["NotApprove_Flag"].ToString() == "1";
                }

                if (getIsNotApprove)
                    dr["Request_Status"] = 2;
                else
                    dr["Request_Status"] = 1;

                dr["Created_By"] = this.UserID;
                dr["Updated_By"] = this.UserID;
                dr["Request_Date"] = DateTime.Now;
                //dr["OrgStruc_Id_Req"] = this.OrgID; 
                dr["OrgStruc_Id_Req"] = orgCtrl.OrgStructID; // Nin Edit
                dtRequest.Rows.Add(dr);
                this.tbRequestName.Text = this.FirstName + " " + this.LastName;
                this.ClearForm();
            }
            else //ในกรณีที่ทำการกดดูรายละเอียด
            {
                btnPrint.Visible = true;
                dtRequest = new RequestDAO().GetRequest(requestId);
                dtRequestItem = new RequestDAO().GetRequestItem(requestId, default_payId);

                if (dtRequest.Rows.Count > 0)
                {
                    try
                    {
                        string fullname = dtRequest.Rows[0]["Account_Fname"].ToString() + " " + dtRequest.Rows[0]["Account_Lname"].ToString();
                        if (!string.IsNullOrEmpty(fullname.Trim()))
                        {
                            this.tbRequestName.Text = fullname;
                        }
                        else
                        {
                            this.tbRequestName.Text = dtRequest.Rows[0]["Request_Name2"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    dr = dtRequest.Rows[0];
                    if (this.Request["approv"] != null)
                    {
                        this.pnApproval.Visible = true; //show apprve panel
                        //if (!string.IsNullOrEmpty(dr["Consider_Id"].ToString()))
                        if ((!string.IsNullOrEmpty(dr["Consider_Type"].ToString())) && !(dr["Consider_Type"].ToString() == "1"))
                        {
                            //Consider_By_Name
                            try
                            {
                                this.tbConsiderDate.Text = Convert.ToDateTime(dr["Consider_Date"]).ToString(this.DateTimeFormat);
                            }
                            catch (Exception) { tbConsiderDate.Text = ""; }
                            this.tbConsiderReason.Text = dr["Consider_Reason"].ToString();
                            this.rblConsiderTypes.SelectedValue = dr["Consider_Type"].ToString();
                            this.tbConsiderBy.Text = dr["Consider_By_Name"].ToString();
                            //this.tbConsiderReason.Enabled = false;
                            this.tbConsiderReason.Enabled = true;
                            this.btnApprove.Visible = false;
                            this.btnCancelApprove.Visible = false;
                        }
                        else
                        {
                            this.tbConsiderDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
                            this.tbConsiderReason.Text = string.Empty;
                            this.rblConsiderTypes.SelectedValue = "1";
                            this.tbConsiderBy.Text = this.FirstName + " " + this.LastName;
                            this.tbConsiderReason.Enabled = true;
                            this.btnApprove.Visible = true;
                            this.btnCancelApprove.Visible = true;
                        }

                        // Green Edit

                        string considerType = dr["Consider_Type"].ToString();
                        if (considerType == null || considerType == "")
                        {
                            rblConsiderTypes.ClearSelection();
                        }
                        else
                        {
                            rblConsiderTypes.SelectedValue = considerType;
                        }

                        btnCancelApprove.Visible = true;
                        btnCancelApprove.Enabled = true;

                        string requestStatus = dtRequest.Rows[0]["Request_Status"].ToString();

                        if (requestStatus == "1" || requestStatus == "2" || (considerType == "0" && requestStatus == "0"))
                        {
                            btnApprove.Visible = true;
                            btnApprove.Enabled = true;
                        }

                        // Green Edit

                        ScriptManager.RegisterStartupScript(
                            this,
                            GetType(),
                            "ScrollDown",
                            "$('html,body').animate({scrollTop: $(window).scrollTop() + 700})",
                            true);

                    }
                    else
                    {
                        this.pnApproval.Visible = false;
                    }

                    //Add 30092013
                    // ในกรณีที่สถานะเป็นส่งกลับไปแก้ไข Consider_Type =1 ให้แสดงดหตุผล
                    if (this.Request["approv"] == null && dr["Consider_Type"].ToString() == "1")
                    {
                        tbShowConsiderReason.Text = dr["Consider_Reason"].ToString();
                        divConsiderReason.Visible = true;
                    }
                    else
                    {
                        divConsiderReason.Visible = false;
                    }

                    this.rblDeptType.SelectedValue = "0"; //selected ฝ่าย
                    this.rdlRequestType.SelectedValue = dr["Request_Type"].ToString(); //selected รอบ
                    this.tbRequestDate.Text = Convert.ToDateTime(dr["Request_Date"]).ToString(this.DateFormat);
                    this.tbRequestNo.Text = dr["Request_No"].ToString();
                    this.tbCreatedBy.Text = dr["Created_By_Name"].ToString();
                    this.tbCreatedDate.Text = Convert.ToDateTime(dr["Created_Date"]).ToString(this.DateTimeFormat);
                    this.tbUpdatedBy.Text = dr["Updated_By_Name"].ToString();
                    if (!string.IsNullOrEmpty(dr["Updated_Date"].ToString()))
                    {
                        this.tbUpdatedDate.Text = Convert.ToDateTime(dr["Updated_Date"]).ToString(this.DateTimeFormat);
                    }
                    else
                    {
                        this.tbUpdatedDate.Text = string.Empty;
                    }

                    DataTable dtDiv = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(dr["OrgStruc_Id_Req"].ToString()));
                    if (dtDiv != null)
                    {
                        if (dtDiv.Rows.Count > 0)
                        {
                            this.txtDivName.Text = dtDiv.Rows[0]["DivName"].ToString();
                            this.txtDepName.Text = dtDiv.Rows[0]["DepName"].ToString();
                        }
                    }

                    //if (!string.IsNullOrEmpty(dr["Dep_Code"].ToString()))
                    //{
                    //    this.txtDivName.Text = dr["Description"].ToString();
                    //}
                    //else
                    //{
                    //    this.txtDepName.Text = dr["Description"].ToString();
                    //}

                    //DataRow drEmployee = new RequestDAO().GetEmployeeDetail(dr["Request_By"].ToString(), dr["OrgStruc_Id_Req"].ToString());
                    //if (drEmployee != null)
                    //{
                    //    this.tbStockName.Text = drEmployee["Stock_Name"].ToString();
                    //}

                    //Nin Edit 18082013
                    if (dr["Stock_Id_From"].ToString() != "")
                    {
                        DataTable dtStockName = new StockDAO().GetStock(dr["Stock_Id_From"].ToString());
                        this.tbStockName.Text = dtStockName.Rows[0]["Stock_Name"].ToString();
                    }

                    //dr["Created_By"] = this.UserID;
                    dr["Updated_By"] = this.UserID;
                    dr.AcceptChanges();
                }
            }


            this.tbQty.Text = "";

            //collect to session
            Session["RequestItem"] = dtRequestItem;
            Session["Request"] = dtRequest;

            this.BindingRequestItem();
        }

        /// <summary>
        /// This method use to set binding request item
        /// </summary>
        private void BindingRequestItem()
        {
            if (Session["RequestItem"] != null)
            {
                DataTable dtt = Session["RequestItem"] as DataTable;

                if (dtt.Rows.Count > 0)
                {
                    Session["request_id"] = Convert.ToInt32((dtt.Rows[0]["Request_Id"].ToString() == "" ? "0" : dtt.Rows[0]["Request_Id"].ToString()));
                    Session["pay_id"] = Convert.ToInt32((dtt.Rows[0]["Pay_Id"].ToString() == "" ? "0" : dtt.Rows[0]["Pay_Id"].ToString()));
                }

                DataView dv = new DataView(dtt);
                dv.RowFilter = "[Is_Delete] = 'N'";
                this.CalculateOrderAmount(dtt);
                if (dv.Count > 0)
                {
                    panelDelItem.Visible = true;
                    pnTotalOrder.Visible = true;
                }
                else
                {
                    panelDelItem.Visible = false;
                    pnTotalOrder.Visible = false;
                }

                if (Session["Show_receive"].ToString() == "true")
                {

                    gvRequestItem.Columns[0].Visible = false; // Hide chkbox
                    gvRequestItem.Columns[9].Visible = true;
                    gvRequestItem.Columns[10].Visible = true;
                }
                else
                {
                    if (this.Request["approv"] == null) //ถ้าเป็นผู้อนุมัติต้อง hide chkbox
                    {
                        gvRequestItem.Columns[0].Visible = true; // Show chkbox
                    }
                    else
                    {
                        gvRequestItem.Columns[0].Visible = false;
                    }
                    gvRequestItem.Columns[9].Visible = false;
                    gvRequestItem.Columns[10].Visible = false;
                    gvRequestItem.Enabled = true;

                }

                //check ว่าหาก status > 3 และ มีสถานะเป็นรอรับให้แสดงเฉพาะปุ่มรอรับเท่านั้น ไม่แสดงปุ่มยกเลิกการรับ
                if (Session["Show_receive"].ToString() == "true" && have_Waitreceive)
                {
                    BtnRec.Visible = true;
                    BtnCancleRec.Visible = false;
                    gvRequestItem.Enabled = true;
                }
                else if (Session["Show_receive"].ToString() == "true" && have_Waitreceive == false)
                {
                    BtnRec.Visible = false;
                    BtnCancleRec.Visible = true;
                    gvRequestItem.Enabled = false;
                }
                else if (Session["Show_receive"].ToString() == "false")
                {
                    BtnRec.Visible = false;
                    BtnCancleRec.Visible = false;
                }

                this.gvRequestItem.DataSource = dv;
                this.gvRequestItem.DataBind();
                _rowCount = 1;


            }
        }
        /// <summary>
        /// This method use to calculate order total amount
        /// </summary>
        /// <param name="dtt"></param>
        private void CalculateOrderAmount(DataTable dtt)
        {
            DataRow[] drs = dtt.Select("[Is_Delete] = 'N'");
            decimal result = 0m;
            try
            {
                foreach (DataRow dr in drs)
                {
                    result += Convert.ToDecimal(dr["Order_Amount"]);
                }
            }
            catch (Exception ex)
            {

            }

            this.txtTotalOrder.Text = result.ToString("#,##0.0000");
        }
        /// <summary>
        /// Set new request employee
        /// </summary>
        private void SetNewReqeustEmp()
        {
            if (Session["Request"] != null)
            {
                DataTable dtt = Session["Request"] as DataTable;
                if (dtt.Rows.Count > 0)
                {
                    string fullname = dtt.Rows[0]["Account_Fname"].ToString() + " " + dtt.Rows[0]["Account_Lname"].ToString();
                    if (!string.IsNullOrEmpty(fullname.Trim()))
                    {
                        this.tbRequestName.Text = fullname;
                    }
                    else
                    {
                        this.tbRequestName.Text = dtt.Rows[0]["Request_Name2"].ToString();
                    }
                    //Nin Add 14082013
                    //this.txtDivName.Text = dtt.Rows[0]["Description"].ToString();
                    //this.txtDepName.Text = "";
                    //End Nin Add
                }
            }
        }
        /// <summary>
        /// This method use for set new Item
        /// </summary>
        private void SetNewItem()
        {
            if (Session["Item"] != null)
            {
                if (Session["ItemQty"] != null)
                {
                    if (tbQty.Text.Trim().Length == 0)
                        this.tbQty.Text = Session["ItemQty"].ToString();
                }
                DataRow drItem = (DataRow)Session["Item"];
                DataRow drItemDetail = new RequestDAO().GetItemDetail(drItem["Inv_ItemID"].ToString());
                if (drItemDetail != null)
                {
                    this.tbItemCode.Text = drItemDetail["Inv_ItemCode"].ToString();
                    this.tbItemName.Text = drItemDetail["Inv_ItemName"].ToString();
                    this.tbPricePerUnit.Text = drItemDetail["Avg_Cost"].ToString();
                    //this.tbUnitName.Text = drItemDetail["Description"].ToString();
                    this.tbUnitName.Text = drItemDetail["Pack_Description"].ToString();
                    //this.tbTotalPrice.Text = (Convert.ToDecimal(this.tbPricePerUnit.Text) * Convert.ToInt32(this.tbQty.Text)).ToString("#,##0.0000");
                    this.tbTotalPrice.Text = (Convert.ToDecimal(this.tbPricePerUnit.Text == "" ? "0" : this.tbPricePerUnit.Text) * (Convert.ToInt32((this.tbQty.Text == "" ? "0" : this.tbQty.Text)))).ToString("#,##0.0000");

                    //int i = Convert.ToInt32((this.tbQty.Text == "" ? "0" : this.tbQty.Text));
                }
                else
                {
                    this.tbItemCode.Text = string.Empty;
                    this.tbItemName.Text = string.Empty;
                    this.tbPricePerUnit.Text = string.Empty;
                    this.tbUnitName.Text = string.Empty;
                    this.tbTotalPrice.Text = string.Empty;
                    if (tbQty.Text.Trim().Length == 0)
                        this.tbQty.Text = "";
                    Session["Item"] = null;
                }
            }
        }

        #region [ Button Event Click ]
        /// <summary>
        /// Cancel button from search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnCancelClick(object sender, EventArgs e)
        {
            this.Response.Redirect("../Request/frmRequest.aspx?approv=true");
        }
        /// <summary>
        /// Search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnSearchClick(object sender, EventArgs e)
        {
            this.pnDetail.Visible = false;
            this.BindingGridRequestList();
        }
        /// <summary>
        /// New Reqeust Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddNewRequestClick(object sender, EventArgs e)
        {
            //DataTable dtOrg = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(this.OrgID));
            //if (dtOrg.Rows.Count > 0)
            //{
            //    if (dtOrg.Rows[0]["Div_Code"].ToString() == "3050")
            //    {
            //        divNotApprove.Visible = true;
            //        cb_NotApproveFlag.Checked = true;
            //    }
            //    else
            //    {
            //        divNotApprove.Visible = false;
            //    }
            //}

            //Nin Add หากเป็น User Groupid = 4,7 สามารถติ๊กเลือกการเบิกโดยไม่ต้องอนุมัติได้
            DataTable dtGroupUser = new DataAccess.UserDAO().GetUserGroupUser(UserID);

            DataRow[] rows = dtGroupUser.Select("UserGroup_ID IN (4,7)");
            if (rows.Length > 0)
            {
                divNotApprove.Visible = true;
                cb_NotApproveFlag.Checked = true;
            }
            else
            {
                divNotApprove.Visible = false;
            }

            divImport.Visible = true;
            string getOrgID = orgCtrl.OrgStructID;

            //หาใบเบิกของหน่วยงานนี้ที่มีการจ่ายสินค้าจากคลังแล้วแต่ยังไม่ได้ทำรับ ถ้ามีบางใบยังไม่ได้ทำรับให้ Popup เตือน
            // DataTable dt = new DataAccess.RequestDAO().CheckRequestReceive(this.OrgID);
            DataTable dt = new DataAccess.RequestDAO().CheckRequestReceive(orgCtrl.OrgStructID);
            String AllNotReceive = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AllNotReceive = AllNotReceive + dt.Rows[i]["Request_No"];
                    }
                    else
                    {
                        AllNotReceive = AllNotReceive + " ," + dt.Rows[i]["Request_No"];
                    }

                }
                ShowMessageBox("ไม่สามารถทำการเบิกวัสดุ – อุปกรณ์ได้ เนื่องจากยังไม่มีการทำรับ เลขที่ใบเบิก ( " + AllNotReceive + " )");
                pnDetail.Visible = false;
                return;
            }
            Session["Can_Save"] = "True";
            Session["Show_receive"] = "false";
            Session["Request_More3"] = "false";
            this.ReadOnlyForm(false);
            this.BindingRequestForm(0, false);
            //ClientScript.RegisterStartupScript(this.GetType(), "Scroll", "<script>$('body,html').animate({ scrollTop: $('body').height() }, 600);</script>;");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'> $('html, body').animate({ scrollTop: 550 }, 'slow');</script>", false);
        }
        /// <summary>
        /// Find Reqeust Name Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnFindRequestNameClick(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>OpenEmployeePopup('" + this.OrgID + "')</script>;");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "OpenEmployeePopup('" + this.OrgID + "')", true);

        }
        /// <summary>
        /// Save Request Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (Session["Request"] != null && Session["RequestItem"] != null)
            {


                /*
                    ให้ตรวจสอบหน่วยงานใน master ว่าเบิกโดยไม่ต้องอนุมัติ หรือไม่
                    - กรณีที่ เบิกโดยไม่ต้องอนุมัติ = 1 ถ้า Request_Status = '1','2' สามารถแก้ไขได้
                    - กรณีที่ เบิกโดยไม่ต้องอนุมัติ = 0 ถ้า Request_Status = '1' สามารถแก้ไขได้
                */

                bool getIsNotApprove = false;
                string request_status = "";
                DataTable dt = Session["Request"] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Request_Id"].ToString() != "") //ไม่ได้บันทึกครั้งแรก
                    {
                        //ทำการดึงข้อมูล request ล่าสุดมา
                        DataTable dtRequestLatest = new RequestDAO().GetRequest(dt.Rows[0]["Request_Id"].ToString());
                        if (dtRequestLatest.Rows.Count > 0)
                        {
                            DataTable dtApprove = new DataAccess.OrgStructureDAO().GetOrgStructure(dtRequestLatest.Rows[0]["OrgStruc_Id_Req"].ToString());
                            request_status = dtRequestLatest.Rows[0]["Request_Status"].ToString();
                            if (dtApprove.Rows.Count > 0)
                            {
                                if (dtApprove.Rows[0]["NotApprove_Flag"].ToString().Trim().Length > 0)
                                    getIsNotApprove = dtApprove.Rows[0]["NotApprove_Flag"].ToString() == "1";
                            }

                            if (getIsNotApprove)
                            {
                                if ((request_status != "") && (request_status != "1") && (request_status != "2"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('ไม่สามารถบันทึกรายการเบิกนี้ได้');", true);
                                    this.ClearForm();
                                    this.BindingGridRequestList();
                                    this.pnDetail.Visible = false;
                                    return;
                                }
                            }
                            else
                            {
                                if ((request_status != "") && (request_status != "1"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('ไม่สามารถบันทึกรายการเบิกนี้ได้');", true);
                                    this.ClearForm();
                                    this.BindingGridRequestList();
                                    this.pnDetail.Visible = false;
                                    return;
                                }
                            }
                        }
                    }
                }




                this.UpdateSomeData();
                DataTable dtRequest = Session["Request"] as DataTable;
                DataTable dtReqItem = Session["RequestItem"] as DataTable;
                //Nin Edit 23082013

                //int i = 0;

                foreach (GridViewRow row in gvRequestItem.Rows)
                {
                    TextBox txt_OrderQty = (TextBox)row.FindControl("txt_OrderQty");
                    TextBox txt_Comment = (TextBox)row.FindControl("txt_Comment");
                    HiddenField hdRemarkOrg = (HiddenField)row.FindControl("hdRemarkOrg");
                    HiddenField hdInvItemID = (HiddenField)row.FindControl("hdInvItemID");
                    HiddenField hdPackID = (HiddenField)row.FindControl("hdPackID");

                    //if (txt_OrderQty.Text == "" || txt_OrderQty.Text == "0")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "compIt", "alert('กรุณาใส่จำนวนที่เบิกค่ามากกว่า 0');", true);
                    //    txt_OrderQty.Focus();
                    //    return;
                    //}

                    //dtReqItem.Rows[i]["Order_Quantity"] = txt_OrderQty.Text;
                    //dtReqItem.Rows[i]["Remark"] = hdRemarkOrg.Value;
                    //dtReqItem.Rows[i].AcceptChanges();
                    //i++;

                    /*Nin Edit 09042014*/
                    DataRow dr = dtReqItem.Select("[Inv_ItemID]='" + hdInvItemID.Value + "' AND [Pack_ID]='" + hdPackID.Value + "'").FirstOrDefault();
                    if (dr != null)
                    {
                        dr["Order_Quantity"] = txt_OrderQty.Text;
                        dr["Remark"] = hdRemarkOrg.Value;
                        dr.AcceptChanges();
                    }

                    /*End Nin Edit 09042014*/

                }

                //End for

                //End Nin Edit

                RequestDAO req = new RequestDAO();
                int reqId = req.InsertOrUpdate(dtRequest, dtReqItem);
                if (reqId != -1)
                {
                    DataTable dtReq = req.GetRequest(reqId);
                    string reqNo = "";
                    if (dtReq.Rows.Count > 0)
                    {
                        reqNo = dtReq.Rows[0]["Request_No"].ToString();
                    }
                    string str = "alert('บันทึกใบเบิกวัสดุ-อุปกรณ์เรียบร้อยแล้ว \\n เลขที่ใบเบิก : " + reqNo + "')"; // "alert('บันทึกใบเบิกวัสดุ-อุปกรณ์เรียบร้อยแล้ว \\n เลขที่ใบเบิก : ');"
                    //ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('บันทึกใบเบิกวัสดุ-อุปกรณ์เรียบร้อยแล้ว')</script>;");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", str, true);
                    this.hdRequestId.Value = reqId.ToString();
                    this.ClearForm();
                    this.BindingGridRequestList();
                    this.pnDetail.Visible = false;
                    //this.BindingReqeustForm(reqId);
                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('กรุณาเลือกวัสดุ-อุปกรณ์ที่ต้องการเบิกอย่างน้อยหนึ่งรายการ')</script>;");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('กรุณาเลือกวัสดุ-อุปกรณ์ที่ต้องการเบิกอย่างน้อยหนึ่งรายการ');", true);
                }
            }

        }
        /// <summary>
        /// Update some mandatory field
        /// </summary>
        private void UpdateSomeData()
        {
            DataTable dt = Session["Request"] as DataTable;
            if (this.Request["approv"] != null)
            {
                dt.Rows[0]["Consider_Type"] = this.rblConsiderTypes.SelectedValue;
                dt.Rows[0]["Consider_Reason"] = this.tbConsiderReason.Text;
                dt.Rows[0]["Consider_Id"] = Convert.ToInt32(this.UserID);
                dt.Rows[0]["Consider_Date"] = DateTime.Now;

                dt.Rows[0]["Request_Status"] = this.rblConsiderTypes.SelectedValue;
            }
            else
            {
                //Nin Edit

                /* 
                   ในกรณีที่ทำการเบิกใหม่ 
                   เพิ่ม ในส่วนที่ถ้า Login หากเป็น User Groupid = 4,7
                   ให้ทำการ check ตรงส่วน ติ๊กเลือก การเบิกโดยไม่ต้องอนุมัติแทน
                */

                if (dt.Rows[0]["Request_Id"].ToString() == "") //กรณีที่ทำการเบิกใหม่
                {

                    DataTable dtGroupUser = new DataAccess.UserDAO().GetUserGroupUser(UserID);

                    DataRow[] rows = dtGroupUser.Select("UserGroup_ID IN (4,7)");
                    if (rows.Length > 0)
                    {
                        bool getIsNotApprove = cb_NotApproveFlag.Checked;
                        if (getIsNotApprove)
                        {
                            dt.Rows[0]["Request_Status"] = 2;
                        }
                        else
                        {
                            dt.Rows[0]["Request_Status"] = 1;
                        }
                    }
                }

                /* ถ้าสถานะก่อนหน้าเป็นส่งกลับมาแก้ไข ในการแก้ไขและบันทึกใหม่ 
                  ให้เปลี่ยน Consider_Type เป็น null และ  
                  Request_Status = 1 ถ้าเป็นเบิกโดยต้องอนุมัติ หรือ Request_Status = 2 ถ้าเป็นเบิกโดยไม่ต้องอนุมัติ*/
                if (dt.Rows[0]["Consider_Type"].ToString() == "1")
                {
                    dt.Rows[0]["Consider_Type"] = DBNull.Value;

                    bool getIsNotApprove = false;

                    DataTable dtApprove = new DataAccess.OrgStructureDAO().GetOrgStructure(dt.Rows[0]["OrgStruc_Id_Req"].ToString());
                    if (dtApprove.Rows.Count > 0)
                    {
                        if (dtApprove.Rows[0]["NotApprove_Flag"].ToString().Trim().Length > 0)
                            getIsNotApprove = dtApprove.Rows[0]["NotApprove_Flag"].ToString() == "1";
                    }

                    if (getIsNotApprove)
                        dt.Rows[0]["Request_Status"] = 2;
                    else
                        dt.Rows[0]["Request_Status"] = 1;
                }


            } //End Else (ไม่ใช่ กรณี Approve)

            dt.Rows[0]["Request_Date"] = this.tbRequestDate.Text;

            dt.Rows[0]["Request_Type"] = this.rdlRequestType.SelectedValue;
            dt.Rows[0]["Stock_Id_Req"] = DBNull.Value;
            dt.Rows[0].AcceptChanges();
        }
        /// <summary>
        /// Delete Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDeleteClick(object sender, EventArgs e)
        {
            if (this.hdRequestId.Value != null)
            {
                RequestDAO req = new RequestDAO();
                int cancelStatus = req.CancelRequest(this.hdRequestId.Value);
                if (cancelStatus == 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('ยกเลิกใบเบิกนี้เรียบร้อย')</script>;");
                    this.hdRequestId.Value = null;
                    this.ClearForm();
                    this.BindingGridRequestList();
                    this.pnDetail.Visible = false;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('เกิดขอผิดพลาดบางอย่าง กรุณาลองใหม่อีกครั้ง')</script>;");
                }
            }
        }
        /// <summary>
        /// Reset Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnResetClick(object sender, EventArgs e)
        {
            this.ClearForm();
            if (this.hdRequestId.Value == null)
            {
                this.BindingRequestForm(0, false);
            }
            else
            {
                //this.BindingRequestForm(Convert.ToInt32(this.hdRequestId.Value),false);
                // Green Edit
                pnApproval.Visible = false;
                pnDetail.Visible = false;
                // Green Edit
            }
        }
        /// <summary>
        /// Print Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnPrintClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Add new item button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddItemClick(object sender, EventArgs e)
        {

            if (Session["Item"] != null)
            {
                if (Session["RequestItem"] != null)
                {
                    DataTable dtt = Session["RequestItem"] as DataTable;
                    DataRow newItem = Session["Item"] as DataRow;
                    DataRow drOldItem = dtt.Select("[Inv_ItemCode]='" + newItem["Inv_ItemCode"] + "'").FirstOrDefault();

                    //ทำการวนลูปเพื่อ update item ที่เลือกแล้วใน Gridview gvRequestItem
                    if (dtt.Rows.Count > 0)
                    {
                        //int i = 0;

                        foreach (GridViewRow row in gvRequestItem.Rows)
                        {
                            TextBox txt_OrderQty = (TextBox)row.FindControl("txt_OrderQty");
                            TextBox txt_Comment = (TextBox)row.FindControl("txt_Comment");
                            HiddenField hdRemarkOrg = (HiddenField)row.FindControl("hdRemarkOrg");
                            HiddenField hdInvItemID = (HiddenField)row.FindControl("hdInvItemID");
                            HiddenField hdPackID = (HiddenField)row.FindControl("hdPackID");

                            //if (txt_OrderQty.Text == "" || txt_OrderQty.Text == "0")
                            //{
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "compIt", "alert('กรุณาใส่จำนวนที่เบิกค่ามากกว่า 0');", true);
                            //    txt_OrderQty.Focus();
                            //    return;
                            //}

                            //dtt.Rows[i]["Order_Quantity"] = txt_OrderQty.Text;
                            //dtt.Rows[i]["Remark"] = hdRemarkOrg.Value;
                            //dtt.Rows[i].AcceptChanges();
                            //i++;

                            /*Nin Edit 09042014*/
                            DataRow dr = dtt.Select("[Inv_ItemID]='" + hdInvItemID.Value + "' AND [Pack_ID]='" + hdPackID.Value + "'").FirstOrDefault();
                            if (dr != null)
                            {
                                dr["Order_Quantity"] = txt_OrderQty.Text;
                                dr["Remark"] = hdRemarkOrg.Value;
                                dr.AcceptChanges();
                            }
                            /*End Nin Edit 09042014*/
                        }

                    }
                    if (drOldItem == null)
                    {
                        DataRow drRow = dtt.NewRow();
                        drRow["Inv_ItemID"] = newItem["Inv_ItemID"];
                        drRow["Inv_ItemCode"] = newItem["Inv_ItemCode"];
                        drRow["Inv_ItemName"] = newItem["Inv_ItemName"];
                        drRow["Pack_ID"] = new RequestDAO().GetItemDetail(newItem["Inv_ItemID"].ToString())["Pack_ID"].ToString();
                        drRow["Description"] = this.tbUnitName.Text;
                        drRow["Avg_Cost"] = this.tbTotalPrice.Text;
                        //drRow["Order_Quantity"] = Convert.ToInt32(tbQty.Text);
                        drRow["Order_Quantity"] = Convert.ToInt32((this.tbQty.Text == "" ? "0" : this.tbQty.Text));
                        drRow["Pay_Qty"] = 0;
                        drRow["Receive_Qty"] = 0;
                        drRow["Req_ItemStatus"] = 0;
                        drRow["Is_New"] = "Y";
                        drRow["Is_Delete"] = "N";
                        drRow["Is_Update"] = "N";
                        drRow["Order_Amount"] = Convert.ToDecimal(drRow["Avg_Cost"]) * Convert.ToInt32(drRow["Order_Quantity"]);
                        dtt.Rows.Add(drRow);
                    }
                    else
                    {

                        //drOldItem["Order_Quantity"] = Convert.ToInt32(drOldItem["Order_Quantity"]) + Convert.ToInt32(tbQty.Text);
                        //drOldItem["Order_Amount"] = Convert.ToDecimal(drOldItem["Avg_Cost"]) * Convert.ToInt32(drOldItem["Order_Quantity"]);
                        //if (drOldItem["Is_New"].ToString() == "N")
                        //{
                        //    drOldItem["Is_Update"] = "Y";
                        //}
                        //drOldItem.AcceptChanges();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('รายการสินค้านี้ถูกเลือกในตารางแล้ว');", true);
                        tbItemCode.Focus();
                        this.tbItemCode.Text = string.Empty;
                        this.tbItemName.Text = string.Empty;
                        this.tbPricePerUnit.Text = string.Empty;
                        this.tbQty.Text = "";
                        this.tbUnitName.Text = string.Empty;
                        this.tbTotalPrice.Text = string.Empty;
                        Session["Item"] = null;
                        Session["ItemQty"] = null;
                        return;

                    }

                    Session["RequestItem"] = dtt;
                    this.BindingRequestItem();

                }
            }
            //if (this.tbQty.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('Test!');", true);
            //    tbQty.Focus();

            //}
            Session["Item"] = null;
            Session["ItemQty"] = null;
            tbItemCode.Focus();
            ClearForm();
        }

        /// <summary>
        /// Find item Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFindItemClick(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>OpenItemCategoryPopup();</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWin", "OpenItemCategoryPopup()", true);
        }
        /// <summary>
        /// Cancel new request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancelFormClick(object sender, EventArgs e)
        {
            new DataAccess.RequestDAO().CancelRequest(hdRequestId.Value);
            this.pnDetail.Visible = false;
            this.ClearForm();
        }
        /// <summary>
        /// Delete item button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDelItemClick(object sender, EventArgs e)
        {
            if (this.gvRequestItem.Rows.Count > 0)
            {
                DataTable dt = ((Session["RequestItem"]) as DataTable);

                foreach (GridViewRow row in gvRequestItem.Rows)
                {
                    TextBox txt_OrderQty = (TextBox)row.FindControl("txt_OrderQty");
                    TextBox txt_Comment = (TextBox)row.FindControl("txt_Comment");
                    HiddenField hdRemarkOrg = (HiddenField)row.FindControl("hdRemarkOrg");
                    HiddenField hdInvItemID = (HiddenField)row.FindControl("hdInvItemID");
                    HiddenField hdPackID = (HiddenField)row.FindControl("hdPackID");
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                    /*Nin Edit 09042014*/
                    DataRow dr = dt.Select("[Inv_ItemID]='" + hdInvItemID.Value + "' AND [Pack_ID]='" + hdPackID.Value + "'").FirstOrDefault();

                    if (dr != null)
                    {
                        dr["Order_Quantity"] = txt_OrderQty.Text;
                        dr["Remark"] = hdRemarkOrg.Value;
                        dr.AcceptChanges();


                        if (chkSelect.Checked == true)
                        {
                            if (dr["Is_New"].ToString() == "Y")
                            {
                                dt.Rows.Remove(dr);
                            }
                            else
                            {
                                dr["Is_Delete"] = "Y";
                                dr["Req_ItemStatus"] = 0;
                                dr.AcceptChanges();
                            }

                        }
                    }

                    /*End Nin Edit 09042014*/
                }

                Session["RequestItem"] = dt;
                this.BindingRequestItem();

                //foreach (GridViewRow gvdr in this.gvRequestItem.Rows)
                //{
                //    if ((gvdr.Cells[0].FindControl("chkSelect") as CheckBox).Checked)
                //    {
                //        string itemCode = gvdr.Cells[2].Text;
                //        DataTable dt = ((Session["RequestItem"]) as DataTable);
                //        DataRow dr = dt.Select("[Inv_ItemCode]='" + itemCode + "'").FirstOrDefault();
                //        if (dr != null)
                //        {
                //            if (dr["Is_New"].ToString() == "Y")
                //            {
                //                dt.Rows.Remove(dr);
                //            }
                //            else
                //            {
                //                dr["Is_Delete"] = "Y";
                //                dr["Req_ItemStatus"] = 0;
                //                dr.AcceptChanges();
                //            }
                //        }

                //        Session["RequestItem"] = dt;
                //        this.BindingRequestItem();
                //    }
                //}
            }
        }
        /// <summary>
        /// Approve button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnApproveClick(object sender, EventArgs e)
        {
            if (Session["Request"] != null && Session["RequestItem"] != null)
            {
                this.UpdateSomeData();
                DataTable dtRequest = Session["Request"] as DataTable;
                DataTable dtReqItem = Session["RequestItem"] as DataTable;
                RequestDAO req = new RequestDAO();
                int reqId = req.InsertOrUpdate(dtRequest, dtReqItem);
                if (reqId != -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('ทำการบันทึกเรียบร้อยแล้ว')</script>;");
                    this.hdRequestId.Value = reqId.ToString();
                    this.ClearForm();
                    this.BindingGridRequestList();
                    this.pnDetail.Visible = false;
                    this.pnApproval.Visible = false;
                    //this.BindingReqeustForm(reqId);
                }
            }
        }
        /// <summary>
        /// Clear form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancelApproveClick(object sender, EventArgs e)
        {
            //this.tbConsiderReason.Text = string.Empty;
            //this.rblConsiderTypes.SelectedValue = "1";

            // Green Edit
            pnApproval.Visible = false;
            pnDetail.Visible = false;
            // Green Edit
        }
        #endregion

        private int _rowCount = 1;
        protected void GvRequestItemRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt_RecQty = (TextBox)e.Row.FindControl("txt_RecQty");
                TextBox txt_OrderQty = (TextBox)e.Row.FindControl("txt_OrderQty");
                TextBox txt_Comment = (TextBox)e.Row.FindControl("txt_Comment");
                TextBox txt_PayQty = (TextBox)e.Row.FindControl("txt_PayQty");
                HiddenField hdRemarkOrg = (HiddenField)e.Row.FindControl("hdRemarkOrg");
                HiddenField hdRemarkStock = (HiddenField)e.Row.FindControl("hdRemarkStock");
                HiddenField hdInvItemID = (HiddenField)e.Row.FindControl("hdInvItemID");
                HiddenField hdPackID = (HiddenField)e.Row.FindControl("hdPackID");
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[1].Text = (_rowCount++).ToString();
                e.Row.Cells[11].Text = (Convert.ToDecimal(drv["Order_Amount"])).ToString("#,###.0000");
                DataTable dtStatus = new RequestDAO().GetRequestStatus();

                hdInvItemID.Value = drv["Inv_ItemID"].ToString();
                hdPackID.Value = drv["Pack_ID"].ToString();

                string chk_enable = "";
                //ถ้าเป็นผู้อนุมัติให้ Disable comment
                //if (this.Request["approv"] == null)
                //{
                //    chk_enable = "t";
                //}
                //else
                //{
                //    chk_enable = "f";
                //}

                //if (Session["Show_receive"].ToString() == "true")
                //{
                //    chk_enable = "f";
                //}
                //else
                //{
                //    chk_enable = "t";
                //}

                //if (Session["Show_receive"].ToString() == "true" && have_Waitreceive == false)
                //{
                //    chk_enable = "f";
                //}
                //else
                //{
                //    chk_enable = "t";
                //}

                if (Session["Can_Save"].ToString() == "True")
                {
                    chk_enable = "t";
                }
                else
                {
                    chk_enable = "f";
                }

                //check ว่าหาก status > 3 และ มีสถานะเป็นรอรับให้แสดงเฉพาะปุ่มรอรับเท่านั้น ไม่แสดงปุ่มยกเลิกการรับ
                if (Session["Show_receive"].ToString() == "true" && have_Waitreceive)
                {
                    //ถ้าเป็นผู้อนุมัติให้ Disable comment
                    if (this.Request["approv"] == null)
                    {
                        txt_Comment.Enabled = true;
                    }
                    else
                    {
                        txt_Comment.Enabled = false;
                    }
                }
                else
                {
                    txt_Comment.Enabled = false;
                }

                hdRemarkOrg.Value = drv["Remark"].ToString() == "" ? "" : drv["Remark"].ToString();

                hdRemarkStock.Value = drv["Pay_Remark"].ToString() == "" ? "" : drv["Pay_Remark"].ToString();

                string show_remarkOrg = drv["Remark"].ToString().Length > 5 ? drv["Remark"].ToString().Substring(0, 5) + "..." : drv["Remark"].ToString().Length == 0 ? "หมายเหตุ" : drv["Remark"].ToString();

                string show_remarkStock = drv["Pay_Remark"].ToString().Length > 5 ? drv["Pay_Remark"].ToString().Substring(0, 5) + "..." : drv["Pay_Remark"].ToString().Length == 0 ? "หมายเหตุ" : drv["Pay_Remark"].ToString();

                e.Row.Cells[14].Text = "<a href=\"javascript:open_popup('pop_Remark.aspx?ctl=" + hdRemarkOrg.ClientID + "&chk=" + chk_enable + "&type=" + "org"
                                          + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">" + show_remarkOrg + "</a>";

                e.Row.Cells[15].Text = "<a href=\"javascript:open_popup('pop_Remark.aspx?ctl=" + hdRemarkStock.ClientID + "&chk=" + "f" + "&type=" + "stk"
                                          + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">" + show_remarkStock + "</a>";

                txt_RecQty.Attributes.Add("onchange", "txt_RecQtyChange('" + txt_RecQty.ClientID + "', '" + txt_PayQty.ClientID + "', '" + txt_Comment.ClientID + "');");

                switch (drv["Req_ItemStatus"].ToString())
                {
                    case "0": e.Row.Cells[12].Text = "รอจ่าย"; break;
                    case "1": e.Row.Cells[12].Text = "ค้างจ่าย"; break;
                    case "2": e.Row.Cells[12].Text = "จ่ายไม่ครบ(แต่ปิดการจ่าย)"; break;
                    case "3": e.Row.Cells[12].Text = "จ่ายครบ"; break;
                }


                if (Session["Request_More3"].ToString() == "false")
                {
                    //ถ้าเป็นผู้อนุมัติให้ Disable ปุ่มคีย์เบิก
                    if (this.Request["approv"] == null)
                    {
                        txt_OrderQty.Enabled = true;
                    }
                    else
                    {
                        txt_OrderQty.Enabled = false;
                    }
                }
                else
                {
                    txt_OrderQty.Enabled = false;
                }




                ////ถ้าเป็นผู้อนุมัติให้ Disable comment
                //if (this.Request["approv"] == null)
                //{
                //    txt_Comment.Enabled = true;
                //}
                //else
                //{
                //    txt_Comment.Enabled = false;
                //}


                if (Session["Show_receive"].ToString() == "true" && have_Waitreceive)
                {
                    txt_RecQty.Text = "";
                    txt_Comment.Text = "";
                }

                //ถ้าจำนวนที่จ่ายเป็น 0 หรือไม่ได้จ่ายมา ให้ Disbale ช่องทำการรับ

                //if (drv["data_pay_qty"].ToString() == "" || drv["data_pay_qty"].ToString() == "0" || drv["data_pay_qty"].ToString() == "0.0000" || drv["data_pay_qty"].ToString() == ".0000")
                if (Convert.ToDecimal(drv["data_pay_qty"].ToString() == "" ? "0" : drv["data_pay_qty"].ToString()) == 0)
                {
                    txt_RecQty.Enabled = false;
                }
                else
                {
                    txt_RecQty.Enabled = true;
                }

            }
        }

        protected void GvRequestListRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                divImport.Visible = false;
                divNotApprove.Visible = false;

                bool can_recive = false;
                DataTable dt = Session["RequestList"] as DataTable;
                DataRow dr = dt.Select("[Request_Id]='" + e.CommandArgument + "'").FirstOrDefault();
                if (dr != null)
                {
                    int reqStatus = Convert.ToInt16(dr["Request_Status"].ToString() == "" ? "0" : dr["Request_Status"].ToString());


                    //if ((reqStatus == 1 || reqStatus == 2))
                    //{
                    //    Session["Can_Save"] = "True";
                    //}
                    //else
                    //{
                    //    Session["Can_Save"] = "False";
                    //}

                    /*
                        ให้ตรวจสอบหน่วยงานใน master ว่าเบิกโดยไม่ต้องอนุมัติ หรือไม่
                        - กรณีที่ เบิกโดยไม่ต้องอนุมัติ = 1 ถ้า Request_Status = '1','2' สามารถแก้ไขได้
                        - กรณีที่ เบิกโดยไม่ต้องอนุมัติ = 0 ถ้า Request_Status = '1' สามารถแก้ไขได้
                    */
                    bool getIsNotApprove = false;

                    DataTable dtApprove = new DataAccess.OrgStructureDAO().GetOrgStructure(dr["OrgStruc_Id_Req"].ToString());
                    if (dtApprove.Rows.Count > 0)
                    {
                        if (dtApprove.Rows[0]["NotApprove_Flag"].ToString().Trim().Length > 0)
                            getIsNotApprove = dtApprove.Rows[0]["NotApprove_Flag"].ToString() == "1";
                    }
                    if (getIsNotApprove)  //เบิกโดยไม่ต้องอนุมัติ = 1
                    {
                        if ((reqStatus == 1 || reqStatus == 2))
                        {
                            Session["Can_Save"] = "True";
                        }
                        else
                        {
                            Session["Can_Save"] = "False";
                        }
                    }
                    else //เบิกโดยไม่ต้องอนุมัติ = 0
                    {
                        if (reqStatus == 1)
                        {
                            Session["Can_Save"] = "True";
                        }
                        else
                        {
                            Session["Can_Save"] = "False";
                        }
                    }



                    if (Convert.ToInt16(dr["Request_Status"].ToString()) > 3)
                    {
                        this.ReadOnlyForm(true);
                        can_recive = true;
                        Session["Request_More3"] = "true";
                    }
                    else
                    {
                        this.ReadOnlyForm(false);
                        can_recive = false;
                        Session["Show_receive"] = "false";
                        Session["Request_More3"] = "false";
                    }
                }

                this.hdRequestId.Value = e.CommandArgument.ToString();

                btnPrint.OnClientClick = "open_popup('RequestReport.aspx?id=" + this.hdRequestId.Value
                + "', 800, 600, 'pop', 'yes', 'yes', 'yes'); return false;";
                this.ClearForm();
                this.BindingRequestForm(Convert.ToInt32(e.CommandArgument), can_recive);
            }
        }

        private int _rowCount2 = 1;
        protected void GvRequestListRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                //e.Row.Cells[1].Text = (_rowCount2++).ToString();
                e.Row.Cells[1].Text = (((pagingControlReqList.CurrentPageIndex - 1) * pagingControlReqList.PageSize) + _rowCount2++).ToString();
                e.Row.Cells[3].Text = Convert.ToDateTime(drv["Request_Date"]).ToString(this.DateFormat);

                GetRequestStatus GetReq = new GetRequestStatus();

                string req_status = GetReq.GetReqStatus(drv["Request_Status"].ToString(), drv["Consider_Type"].ToString(), drv["Request_Id"].ToString());
                switch (drv["Request_Status"].ToString())
                {
                    case "0": e.Row.Cells[9].Text = "<span style='color:red'>" + req_status + "</span>"; break;
                    case "1": e.Row.Cells[9].Text = "<span style='color:green'>" + req_status + "</span>"; break;
                    case "2": e.Row.Cells[9].Text = "<span style='color:blue'>" + req_status + "</span>"; break;
                    case "3": e.Row.Cells[9].Text = "<span style='color:blue'>" + req_status + "</span>"; break;
                    case "4": e.Row.Cells[9].Text = "<span style='color:blue'>" + req_status + "</span>"; break;
                    case "5": e.Row.Cells[9].Text = "<span style='color:blue'>" + req_status + "</span>"; break;
                    case "6": e.Row.Cells[9].Text = "<span style='color:blue'>" + req_status + "</span>"; break;
                }

                //switch (drv["Request_Status"].ToString())
                //{
                //    case "0": e.Row.Cells[9].Text = "<span style='color:red'>ยกเลิกการเบิก</span>"; break;
                //    case "1": e.Row.Cells[9].Text = "<span style='color:green'>รออนุมัติเบิก</span>"; break;
                //    case "2": e.Row.Cells[9].Text = "<span style='color:blue'>รอจ่าย</span>"; break;
                //    case "3": e.Row.Cells[9].Text = "<span style='color:blue'>พิมพ์สรุปจ่าย</span>"; break;
                //    case "4": e.Row.Cells[9].Text = "<span style='color:blue'>Allocated</span>"; break;
                //    case "5": e.Row.Cells[9].Text = "<span style='color:blue'>ค้างจ่าย</span>"; break;
                //    case "6": e.Row.Cells[9].Text = "<span style='color:blue'>จ่ายเรียบร้อยแล้ว</span>"; break;
                //}

                if (drv["OrgStruc_Id_Req"].ToString() != "")
                {
                    string orgid = drv["OrgStruc_Id_Req"].ToString();
                    DataTable dtOrg = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(orgid == "" ? "0" : orgid));

                    if (dtOrg.Rows.Count > 0)
                    {
                        if (dtOrg.Rows[0]["DivName"].ToString() != "")
                        {
                            e.Row.Cells[4].Text = dtOrg.Rows[0]["DivName"].ToString();
                        }
                        else
                        {
                            e.Row.Cells[4].Text = "";
                        }
                        if (dtOrg.Rows[0]["DepName"].ToString() != "")
                        {
                            e.Row.Cells[5].Text = dtOrg.Rows[0]["DepName"].ToString();
                        }
                        else
                        {
                            e.Row.Cells[5].Text = "";
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(drv["Dep_Code"].ToString()))
                //{
                //    e.Row.Cells[5].Text = drv["Description"].ToString();
                //}
                //else
                //{
                //    e.Row.Cells[4].Text = drv["Description"].ToString();
                //}
                LinkButton linkButton = e.Row.Cells[0].FindControl("btnSelect") as LinkButton;
                linkButton.CommandArgument = drv["Request_Id"].ToString();

                ImageButton btnPrint = (ImageButton)e.Row.FindControl("btnPrint");
                btnPrint.OnClientClick = "open_popup('RequestReport.aspx?id=" + drv["Request_Id"].ToString()
                + "', 800, 600, 'pop', 'yes', 'yes', 'yes'); return false;";
            }
        }
        /// <summary>
        /// This method use to clear form
        /// </summary>
        private void ClearForm()
        {
            this.tbItemCode.Text = string.Empty;
            this.tbItemName.Text = string.Empty;
            this.tbPricePerUnit.Text = string.Empty;
            this.tbQty.Text = "";
            this.tbUnitName.Text = string.Empty;
            this.tbTotalPrice.Text = string.Empty;
            Session["Item"] = null;
        }
        /// <summary>
        /// This method use for set is read only to form
        /// </summary>
        /// <param name="isReadOnly">is read only?</param>
        public void ReadOnlyForm(bool isReadOnly)
        {
            tbItemCode.Focus();
            if (this.Request["approv"] != null)
            {
                this.PrepareToSetApproveForm();
            }
            else
            {
                if (isReadOnly)
                {
                    this.btnAddItem.Visible = false;
                    //this.btnDelete.Visible = false;
                    this.btnDelItem.Visible = false;
                    this.btnFindItem.Visible = false;
                    tbItemCode.Enabled = false;
                    this.btnFindRequestName.Visible = false;
                    //this.btnCancelForm.Visible = false;
                    //this.btnReset.Visible = false;
                    //this.btnSave.Visible = false;
                    this.rdlRequestType.Enabled = false;
                    this.rblDeptType.Enabled = false;
                    this.gvRequestItem.Enabled = true;



                    //Section Receive
                    this.BtnRec.Visible = true;
                    this.BtnCancleRec.Visible = true;
                }
                else
                {
                    this.btnAddItem.Visible = true;
                    //this.btnDelete.Visible = true;
                    this.btnDelItem.Visible = true;
                    this.btnFindItem.Visible = true;
                    tbItemCode.Enabled = true;
                    this.btnFindRequestName.Visible = true;
                    //this.btnCancelForm.Visible = true;
                    //this.btnReset.Visible = true;
                    //this.btnSave.Visible = true;
                    this.rdlRequestType.Enabled = true;
                    this.rblDeptType.Enabled = true;
                    this.gvRequestItem.Enabled = true;



                    //Section Receive
                    this.BtnRec.Visible = false;
                    this.BtnCancleRec.Visible = false;
                }

                //BtnCancle and Receive
                if (Session["Can_Save"] == "True")
                {
                    this.btnSave.Visible = true;
                    this.btnCancelForm.Visible = true;
                }
                else
                {
                    this.btnSave.Visible = false;
                    this.btnCancelForm.Visible = false;
                }
            }
        }

        protected void btnFileItem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataAccess.ItemDAO().GetItemPack(tbItemCode.Text.Trim(), "", "", "",
                1, 1000, this.SortColumn, this.SortOrder, "1").Tables[0];

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (tbItemCode.Text.Trim() == dt.Rows[i]["Inv_ItemCode"].ToString())
                    {
                        if (dt.Rows[i]["Asset_Status"].ToString() == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('สินค้านี้ได้ถูกยกเลิกไปแล้ว!');", true);
                            tbItemCode.Focus();
                            this.tbItemName.Text = string.Empty;
                            this.tbPricePerUnit.Text = string.Empty;
                            this.tbQty.Text = "";
                            this.tbUnitName.Text = string.Empty;
                            this.tbTotalPrice.Text = string.Empty;
                            Session["Item"] = null;
                            Session["ItemQty"] = null;
                            return;
                        }
                        Session["ItemQty"] = null;
                        tbQty.Text = "";
                        Session["Item"] = dt.Rows[0];
                        SetNewItem();
                        tbQty.Focus();
                        return;
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "errIt", "alert('รหัสสินค้าไม่ถูกต้อง!');", true);
            tbItemCode.Focus();
            this.tbItemName.Text = string.Empty;
            this.tbPricePerUnit.Text = string.Empty;
            this.tbQty.Text = "";
            this.tbUnitName.Text = string.Empty;
            this.tbTotalPrice.Text = string.Empty;
            Session["Item"] = null;
            Session["ItemQty"] = null;
        }

        #region Nin 17/08/2013

        private string default_payId = "0";
        private string receive_no = "";
        private bool show_receive = false;
        private bool have_Waitreceive = false;
        public int request_id;

        protected void BtnCancleRec_Click(object sender, EventArgs e)
        {
            BtnCancleRec.Enabled = false;
            bool result = new RequestDAO().DeleteStockReqRec(Session["request_id"].ToString(), Session["pay_id"].ToString());

            if (result)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "complete", "alert('ทำการยกเลิกการรับเรียบร้อยแล้ว');", true);
                //BindingReqeustForm(request_id, true);
                BindingGridRequestList();
                BindingRequestForm(Convert.ToInt32(Session["request_id"].ToString()), true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
            }
            BtnCancleRec.Enabled = true;
        }

        protected void txt_RecQty_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)(((TextBox)sender).Parent.Parent));
            TextBox txt_RecQty = (TextBox)sender;
            TextBox txt_Comment = (TextBox)row.FindControl("txt_Comment");
            TextBox txt_PayQty = (TextBox)row.FindControl("txt_PayQty");
            if (txt_RecQty.Text != "")
            {
                if (Convert.ToInt32(txt_RecQty.Text) != Convert.ToInt32((txt_PayQty.Text == "" ? "0" : txt_PayQty.Text)))
                {
                    if (txt_Comment.Text == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('กรุณาระบุหมายเหตุ \\n เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย')</script>;");
                        ShowMessageBox("กรุณาระบุหมายเหตุ \\n เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย");
                        txt_Comment.Focus();
                    }
                }
            }
            else if (txt_RecQty.Text == "")
            {
                if (txt_PayQty.Text != "" && txt_PayQty.Text != "0")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('กรุณาระบุจำนวนที่รับ \\n หากไม่มีจำนวนที่รับ กรุณาใส่ 0 ')</script>;");
                    ShowMessageBox("กรุณาระบุจำนวนที่รับ \\n หากไม่มีจำนวนที่รับ กรุณาใส่ 0");
                    txt_RecQty.Focus();
                }
            }
        }

        protected void gvStockPayRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (drv["Pay_Date"].ToString() != "" || drv["Pay_Date"] != System.DBNull.Value)
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(drv["Pay_Date"]).ToString(this.DateFormat);
                }
                if (drv["Receive_Date"].ToString() != "" || drv["Receive_Date"] != System.DBNull.Value)
                {
                    e.Row.Cells[4].Text = Convert.ToDateTime(drv["Receive_Date"]).ToString(this.DateFormat);
                }
                LinkButton linkButton = e.Row.Cells[0].FindControl("btnSelect") as LinkButton;

                if (drv["Pay_Status"].ToString() == "0")
                {
                    linkButton.Visible = false;
                }
                else
                {
                    linkButton.Visible = true;
                    if (receive_no == "")
                    {
                        receive_no = drv["row_num"].ToString();
                    }
                }


                linkButton.CommandArgument = drv["Request_Id"].ToString() + "," + drv["Pay_Id"].ToString() + "," + drv["row_num"].ToString() + "," + drv["Rec_Status"].ToString();

                //btnPrint.OnClientClick = "open_popup('RequestReport.aspx?id=" + drv["Request_Id"].ToString()
                //+ "', 800, 600, 'pop', 'yes', 'yes', 'yes'); return false;";
            }
        }


        protected void gvStockPayRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                Session["Show_receive"] = "true";
                string[] str_cmd;
                try
                {
                    str_cmd = e.CommandArgument.ToString().Split(',');
                    DataTable dtRequestItem = new RequestDAO().GetRequestItem(Convert.ToInt32(str_cmd[0]), str_cmd[1]);
                    receive_no = str_cmd[2];
                    if (str_cmd[3] == "รอรับ")
                    {
                        have_Waitreceive = true;
                    }
                    else if (str_cmd[3] == "ทำรับแล้ว")
                    {
                        have_Waitreceive = false;
                    }
                    else if (str_cmd[3] == "ยกเลิกจ่าย")
                    {
                        have_Waitreceive = false;
                    }

                    Session["RequestItem"] = dtRequestItem;
                    BindingRequestItem();
                }
                catch
                {
                    ShowMessageBox("ไม่สามารถดูข้อมูลรายการรับครั้งนี้ได้");
                }
            }
        }

        protected void GvRequestItem_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow headerow = new GridViewRow(0, 0, DataControlRowType.Header,
                                                          DataControlRowState.Insert);
                e.Row.Cells.Clear();

                if (Session["Show_receive"].ToString() == "false") //ถ้าไม่สามารถรับของได้ จึงจะแสดง chkbox
                {
                    if (this.Request["approv"] == null) //ถ้าเป็นผู้อนุมัติจะต้องไม่แสดง chkbox
                    {
                        TableCell headercell1 = new TableCell()
                        {
                            ColumnSpan = 1,
                            RowSpan = 2,
                            Height = 40,
                            Text = "",
                            HorizontalAlign = HorizontalAlign.Center
                        };
                        headerow.Cells.Add(headercell1);
                    }
                }


                TableCell headercell2 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "ลำดับ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell2);


                TableCell headercell3 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "รหัสสินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell3);

                TableCell headercell4 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "ชื่อสินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell4);

                TableCell headercell5 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "หน่วยที่เบิก",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell5);

                TableCell headercell6 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "จำนวนเบิก",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell6);

                GridViewRow headerow2 = new GridViewRow(0, 0, DataControlRowType.Header,
                                                               DataControlRowState.Insert);

                TableCell headercell14 = new TableCell()
                {
                    ColumnSpan = 2,
                    RowSpan = 1,
                    Height = 20,
                    Text = "จำนวนที่",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell14);

                TableCell headercell7 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "จ่ายสะสม",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell7);

                TableCell headercell8 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "รับสะสม",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell8);


                if (Session["Show_receive"].ToString() == "true")
                {
                    TableCell headercell9 = new TableCell()
                    {
                        ColumnSpan = 2,
                        RowSpan = 1,
                        Height = 20,
                        Text = "การรับครั้งที่ " + receive_no,
                        HorizontalAlign = HorizontalAlign.Center
                    };
                    headerow.Cells.Add(headercell9);


                    TableCell headercell10 = new TableCell()
                    {
                        ColumnSpan = 1,
                        RowSpan = 1,
                        Height = 20,
                        Text = "จำนวนจ่าย",
                        HorizontalAlign = HorizontalAlign.Center
                    };
                    headerow2.Cells.Add(headercell10);


                    TableCell headercell11 = new TableCell()
                    {
                        ColumnSpan = 1,
                        RowSpan = 1,
                        Height = 20,
                        Text = "จำนวนที่รับ",
                        HorizontalAlign = HorizontalAlign.Center
                    };
                    headerow2.Cells.Add(headercell11);

                }

                TableCell headercell12 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "สถานะ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell12);

                //TableCell headercell13 = new TableCell()
                //{
                //    ColumnSpan = 1,
                //    RowSpan = 2,
                //    Height = 40,
                //    Text = "หมายเหตุ",
                //    HorizontalAlign = HorizontalAlign.Center
                //};
                //headerow.Cells.Add(headercell13);


                TableCell headercell15 = new TableCell()
                {
                    ColumnSpan = 3,
                    RowSpan = 1,
                    Height = 20,
                    Text = "หมายเหตุ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell15);

                TableCell headercell16 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "รับสินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell16);

                TableCell headercell17 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "หน่วยงาน",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell17);

                TableCell headercell18 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "คลัง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell18);

                gvRequestItem.Controls[0].Controls.AddAt(0, headerow);
                gvRequestItem.Controls[0].Controls.AddAt(1, headerow2);
                //if (Session["Show_receive"].ToString() == "true")
                //{
                //    gvRequestItem.Controls[0].Controls.AddAt(1, headerow2);
                //}

            }
        }



        protected void BtnRec_Click(object sender, EventArgs e)
        {
            DataTable dtRequestItem = Session["RequestItem"] as DataTable;
            DataTable dtTemp1 = new DataTable();

            dtTemp1.Columns.Add("Request_Id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Pay_Id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Receive_By", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Receive_Status", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Inv_ItemID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Pack_ID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Receive_Qty", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Remark", System.Type.GetType("System.String"));



            int i = 0;

            foreach (GridViewRow row in gvRequestItem.Rows)
            {
                TextBox txt_PayQty = (TextBox)row.FindControl("txt_PayQty");
                TextBox txt_RecQty = (TextBox)row.FindControl("txt_RecQty");
                TextBox txt_Comment = (TextBox)row.FindControl("txt_Comment");

                //if (txt_RecQty.Text != "")
                //{
                //    if (Convert.ToInt32(txt_RecQty.Text) != Convert.ToInt32((txt_PayQty.Text == "" ? "0" : txt_PayQty.Text)))
                //    {
                //        if (txt_Comment.Text == "")
                //        {
                //            //ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('กรุณาระบุหมายเหตุ \\n เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย')</script>;");
                //            ShowMessageBox("กรุณาระบุหมายเหตุ \\n เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย");
                //            txt_Comment.Focus();
                //            return;
                //        }
                //    }
                //}
                //else if (txt_RecQty.Text == "")
                //{
                //    if (txt_PayQty.Text != "" && txt_PayQty.Text != "0")
                //    {
                //        //ClientScript.RegisterStartupScript(this.GetType(), "onClick", "<script>alert('กรุณาระบุจำนวนที่รับ \\n หากไม่มีจำนวนที่รับ กรุณาใส่ 0 ')</script>;");
                //        ShowMessageBox("กรุณาระบุจำนวนที่รับ \\n หากไม่มีจำนวนที่รับ กรุณาใส่ 0");
                //        txt_RecQty.Focus();
                //        return;
                //    }
                //}

                DataRow dr = dtTemp1.NewRow();


                dr["Request_Id"] = dtRequestItem.Rows[i]["Request_Id"].ToString();
                dr["Pay_Id"] = dtRequestItem.Rows[i]["Pay_Id"].ToString(); ;
                dr["Receive_By"] = Convert.ToInt32(this.UserID);
                dr["Receive_Status"] = Convert.ToInt32("1");
                dr["Inv_ItemID"] = dtRequestItem.Rows[i]["Inv_ItemID"].ToString();
                dr["Pack_ID"] = dtRequestItem.Rows[i]["Pack_ID"].ToString();
                dr["Receive_Qty"] = Convert.ToInt32((txt_RecQty.Text == "" ? "0" : txt_RecQty.Text));
                dr["Remark"] = txt_Comment.Text;
                request_id = Convert.ToInt32(dr["Request_Id"].ToString() == "" ? "0" : dr["Request_Id"].ToString());
                dtTemp1.Rows.Add(dr);

                i++;
            } //End for



            try
            {
                BtnRec.Enabled = false; //lock ปุ่มไว้หลังจากที่กดทำการรับ

                bool result = new RequestDAO().InsertStockReqRec(dtTemp1);

                if (result)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมุล');", true);
                    ShowMessageBox("บันทึกข้อมุล");
                    BindingRequestForm(request_id, true);
                    BindingGridRequestList();
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
                    ShowMessageBox("ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง");
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('" + ex.Message + @"');", true);
                ShowMessageBox(" + ex.Message + @");
            }
            BtnRec.Enabled = true;    //ปลด lock ปุ่มหลังจากที่ทำการรับ 
        }


        #endregion

        protected void ItemCodeSearchChanged(object sender, EventArgs e)
        {
            btnFileItem_Click(sender, e);

            //ScriptManager.RegisterStartupScript(
            //);
            //    Label lbl_item_no;
            //    TextBox txt_qty_row = new TextBox();
            //    Label lbl_item_name_grid;
            //    Label lbl_pack_name_grid;

            //    int rowIndex = new int();

            //    bool IsConstain = false;

            //    foreach (GridViewRow item in gvDetail_withdrawal.Rows)
            //    {
            //        lbl_item_no = (Label)item.FindControl("lbl_item_no");
            //        txt_qty_row = (TextBox)item.FindControl("txt_qty");

            //        lbl_item_name_grid = (Label)item.FindControl("lbl_item_name");
            //        lbl_pack_name_grid = (Label)item.FindControl("lbl_pack_name");

            //        if (lbl_item_no.Text.Trim().Equals(txt_No.Text.Trim()))
            //        {
            //            IsConstain = true;

            //            txt_item_name.Text = lbl_item_name_grid.Text;
            //            txt_pack.Text = lbl_pack_name_grid.Text;

            //            rowIndex = item.RowIndex;
            //            break;
            //        }
            //    }
            //    if (IsConstain)
            //    {
            //        txt_lot_no.Focus();
            //    }
            //    else
            //    {
            //        txt_item_name.Text = "";
            //        txt_pack.Text = "";
            //        txt_No.Focus();
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบสินค้า');", true);
            //        return;
            //    }
            //}
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DataTable dt_MainReqItem = Session["RequestItem"] as DataTable;
            if (dt_MainReqItem.Rows.Count > 0)
            {
                ShowMessageBox("ไม่สามารถทำการ Import ข้อมูลได้ \\n เนื่องจากมีข้อมูลวัสดุอุปกรณ์ในตารางอยู่แล้ว");
            }
            else
            {
                ImportFrmRequest();
            }
        }
        private void ImportFrmRequest()
        {
            //file upload path
            string strConnection = new DataAccess.DatabaseHelper().ConnectionString;
            if (Path.GetFileNameWithoutExtension(FileImport.PostedFile.FileName) == "")
            {
                ShowMessageBox("กรณา Browse file ใหม่อีกครั้ง");
                return;
            }

            string filename = Path.GetFileNameWithoutExtension(FileImport.PostedFile.FileName);
            string extension = Path.GetExtension(FileImport.PostedFile.FileName);
            string saveFileName = filename + System.DateTime.Now.ToString("_ddMMyyhhmmss") + extension;
            //FileImport.SaveAs(Server.MapPath("Files/" + saveFileName));
            //string path = Server.MapPath("Files/" + saveFileName);
            FileImport.SaveAs(Server.MapPath("~/Files/" + saveFileName));
            string path = Server.MapPath("~/Files/" + saveFileName);
            string excelConnectionString = "";
            //Create connection string to Excel work book
            switch (extension)
            {
                case ".xls": //Excel 97-03
                    excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0;Persist Security Info=False";
                    break;
                case ".xlsx": //Excel 07
                    excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    break;
            }

            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);

            try
            {
                //Create OleDbCommand to fetch data from Excel
                OleDbCommand cmd = new OleDbCommand("Select [Order_Qty],[Inv_ItemName],[Inv_ItemCode] from [Sheet1$] WHERE [Inv_ItemCode] <> ''''", excelConnection);
                excelConnection.Open();
                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                ds.Load(dReader, LoadOption.PreserveChanges, "hh");
                DataTable dt = ds.Tables[0];

                string chkImpData = "";

                if (dt.Rows.Count > 0)
                {
                    DataTable dt_MainReqItem = Session["RequestItem"] as DataTable;
                    DataTable dt_reqItem = dt_MainReqItem.Clone();
                    dt_reqItem.Merge(dt_MainReqItem);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow drOldItem = dt_reqItem.Select("[Inv_ItemCode]='" + dt.Rows[i]["Inv_ItemCode"].ToString().Trim() + "'").FirstOrDefault();

                        if (drOldItem == null)
                        {
                            DataRow drRow = dt_reqItem.NewRow();

                            DataSet dsChkItem = new DataAccess.ItemDAO().GetItem(dt.Rows[i]["Inv_ItemCode"].ToString().Trim(), "", "",
                                         "", "1", 1, 1000, "", "");

                            DataTable dt_itemPack = new DataAccess.ItemDAO().GetItemPack(dt.Rows[i]["Inv_ItemCode"].ToString().Trim(), "", "", "",
                                      1, 1000, this.SortColumn, this.SortOrder, "1").Tables[0];

                            // ตรวจสอบว่า Inv_ItemCode ที่รับเข้ามาสามารถหา PackId ได้หรือไม่ ถ้าค่าเท่ากับ 0 คือไม่สามารถหา PackId ได้ 
                            if (dt_itemPack.Rows.Count == 0 || dsChkItem.Tables[0].Rows.Count == 0)
                            {
                                if (chkImpData == "")
                                {
                                    chkImpData += dt.Rows[i]["Inv_ItemCode"].ToString().Trim();
                                }
                                else
                                {
                                    chkImpData += ", " + dt.Rows[i]["Inv_ItemCode"].ToString().Trim();
                                }
                            }
                            else
                            {
                                drRow["Inv_ItemID"] = dt_itemPack.Rows[0]["Inv_ItemID"];
                                drRow["Inv_ItemCode"] = dt_itemPack.Rows[0]["Inv_ItemCode"];
                                drRow["Inv_ItemName"] = dt_itemPack.Rows[0]["Inv_ItemName"];
                                drRow["Pack_ID"] = dt_itemPack.Rows[0]["Pack_ID"];
                                drRow["Description"] = dt_itemPack.Rows[0]["Description"];
                                drRow["Avg_Cost"] = dt_itemPack.Rows[0]["Avg_Cost"];
                                drRow["Order_Quantity"] = Convert.ToInt32((dt.Rows[i]["Order_Qty"].ToString() == "" ? "0" : dt.Rows[i]["Order_Qty"].ToString()));
                                drRow["Pay_Qty"] = 0;
                                drRow["Receive_Qty"] = 0;
                                drRow["Req_ItemStatus"] = 0;
                                drRow["Is_New"] = "Y";
                                drRow["Is_Delete"] = "N";
                                drRow["Is_Update"] = "N";
                                drRow["Order_Amount"] = Convert.ToDecimal(dt_itemPack.Rows[0]["Avg_Cost"].ToString() == "" ? "0" : dt_itemPack.Rows[0]["Avg_Cost"].ToString()) * Convert.ToInt32(dt.Rows[i]["Order_Qty"].ToString() == "" ? "0" : dt.Rows[i]["Order_Qty"].ToString());
                                dt_reqItem.Rows.Add(drRow);
                            }
                        }
                        else
                        {
                            drOldItem["Order_Quantity"] = Convert.ToInt32(drOldItem["Order_Quantity"].ToString() == "" ? "0" : drOldItem["Order_Quantity"].ToString()) + Convert.ToInt32(dt.Rows[i]["Order_Qty"].ToString() == "" ? "0" : dt.Rows[i]["Order_Qty"].ToString());
                            drOldItem["Order_Amount"] = Convert.ToDecimal(drOldItem["Avg_Cost"].ToString() == "" ? "0" : drOldItem["Avg_Cost"].ToString()) * Convert.ToInt32(drOldItem["Order_Quantity"].ToString() == "" ? "0" : drOldItem["Order_Quantity"].ToString());
                            if (drOldItem["Is_New"].ToString() == "N")
                            {
                                drOldItem["Is_Update"] = "Y";
                            }
                            drOldItem.AcceptChanges();
                        }

                    } // End For

                    if (chkImpData == "") // ข้อมูลทุกตัวสามารถทำการ Import เข้าได้
                    {
                        DataTable dt_Import = dt_reqItem;
                        Session["RequestItem"] = dt_Import;
                        this.BindingRequestItem();
                    }
                    else // มีข้อมูลบางตัวไม่ถูกต้อง ทำให้ไม่สามารทำการ Import เข้าได้
                    {
                        ShowMessageBox("มีข้อมูลบางรายการไม่ถูกต้อง ได้แก่ รหัสสินค้าดังต่อไปนี้ : \\n " + chkImpData);
                        //dt_reqItem.Clear();
                    }
                    DataTable old = Session["RequestItem"] as DataTable;
                    excelConnection.Close();
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox("ไม่สามารถ Import ข้อมูลได้");
                excelConnection.Close();
                File.Delete(path);
            }

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

        }


    }
}
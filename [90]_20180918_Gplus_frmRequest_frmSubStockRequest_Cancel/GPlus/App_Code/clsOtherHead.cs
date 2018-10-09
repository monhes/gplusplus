using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for clsOtherHead
/// </summary>
public class clsOtherHead
{
    public String transactionID { get; set; }

    public String receiveID { get; set; }

    public String receiveDate { get; set; }

    public String refNumber { get; set; }//เอกสารอ้างอิง

    public String type { get; set; }

    public String stockID { get; set; }

    public String supplier { get; set; }
    
    public String supplierName { get; set; }

    public String receiveUser { get; set; }// ผู้รับ

    public String receiveUserID { get; set; }// ผู้รับ

    public String sendUser { get; set; }// ผู้ส่ง

    public String reason { get; set; }

    public String total { get; set; }

    public String flag { get; set; }

    public String createDate { get; set; }

    public String createBy { get; set; }

    public String updateDate { get; set; }

    public String updateBy { get; set; }

    public String pay_id { get; set; }


	public clsOtherHead()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public List<clsOtherHead> fn_getHead(String str_sort,String str_rowPer, String str_rowStart, String str_dateStart, String str_dateEnd, String str_receive, String str_stock)//รับอื่นๆ
    {
        List<clsOtherHead> list_head = new List<clsOtherHead>();
        clsOtherHead obj_head = new clsOtherHead();
        clsConnDb obj_con = new clsConnDb();
        DataTable obj_dt = new DataTable();

        string str_orderBy = "" + (String.IsNullOrEmpty(str_sort) ? "Transaction_ID" : str_sort) + " DESC";
        String str_sql = "WITH Paging(RowNo,[Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID,Refer_Supplier " +
                         ",[Refer_Document],[Refer_Contact_Person],[Transaction_Status],Transaction_By,Total,Transaction_ByUser,[Reason],[Create_Date],[Update_Date]" +
                         ",[Create_By],[Update_By],Create_name,Update_Name,pay_id) AS " +
                         "(SELECT ROW_NUMBER() OVER (ORDER BY " + str_orderBy + ") AS RowNo,[Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID" +
                         " , (select [Supplier_Name] from Inv_Supplier where Inv_Supplier.[Supplier_ID] = [Inv_TransHead].[Refer_Supplier_ID]) as Refer_Supplier" +
                         ",[Refer_Document],[Refer_Contact_Person],[Transaction_Status],Transaction_By," +
                         "(select sum(Amount) from [Inv_TransDetail] where [Inv_TransDetail].[Transaction_ID] = [Inv_TransHead].[Transaction_ID] ) as Total," +
                         " (SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Transaction_By]) as Transaction_ByUser," +
                         "[Reason],[Create_Date],[Update_Date],[Create_By],[Update_By]," +
                         "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Create_By]) as Create_name, " +
                         "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Update_By]) as Update_Name,pay_id " +
                         "FROM [Inv_TransHead] where [Transaction_Type] = 'G' ";

        if (str_dateStart != "" && str_dateEnd != "")
        {
            str_sql += " and [Transaction_Date] between   '" + obj_con._fn_setDateFormatBuddhist(str_dateStart) + "' and '" + obj_con._fn_setDateFormatBuddhist(str_dateEnd) + "'  ";
        }

        if (str_receive != "")
        {
            str_sql += " and  [Transaction_No] like '%" + str_receive + "%'   ";
        }

        if (str_stock != "")
        {
            str_sql += " and [Stock_ID] = " + str_stock + " ";
        }

        str_sql = str_sql + "GROUP BY [Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID" +
                         ",[Refer_Document],[Refer_Contact_Person],[Transaction_Status],Transaction_By,[Reason],[Create_Date],[Update_Date]" +
                         ",[Create_By],[Update_By],pay_id";
        str_sql = str_sql + ") SELECT *, " + str_rowStart + " AS CurrentPage, (SELECT COUNT(*) FROM Paging) AS TotalResults " +
                              "FROM Paging " +
                              "WHERE RowNo BETWEEN ((" + str_rowStart + " - 1 ) * " + str_rowPer + ") + 1 AND " + str_rowStart + " * " + str_rowPer;




        //string str_orderBy = "a." + (String.IsNullOrEmpty(str_sort) ? "Transaction_ID" : str_sort) + " DESC";
        //string str_sql = "SELECT [Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID" +
        //                 " , (select [Supplier_Name] from Inv_Supplier where Inv_Supplier.[Supplier_ID] = [Inv_TransHead].[Refer_Supplier_ID]) as Refer_Supplier" +
        //                 ",[Refer_Document],[Refer_Contact_Person],[Transaction_Status],Transaction_By," +
        //                 "(select sum(Amount) from [Inv_TransDetail] where [Inv_TransDetail].[Transaction_ID] = [Inv_TransHead].[Transaction_ID] ) as Total," +
        //                 " (SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Transaction_By]) as Transaction_ByUser," +
        //                 "[Reason],[Create_Date],[Update_Date],[Create_By],[Update_By]," +
        //                 "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Create_By]) as Create_name, " +
        //                 "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Update_By]) as Update_Name " +
        //                 "FROM [Inv_TransHead] where [Transaction_Type] = 'G' ";

        //if (str_dateStart != "" && str_dateEnd != "")
        //{
        //    str_sql += " and [Transaction_Date] between   '" + obj_con.convertDate(str_dateStart) + "' and '" + obj_con.convertDate(str_dateEnd) + "'  ";
        //}

        //if (str_receive != "")
        //{
        //    str_sql += " and  [Transaction_No] like '%" + str_receive + "%'   ";
        //}

        //if(str_stock != "")
        //{
        //    str_sql += " and [Stock_ID] = " + str_stock + " ";
        //}
       
        obj_con._fn_openConn();
        obj_dt = obj_con._fn_queryForDataTable(str_sql);
        obj_con._fn_closeConn();


        for (int i = 0; i < obj_dt.Rows.Count; i++)
        {
            obj_head.transactionID = obj_dt.Rows[i]["Transaction_ID"].ToString();
            obj_head.receiveID = obj_dt.Rows[i]["Transaction_No"].ToString();
            obj_head.receiveDate = obj_con._fn_setDateFormatChirtForWeb(obj_dt.Rows[i]["Transaction_Date"].ToString());
            obj_head.refNumber = obj_dt.Rows[i]["Refer_Document"].ToString();
            obj_head.type = obj_dt.Rows[i]["Transaction_Sub_Type"].ToString();
            obj_head.stockID = obj_dt.Rows[i]["Stock_ID"].ToString();
            obj_head.supplier = obj_dt.Rows[i]["Refer_Supplier_ID"].ToString();
            obj_head.supplierName = obj_dt.Rows[i]["Refer_Supplier"].ToString();
            obj_head.receiveUser = obj_dt.Rows[i]["Transaction_ByUser"].ToString();
            obj_head.receiveUserID = obj_dt.Rows[i]["Transaction_By"].ToString();
            obj_head.sendUser = obj_dt.Rows[i]["Refer_Contact_Person"].ToString();
            obj_head.total = obj_dt.Rows[i]["Total"].ToString();
            obj_head.flag = obj_dt.Rows[i]["Transaction_Status"].ToString();
            obj_head.reason = obj_dt.Rows[i]["Reason"].ToString();
            obj_head.createBy = obj_dt.Rows[i]["Create_name"].ToString();
            obj_head.createDate = obj_con._fn_setDateFormatChirtForWeb(obj_dt.Rows[i]["Create_Date"].ToString());
            obj_head.updateBy = obj_dt.Rows[i]["Update_Name"].ToString();
            obj_head.updateDate = obj_con._fn_setDateFormatChirtForWeb(obj_dt.Rows[i]["Update_Date"].ToString());
            obj_head.pay_id = obj_dt.Rows[i]["pay_id"].ToString();
            list_head.Add(obj_head);
            obj_head = new clsOtherHead();
        }
        return list_head;
    }


    public List<clsOtherHead> fn_getHeadPay(String str_sort, String str_rowPer, String str_rowStart, String str_dateStart, String str_dateEnd, String str_pay, String str_stock)
    {
        List<clsOtherHead> list_head = new List<clsOtherHead>();
        clsOtherHead obj_head = new clsOtherHead();
        clsConnDb obj_con = new clsConnDb();
        DataTable obj_dt = new DataTable();

        string str_orderBy = "" + (String.IsNullOrEmpty(str_sort) ? "Transaction_ID" : str_sort) + " DESC";
        String str_sql = "WITH Paging(RowNo,[Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID,Refer_Supplier " +
                         ",[Refer_Document],Approver_Name,Approver_By,[Refer_Contact_Person],[Transaction_Status],Transaction_By,Total,Transaction_ByUser,[Reason],[Create_Date],[Update_Date]" +
                         ",[Create_By],[Update_By],Create_name,Update_Name) AS " +
                         "(SELECT ROW_NUMBER() OVER (ORDER BY " + str_orderBy + ") AS RowNo,[Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID" +
                         " , (select [Supplier_Name] from Inv_Supplier where Inv_Supplier.[Supplier_ID] = [Inv_TransHead].[Refer_Supplier_ID]) as Refer_Supplier" +
                         ",[Refer_Document],(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Approver_By]) as Approver_Name " +
                         ",[Approver_By],[Refer_Contact_Person],[Transaction_Status],Transaction_By,(select sum(Amount) from [Inv_TransDetail] where [Inv_TransDetail].[Transaction_ID] = [Inv_TransHead].[Transaction_ID] ) as Total," +
                         " (SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Transaction_By]) as Transaction_ByUser," +
                         "[Reason],[Create_Date],[Update_Date],[Create_By],[Update_By]," +
                         "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Create_By]) as Create_name, " +
                         "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Update_By]) as Update_Name " +
                         "FROM [Inv_TransHead] where [Transaction_Type] = 'U' ";

        if (str_dateStart != "" && str_dateEnd != "")
        {
            str_sql += " and [Transaction_Date] between   '" + obj_con._fn_setDateFormatBuddhist(str_dateStart) + "' and '" + obj_con._fn_setDateFormatBuddhist(str_dateEnd) + "'  ";
        }

        if (str_pay != "")
        {
            str_sql += " and  [Transaction_No] like '%" + str_pay + "%'   ";
        }

        if (str_stock != "")
        {
            str_sql += " and [Stock_ID] = " + str_stock + " ";
        }

        str_sql = str_sql + "GROUP BY [Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID" +
                         ",[Refer_Document],[Approver_By],[Refer_Contact_Person],[Transaction_Status],Transaction_By,[Reason],[Create_Date],[Update_Date]" +
                         ",[Create_By],[Update_By]";
        str_sql = str_sql + ") SELECT *, " + str_rowStart + " AS CurrentPage, (SELECT COUNT(*) FROM Paging) AS TotalResults " +
                              "FROM Paging " +
                              "WHERE RowNo BETWEEN ((" + str_rowStart + " - 1 ) * " + str_rowPer + ") + 1 AND " + str_rowStart + " * " + str_rowPer;




        //string str_orderBy = "a." + (String.IsNullOrEmpty(str_sort) ? "Transaction_ID" : str_sort) + " DESC";
        //string str_sql = "SELECT [Transaction_ID],[Transaction_No],[Transaction_Date],[Transaction_Sub_Type],[Stock_ID],Refer_Supplier_ID" +
        //                 " , (select [Supplier_Name] from Inv_Supplier where Inv_Supplier.[Supplier_ID] = [Inv_TransHead].[Refer_Supplier_ID]) as Refer_Supplier" +
        //                 ",[Refer_Document],[Refer_Contact_Person],[Transaction_Status],Transaction_By," +
        //                 "(select sum(Amount) from [Inv_TransDetail] where [Inv_TransDetail].[Transaction_ID] = [Inv_TransHead].[Transaction_ID] ) as Total," +
        //                 " (SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Transaction_By]) as Transaction_ByUser," +
        //                 "[Reason],[Create_Date],[Update_Date],[Create_By],[Update_By]," +
        //                 "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Create_By]) as Create_name, " +
        //                 "(SELECT [Account_Fname] + ' ' + [Account_Lname] from [GPLUZ_ACCOUNT] where [GPLUZ_ACCOUNT].[Account_ID] = [Inv_TransHead].[Update_By]) as Update_Name " +
        //                 "FROM [Inv_TransHead] where [Transaction_Type] = 'G' ";

        //if (str_dateStart != "" && str_dateEnd != "")
        //{
        //    str_sql += " and [Transaction_Date] between   '" + obj_con.convertDate(str_dateStart) + "' and '" + obj_con.convertDate(str_dateEnd) + "'  ";
        //}

        //if (str_receive != "")
        //{
        //    str_sql += " and  [Transaction_No] like '%" + str_receive + "%'   ";
        //}

        //if(str_stock != "")
        //{
        //    str_sql += " and [Stock_ID] = " + str_stock + " ";
        //}

        obj_con._fn_openConn();
        obj_dt = obj_con._fn_queryForDataTable(str_sql);
        obj_con._fn_closeConn();


        for (int i = 0; i < obj_dt.Rows.Count; i++)
        {
            obj_head.transactionID = obj_dt.Rows[i]["Transaction_ID"].ToString();
            obj_head.receiveID = obj_dt.Rows[i]["Transaction_No"].ToString();
            obj_head.receiveDate = obj_con._fn_setDateFormatChirtForWeb(obj_dt.Rows[i]["Transaction_Date"].ToString());
            obj_head.refNumber = obj_dt.Rows[i]["Transaction_by"].ToString();
            obj_head.type = obj_dt.Rows[i]["Transaction_Sub_Type"].ToString();
            obj_head.stockID = obj_dt.Rows[i]["Stock_ID"].ToString();
            obj_head.supplier = obj_dt.Rows[i]["Transaction_by"].ToString();
            obj_head.supplierName = obj_dt.Rows[i]["Transaction_byUser"].ToString();
            obj_head.receiveUser = obj_dt.Rows[i]["Approver_Name"].ToString();
            obj_head.receiveUserID = obj_dt.Rows[i]["Approver_By"].ToString();
            obj_head.sendUser = obj_dt.Rows[i]["Refer_Contact_Person"].ToString();
            obj_head.total = obj_dt.Rows[i]["Total"].ToString();
            obj_head.flag = obj_dt.Rows[i]["Transaction_Status"].ToString();
            obj_head.reason = obj_dt.Rows[i]["Reason"].ToString();
            obj_head.createBy = obj_dt.Rows[i]["Create_name"].ToString();
            obj_head.createDate = obj_dt.Rows[i]["Create_Date"].ToString();
            obj_head.updateBy = obj_dt.Rows[i]["Update_Name"].ToString();
            obj_head.updateDate = obj_dt.Rows[i]["Update_Date"].ToString();

            list_head.Add(obj_head);
            obj_head = new clsOtherHead();
        }
        return list_head;
    }
}
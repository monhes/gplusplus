using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;

namespace GPlus.PRPO
{
    public partial class ReceiveDailyReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                this.PageID = "415";
                BindDropdown();
                //ReportViewer1.Visible = false;

            }
            //bindData2("1950-01-01", "2099-01-01", "1", "1", "1", "1", "1");
          // loadReport();
        }

      

        private void bindData2(String receiveDate1,String receiveDate2,String stockId,String cateId,String subCateId,String invItemCode,String itemSearchDesc ){

        



            ReportDocument rpt = new ReportDocument();
            // string directory = Application.Info.DirectoryPath;
            //rpt.Load(directory & "\myCrystalReport.rpt")


            String reportPath = "";

            if (rdbOnHand0.SelectedIndex == 0)
            {
                reportPath = Server.MapPath("..") + "/reports/" + "ReceiveDailyReport2.rpt";
            }else{
                reportPath = Server.MapPath("..") + "/reports/" + "PayDailyReport.rpt";
            }
              


            // rpt.Load("c:\\reports/ReceiveDailyReport2.rpt");
            rpt.Load(reportPath);
           // rpt.SetParameterValue("accountId", "3");
            rpt.SetParameterValue("stockId", stockId);
            rpt.SetParameterValue("cateId", cateId);
            rpt.SetParameterValue("subCateId", subCateId);
            rpt.SetParameterValue("invItemCode", invItemCode);
            rpt.SetParameterValue("itemSearchDesc", itemSearchDesc);

            if (rdbOnHand0.SelectedIndex == 0)
            {
                 rpt.SetParameterValue("receiveDate1", receiveDate1);
                 rpt.SetParameterValue("receiveDate2", receiveDate2);
            }
            else
            {
                rpt.SetParameterValue("payDate1", receiveDate1);
                rpt.SetParameterValue("payDate2", receiveDate2);
            }
        

      

            String serverName = System.Configuration.ConfigurationManager.AppSettings["server_name"];
            String dbName = System.Configuration.ConfigurationManager.AppSettings["db_name"];
            String dbUserName = System.Configuration.ConfigurationManager.AppSettings["db_user_name"];
            String dbPass = System.Configuration.ConfigurationManager.AppSettings["db_user_pass"];


           rpt.SetDatabaseLogon(dbUserName, dbPass, serverName, dbName);

            rpt.DataSourceConnections[0].SetConnection(serverName, dbName, dbUserName, dbPass);

       

            this.CrystalReportViewer1.ReportSource = rpt;
        //    this.CrystalReportViewer1.RefreshReport();





        }



        public DataTable ReportStockOnHandPackageTable
        {
            get
            {
                return (DataTable)Session["ReportStockOnHand"];
            }
            set
            {
                Session["ReportStockOnHand"] = value;
            }
        }



      



        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            ddlMaterialType.DataSource = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
            ddlMaterialType.DataTextField = "Cat_Name";
            ddlMaterialType.DataValueField = "Cate_ID";
            ddlMaterialType.DataBind();
            ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
        }

        private void BindSubMaterialType()
        {
            if (ddlMaterialType.SelectedIndex > 0)
            {
                ddlSubMaterialType.DataSource = new DataAccess.CategoryDAO().GetSubCate("", ddlMaterialType.SelectedValue, "",
                    "1", 1, 1000, "", "").Tables[0];
                ddlSubMaterialType.DataTextField = "SubCate_Name";
                ddlSubMaterialType.DataValueField = "SubCate_ID";
                ddlSubMaterialType.DataBind();
                ddlSubMaterialType.Items.Insert(0, new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
            }
            else
                ddlSubMaterialType.Items.Clear();
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //BindData();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Err", "window.open('test.aspx')", true);
        

            /* 
              String url = "ReceivDailyReportShow.aspx?InvoiceID=777";
              Session["url"] = url;
              string winFeatures = "toolbar=no,status=no,menubar=no,location=center,scrollbars=yes,resizable=no,height=650,width=825";
              ClientScript.RegisterStartupScript(this.GetType(), "newWindow", string.Format("<script type='text/javascript'>var popup=window.open('{0}', 'yourWin', '{1}'); popup.focus();</script>", url, winFeatures));
         
              */





            loadReport();
            
                


        }

        private void loadReport()
        {

            String receiveDate1 = "";
                String receiveDate2 ="";

            if (Request.Form["receive_date1"] != null)
            {
                receiveDate1 = Request.Form["receive_date1"];
            }

            if (Request.Form["receive_date2"] != null)
            {
                receiveDate2 = Request.Form["receive_date2"];
            }

            String stockId = ddlStock.SelectedValue;
            String cateId = ddlMaterialType.SelectedValue;
            String subCateId = ddlSubMaterialType.SelectedValue;

            String invItemCode = txtItemCode.Text;
            String itemSearchDesc = txtItemName.Text;


           
            if (receiveDate1.Length < 8 || receiveDate2.Length < 8)
            {
                receiveDate1 = "1950-01-01";
                receiveDate2 = "2099-01-01";
            }
            else
            {
                receiveDate1 = formatDate(receiveDate1);
                receiveDate2 = formatDate(receiveDate2);

            }


            bindData2(receiveDate1, receiveDate2, stockId, cateId, subCateId, invItemCode, itemSearchDesc);


        }

        private String formatDate(String strDate1)
        {

          //  String[] a_date=new String[3];

            string[] a_date = strDate1.Split('/');

            int numYear = Int32.Parse(a_date[2])-543;


            return numYear + "-" + a_date[1] + "-" + a_date[0];
        }
      
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {

            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            ddlStock.SelectedIndex = 0;
            txtItemCode.Text = "bbb";
            txtItemName.Text = "bbb";
            // rdbStatus.SelectedIndex = 0;
            //rdbOnHand.SelectedIndex = 0;

        }

        private void BindData()
        {

            /*

          //  ReportViewer1.Visible = true;

            string status = "";
            string onHand = "";

          
             */


            /*
             if (rdbStatus.SelectedIndex == 0)
             {
                 status = "1";
             }
             else if (rdbStatus.SelectedIndex == 1)
             {
                 status = "0";
             }

             */
            /*


            if (rdbOnHand.SelectedIndex == 0)
            {
                onHand = "1";
            }
            else if (rdbOnHand.SelectedIndex == 1)
            {
                onHand = "0";
            }

            GPlus.DataAccess.StockDAO st = new DataAccess.StockDAO();

            DataSet ds = st.GetStocOnHandkReport(ddlStock.SelectedValue, ddlMaterialType.SelectedValue, ddlSubMaterialType.SelectedValue, txtItemCode.Text, txtItemName.Text, status, onHand);
            ReportDataSource rds1 = new ReportDataSource("StockOnHandDataSet", ds.Tables[0]);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds1);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ReportViewer1.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            this.ReportViewer1.LocalReport.Refresh();

            */


        }



    }
}
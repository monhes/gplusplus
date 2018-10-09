using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using GPlus.DataAccess;

namespace GPlus
{
    public partial class PayReport  : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

                    if(!Page.IsPostBack){

                        ReportViewer.Visible = false;
                        ddlStock.DataSource = new StockDAO().RepGetAllStock();
                        ddlStock.DataBind();
                        ddlMaterialType.DataSource = new StockDAO().RepInvGetAllCatagory();
                        ddlMaterialType.DataBind();
                        ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", "0"));

                        int thisYear = DateTime.Now.Year + 543;

                        for (int i = thisYear; i >= 2550; i--)
                        {
                            ddlYearStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
             ReportViewer.Visible = true;

             string stockID =   ddlStock.SelectedItem.Value.ToString();
             string catId = ddlMaterialType.SelectedItem.Value.ToString();
             string monthEndSum = ddlYearStart.SelectedItem.Value + ddlMonthStart.SelectedItem.Value;
             string monthEndSumPrv  = "";
            if(ddlMonthStart.SelectedItem.Value == "01"){
               monthEndSumPrv =  (int.Parse(ddlYearStart.SelectedItem.Value) - 1).ToString() + "12";


            }else{
               int mval = (int.Parse(ddlMonthStart.SelectedItem.Value)-1);
                if(mval < 10){
                     monthEndSumPrv = ddlYearStart.SelectedItem.Value + "0" + mval.ToString();

                }else{
                     monthEndSumPrv = ddlYearStart.SelectedItem.Value  + mval.ToString();
                }
            }

            string ss = "ประจำเดือน" + ddlMonthStart.SelectedItem.Text + " พ.ศ.  " + ddlYearStart.SelectedItem.Text;
            
            List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@Stock_ID", stockID));
             param.Add(new SqlParameter("@Cate_ID", catId));
             param.Add(new SqlParameter("@MonthEndSum", monthEndSum));
             param.Add(new SqlParameter("@MonthEndSumPrv", monthEndSumPrv));
             param.Add(new SqlParameter("@MonthYearText",ss ));
         
  
         // @Cate_ID

             DataTable dt = new DataAccess.DatabaseHelper().ExecuteDataTable("sp_test", param);

             for (int i = 0; i < dt.Rows.Count; i++)
             {

                 if (i > 0)
                 {
                     if ((dt.Rows[i - 1]["CAT_NAME"].ToString().Trim() == dt.Rows[i]["CAT_NAME"].ToString().Trim()))
                     {
                         if ((dt.Rows[i - 1]["TYPE_NAME"].ToString().Trim() == dt.Rows[i]["TYPE_NAME"].ToString().Trim()))
                         {
                             dt.Rows[i]["Pay_AmountInDept"] = DBNull.Value;
                             dt.Rows[i]["Pay_AmountAdjust"] = DBNull.Value;
                             dt.Rows[i]["Pay_AmountExists"] = DBNull.Value;
                             dt.Rows[i]["Pay_AmountGetInCash"] = DBNull.Value;
                             dt.Rows[i]["Pay_AmountGetInPrintFac"] = DBNull.Value;
                             dt.Rows[i]["Pay_AmountBalance"] = DBNull.Value;
                         }

                     }
                 }
           
   

             }

            ReportViewer.LocalReport.DataSources.Clear();

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer.LocalReport.DataSources.Add(rds);
            ReportViewer.LocalReport.Refresh();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
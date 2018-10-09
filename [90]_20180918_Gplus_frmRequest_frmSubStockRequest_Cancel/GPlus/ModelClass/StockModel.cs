using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GPlus.DataAccess;

namespace GPlus.ModelClass
{
    public class StockModel
    {
       // public Get

        public int RecountCount
        {
            get;
            set;
        }

        public DataTable StockView
        {
            get;
            set;
        }

        //public void GetReceiveStkNotCompleteView(int pageNo,int pageNumPerPage,string poCode,string stkNo)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        DataTable dtt = new DataTable();
        //        dt = new ReceiveStockDAO().GetReceiveStkNotComplete();

        //        foreach (DataColumn dc in dt.Columns)
        //        {
        //            dtt.Columns.Add(dc.ColumnName, dc.DataType);
        //        }

           
        //        var rows = dt.AsEnumerable().Skip((pageNo - 1) * pageNumPerPage).Take(pageNumPerPage);
             
        //        rows.CopyToDataTable(dtt, LoadOption.OverwriteChanges);

        //        this.RecountCount = dt.Rows.Count;
        //        this.StockView = dtt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
                  
        //}



        public void GetReceiveStkNotCompleteViewSearch(int pageNo, int pageNumPerPage, string poCode, string stkNo,DateTime? dateStPO, DateTime? dateEnPO,DateTime? dateStStk, DateTime? dateEnStk)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtt = new DataTable();
                dt = new ReceiveStockDAO().GetReceiveStkNotCompleteSearch(poCode,stkNo,dateStPO,dateEnPO,dateStStk,dateEnStk);

                foreach (DataColumn dc in dt.Columns)
                {
                    dtt.Columns.Add(dc.ColumnName, dc.DataType);
                }

               var rows = dt.AsEnumerable().Skip((pageNo - 1) * pageNumPerPage).Take(pageNumPerPage);
                  rows.CopyToDataTable(dtt, LoadOption.OverwriteChanges);

                this.RecountCount = dt.Rows.Count;
                this.StockView = dtt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public void GetReceiveStkCompleteViewSearch(int pageNo, int pageNumPerPage, string poCode, string stkNo, DateTime? dateStPO, DateTime? dateEnPO, DateTime? dateStStk, DateTime? dateEnStk)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtt = new DataTable();
                dt = new ReceiveStockDAO().GetReceiveStkCompleteSearch(poCode, stkNo, dateStPO, dateEnPO, dateStStk, dateEnStk);

                foreach (DataColumn dc in dt.Columns)
                {
                    dtt.Columns.Add(dc.ColumnName, dc.DataType);
                }

                var rows = dt.AsEnumerable().Skip((pageNo - 1) * pageNumPerPage).Take(pageNumPerPage);
                rows.CopyToDataTable(dtt, LoadOption.OverwriteChanges);

                this.RecountCount = dt.Rows.Count;
                this.StockView = dtt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        //public void GetReceiveStkCompleteView(int pageNo, int pageNumPerPage, string poCode, string stkNo)
        //{

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        DataTable dtt = new DataTable();
        //        dt = new ReceiveStockDAO().GetReceiveStkComplete();

        //        foreach (DataColumn dc in dt.Columns)
        //        {
        //            dtt.Columns.Add(dc.ColumnName, dc.DataType);
        //        }

        //        var rows = dt.AsEnumerable().Skip((pageNo - 1) * pageNumPerPage).Take(pageNumPerPage)
        //              .Where(r => r["PO_Code"].ToString().Contains(poCode) && r["Receive_Stk_No"].ToString().Contains(stkNo));
        //        rows.CopyToDataTable(dtt, LoadOption.OverwriteChanges);

        //        this.RecountCount = dt.Rows.Count;
        //        this.StockView = dtt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
         

        //}



        public void GetToReceiveStock()
        {

        }
    }
}
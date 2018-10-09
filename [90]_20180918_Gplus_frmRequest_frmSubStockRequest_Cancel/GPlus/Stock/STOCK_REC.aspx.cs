using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.Stock
{
    public partial class StockRec : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {



        }


        /// <summary>
        /// Search Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnCancelClick(object sender, EventArgs e)
        {
            PagingControl1.Visible = false;
            gvStk.DataSource = null;
            gvStk.DataBind();
        }
        /// <summary>
        /// Cancel Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnSearchClick(object sender, EventArgs e)
        {
            //BindData();
            PagingControl1.Visible = true;
        }
    }
}
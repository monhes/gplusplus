using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;


namespace GPlus
{
    public class Pagebase : System.Web.UI.Page
    {
        public string SelectedCulture
        {
            get
            {
                if (Session["SelectedCulture"] == null) Session["SelectedCulture"] = "th-TH";
                return Session["SelectedCulture"].ToString();
            }
            set
            {
                Session["SelectedCulture"] = value;
            }
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = new
                CultureInfo(this.SelectedCulture);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(this.SelectedCulture);

            base.InitializeCulture();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Title = "G-Plus";
            if (this.UserID == "" || this.UserName == "")
            {
                Response.Redirect("~/Default.aspx", true);
            }
        }

        public string DateFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["DateFormat"];
            }
        }

        public string DateTimeFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["DateTimeFormat"];
            }
        }

        public string CurrencyFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrencyFormat"];
            }
        }

        public string PageTitle
        {
            get
            {
                return ConfigurationManager.AppSettings["PageTitle"];
            }
        }

        public string PercentVat
        {
            get { return ConfigurationManager.AppSettings["PercentVat"]; }
        }

        public string PurchaseDivID
        {
            get { return ConfigurationManager.AppSettings["PurchaseDivID"]; }
        }



        public string PageID
        {
            get
            {
                if (Session["PageID"] == null)
                    Session["PageID"] = "";

                return Session["PageID"].ToString();
            }
            set
            {
                Session["PageID"] = value;
            }
        }

        public string UserID
        {
            get
            {
                if (Session["UserID"] == null)
                    Session["UserID"] = "";

                return Session["UserID"].ToString();
            }
            set
            {
                Session["UserID"] = value;
            }
        }


        public string OrgID
        {
            get
            {
                if (Session["OrgID"] == null)
                    Session["OrgID"] = "";

                return Session["OrgID"].ToString();
            }
            set
            {
                Session["OrgID"] = value;
            }
        }

        public string OrgName
        {
            get
            {
                if (Session["OrgName"] == null)
                    Session["OrgName"] = "";

                return Session["OrgName"].ToString();
            }
            set
            {
                Session["OrgName"] = value;
            }
        }

        public string UserName
        {
            get
            {
                if (Session["UserName"] == null)
                    Session["UserName"] = "";

                return Session["UserName"].ToString();
            }
            set
            {
                Session["UserName"] = value;
            }
        }

        public string FirstName
        {
            get
            {
                if (Session["FirstName"] == null)
                    Session["FirstName"] = "";

                return Session["FirstName"].ToString();
            }
            set
            {
                Session["FirstName"] = value;
            }
        }


        public string LastName
        {
            get
            {
                if (Session["LastName"] == null)
                    Session["LastName"] = "";

                return Session["LastName"].ToString();
            }
            set
            {
                Session["LastName"] = value;
            }
        }



        public bool IsAdmin
        {
            get
            {
                if (Session["IsAdmin"] == null)
                    Session["IsAdmin"] = false;

                return (bool)Session["IsAdmin"];
            }
            set
            {
                Session["IsAdmin"] = value;
            }
        }

        public DataRow UserProfile
        {
            get
            {
                return (DataRow)Session["UserProfile"];
            }
            set
            {
                Session["UserProfile"] = value;
            }
        }

        public DataTable Permission
        {
            get
            {
                return (DataTable)Session["Permission"];
            }
            set
            {
                Session["Permission"] = value;
            }
        }



        public string SortColumn
        {
            get
            {
                if (ViewState["SortColumn"] == null)
                    ViewState["SortColumn"] = "";

                return ViewState["SortColumn"].ToString();
            }
            set
            {
                ViewState["SortColumn"] = value;
            }
        }

        public string SortOrder
        {
            get
            {
                if (ViewState["SortOrder"] == null)
                    ViewState["SortOrder"] = "";

                return ViewState["SortOrder"].ToString();
            }
            set
            {
                ViewState["SortOrder"] = value;
            }
        }

        public void SetSortGridView(string sortColumn)
        {
            if (this.SortColumn == sortColumn) this.SortOrder = this.SortOrder == "ASC" ? "DESC" : "ASC";
            else this.SortOrder = "ASC";
            this.SortColumn = sortColumn;
        }

        public void GridViewSort(GridView gv)
        {
            GridViewRow headerRow = gv.HeaderRow;
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if ((this.SortColumn == gv.Columns[i].SortExpression) && (SortColumn != ""))
                {
                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                    if (this.SortOrder == "ASC")
                    {
                        img.ImageUrl = "~/Images/arrow_up.gif";
                    }
                    else
                    {
                        img.ImageUrl = "~/Images/arrow_down.gif";
                    }
                    headerRow.Cells[i].Controls.Add(new LiteralControl(" "));
                    headerRow.Cells[i].Controls.Add(img);
                }
            }
        }


        public void Export(GridView gv)
        {
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                for (int j = 0; j < gv.Columns.Count; j++)
                {
                    if (HttpUtility.HtmlDecode(gv.Columns[j].HeaderText).Trim().Length == 0 || gv.Columns[j].HeaderText.IndexOf("input") > -1)
                        gv.Rows[i].Cells[j].Text = "";
                    else
                        gv.Rows[i].Cells[j].Attributes.Add("class", "<style>{mso-number-format:\\@;}</style>");
                }
            }

            string contentType = "application/vnd.xls";

            //PrepareGridViewForExport(gv);

            HttpContext.Current.Response.Charset = "window-874";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=export.xls");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = contentType;

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a table to contain the grid 
                    Table table = new Table();

                    //  include the gridline settings
                    table.GridLines = gv.GridLines;

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        PrepareGridViewForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        PrepareGridViewForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        PrepareGridViewForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }

        }

        private void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            string name = String.Empty;

            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(HyperLink))
                {
                    l.Text = (gv.Controls[i] as HyperLink).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(ImageButton))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(HiddenField))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(TextBox))
                {
                    l.Text = (gv.Controls[i] as TextBox).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType().ToString().Contains("DataControlLinkButton"))
                {
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }

                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }

        public DataTable GetSampleData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Index");
            dt.Rows.Add(new object[] { "1" });
            dt.Rows.Add(new object[] { "2" });
            dt.Rows.Add(new object[] { "3" });

            return dt;
        }

        public void ShowMessageBox(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert(\"" + message + "\");", true);
        }

        #region Nin 21/03/2014

        public DataTable GetAccessMenu(string menuId, string userId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@User_ID", userId));
            param.Add(new SqlParameter("@Menu_ID", menuId));
            DataTable dt = new DataAccess.DatabaseHelper().ExecuteDataTable("sp_Gpluz_Account_USERGROUPMENU_SelectByID", param);
            return dt;
        }

        #endregion

    }
}
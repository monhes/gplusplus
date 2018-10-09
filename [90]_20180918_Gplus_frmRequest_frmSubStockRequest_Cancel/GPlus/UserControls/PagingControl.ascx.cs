using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class PagingControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.Page.IsPostBack && !this.Page.IsCallback) && (this.ddlCurrentPage.Items.Count == 0))
            {
                this.ddlCurrentPage.Items.Add("--");
                this.lblPageCount.Text = "--";
            }
        }


        // Properties
        public int CurrentPageIndex
        {
            get
            {
                int currentPage = 1;
                if (int.TryParse(ddlCurrentPage.SelectedValue, out currentPage))
                {
                }
                if (currentPage == 0) currentPage = 1;
                return currentPage;
            }
            set
            {
                this.ViewState["CurrentPageIndex"] = value;
                if (this.ddlCurrentPage.Items.FindByValue(this.ViewState["CurrentPageIndex"].ToString()) != null)
                {
                    this.ddlCurrentPage.SelectedValue = this.ViewState["CurrentPageIndex"].ToString();
                }
            }
        }

        public int PageSize
        {
            get
            {
                return int.Parse(ddlPageSize.SelectedValue);
            }
            set
            {
                Session["PageSize"] = value;
                if (int.Parse(ddlPageSize.SelectedValue) != value)
                {
                    ddlPageSize.SelectedValue = value.ToString();
                    OnCurrentPageIndexChanged(new EventArgs());
                }
            }
        }

        public int RecordCount
        {
            get
            {
                if (this.ViewState["ReCC" + this.ClientID] == null)
                {
                    return 0;
                }
                return int.Parse(this.ViewState["ReCC" + this.ClientID].ToString());
            }
            set
            {
                this.ViewState["ReCC" + this.ClientID] = value;
                this.lblRecord.Text = this.RecordCount.ToString();
                double maxPage = Math.Ceiling((double)(((double)this.RecordCount) / ((double)this.PageSize)));
                this.lblPageCount.Text = maxPage.ToString();
                int currentSelectedIndex = this.ddlCurrentPage.SelectedIndex;
                this.ddlCurrentPage.Items.Clear();
                if (maxPage > 0)
                {
                    for (double i = 1; i <= maxPage; i++)
                    {
                        this.ddlCurrentPage.Items.Add(i.ToString());
                    }
                }
                else
                {
                    this.ddlCurrentPage.Items.Add("--");
                    this.lblPageCount.Text = "--";
                }
                if (this.ddlCurrentPage.Items.Count > currentSelectedIndex)
                {
                    this.ddlCurrentPage.SelectedIndex = currentSelectedIndex;
                }
            }
        }

        #region Custom Event
        public event EventHandler CurrentPageIndexChanged;

        protected virtual void OnCurrentPageIndexChanged(EventArgs e)
        {
            if (CurrentPageIndexChanged != null)
                CurrentPageIndexChanged(this, e);
        }

        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPageIndex = Int32.Parse(ddlCurrentPage.SelectedItem.Text);

            OnCurrentPageIndexChanged(new EventArgs());
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = int.Parse(ddlPageSize.SelectedItem.Text);
            ddlCurrentPage.SelectedIndex = 0;
            OnCurrentPageIndexChanged(new EventArgs());
        }

        protected void btnFirst_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCurrentPage.SelectedIndex > 0 && this.RecordCount > 0)
            {
                ddlCurrentPage.SelectedIndex = 0;
                this.CurrentPageIndex = Int32.Parse(ddlCurrentPage.SelectedItem.Text);
                OnCurrentPageIndexChanged(new EventArgs());
            }
        }

        protected void btnPrevious_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCurrentPage.SelectedIndex > 0 && this.RecordCount > 0)
            {
                ddlCurrentPage.SelectedIndex -= 1;
                this.CurrentPageIndex = Int32.Parse(ddlCurrentPage.SelectedItem.Text);
                OnCurrentPageIndexChanged(new EventArgs());
            }
        }

        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCurrentPage.SelectedIndex < ddlCurrentPage.Items.Count - 1 && this.RecordCount > 0)
            {
                ddlCurrentPage.SelectedIndex += 1;
                this.CurrentPageIndex = Int32.Parse(ddlCurrentPage.SelectedItem.Text);
                OnCurrentPageIndexChanged(new EventArgs());
            }
        }

        protected void btnLast_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCurrentPage.SelectedIndex < ddlCurrentPage.Items.Count - 1 && this.RecordCount > 0)
            {
                ddlCurrentPage.SelectedIndex = ddlCurrentPage.Items.Count - 1;
                this.CurrentPageIndex = Int32.Parse(ddlCurrentPage.SelectedItem.Text);
                OnCurrentPageIndexChanged(new EventArgs());
            }
        }

        #endregion
    }
}
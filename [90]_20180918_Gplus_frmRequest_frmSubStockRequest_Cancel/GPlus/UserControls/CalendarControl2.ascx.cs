using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class CalendarControl2 : System.Web.UI.UserControl
    {
        private string FormatDateDefault = "dd/MM/yyyy";

        public override string ClientID
        {
            get { return txtDate.ClientID; }
        }

        public bool IsRequire
        {
            get
            {
                return rfvDate.Visible;
            }
            set
            {
                rfvDate.Visible = value;
                if (rfvDate.Visible)
                {
                    txtDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFEDB5");
                }
            }
        }

        public string ErrorObject
        {
            get
            {
                return rfvDate.ErrorMessage;
            }
            set
            {
                rfvDate.ErrorMessage = String.Format(rfvDate.ErrorMessage, value);
            }
        }

        public bool Enabled
        {
            set
            {
                btnShowCalendar.Enabled = value;
                txtDate.ReadOnly = !value;
                if (value)
                {
                    btnShowCalendar.Style.Add("cursor", "hand");
                    btnShowCalendar.ImageUrl = "../images/Commands/calendar_selector.gif";
                    txtDate.Style.Add("background-color", "");
                }
                else
                {
                    btnShowCalendar.Style.Add("cursor", "default");
                    btnShowCalendar.ImageUrl = "../images/Commands/calendar_selector_disable.gif";
                    txtDate.Style.Add("background-color", "#EEEEEE");
                }
            }
        }

        public string CssClass
        {
            get { return txtDate.CssClass; }
            set { txtDate.CssClass = value; }
        }

        public short TabIndex
        {
            get
            {
                ViewState["TAB_INDEX"] = txtDate.TabIndex;
                return (short)ViewState["TAB_INDEX"];
            }
            set
            {
                txtDate.TabIndex = value;
                btnShowCalendar.TabIndex = (short)(value + 1);
                ViewState["TAB_INDEX"] = value;
            }
        }

        public string Text
        {
            get { return txtDate.Text; }
            set { txtDate.Text = value; }
        }

        public System.DateTime Value
        {
            get
            {
                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    DateTime dtm = DateTime.ParseExact(txtDate.Text, FormatDateDefault, null);
                    return dtm;
                }
                return DateTime.MinValue;
            }
            set { txtDate.Text = value.ToString(FormatDateDefault); }
        }

        public void Clear()
        {
            Value = DateTime.MinValue;
            txtDate.Text = "";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            cdeMain = new  AjaxControlToolkit.CalendarExtender();
            cdeMain.Format = this.FormatDateDefault;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SetDate",
                "function SetDate(sender){var sDate = sender.get_selectedDate().localeFormat(sender._format);sender.value = sDate;}", true);
        }

        public void SetAttributesOnTextbox(string key, string value)
        {
            txtDate.Attributes.Add(key, value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class CalendarDependOnMonthYearControl : System.Web.UI.UserControl
    {
        private string FormatDateDefault = "dd/MM/yyyy";

        public override string ClientID
        {
            get { return txtDate.ClientID; }
        }

        public string ControlClientID
        {
            get { return tblCalendar.ClientID; }
        }

        public string ValidationGroup
        {
            get { return rfvDate.ValidationGroup; }
            set
            {
                rfvDate.ValidationGroup = value;
            }
        }

        public bool Display
        {
            get
            {
                return tblCalendar.Style["display"] != "";
            }
            set
            {
                if (value)
                    tblCalendar.Style["display"] = "";
                else
                    tblCalendar.Style["display"] = "none";
            }
        }

        public bool Enabled
        {
            get
            {
                return !tblCalendar.Disabled;
            }
            set
            {
                tblCalendar.Disabled = !value;
                txtDate.Enabled = value;
                //imgCalendar.Enabled = value;

            }
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
                rfvDate.ErrorMessage = value;
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
                ViewState["TAB_INDEX"] = value;
            }
        }

        public string Text
        {
            get
            {
                return GetDateDB(txtDate.Text);
            }
            set
            {
                txtDate.Text = GetDateCulture(value);
            }
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
            //imgCalendar.OnClientClick = "popUpCalendar(document.getElementById('" + imgCalendar.ClientID + "'), document.getElementById('" + txtDate.ClientID + "'), 'dd/mm/yyyy'); retutn false;";

            //imgCalendar.OnClientClick = "open_popup('" + Util.GetSiteRoot() + "UserControls/PopCalendar.aspx?cid=" + txtDate.ClientID + "', 180, 200, 'calendar', 'no', 'no', 'no');return false;";
            //cdeMain.Format = this.FormatDateDefault;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SetDate",
            //    "function SetDate(sender){var sDate = sender.get_selectedDate().localeFormat(sender._format);sender.value = sDate;}", true);
        }

        public void SetAttributesOnTextbox(string key, string value)
        {
            txtDate.Attributes.Add(key, value);
        }

        public string GetDateCulture(string date)
        {
            string[] arr = date.Split('/');
            if (arr.Length == 3)
            {
                int intYear = int.Parse(arr[2].Split(new string[] { " " }, StringSplitOptions.None)[0]);
                Pagebase pb = new Pagebase();

                //if (pb.SelectedCulture == "en-US")
                //{
                //    if (intYear > 2450) intYear -= 543;
                //}
                //else
                //{
                if (intYear < 2450) intYear += 543;
                //}

                return arr[0] + "/" + arr[1] + "/" + intYear.ToString();
            }
            else
                return "";
        }

        public string GetDateDB(string date)
        {
            string[] arr = date.Split('/');
            if (arr.Length == 3)
            {
                int intYear = int.Parse(arr[2]);
                if (intYear > 2450) intYear -= 543;

                return arr[0] + "/" + arr[1] + "/" + intYear.ToString();
            }
            else
                return "";
        }
    }
}
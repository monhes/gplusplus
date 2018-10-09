using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GPlus.MasterPage
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = ConfigurationManager.AppSettings["PageTitle"];
            if (!IsPostBack)
            {
                BindMenu();
                if (((Pagebase)Page).PageID.Trim().Length > 0)
                {
                    if (int.Parse(((Pagebase)Page).PageID) < 200)
                    {
                        MyAccordion.SelectedIndex = 0;
                        if (mnu1.FindItem(((Pagebase)Page).PageID) != null)
                            mnu1.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else if (int.Parse(((Pagebase)Page).PageID) < 300)
                    {
                        MyAccordion.SelectedIndex = 1;
                        if (mnu2.FindItem(((Pagebase)Page).PageID) != null)
                            mnu2.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else if (int.Parse(((Pagebase)Page).PageID) < 400)
                    {
                        MyAccordion.SelectedIndex = 2;
                        if (mnu3.FindItem(((Pagebase)Page).PageID) != null)
                            mnu3.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else if (int.Parse(((Pagebase)Page).PageID) < 500)
                    {
                        MyAccordion.SelectedIndex = 3;
                        if (mnu3.FindItem(((Pagebase)Page).PageID) != null)
                            mnu3.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else if (int.Parse(((Pagebase)Page).PageID) < 600)
                    {
                        MyAccordion.SelectedIndex = 4;
                        if (mnu3.FindItem(((Pagebase)Page).PageID) != null)
                            mnu3.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else if (int.Parse(((Pagebase)Page).PageID) < 700)
                    {
                        MyAccordion.SelectedIndex = 5;
                        if (mnu3.FindItem(((Pagebase)Page).PageID) != null)
                            mnu3.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else if (int.Parse(((Pagebase)Page).PageID) < 800)
                    {
                        MyAccordion.SelectedIndex = 6;
                        if (mnu3.FindItem(((Pagebase)Page).PageID) != null)
                            mnu3.FindItem(((Pagebase)Page).PageID).Selected = true;
                    }
                    else
                    {
                        MyAccordion.SelectedIndex = -1;
                    }
                }
                lblUser.Text = ((Pagebase)Page).FirstName + " " + ((Pagebase)Page).LastName;
            }
        }

        private void BindMenu()
        {
            Pagebase pb = new Pagebase();
            if (pb.UserID != "1")
            {
                AccordionPane1.Visible = false;
                AccordionPane2.Visible = false;
                AccordionPane3.Visible = false;
                AccordionPane4.Visible = false;
                AccordionPane5.Visible = false;
                AccordionPane6.Visible = false;
                //AccordionPane7.Visible = false;
                mnu1.Items.Clear();
                mnu2.Items.Clear();
                mnu3.Items.Clear();
                mnu4.Items.Clear();
                mnu5.Items.Clear();
                mnu6.Items.Clear();
                //mnu7.Items.Clear();

                //for (int index = 1; index < 8; index++)
                for (int index = 1; index < 7; index++)
                {
                    DataView dv = pb.Permission.DefaultView;
                    dv.RowFilter = "MenuGroup_ID = " + index.ToString();
                    AjaxControlToolkit.AccordionPane ap = (AjaxControlToolkit.AccordionPane)AccordionPane1.FindControl("AccordionPane" + index.ToString());
                    ap.Visible = dv.Count > 0;
                    Menu mnu = (Menu)ap.FindControl("mnu" + index.ToString());
                    for (int i = 0; i < dv.Count; i++)
                    {
                        if (mnu.FindItem(dv[i]["Menu_Name"].ToString()) == null)
                        {
                            MenuItem item = new MenuItem(dv[i]["Menu_Name"].ToString(), dv[i]["Menu_Code"].ToString());
                            item.NavigateUrl = dv[i]["Menu_Url"].ToString();

                            if (i+1 < dv.Count)
                            {
                                item.SeparatorImageUrl = "../Images/menu_separate.png";
                            }
                            mnu.Items.Add(item);
                        }
                    }
                }

            }

        }

    }

}
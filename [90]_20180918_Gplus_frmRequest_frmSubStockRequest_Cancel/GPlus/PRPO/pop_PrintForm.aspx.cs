using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PrintForm : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDepartment.Text = this.OrgName;
                txt1_Department.Text = this.OrgName;

                BindData();
            }
            if (Request["n"] != null)
            {
                btnOK.Enabled = false;
                btnRefreshSelect.Enabled = false;
                btnSelect1.Enabled = false;
                btnSelect2.Enabled = false;
            }

            btnSelect1.OnClientClick = Util.CreatePopUp
            (
                "pop_ProductPrintForm.aspx",
                new string[] { "ItemID", "PackID", "ItemCode", "ItemName" },
                new string[] { hdID1.ClientID, hdPackID1.ClientID, txtFormPrintCode.ClientID, txtFormPrintName.ClientID },
                "pop_ProductPrintForm"
            );

            btnSelect2.OnClientClick = Util.CreatePopUp
            (
                "pop_ProductPrintForm.aspx",
                new string[] { "ItemID", "PackID", "ItemCode", "ItemName" },
                new string[] { hdID2.ClientID, hdPackID2.ClientID, txt1_FormPrintCode.ClientID, txt1_FormPrintName.ClientID },
                "pop_ProductPrintForm"
            );

            //btnSelect1.OnClientClick = "open_popup('pop_ProductPrintForm.aspx?id=" + hdID1.ClientID +
            //            "&code=" + txtFormPrintCode.ClientID + "&name=" + txtFormPrintName.ClientID
            //            + "&pack=" + hdPackID1.ClientID + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";

            //btnSelect2.OnClientClick = "open_popup('pop_ProductPrintForm.aspx?id=" + hdID2.ClientID +
            //           "&code=" + txt1_FormPrintCode.ClientID + "&name=" + txt1_FormPrintName.ClientID
            //           + "&pack=" + hdPackID2.ClientID + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";

            // แบบพิมพ์เดิม
            if (rblType.SelectedIndex == 1)
            {
                if (rdb1_FormBorrowType1.Checked)
                {
                    DisabledOldFormBorrowType2();
                    EnableOldFormBorrowType1();
                }

                if (rdb1_FormBorrowType2.Checked)
                {
                    DisabledOldFormBorrowType1();
                    EnableOldFormBorrowType2();
                }
            }
            // แบบพิมพ์ใหม่
            else if (rblType.SelectedIndex == 0)
            {
                if (rblFormBorrowType1.Checked)
                {
                    DisabledNewFormBorrowType2();
                    EnableNewFormBorrowType1();
                }

                if (rdbFormBorrowType2.Checked)
                {
                    DisabledNewFormBorrowType1();
                    EnableNewFormBorrowType2();
                }
            }
         
        }

        private void BindData()
        {
            DataAccess.PRDAO db = new DataAccess.PRDAO();
            string prID = "";
            string poID = "";
            if (Request["prid"] != null) prID = Request["prid"];
            if (Request["poid"] != null) poID = Request["poid"];
            DataTable dt = db.GetFromPrint(prID, poID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                hdID.Value = dr["PR_FormPrint_ID"].ToString();
                if (rblType.Items.FindByValue(dr["Form_Type"].ToString()) != null)
                {
                    rblType.SelectedValue = dr["Form_Type"].ToString();
                    rblType_SelectedIndexChanged(this, new EventArgs());
                }
                // แบบพิมพ์ใหม่
                if (rblType.SelectedIndex == 0)
                {
                    btnSelect1.OnClientClick = "open_popup('pop_ProductPrintForm.aspx?id="+hdID1.ClientID+
                        "&code="+txtFormPrintCode.ClientID+"&name="+txtFormPrintName.ClientID+"', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
                    txtFormPrintCode.Text = dr["FormPrint_Code"].ToString();
                    txtFormPrintName.Text = dr["FormPrint_Name"].ToString();
                    BindPackage();

                    if (rblFormat.Items.FindByValue(dr["Format"].ToString()) != null)
                        rblFormat.SelectedValue = dr["Format"].ToString();

                    txtPaperType.Text = dr["Paper_Type"].ToString();
                    txtPaperColor.Text = dr["Paper_Color"].ToString();
                    txtPaperGram.Text = dr["Paper_Gram"].ToString();

                    txtFontColor.Text = dr["Font_Color"].ToString();
                    if (rblPrintType.Items.FindByValue(dr["Print_Type"].ToString()) != null)
                        rblPrintType.SelectedValue = dr["Print_Type"].ToString();

                    rblFormBorrowType1.Checked = dr["FormBorrow_Type"].ToString() == "1";
                    rdbFormBorrowType2.Checked = dr["FormBorrow_Type"].ToString() == "2";

                    if(dr["Borrow_Quantity"].ToString().Trim().Length > 0)
                        txtBorrowQuantity.Text = ((decimal)dr["Borrow_Quantity"]).ToString("0");

                    if (ddlBorrowUnit1.Items.FindByValue(dr["Borrow_Unit_ID"].ToString()) != null)
                        ddlBorrowUnit1.SelectedValue = dr["Borrow_Unit_ID"].ToString();

                    if(dr["Borrow_Month_Quantity"].ToString().Trim().Length > 0)
                        txtBorrowMonthQuantity.Text = ((decimal)dr["Borrow_Month_Quantity"]).ToString("0");

                    if(dr["Borrow_First_Quantity"].ToString().Trim().Length > 0)
                    txtBorrowFirstQuantity.Text = ((decimal)dr["Borrow_First_Quantity"]).ToString("0");

                    if (ddlBorrowTypeUnit2.Items.FindByValue(dr["Borrow_Month_Unit_ID"].ToString()) != null)
                    {
                        ddlBorrowTypeUnit2.SelectedValue = dr["Borrow_Month_Unit_ID"].ToString();
                        ddlOrederTypeUnit.SelectedValue = dr["Borrow_Month_Unit_ID"].ToString();
                    }

                    if (dr["FormBorrow_Type"].ToString() == "2")
                    {
                        if (dr["Unit_Quantity"].ToString().Trim().Length > 0)
                            txtOredrQuantity.Text = ((int)dr["Unit_Quantity"]).ToString("0");
                    }

                    txtRemark.Text = dr["Remark"].ToString();

                    // Begin Green Edit
                    tbSizeDetailNew.Text = dr["Size_Detail"].ToString();
                    if (Session["print_form_hdID1"] != null)
                        hdID1.Value = Session["print_form_hdID1"].ToString();
                    // End Green Edit
                }
                // แบบพิมพ์เดิม
                else
                {
                    btnSelect1.OnClientClick = "open_popup('pop_ProductPrintForm.aspx?id=" + hdID2.ClientID +
                        "&code=" + txt1_FormPrintCode.ClientID + "&name=" + txt1_FormPrintName.ClientID + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
                    txt1_FormPrintCode.Text = dr["FormPrint_Code"].ToString();
                    txt1_FormPrintName.Text = dr["FormPrint_Name"].ToString();
                    BindPackage();

                    rdb1_FormBorrowType1.Checked = dr["FormBorrow_Type"].ToString() == "1";
                    rdb1_FormBorrowType2.Checked = dr["FormBorrow_Type"].ToString() == "2";

                    if(dr["Borrow_Quantity"].ToString().Trim().Length > 0)
                        txt1_BorrowQuantity.Text = ((decimal)dr["Borrow_Quantity"]).ToString("0");

                    if(dr["Borrow_Month_Quantity"].ToString().Trim().Length > 0)
                        txt1_MonthQuantity.Text = ((decimal)dr["Borrow_Month_Quantity"]).ToString("0");

                    if (ddl1_BorrowUnit2.Items.FindByValue(dr["Borrow_Month_Unit_ID"].ToString()) != null)
                    {
                        ddl1_BorrowUnit2.SelectedValue = dr["Borrow_Month_Unit_ID"].ToString();
                        ddl1_OrderUnit2.SelectedValue = dr["Borrow_Month_Unit_ID"].ToString();
                    }
                    if (dr["FormBorrow_Type"].ToString() == "2")
                    {
                        if (dr["Unit_Quantity"].ToString().Trim().Length > 0)
                            txt1_OrderQuantity.Text = ((int)dr["Unit_Quantity"]).ToString("0");
                    }

                    chk1_IsRequestModify.Checked = dr["Is_Request_Modify"].ToString() == "1";
                    chk1_IsFixedContent.Checked = dr["Is_Fixed_Content"].ToString() == "1";

                    chk1_IsFont.Checked = dr["Is_Paper"].ToString() == "1";
                    chk1_IsPaper.Checked = dr["Is_Font"].ToString() == "1";

                    // Begin Green Edit
                    tbRequestModifyDesc.Text = dr["Request_ModifyDesc"].ToString();
                    tbSizeDetail.Text = dr["Size_Detail"].ToString();
                    if (dr["Borrow_First_Quantity"].ToString().Trim().Length > 0)
                        txtBorrowFirstQuantityOld.Text = ((decimal)dr["Borrow_First_Quantity"]).ToString("0");

                    if (Session["print_form_hdID2"] != null)
                        hdID2.Value = Session["print_form_hdID2"].ToString();

                    txt1_PaperType.Text = dr["Paper_Type"].ToString();
                    txt1_PaperColor.Text = dr["Paper_Color"].ToString();
                    txt1_PaperGram.Text = dr["Paper_Gram"].ToString();
                    txt1_FontColor.Text = dr["Font_Color"].ToString();
                    txt1_Remark.Text = dr["Remark"].ToString();
                    txtRemark2.Text = dr["Remark2"].ToString();
                    rbl1_PrintType.SelectedValue = dr["Print_Type"].ToString();

                    if (rdb1_FormBorrowType1.Checked)
                    {
                        EnableOldFormBorrowType1();
                        DisabledOldFormBorrowType2();
                    }

                    if (rdb1_FormBorrowType2.Checked)
                    {
                        EnableOldFormBorrowType2();
                        DisabledOldFormBorrowType1();
                    }
                    // End Green Edit
                }

                rdbBorrowType0.Checked = dr["Borrow_Type"].ToString() == "0";
                rdbBorrowType1.Checked = dr["Borrow_Type"].ToString() == "1";
                rdbBorrowType2.Checked = dr["Borrow_Type"].ToString() == "2";
                rdbBorrowType3.Checked = dr["Borrow_Type"].ToString() == "3";

                txtRemark2.Text = dr["Remark2"].ToString();
                if(dr["NewBorrow_Date"].ToString().Trim().Length > 0)
                    ccNewBorrowDate.Text = ((DateTime)dr["NewBorrow_Date"]).ToString(this.DateFormat);

            }
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.Items[0].Selected)
            {
                pnlNew.Visible = true;
                pnlOld.Visible = false;
            }
            else
            {
                pnlNew.Visible = false;
                pnlOld.Visible = true;

                // Begin Green Edit
                if (rdb1_FormBorrowType1.Checked)
                {
                    DisabledOldFormBorrowType2();
                    EnableOldFormBorrowType1();
                }

                if (rdb1_FormBorrowType2.Checked)
                {
                    DisabledOldFormBorrowType1();
                    EnableOldFormBorrowType2();
                }
                // End Green Edit

            }
            BindPackage();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DataAccess.PRDAO db = new DataAccess.PRDAO();
            string prID = "";
            string poID = "";
            if(Request["prid"] != null) prID = Request["prid"];
            if(Request["poid"] != null) poID = Request["poid"];
            int unit_qty = 0;

            //Add 19092013
            if (rblType.SelectedIndex == 0)
            {
                if (rblFormBorrowType1.Checked == true)
                {
                    if (int.TryParse(txtBorrowQuantity.Text, out unit_qty) == false)
                    {
                        ShowMessageBox("กรุณาระบุจำนวนพิมพ์ให้ถูกต้อง");
                        return;
                    }

                }

                if (rdbFormBorrowType2.Checked == true)
                {
                    if (int.TryParse(txtOredrQuantity.Text, out unit_qty) == false)
                    {
                        ShowMessageBox("กรุณาระบุจำนวนสั่งให้ถูกต้อง");
                        return;
                    }
                }
            }
            // แบบพิมพ์เดิม
            if (rblType.SelectedIndex == 1)
            {

                if (rdb1_FormBorrowType1.Checked == true)
                {
                    if (int.TryParse(txt1_BorrowQuantity.Text, out unit_qty) == false)
                    {
                        ShowMessageBox("กรุณาระบุจำนวนพิมพ์ให้ถูกต้อง");
                        return;
                    }
                }

                if (rdb1_FormBorrowType2.Checked == true)
                {
                    if (int.TryParse(txt1_OrderQuantity.Text, out unit_qty) == false)
                    {
                        ShowMessageBox("กรุณาระบุจำนวนสั่งให้ถูกต้อง");
                        return;
                    }
                }
            }
            

            string formBorrowType = "";

            string hdOrderAmount = "";
            string borrowType = "";
             
            if(rdbBorrowType0.Checked) borrowType = "0";
                else if(rdbBorrowType1.Checked) borrowType = "1";
                else if(rdbBorrowType2.Checked) borrowType = "2";
                else if(rdbBorrowType3.Checked) borrowType = "3";
            // แบบพิม์พ์ใหม่

            if (rblType.SelectedIndex == 0)
            {
                 if(rblFormBorrowType1.Checked) formBorrowType = "1";
                else if(rdbFormBorrowType2.Checked) formBorrowType = "2";

                 if (hdID.Value.Trim().Length == 0)
                 {
                   hdID.Value =  db.AddFromPrint(prID, poID, txtFormPrintCode.Text, txtFormPrintName.Text, "0", rblFormat.SelectedValue, txtPaperType.Text,
                         txtPaperColor.Text, txtPaperGram.Text, txtFontColor.Text, rblPrintType.SelectedValue, borrowType, ccNewBorrowDate.Value,
                         txtRemark.Text, formBorrowType, txtBorrowQuantity.Text, txtBorrowMonthQuantity.Text, txtBorrowFirstQuantity.Text, ddlBorrowUnit1.SelectedValue,
                         ddlBorrowTypeUnit2.SelectedValue, "0", "0", "0", "0", txtRemark2.Text, unit_qty, 
                         // Begin Green Edit 
                         "", 
                         tbSizeDetailNew.Text
                         // End Green Edit
                         );
                 }
                 else
                 {
                     db.UpdateFromPrint(hdID.Value, txtFormPrintCode.Text, txtFormPrintName.Text, "0", rblFormat.SelectedValue, txtPaperType.Text,
                         txtPaperColor.Text, txtPaperGram.Text, txtFontColor.Text, rblPrintType.SelectedValue, borrowType, ccNewBorrowDate.Value,
                         txtRemark.Text, formBorrowType, txtBorrowQuantity.Text, txtBorrowMonthQuantity.Text, txtBorrowFirstQuantity.Text, ddlBorrowUnit1.SelectedValue,
                         ddlBorrowTypeUnit2.SelectedValue, "0", "0", "0", "0", txtRemark2.Text, unit_qty, 
                         // Begin Green Edit
                         "", 
                         tbSizeDetailNew.Text
                         // End Green Edit
                    );
                 }

                 string packID = "0", quantity = "0";
                 if (rblFormBorrowType1.Checked)
                 {
                     quantity = txtBorrowQuantity.Text;
                     packID = ddlBorrowUnit1.SelectedValue;
                 }
                 else
                 {
                     quantity = txtBorrowMonthQuantity.Text;
                     packID = ddlBorrowTypeUnit2.SelectedValue;
                 }
                 if (poID.Trim().Length == 0)
                     db.UpdatePRItemForFormPrint(prID, hdID.Value, hdID1.Value, packID, hdPrice.Value, quantity, "", "", "", "", "", "", "", "");
                 else
                 {
                     // Begin Green Edit
                     Session["print_form_hdID1"] = hdID1.Value;
                     // End Green Edit
                     new DataAccess.PODAO().UpdatePOItemForFormPrint(poID, hdID.Value, hdID1.Value, packID, hdPrice.Value, quantity, "", "", "", "", "", "", "", "");
                 }

                 // Begin Green Edit
                 if (rblFormBorrowType1.Checked)
                 {
                     hdOrderAmount = txtBorrowQuantity.Text;
                 }
                 else if (rdbFormBorrowType2.Checked)
                 {
                     hdOrderAmount = txtOredrQuantity.Text;
                 }
                // End Green Edit
            }
            // แบบพิมพ์เดิม
            else
            {
                 if(rdb1_FormBorrowType1.Checked) formBorrowType = "1";
                else if(rdb1_FormBorrowType2.Checked) formBorrowType = "2";

                 if (hdID.Value.Trim().Length == 0)
                 {
                     hdID.Value = db.AddFromPrint(
                         prID, 
                         poID, 
                         txt1_FormPrintCode.Text, 
                         txt1_FormPrintName.Text, 
                         "1", 
                         "0", 
                         txt1_PaperType.Text, 
                         txt1_PaperColor.Text,
                         txt1_PaperGram.Text, 
                         txt1_FontColor.Text, 
                         rbl1_PrintType.SelectedValue, 
                         borrowType, 
                         ccNewBorrowDate.Value, 
                         txt1_Remark.Text,
                         formBorrowType, 
                         txt1_BorrowQuantity.Text, 
                         txt1_MonthQuantity.Text, 
                         txtBorrowFirstQuantityOld.Text, 
                         ddl1_BorrowUnit.SelectedValue, 
                         ddl1_BorrowUnit2.SelectedValue,
                         chk1_IsRequestModify.Checked ? "1" : "0", 
                         chk1_IsFixedContent.Checked ? "1" : "0", 
                         chk1_IsPaper.Checked ? "1" : "0", 
                         chk1_IsFont.Checked ? "1" : "0",
                         txtRemark2.Text, unit_qty, 
                         // Begin Green Edit
                         tbRequestModifyDesc.Text,      // ลักษณะการแก้ไข
                         tbSizeDetail.Text              // ขนาด
                         // End Green Edit
                    );
                 }
                 else
                 {
                     db.UpdateFromPrint(hdID.Value, txt1_FormPrintCode.Text, txt1_FormPrintName.Text, "1", "0", txt1_PaperType.Text, txt1_PaperColor.Text,
                        txt1_PaperGram.Text, txt1_FontColor.Text, rbl1_PrintType.SelectedValue, borrowType, ccNewBorrowDate.Value, txt1_Remark.Text,
                        formBorrowType, txt1_BorrowQuantity.Text, txt1_MonthQuantity.Text, txtBorrowFirstQuantityOld.Text, ddl1_BorrowUnit.SelectedValue, ddl1_BorrowUnit2.SelectedValue,
                        chk1_IsRequestModify.Checked ? "1" : "0", chk1_IsFixedContent.Checked ? "1" : "0", chk1_IsPaper.Checked ? "1" : "0", chk1_IsFont.Checked ? "1" : "0",
                        txtRemark2.Text, unit_qty, 
                        // Begin Green Edit
                        tbRequestModifyDesc.Text, 
                        tbSizeDetail.Text
                        // End Green Edit
                        );
                 }

                string packID = "0", quantity = "0";
                
                 if (rdb1_FormBorrowType1.Checked)
                 {
                     quantity = txt1_BorrowQuantity.Text;
                     packID = ddl1_BorrowUnit.SelectedValue;
                 }
                 else
                 {
                     quantity = txt1_MonthQuantity.Text;
                     packID = ddl1_BorrowUnit2.SelectedValue;
               
                 }

                 if (poID.Trim().Length == 0)
                     db.UpdatePRItemForFormPrint(prID, hdID.Value, hdID2.Value, packID, hdPrice.Value, quantity, "", "", "", "", "", "", "", "");
                 else
                 {
                     // Begin Green Edit
                     Session["print_form_hdID2"] = hdID2.Value;
                     // End Green Edit
                     new DataAccess.PODAO().UpdatePOItemForFormPrint(poID, hdID.Value, hdID2.Value, packID, hdPrice.Value, quantity, "", "", "", "", "", "", "", "");
                 }
                    
                // Begin Green Edit
                 if (rdb1_FormBorrowType1.Checked)
                 {
                     hdOrderAmount = txt1_BorrowQuantity.Text;
                 }
                 else if (rdb1_FormBorrowType2.Checked)
                 {
                     hdOrderAmount = txt1_OrderQuantity.Text;
                 }
                // End Green Edit
            }

            // Begin Green Edit
            if (hdOrderAmount == "")
                hdOrderAmount = "0";
            // End Green Edit

            ScriptManager.RegisterStartupScript(
                this, 
                this.GetType(), 
                "cl", 
                "if (window.opener) {" + 
                "   window.opener.document.getElementById('hdIsPrintFormOrProduct').value = 'printForm';" +
                "   window.opener.document.getElementById('hdOrderAmount').value = " + hdOrderAmount + ";" +
                "   window.opener.document.getElementById('btnRefreshI').click();" +
                "}" +
                "window.close();", 
                true
            );
        }

        protected void btnRefreshSelect_Click(object sender, EventArgs e)
        {
            BindPackage();
        }

        private void BindPackage()
        {
            DataTable dtUnit = null;
            if (rblType.SelectedIndex == 0)
                dtUnit = new DataAccess.ItemDAO().GetItemPackID(txtFormPrintCode.Text);
            else
                dtUnit = new DataAccess.ItemDAO().GetItemPackID(txt1_FormPrintCode.Text);
            if (dtUnit.Rows.Count > 0)
            {
                ddl1_BorrowUnit.DataSource = dtUnit;
                ddl1_BorrowUnit.DataBind();
                ddl1_BorrowUnit2.DataSource = dtUnit;
                ddl1_BorrowUnit2.DataBind();

                ddlBorrowTypeUnit2.DataSource = dtUnit;
                ddlBorrowTypeUnit2.DataBind();

                ddlBorrowUnit1.DataSource = dtUnit;
                ddlBorrowUnit1.DataBind();

                ddlOrederTypeUnit.DataSource = dtUnit;
                ddlOrederTypeUnit.DataBind();

                ddl1_OrderUnit2.DataSource = dtUnit;
                ddl1_OrderUnit2.DataBind();

                if (dtUnit.Rows.Count > 0)
                {
                    hdPrice.Value = dtUnit.Rows[0]["Avg_Cost"].ToString();
                }

                try
                {
                    if (rblType.SelectedIndex == 0)
                    {
                        ddl1_BorrowUnit.SelectedValue = hdPackID1.Value;
                        ddl1_BorrowUnit2.SelectedValue = hdPackID1.Value;
                        ddlBorrowTypeUnit2.SelectedValue = hdPackID1.Value;
                        ddlBorrowUnit1.SelectedValue = hdPackID1.Value;
                        ddlOrederTypeUnit.SelectedValue = hdPackID1.Value;
                        ddl1_OrderUnit2.SelectedValue = hdPackID1.Value;
                    }
                    else
                    {
                        ddl1_BorrowUnit.SelectedValue = hdPackID2.Value;
                        ddl1_BorrowUnit2.SelectedValue = hdPackID2.Value;
                        ddlBorrowTypeUnit2.SelectedValue = hdPackID2.Value;
                        ddlBorrowUnit1.SelectedValue = hdPackID2.Value;
                        ddlOrederTypeUnit.SelectedValue = hdPackID2.Value;
                        ddl1_OrderUnit2.SelectedValue = hdPackID2.Value;
                    }
                }
                catch { }
            }
        }

        #region Nin 19092013

        protected void CalOrderQty(object sender, EventArgs e)
        {
            txtOredrQuantity.Text = ((Convert.ToInt32(txtBorrowMonthQuantity.Text == "" ? "0" : txtBorrowMonthQuantity.Text) * 6) + Convert.ToInt32(txtBorrowFirstQuantity.Text == "" ? "0" : txtBorrowFirstQuantity.Text)).ToString();
        }

        protected void txt1_MonthQuantity_TextChanged(object sender, EventArgs e)
        {
            txt1_OrderQuantity.Text = (Convert.ToInt32(txt1_MonthQuantity.Text == "" ? "0" : txt1_MonthQuantity.Text) * 6).ToString();
        }

        protected void ddlBorrowTypeUnit2_IndexChanged(object sender, EventArgs e)
        {
            ddlOrederTypeUnit.SelectedIndex = ddlBorrowTypeUnit2.SelectedIndex;
        }

        protected void ddl1_BorrowUnit2_IndexChanged(object sender, EventArgs e)
        {
            ddl1_OrderUnit2.SelectedIndex = ddl1_BorrowUnit2.SelectedIndex;
        }

        #endregion

        #region Green

        private void DisabledOldFormBorrowType1()
        {
            txt1_BorrowQuantity.Text = "";
            txt1_BorrowQuantity.Enabled = false;
            ddl1_BorrowUnit.Enabled = false;
        }

        private void DisabledOldFormBorrowType2()
        {
            txt1_MonthQuantity.Text = "";
            txtBorrowFirstQuantityOld.Text = "";
            txt1_OrderQuantity.Text = "";

            txt1_MonthQuantity.Enabled = false;
            txtBorrowFirstQuantityOld.Enabled = false;
            txt1_OrderQuantity.Enabled = false;
            ddl1_BorrowUnit2.Enabled = false;
        }

        private void DisabledNewFormBorrowType1()
        {
            txtBorrowQuantity.Text = "";
            ddlBorrowUnit1.Enabled = false;
            txtBorrowQuantity.Enabled = false;
        }

        private void DisabledNewFormBorrowType2()
        {
            txtBorrowMonthQuantity.Text = "";
            txtBorrowFirstQuantity.Text = "";
            ddlBorrowTypeUnit2.Enabled = false;
            txtOredrQuantity.Text = "";

            txtBorrowMonthQuantity.Enabled = false;
            txtBorrowFirstQuantity.Enabled = false;
            txtOredrQuantity.Enabled = false;
        }

        private void EnableNewFormBorrowType1()
        {
            ddlBorrowUnit1.Enabled = true;
            txtBorrowQuantity.Enabled = true;
        }

        private void EnableNewFormBorrowType2()
        {
            ddlBorrowTypeUnit2.Enabled = true;
            txtBorrowMonthQuantity.Enabled = true;
            txtBorrowFirstQuantity.Enabled = true;
            txtOredrQuantity.Enabled = true;
        }

        private void EnableOldFormBorrowType2()
        {
            txt1_MonthQuantity.Enabled = true;
            txtBorrowFirstQuantityOld.Enabled = true;
            txt1_OrderQuantity.Enabled = true;
            ddl1_BorrowUnit2.Enabled = true;
        }

        private void EnableOldFormBorrowType1()
        {
            txt1_BorrowQuantity.Enabled = true;
            ddl1_BorrowUnit.Enabled = true;
        }

        #endregion
    }
}
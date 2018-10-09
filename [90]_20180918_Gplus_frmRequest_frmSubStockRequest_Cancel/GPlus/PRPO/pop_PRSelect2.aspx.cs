using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using GPlus.PRPO.PRPOHelper;

namespace GPlus.PRPO
{
    public partial class pop_PRSelect2 : Pagebase
    {
        public string Script
        {
            get
            {
                if (ViewState["Script"] == null)
                    ViewState["Script"] = "";

                return ViewState["Script"].ToString();
            }
            set
            {
                ViewState["Script"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropdown();
                BindData();
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindDropdown()
        {
            ddlSupplier.DataSource = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
            ddlSupplier.DataTextField = "Supplier_Name";
            ddlSupplier.DataValueField = "Supplier_ID";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlSupplier.SelectedIndex = 0;
            ccFrom.Text = "";
            ccTo.Text = "";
            //chkApprove.Checked = true;
            tbPRCode.Text = "";
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.PRDAO().GetPRForm1andItem
            (
                ccFrom.Text
                , ccTo.Text
                , ddlSupplier.SelectedValue
                , "2"//(chkApprove.Checked ? "2" : "")       // Status
                , Request["prtype"]                     // type
                , PagingControl1.CurrentPageIndex
                , PagingControl1.PageSize
                , this.SortColumn
                , this.SortOrder
                , tbPRCode.Text
            );

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPR.DataSource = ds.Tables[0];
            gvPR.DataBind();
        }

        protected void gvPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.Script = "function CheckAllP(state){";
                ((CheckBox)e.Row.FindControl("chkDH")).Attributes.Add("onclick", "CheckAllP(this.checked);");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv["Unit_Quantity"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((decimal)drv["Unit_Quantity"]).ToString("#,##0");

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateFormat);

                //switch (drv["Status"].ToString())
                //{
                //    case "0": e.Row.Cells[9].Text = "ยกเลิก"; break;
                //    case "1": e.Row.Cells[9].Text = "รออนุมัติ"; break;
                //    case "2": e.Row.Cells[9].Text = "อนุมัติ"; break;
                //    case "3": e.Row.Cells[9].Text = "ไม่อนุมัติ"; break;
                //    case "4": e.Row.Cells[9].Text = "จัดซื้อไม่ดำเนินการ"; break;
                //    case "5": e.Row.Cells[9].Text = "ออก PO แล้ว"; break;
                //}

                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                HiddenField hdItemID = (HiddenField)e.Row.FindControl("hdItemID");
                // Begin Green Edit
                HiddenField hdInvItemID = (HiddenField)e.Row.FindControl("hdInvItemID");
                HiddenField hdPackID = (HiddenField)e.Row.FindControl("hdPackID");
                HiddenField hdUnitPrice = (HiddenField)e.Row.FindControl("hdUnitPrice");
                HiddenField hfPrintFormId = e.Row.FindControl("hfPrintFormID") as HiddenField;

                hfPrintFormId.Value = drv["PR_FormPrint_ID"].ToString();
                hdInvItemID.Value = drv["Inv_ItemID"].ToString();
                hdPackID.Value = drv["Pack_ID"].ToString();
                hdUnitPrice.Value = PRPOHelper.PRPOUtility.To2PointString(drv["Unit_Price"].ToString());
                // End Green Edit

                hdID.Value = drv["PR_ID"].ToString();
                hdItemID.Value = drv["PRItem_ID"].ToString();

                CheckBox chkD = (CheckBox)e.Row.FindControl("chkD");
                this.Script += "document.getElementById('" + chkD.ClientID + "').checked = state;";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                this.Script += "}";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chk", this.Script, true);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            PRPOActualTable pat = PRPOActualTableFactory.CreateTable(Request["main"], Request["prtype"]);

            // ตรวจสอบรายการแบบพิมพ์ของ สั่งซื้อ ต้องไม่มีร่วมกับรายการอื่น
            bool hasPrintForm = false;
            int count = 0;
            if (pat is PRPOPurchaseActualTable)
            {
                for (int i = 0; i < gvPR.Rows.Count; i++)
                {
                    CheckBox chkD = (CheckBox)gvPR.Rows[i].FindControl("chkD");
                    if (chkD.Checked)
                    {
                        HiddenField hfPrintFormID = (HiddenField)gvPR.Rows[i].FindControl("hfPrintFormID");

                        if (!string.IsNullOrEmpty(hfPrintFormID.Value))
                            hasPrintForm = true;

                        count++;
                    }
                }


                if ((hasPrintForm && pat.Table.Rows.Count >= 1) || (hasPrintForm && count > 1))
                {
                    ScriptManager.RegisterStartupScript
                    (
                        this,
                        GetType(),
                        "popPrSelect",
                        "alert('รายการแบบพิมพ์ไม่สามารถร่วมกับรายการอื่นได้');",
                        true
                    );
                    return;
                }
            }

            for (int i = 0; i < gvPR.Rows.Count; i++)
            {
                CheckBox chkD = (CheckBox)gvPR.Rows[i].FindControl("chkD");
                if (chkD.Checked)
                {
                    HiddenField hdPrID = (HiddenField)gvPR.Rows[i].FindControl("hdID");               // prId
                    HiddenField hdPrItemID = (HiddenField)gvPR.Rows[i].FindControl("hdItemID");       // prItemId
                    HiddenField hdInvItemID = (HiddenField)gvPR.Rows[i].FindControl("hdInvItemID");
                    HiddenField hdPackID = (HiddenField)gvPR.Rows[i].FindControl("hdPackID");
                    HiddenField hdUnitPrice = (HiddenField)gvPR.Rows[i].FindControl("hdUnitPrice");
                    HiddenField hfPrintFormID = (HiddenField)gvPR.Rows[i].FindControl("hfPrintFormID");

                    DataRow[] row = pat.FindItem(hdInvItemID.Value, hdPackID.Value);
                    if (row.Length > 0)
                    {
                        row = pat.FindItem(hdInvItemID.Value, hdPackID.Value, hdPrID.Value, hdPrItemID.Value);

                        if (row.Length == 0)
                        {
                            pat.AddItem
                            (
                                hdInvItemID.Value, hdPackID.Value, hdPrID.Value, hdPrItemID.Value, gvPR.Rows[i].Cells[2].Text,
                                gvPR.Rows[i].Cells[3].Text, gvPR.Rows[i].Cells[5].Text, hdUnitPrice.Value, gvPR.Rows[i].Cells[4].Text,
                                PRPOPopup.PR, "N"
                            );
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript
                            (
                                this,
                                GetType(),
                                "popPrSelect",
                                "alert('รายการที่เลือกซ้ำ');",
                                true
                             );
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hfPrintFormID.Value))
                        {
                            PRPOPrintFormTable ppft = new PRPOPrintFormTable(PRPOSession.PrintFormTable);
                            DataTable dtPrintForm = new PODAO2().GetPrintForm(Convert.ToInt32(hfPrintFormID.Value));

                            if (dtPrintForm.Rows.Count > 0)
                            {
                                DataRow r = dtPrintForm.Rows[0];

                                string borrowQuantity = (r["BorrowQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowQuantity"]).ToString("0") : "";
                                string borrowMonthQuantity = (r["BorrowMonthQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowMonthQuantity"]).ToString("0") : "";
                                string borrowFirstQuantity = (r["BorrowFirstQuantity"] as decimal? != null) ? Convert.ToDecimal(r["BorrowFirstQuantity"]).ToString("0") : "";

                                string newBorrowDate = string.Format("{0:dd/MM/YYYY}", r["NewBorrowDate"].ToString());

                                DataTable dtPrItemPrintForm = new PODAO2().GetPrItemPrintForm(Convert.ToInt32(hdPrItemID.Value));

                                ppft.AddItem
                                (
                                      ""
                                    , ""
                                    , ""
                                    , r["FormPrintCode"].ToString()
                                    , r["FormPrintName"].ToString()
                                    , r["FormType"].ToString()
                                    , r["Format"].ToString()
                                    , r["PaperType"].ToString()
                                    , r["PaperGram"].ToString()
                                    , r["PaperColor"].ToString()
                                    , r["FontColor"].ToString()
                                    , r["PrintType"].ToString()
                                    , r["BorrowType"].ToString()
                                    , newBorrowDate
                                    , r["Remark"].ToString()
                                    , r["FormBorrowType"].ToString()
                                    , borrowQuantity
                                    , borrowMonthQuantity
                                    , borrowFirstQuantity
                                    , PRPOUtility.To2PointString(dtPrItemPrintForm.Rows[0]["UnitPrice"].ToString())
                                    , r["BorrowUnitID"].ToString()
                                    , r["BorrowMonthUnitID"].ToString()
                                    , r["IsRequestModify"].ToString()
                                    , r["IsFixedContent"].ToString()
                                    , r["IsPaper"].ToString()
                                    , r["IsFont"].ToString()
                                    , r["Remark2"].ToString()
                                    , r["RequestModifyDesc"].ToString()
                                    , r["SizeDetail"].ToString()
                                    , hdInvItemID.Value
                                    , hdPackID.Value
                                    , dtPrItemPrintForm.Rows[0]["InvItemCode"].ToString()
                                    , dtPrItemPrintForm.Rows[0]["InvItemName"].ToString()
                                    , dtPrItemPrintForm.Rows[0]["PackDescription"].ToString()
                                    , PRPOUtility.To2PointString(dtPrItemPrintForm.Rows[0]["UnitQuantity"].ToString())
                                    , dtPrItemPrintForm.Rows[0]["InvSpecPurchase"].ToString()
                                );
                            }
                        }

                        try
                        {
                            // AddItem() เป็น virtual method หาก pat เป็นชนิด PRPOHireActualTable จะเรียกใช้ AddItem ของคลาสตัวเอง
                            pat.AddItem
                            (
                                hdInvItemID.Value, hdPackID.Value, hdPrID.Value, hdPrItemID.Value, gvPR.Rows[i].Cells[2].Text,
                                gvPR.Rows[i].Cells[3].Text, gvPR.Rows[i].Cells[5].Text, hdUnitPrice.Value, gvPR.Rows[i].Cells[4].Text,
                                PRPOPopup.PR, "N"
                            );
                        }
                        // เกิดขึ้นเมื่อผู้ใช้เลือก PO สั่งจ้าง และมีการเลือกรายการ PR คนละใบกัน
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript
                            (
                                this,
                                GetType(),
                                "pop_PRSelect",
                                "alert('" + ex.Message + "');",
                                true
                            );
                            return;
                        }
                    }
                }
            }

            string js = "if (window.opener)"
                      + "{"
                      + "    window.opener.document.getElementById('bCancelPurchase').style.display = 'block';"
                      + "    window.opener.document.getElementById('btnRefreshI').click();"
                      + "}"
                      + "window.close();";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "cl", js, true);
        }
    }
}
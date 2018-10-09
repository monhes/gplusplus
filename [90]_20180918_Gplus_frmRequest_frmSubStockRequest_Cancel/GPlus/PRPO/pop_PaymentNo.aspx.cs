using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.PRPO
{
    public partial class pop_PaymentNo : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string js = "if (window.opener)"
                          + "{"
                          + "    val = window.opener.document.getElementById('" + Request["tbPaymentNo"] + "').value;"
                          + "    document.getElementById('" + tbDetail.ClientID + "').value = val;"
                          + "}";

                ScriptManager.RegisterStartupScript
                (
                    this
                    , GetType()
                    , "js"
                    , js
                    , true
                );
            }

        }

        protected void bSave_Click(object sender, EventArgs e)
        {
            string value = HttpUtility.JavaScriptStringEncode(tbDetail.Text);

            string js = "if (window.opener)"
                      + "{"
                      + "    window.opener.document.getElementById('" + Request["tbPaymentNo"] + "').value = '" + value + "';"
                      + "} window.close();";

            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "js"
                , js
                , true
            );

            // กรณี PO มีการสร้างแล้ว ให้อัพเดตได้เลยไม่ต้องรอผู้ใช้กดปุ่ม "บันทึก"
            // ซึ่งจะทำให้ผู้ใช้สามารถอัพเดตค่า payment_no ได้ 
            if (!string.IsNullOrEmpty(Request["poId"]))
            {
                int poId = Convert.ToInt32(Request["poId"]);
                new PODAO2().UpdatePOForm1PaymentNo(poId, tbDetail.Text);
            }
        }
    }
}
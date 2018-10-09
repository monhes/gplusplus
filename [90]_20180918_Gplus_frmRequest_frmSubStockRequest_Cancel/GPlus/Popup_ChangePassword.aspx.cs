using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.Data;

namespace GPlus
{
    public partial class Popup_ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            LbUserName.Text = "";
            LbOldPassword.Text = "";
            LbNewPassword.Text = "";
            LbConfirmPassword.Text = "";

            if (TbUserName.Text.Length == 0)
            {
                LbUserName.Text = "กรุณาระบุชื่อผู้ใช้";
                isValid = false;
            }

            if (TbOldPassword.Text.Length == 0)
            {
                LbOldPassword.Text = "กรุณาระบุรหัสผ่านเดิม";
                isValid = false;
            }

            if (TbNewPassword.Text.Length == 0)
            {
                LbNewPassword.Text = "กรุณาระบุรหัสผ่านใหม่";
                isValid = false;
            }

            if (TbConfirmPassword.Text.Length == 0)
            {
                LbConfirmPassword.Text = "กรุณาระบุยืนยันรหัสผ่านใหม่";
                isValid = false;
            }

            if (isValid)
            {
                if (TbNewPassword.Text != TbConfirmPassword.Text)
                {
                    ScriptManager.RegisterStartupScript
                    (
                       this,
                       GetType(),
                       "ChangePassword",
                       "alert('รหัสผ่านใหม่กับยืนยันรหัสผ่านใหม่ไม่ตรงกัน')",
                       true
                    );
                    return;
                }

                if (TbNewPassword.Text.Length <= 5)
                {
                    ScriptManager.RegisterStartupScript
                    (
                       this,
                       GetType(),
                       "ChangePassword",
                       "alert('กรุณาระบุรหัสผ่านใหม่อย่างน้อย 6 ตัวขึ้นไป')",
                       true
                    );
                    return;
                }

                UserDAO usrDao = new UserDAO();

                DataTable dt = usrDao.GetAccount
                (
                    TbUserName.Text,
                    Util.EncryptPassword(TbOldPassword.Text)
                ).Tables[0];

                if (dt.Rows.Count >= 1)
                {
                    string newPassword = Util.EncryptPassword(TbNewPassword.Text);
                    string updateBy = dt.Rows[0]["Update_By"].ToString();
                    string accountId = dt.Rows[0]["Account_ID"].ToString();

                    usrDao.UpdateAccount(accountId, newPassword, updateBy);

                    ScriptManager.RegisterClientScriptBlock(
                        this,
                        this.GetType(), 
                        "ChangePassword", 
                        "alert('บันทึกรหัสผ่านใหม่เรียบร้อยแล้ว'); window.close();", 
                        true
                    );
                }
                else
                {
                    ScriptManager.RegisterStartupScript
                    (
                        this,
                        GetType(),
                        "ChangePassword",
                        "alert('กรุณาตรวจสอบชื่อผู้ใช้หรือรหัสผ่านเดิมให้ถูกต้อง')",
                        true
                    );
                }
            }
        }
    }
}
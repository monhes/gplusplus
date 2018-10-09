using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net.Mail;


public class EmailService
{

    public static void Send_mail_LPA(string mail_from,string mail_to, string subject, string message, string filePath)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //mail.From = new MailAddress("ชื่อ e-mail"); // ต้อง gmail เท่านั้น
        mail.From = new MailAddress("thaina.chayanin@gmail.com");
        mail.To.Add(mail_to);
        mail.Subject = subject;
        mail.Body = message;
        //mail.Attachments.Add(new Attachment(filePath));

        SmtpServer.Port = 587;
        //SmtpServer.Credentials = new System.Net.NetworkCredential("ชื่อ e-mail", "ใส่passwordเมลที่นี่");// ต้อง gmail เท่านั้น
        SmtpServer.Credentials = new System.Net.NetworkCredential("thaina.chayanin@gmail.com", "nvd#4vlg");// ต้อง gmail เท่านั้น
        SmtpServer.EnableSsl = true;

        SmtpServer.Send(mail);
    }

    public static void SendMail(string mailFrom, string mailTo, string subject, string body, string filePath)
    {
        MailMessage mm = new MailMessage(mailFrom, mailTo);

        mm.Subject = subject;
        mm.Body = body;
        mm.IsBodyHtml = true;

        //mm.Attachments.Add(new Attachment(filePath));

        SmtpClient smtp = new SmtpClient();
        smtp.Credentials = new System.Net.NetworkCredential(mailFrom, "purchasing");

        try
        {
            smtp.Send(mm);
        }
        catch (Exception ex)
        {

        }
    }

}
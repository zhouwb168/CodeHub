using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.IO;


/// <summary>
/// Summary description for SendEmail
/// </summary>
public class SendEmails
{
    public SendEmails()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="arrStrTO">接收人</param>
    /// <param name="strFrom">发送人</param>
    /// <param name="senderDisplayName">发件人名称</param>
    /// <param name="userPswd">用户密码</param>
    /// <param name="strSubject">邮件标题</param>
    /// <param name="strBody">邮件内容</param>
    /// <param name="strHost">邮件服务地址</param>
    /// <param name="port">邮件服务端口</param>
    /// <param name="strFilePath">附件路径</param>
    /// <returns>成功返回true,失败返回false</returns>
    public bool SendEmailTo(string arrStrTO, string strFrom, string senderDisplayName, string userPswd, string strSubject, string strBody, string strHost, int port, string strFilePath)
    {
        //邮箱帐号的登录名
        string username = strFrom;
        //邮件发送者
        MailAddress from = new MailAddress(strFrom/*, senderDisplayName*/);
        //邮件接收者
        MailAddress to = new MailAddress(arrStrTO);
        SmtpClient smtp = new SmtpClient(strHost, port);
        //不使用默认凭据访问服务器
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(username, userPswd);
        //使用network发送到smtp服务器
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        MailMessage mailobj = new MailMessage(from, to);
        //添加发送和抄送
        // mailobj.To.Add(""); 
        // mailobj.CC.Add("");
        //邮件标题
        mailobj.Subject = strSubject;
        //邮件内容
        mailobj.Body = strBody;
        //邮件不是html格式
        mailobj.IsBodyHtml = true;
        //邮件编码格式
        mailobj.BodyEncoding = Encoding.UTF8;
        //邮件优先级
        mailobj.Priority = MailPriority.High;
        if (strFilePath != "")
        {
            //添加附件
            Attachment attachment = new Attachment(strFilePath);
            mailobj.Attachments.Add(attachment);
        }
        try
        {
            //开始发送邮件
            smtp.Send(mailobj);
            return true;
        }
        catch
        {
            return false;
        }
    }










    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="arrStrTO">收件人地址</param>
    /// <param name="arrStrCC">CC地址</param>
    /// <param name="strFrom">发件人地址</param>
    /// <param name="strSubject">邮件标题</param>
    /// <param name="strBody">邮件内容</param>
    /// <param name="strAttPath">附件文件地址</param>
    /// <param name="strHost">邮件服务器地址</param>
    /// <returns></returns>
    public string SendEmail(string arrStrTO, string strFrom, string strSubject, string strBody, string strAttPath, string strHost)
    {

        MailMessage mail = new MailMessage();
        //收件人地址
        mail.To.Add(arrStrTO);
        ////CC人地址
        //mail.CC.Add(arrStrCC);
        //发件人地址
        mail.From = new MailAddress(strFrom);
        //邮件标题
        mail.Subject = strSubject;
        //邮件内容
        mail.Body = strBody;

        //如果内容为HTML网页可用下方法
        //StreamReader sr=new StreamReader(@"c:\\zhouqiang.htm",Encoding.GetEncoding("gb2312"));
        //mail.Body = sr.ReadToEnd();

        //邮件内容格式
        mail.IsBodyHtml = true;
        //添加附件
        if (!string.IsNullOrEmpty(strAttPath))
        {
            mail.BodyEncoding = Encoding.GetEncoding("gb2312");
            Attachment att = new Attachment(@strAttPath);
            mail.Attachments.Add(att);
        }

        //SMTP服务
        SmtpClient smtp = new SmtpClient();
        smtp.Host = strHost;
        try
        {
            //发送
            smtp.Send(mail);
            return "ok";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }


    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="arrStrTO">收件人地址</param>
    /// <param name="arrStrCC">CC地址</param>
    /// <param name="strFrom">发件人地址</param>
    /// <param name="strSubject">邮件标题</param>
    /// <param name="strBody">邮件内容</param>
    /// <param name="strAttPath">附件文件地址</param>
    /// <param name="strHost">邮件服务器地址</param>
    /// <returns></returns>
    public string SendErroeEmail(string[] arrStrTO, string[] arrStrCC, string strFrom, string strSubject, string strBody, string strAttPath, string strHost)
    {

        MailMessage mail = new MailMessage();
        //收件人地址
        if (arrStrTO != null && arrStrTO.Length > 0)
        {
            foreach (string strTO in arrStrTO)
            {
                mail.To.Add(strTO);
            }
        }
        //CC人地址
        if (arrStrCC != null && arrStrCC.Length > 0)
        {
            foreach (string strCC in arrStrCC)
            {
                mail.CC.Add(strCC);
            }
        }
        //发件人地址
        mail.From = new MailAddress(strFrom);
        //邮件标题
        mail.Subject = strSubject;
        //邮件内容
        mail.Body = strBody;

        //如果内容为HTML网页可用下方法
        //StreamReader sr=new StreamReader(@"c:\\zhouqiang.htm",Encoding.GetEncoding("gb2312"));
        //mail.Body = sr.ReadToEnd();

        //邮件内容格式
        mail.IsBodyHtml = true;
        //添加附件
        if (!string.IsNullOrEmpty(strAttPath))
        {
            mail.BodyEncoding = Encoding.GetEncoding("gb2312");
            Attachment att = new Attachment(@strAttPath);
            mail.Attachments.Add(att);
        }

        //SMTP服务
        SmtpClient smtp = new SmtpClient();
        smtp.Host = strHost;
        try
        {
            //发送
            smtp.Send(mail);
            return "ok";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }


    /// <summary>           
    /// 用HttpWebRequest取得网页源码           
    /// 对于带BOM的网页很有效，不管是什么编码都能正确识别           
    /// </summary>           
    /// <param name="url">网页地址"</param>            
    /// <returns>返回网页源文件</returns>           
    public static string GetHtmlSource(string url)
    {
        //处理内容
        string html = "";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Accept = "*/*"; //接受任意文件
        // 模拟使用IE在浏览
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)";
        request.AllowAutoRedirect = true;//是否允许302            
        //request.CookieContainer = new CookieContainer();//cookie容器，            
        request.Referer = url; //当前页面的引用             
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        html = reader.ReadToEnd();
        stream.Close();
        return html;
    }

}

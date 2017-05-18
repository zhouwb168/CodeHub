using System;

using System.Web.Configuration;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.GsmMessage.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "GsmMessage";
            this.Load();

            this.Response.Write("<script type=\"text/javascript\">var Zhhwy = '" + WebConfigurationManager.AppSettings["Zhhwy"] + "';</script>");
        }
    }
}
using System;

using System.Web.Configuration;

using Wodeyun.Bf.Base.Webs;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.Www
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "Index";
            if (this.Load() == false) return;

            Entity account = (Entity)((Entity)this.Session["Token"]).GetValue("Account");
            Username.InnerText = Helper.Deserialize(account.GetValue("Description").ToString()).GetValue("Name").ToString();
            this.Response.Write("<script type=\"text/javascript\">var Address = '" + this.Request.Url.Host + "';</script>");
        }
    }
}
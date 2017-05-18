using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.Recover.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "Recover";
            this.Load();
        }
    }
}
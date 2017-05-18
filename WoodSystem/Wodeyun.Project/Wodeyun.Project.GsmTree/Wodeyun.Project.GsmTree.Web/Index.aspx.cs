using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.GsmTree.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "GsmTree";
            this.Load();
        }
    }
}
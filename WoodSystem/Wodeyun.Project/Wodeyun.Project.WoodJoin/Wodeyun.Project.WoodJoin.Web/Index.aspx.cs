using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.WoodJoin.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodJoin";
            this.Load();
        }
    }
}
using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.GsmArea.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "GsmArea";
            this.Load();
        }
    }
}
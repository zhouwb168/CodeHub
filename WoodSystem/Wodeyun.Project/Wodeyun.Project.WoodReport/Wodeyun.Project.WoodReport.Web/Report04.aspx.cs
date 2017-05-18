using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.WoodReport.Web
{
    public partial class Report04 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodReport";
            this.Load();
        }
    }
}
using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.WoodDataList.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodDataList";
            this.Load();
        }
    }
}
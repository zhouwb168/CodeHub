using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.WoodPrice.Web
{
    public partial class CostList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodPrice";
            this.Load();
        }
    }
}
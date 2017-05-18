using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.WoodPrice.Web
{
    public partial class setWoodPrice : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodPrice";
            this.Load();
        }
    }
}
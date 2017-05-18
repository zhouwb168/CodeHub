using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.WoodCard.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodCard";
            this.Load();
        }
    }
}
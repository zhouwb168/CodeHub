using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.WoodPowerOfReadCard.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodPowerOfReadCard";
            this.Load();
        }
    }
}
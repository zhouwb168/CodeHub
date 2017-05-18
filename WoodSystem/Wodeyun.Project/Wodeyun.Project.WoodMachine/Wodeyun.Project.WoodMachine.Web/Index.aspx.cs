using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.WoodMachine.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "WoodMachine";
            this.Load();
        }
    }
}
using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.GsmSupplier.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "GsmSupplier";
            this.Load();
        }
    }
}
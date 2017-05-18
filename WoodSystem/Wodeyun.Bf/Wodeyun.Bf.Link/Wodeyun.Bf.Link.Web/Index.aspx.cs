using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.Link.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "Link";
            this.Load();
        }
    }
}
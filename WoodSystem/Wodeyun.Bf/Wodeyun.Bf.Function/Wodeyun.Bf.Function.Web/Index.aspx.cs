using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.Function.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "Function";
            this.Load();
        }
    }
}
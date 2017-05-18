using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.Role.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "Role";
            this.Load();
        }
    }
}
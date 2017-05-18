using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.EmptyPound.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "EmptyPound";
            this.Load();
        }
    }
}
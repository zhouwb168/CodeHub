using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Project.FullPound.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "FullPound";
            this.Load();
        }
    }
}
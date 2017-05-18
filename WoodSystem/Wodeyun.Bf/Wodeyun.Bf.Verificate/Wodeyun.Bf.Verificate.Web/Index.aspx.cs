using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.Verificate.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "Verificate";
            this.Load();
        }
    }
}
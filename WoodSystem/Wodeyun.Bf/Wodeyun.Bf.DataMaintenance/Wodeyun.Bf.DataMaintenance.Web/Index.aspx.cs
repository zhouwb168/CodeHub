using System;

using Wodeyun.Bf.Base.Webs;

namespace Wodeyun.Bf.DataMaintenance.Web
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.No = "DataMaintenance";
            this.Load();
        }
    }
}
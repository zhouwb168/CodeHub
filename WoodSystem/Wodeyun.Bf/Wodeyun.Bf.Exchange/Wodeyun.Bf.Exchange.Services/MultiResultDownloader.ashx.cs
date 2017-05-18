using System;

using System.Threading;
using System.Web;

using Wodeyun.Bf.Exchange.Manager;

namespace Wodeyun.Bf.Exchange.Services
{
    public class MultiResultDownloader : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["Uniques"] == null) return;

            string[] items = context.Request.QueryString["Uniques"].Split(",".ToCharArray());

            for (int i = 0; i < items.Length; i++)
            {
                MultiManager.GetInstance().AddResultDownloader(items[i], context);
            }

            string script = string.Format("<script language='javascript'>//{0}</script>", string.Empty.PadLeft(256, '0'));
            context.Response.Write(script + Environment.NewLine);
            context.Response.Flush();

            context.Response.Write("<script language='javascript'>parent.Connected();</script>" + Environment.NewLine);
            context.Response.Flush();

            while (context.Response.IsClientConnected == true)
            {
                context.Response.Write("<script language='javascript'>parent.Alive();</script>" + Environment.NewLine);
                context.Response.Flush();

                Thread.Sleep(60000);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
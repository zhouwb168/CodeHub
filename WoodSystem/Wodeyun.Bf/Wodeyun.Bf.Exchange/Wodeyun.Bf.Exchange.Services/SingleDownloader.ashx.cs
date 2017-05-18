using System;

using System.Threading;
using System.Web;

using Wodeyun.Bf.Exchange.Manager;

namespace Wodeyun.Bf.Exchange.Services
{
    public class SingleDownloader : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["Uniques"] == null) return;

            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;

            string[] items = context.Request.QueryString["Uniques"].Split(",".ToCharArray());

            for (int i = 0; i < items.Length; i++)
            {
                SingleManager.GetInstance().AddDownloader(items[i], context);
            }

            while (context.Response.IsClientConnected == true)
            {
                context.Response.Write("{\"Command\":\"Connected\"}" + Environment.NewLine);
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
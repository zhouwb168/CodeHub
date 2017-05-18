using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing;
using Wodeyun.Project.Barrier.Web.App_Code;

namespace Wodeyun.Project.Barrier.Web.Handers
{
    /// <summary>
    /// 获取相片
    /// </summary>
    public class ShowImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;

            System.Drawing.Image photo = new CarPhoto().GetTempPhoto(context.Request.QueryString["FN"].ToString());
            photo.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            photo.Dispose();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
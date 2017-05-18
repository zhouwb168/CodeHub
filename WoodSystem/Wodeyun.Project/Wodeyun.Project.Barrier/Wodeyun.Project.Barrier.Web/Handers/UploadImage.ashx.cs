using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Wodeyun.Project.Barrier.Web.App_Code;

namespace Wodeyun.Project.Barrier.Web.Handers
{
    /// <summary>
    /// GPS拍照图片上传
    /// </summary>
    public class UploadImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                /* 保存照片到数据库表，作为临时照片使用，注意，请在数据库里运行一个定时作业，定期清除该表的过期记录 */
                HttpPostedFile hpf = context.Request.Files[0];
                string imageFileName = hpf.FileName;
                byte[] imageData =new byte[hpf.InputStream.Length];
                hpf.InputStream.Read(imageData, 0, imageData.Length);

                new CarPhoto().SaveTempPhoto(imageFileName, imageData);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("OK");
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
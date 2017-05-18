using System;

using System.Web;
using System.Drawing;

namespace Wodeyun.Bf.Base.Helpers
{
    public class PictureHelper : IDisposable
    {
        public int GetHeight(string filename)
        {
            return Image.FromFile(HttpContext.Current.Server.MapPath(filename)).Height;
        }

        public int GetWidth(string filename)
        {
            return Image.FromFile(HttpContext.Current.Server.MapPath(filename)).Width;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            return;
        }

        #endregion
    }
}

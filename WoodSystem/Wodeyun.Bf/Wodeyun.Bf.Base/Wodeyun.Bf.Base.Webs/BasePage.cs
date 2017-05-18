using System.ServiceModel;
using System.Web.UI;
using System.Text;
using System.Web;
using System;

using Wodeyun.Bf.Execute.Wrappers;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.Base.Webs
{
    public class BasePage : Page
    {
        private string _No = string.Empty;

        public string No
        {
            set { this._No = value; }
        }

        public bool Load()
        {
            if (this.Request.QueryString["Token"] == null) return this.Redirect();

            Entity entity;
            if (this.Request.QueryString["Token"] != null)
            {
                string token = this.Request.QueryString["Token"].ToString();

                try
                {
                    string oldEntity = this.Request.Cookies[token].ToString();
                    entity = Helper.Deserialize(oldEntity);

                    this.Session["Token"] = entity;
                }
                catch
                {
                    using (ExecutorClient client = new ExecutorClient())
                    {
                        entity = (Entity)client.Execute("Token", "CheckToken", new object[] { token });

                        if (entity.PropertyCollection.IndexOf("Success") != -1) return this.Redirect();

                        if (this._No != "Index")
                        {
                            if (((EntityCollection)entity.GetValue("Functions")).GetEntityCollection("No", this._No).Count == 0) return this.Redirect();
                        }

                        AddCookie("Token", entity.Serialize(entity), 60 * 8, this._No);

                        this.Session["Token"] = entity;
                    }
                }
            }

            Entity account = (Entity)((Entity)this.Session["Token"]).GetValue("Account");
            this.SetAccount(account);

            return true;
        }

        /// <summary>
        /// 保存cookie
        /// 注意！cookie值将会使用C#语言的HttpUtility.UrlEncode方法加密保存
        /// JS取值时请使用decodeURIComponent方法解密
        /// cookie的Path和Domain属性没设置，默认为当前域
        /// </summary>
        /// <param name="objName">要保存的cookie名称</param>
        /// <param name="objValue">要保存的cookie值</param>
        /// <param name="objMinutes">cookie的有效期（单位为分钟）</param>
        /// <param name="pageNo">请求页面编号</param>
        private static void AddCookie(string objName, string objValue, int objMinutes, string pageNo)
        {
            if (HttpContext.Current.Request.Cookies[objName] == null) HttpContext.Current.Request.Cookies.Remove(objName);
            HttpCookie cookie = new HttpCookie(objName);
            cookie.Value = objValue;
            if (pageNo == "FullPound" || pageNo == "EmptyPound") cookie.Expires = DateTime.MaxValue; // 因为地磅的电脑的当前时间异常，又与磅绑定，不能改日期时间，所以cookie无限期
            else cookie.Expires = DateTime.Now.AddMinutes(objMinutes);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        private bool Redirect()
        {
            this.Response.Write("<script type=\"text/javascript\">top.location.href = \"http://" + this.Request.Url.Host + ":11004/Logon.aspx\";</script>");

            return false;
        }

        public void SetSetter()
        {
            StringBuilder jsBuild = new StringBuilder();
            jsBuild.Append("<script type='text/javascript'>var Setter = { Url: 'http://" + this.Request.Url.Host + ":10000/ExecuteService.svc/Execute', Max: 1073741823 };");
            jsBuild.Append("</script>");
            this.Response.Write(jsBuild.ToString());
        }

        private void SetAccount(Entity account)
        {
            StringBuilder jsBuild = new StringBuilder();
            jsBuild.AppendFormat("<script type=\"text/javascript\">var Account = {0};", account.GetValue("Unique"));
            jsBuild.Append("</script>");

            this.Response.Write(jsBuild.ToString());

            SetSetter();
        }
    }
}

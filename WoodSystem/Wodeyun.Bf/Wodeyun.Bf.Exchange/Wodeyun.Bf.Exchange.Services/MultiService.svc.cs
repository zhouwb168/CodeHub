using System;
using System.Collections.Generic;

using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

using Wodeyun.Bf.Exchange.Manager;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;

namespace Wodeyun.Bf.Exchange.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MultiService : IDisposable
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/Start", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Entity Start(string uniques, Entity entity)
        {
            try
            {
                string[] items = uniques.Split(",".ToCharArray());

                for (int i = 0; i < items.Length; i++)
                {
                    MultiManager.GetInstance().Start(items[i], entity);
                }

                return Helper.GetEntity(true, "执行开始成功！");
            }
            catch (KeyNotFoundException)
            {
                return Helper.GetEntity(false, "服务端未连接成功，请重试！");
            }
            catch (ConnectionLostException)
            {
                return Helper.GetEntity(false, "服务端连接已断开，请重试！");
            }
            catch (Exception exception)
            {
                return Helper.GetEntity(false, exception.Message);
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/Finish", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Entity Finish(string uniques, Entity entity)
        {
            try
            {
                string[] items = uniques.Split(",".ToCharArray());

                for (int i = 0; i < items.Length; i++)
                {
                    MultiManager.GetInstance().Finish(items[i], entity);
                }

                return Helper.GetEntity(true, "执行结束成功！");
            }
            catch (KeyNotFoundException)
            {
                return Helper.GetEntity(false, "服务端未连接成功，请重试！");
            }
            catch (ConnectionLostException)
            {
                return Helper.GetEntity(false, "服务端连接已断开，请重试！");
            }
            catch (Exception exception)
            {
                return Helper.GetEntity(false, exception.Message);
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/Upload", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void Upload(string unique, Entity entity)
        {
            MultiManager.GetInstance().Upload(unique, entity);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            return;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;

using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Threading;

using Wodeyun.Bf.Exchange.Manager;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;

namespace Wodeyun.Bf.Exchange.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SingleService : IDisposable
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/Execute", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Entity Execute(string uniques, Entity entity, int second)
        {
            try
            {
                string[] items = uniques.Split(",".ToCharArray());

                for (int i = 0; i < items.Length; i++)
                {
                    SingleManager.GetInstance().AddCommand(items[i], entity);
                }

                DateTime start = DateTime.Now;

                while (true)
                {
                    Thread.Sleep(10);

                    if ((DateTime.Now - start).TotalMilliseconds >= 1000 * second)
                        throw new TimeoutException();

                    for (int i = 0; i < items.Length; i++)
                    {
                        Entity result = SingleManager.GetInstance().GetResult(items[i]);
                        if (result != null) return result;
                    }
                }
            }
            catch (TimeoutException)
            {
                return Helper.GetEntity(false, "失败！请再试一次");
            }
            catch (KeyNotFoundException)
            {
                return Helper.GetEntity(false, "失败！网络已断开，请关闭系统并检查网络");
            }
            catch (ConnectionLostException)
            {
                return Helper.GetEntity(false, "失败！网络已断开，请关闭系统并检查网络");
            }
            catch (Exception exception)
            {
                return Helper.GetEntity(false, exception.Message);
            }
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/Update", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void Update(string unique, Entity entity)
        {
            SingleManager.GetInstance().Update(unique, entity);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            return;
        }

        #endregion
    }
}

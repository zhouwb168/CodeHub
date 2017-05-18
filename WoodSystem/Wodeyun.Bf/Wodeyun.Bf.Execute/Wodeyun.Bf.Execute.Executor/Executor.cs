using System;

using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Configuration;
using System.Web;
using System.IO;
using System.Text;

using Wodeyun.Bf.Esb.Wrappers;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;

namespace Wodeyun.Bf.Execute.Executor
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Executor : IDisposable
    {
        [OperationContract]
        [ServiceKnownType(typeof(Entity))]
        [ServiceKnownType(typeof(EntityCollection))]
        [WebInvoke(UriTemplate = "/Execute", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public object Execute(string service, string method, object[] args)
        {
            try
            {
                return this.GetResult(service, method, args);
            }
            catch (Exception exception)
            {
                return Helper.GetEntity(false, exception.Message);
            }
        }

        private object GetResult(string service, string method, object[] args)
        {
            try
            {
                using (ServiceInterfaceClient client = new ServiceInterfaceClient())
                {
                    EntityCollection collection = client.GetEntitiesByField("Name", service, "=");

                    if (collection.Count == 0)
                        throw new ValueNotFoundException("该服务不存在，请重新输入！");

                    string filename = string.Format(collection[0].GetValue("Filename").ToString(), WebConfigurationManager.AppSettings["Path"]);
                    //string url = string.Format(collection[0].GetValue("Url").ToString(), WebConfigurationManager.AppSettings["Address"]);
                    string url = string.Format(collection[0].GetValue("Url").ToString(), "localhost");
                    string contract = collection[0].GetValue("Contract").ToString();

                    using (ServiceHelper helper = new ServiceHelper(filename, url, contract))
                    {
                        return helper.ExecuteMethod(method, this.Deserialize(args));
                    }
                }
            }
            catch (Exception exception)
            {
                WriteExceptLog(exception, service, method, this.Deserialize(args));

                if (exception.InnerException is ServiceActivationException)
                    return Helper.GetEntity(false, exception.InnerException.Message);
                if(exception.InnerException.InnerException.InnerException is SocketException)
                    return Helper.GetEntity(false, exception.InnerException.InnerException.InnerException.Message);

               

                return Helper.GetEntity(false, exception.Message);
            }
        }

        private object[] Deserialize(object[] args)
        {
            object[] results = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToString().StartsWith("{") == true && args[i].ToString().EndsWith("}") == true)
                    results[i] = Helper.Deserialize(args[i].ToString());
                else
                    results[i] = args[i];
            }

            return results;
        }

        public void Dispose()
        {
            return;
        }

        private void WriteExceptLog(Exception ex, string service, string method, object[] args)
        {
            string msg = "";
            string traMessage = "";
            while (ex != null && ex.InnerException != null) ex = ex.InnerException;
            msg = ex.Message;
            traMessage = ex.StackTrace;
            DateTime nowDateTime = DateTime.Now;
            StringBuilder errBuild = new StringBuilder();
            errBuild.Append("#############################################################################################");
            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("时间：{0}", nowDateTime.ToString());
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("Message：{0}", msg);
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("StackTrace：{0}", traMessage);
            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.Append("附加信息：");
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("Service：{0}", service);
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("Method：{0}", method);
            errBuild.Append(Environment.NewLine);
            errBuild.Append("Args:");

            int i = 0;
            foreach (var o in args)
            {
                errBuild.AppendFormat("args[{0}] = {1}，    ", i, o);
                i++;
            }

            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.Append("#############################################################################################");

            string filePath = HttpContext.Current.Server.MapPath(string.Format("~/Except_{0}.txt", nowDateTime.ToString("yyyy_MM_dd")));
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.Write(errBuild);
                sw.Close();
            }
        }
    }
}

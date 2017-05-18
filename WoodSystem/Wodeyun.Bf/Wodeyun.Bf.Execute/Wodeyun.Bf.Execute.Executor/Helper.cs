using System;

using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Wodeyun.Bf.Execute.Executor
{
    public class ServiceHelper : IDisposable
    {
        private string _Filename = string.Empty;
        private string _Url = string.Empty;
        private string _Contract = string.Empty;

        private Assembly _Assembly = null;

        public ServiceHelper(string filename, string url, string contract)
        {
            this._Filename = filename;
            this._Url = url;
            this._Contract = contract;

            this.LoadAssembly();
        }

        private void LoadAssembly()
        {
            try
            {
                byte[] data = File.ReadAllBytes(this._Filename);
                this._Assembly = Assembly.Load(data);
            }
            catch
            {
                throw;
            }
        }

        public object ExecuteMethod(string method, params object[] args)
        {
            try
            {
                Type contract = this._Assembly.GetType(this._Contract);
                EndpointAddress address = new EndpointAddress(this._Url);
                WebHttpBinding binding = new WebHttpBinding();
                binding.MaxReceivedMessageSize = 6553500;
                binding.SendTimeout = new TimeSpan(0, 5, 0);

                using (IDisposable factory = Activator.CreateInstance(typeof(ChannelFactory<>).MakeGenericType(contract), binding, address) as IDisposable)
                {
                    ServiceEndpoint endpoint = this.GetEndpoint(factory);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    
                    
                    object channel = this.CreateChannel(factory);
                    return this.InvokeMethod(channel, contract, method, args);
                }
            }
            catch
            {
                throw;
            }
        }

        private ServiceEndpoint GetEndpoint(object factory)
        {
            try
            {
                PropertyInfo property = factory.GetType().GetProperty("Endpoint");
                return property.GetValue(factory, new object[] { }) as ServiceEndpoint;
            }
            catch
            {
                throw;
            }
        }

        private object CreateChannel(object factory)
        {
            try
            {
                MethodInfo channel = factory.GetType().GetMethod("CreateChannel", new Type[] { });
                return channel.Invoke(factory, new object[] { });
            }
            catch
            {
                throw;
            }
        }

        private object InvokeMethod(object channel, Type contract, string method, params object[] args)
        {
            try
            {
                MethodInfo info = contract.GetMethod(method);
                return info.Invoke(channel, args);
            }
            catch
            {
                throw;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            return;
        }

        #endregion
    }
}

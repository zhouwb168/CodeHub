using System.Collections;
using GEIIO.Log;

namespace GEIIO.Service
{
    public interface IServiceManager<TKey, TValue> : ILoggerProvider, IEnumerable where TValue : IService
    {
        TValue this[int index] { get; }

        TValue this[string key] { get; }

        bool AddService(string serviceKey, IService service);

        IService GetService(string serviceKey);

        bool RemoveService(string serviceKey);

        bool ContainsService(string serviceKey);

        void BatchUpdateDevice(string deviceID, object obj);

        void BatchRemoveDevice(string deviceID);

        void RemoveAllService();

        int Count { get; }
    }
}

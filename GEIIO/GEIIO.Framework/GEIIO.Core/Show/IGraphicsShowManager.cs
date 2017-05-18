using System.Collections;
using GEIIO.Log;

namespace GEIIO.Show
{
    public interface IGraphicsShowManager<TKey, TValue> : ILoggerProvider, IEnumerable where TValue : IGraphicsShow
    {
        TValue this[int index] { get; }

        TValue this[string key] { get; }

        bool AddShow(string showKey, IGraphicsShow service);

        IGraphicsShow GetShow(string showKey);

        bool RemoveShow(string showKey);

        bool ContainsShow(string showKey);

        void BatchUpdateDevice(string deviceID, object obj);

        void BatchRemoveDevice(string deviceID);

        void RemoveAllShow();

        int Count { get; }
    }
}

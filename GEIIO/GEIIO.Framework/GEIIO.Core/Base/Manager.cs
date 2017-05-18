using System.Collections.Concurrent;

namespace GEIIO.Base
{
    public class Manager<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {

    }
}

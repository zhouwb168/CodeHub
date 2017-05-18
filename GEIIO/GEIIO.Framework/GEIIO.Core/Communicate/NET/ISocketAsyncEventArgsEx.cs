using GEIIO.DataCache;

namespace GEIIO.Communicate.NET
{
    public interface ISocketAsyncEventArgsEx : IReceiveCache
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();
    }
}

using GEIIO.Base;

namespace GEIIO.Device
{
    public interface IVirtualDevice : IPlugin
    {
        /// <summary>
        /// 运行虚拟设备
        /// </summary>
        /// <param name="devid">设备ID</param>
        /// <param name="obj">数据对象</param>
        void RunVirtualDevice(string devid, object obj);
    }
}

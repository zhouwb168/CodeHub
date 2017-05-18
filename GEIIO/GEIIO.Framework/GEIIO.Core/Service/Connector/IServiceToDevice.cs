namespace GEIIO.Service.Connector
{
    /// <summary>
    /// 发送给指定的设备
    /// </summary>
    public interface IServiceToDevice
    {
        string DeviceCode { get; }

        string Text { get; }

        byte[] DataBytes { get; }

        object Object { get; }
    }
}

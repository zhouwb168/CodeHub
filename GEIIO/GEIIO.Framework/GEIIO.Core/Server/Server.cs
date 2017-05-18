using GEIIO.Config;
using GEIIO.Device;
using GEIIO.Log;

namespace GEIIO.Server
{
    public sealed class Server : SocketServer
    {
        internal Server(IServerConfig config, IDeviceContainer deviceContainer = null, ILogContainer logContainer = null) : base(config, deviceContainer, logContainer)
        {

        }

        public override void Start()
        {
            base.Start();
            Logger.InfoFormat(false, "{0}-{1}", ServerName, "启动服务");
        }

        public override void Stop()
        {
            base.Stop();
            Logger.InfoFormat(false, "{0}-{1}", ServerName, "停止服务...");
        }
    }
}

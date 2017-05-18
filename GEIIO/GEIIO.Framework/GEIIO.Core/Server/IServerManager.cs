using System.Collections;
using GEIIO.Config;
using GEIIO.Device;
using GEIIO.Log;

namespace GEIIO.Server
{
    public interface IServerManager : IEnumerable
    {
        IServer this[int index] { get; }

        IServer CreateServer(IServerConfig config, IDeviceContainer deviceContainer = null, ILogContainer logContainer = null);

        void AddServer(IServer server);

        IServer GetServer(string serverName);

        bool ContainsServer(string serverName);

        IServer[] GetServers();

        void RemoveServer(string serverName);

        void RemoveAllServer();

        int Count { get; }
    }
}

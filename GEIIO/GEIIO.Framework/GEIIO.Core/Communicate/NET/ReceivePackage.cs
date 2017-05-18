using System;
using System.Collections.Generic;

namespace GEIIO.Communicate.NET
{
    public class ReceivePackage : IReceivePackage
    {
        public ReceivePackage()
        {
            RemoteIP = String.Empty;
            RemotePort = -1;
            RequestInfos = new List<IRequestInfo>();
            // DeviceCode = String.Empty;
        }

        public ReceivePackage(string remoteIP, int remotePort, IList<IRequestInfo> listRI)
        {
            RemoteIP = remoteIP;
            RemotePort = remotePort;
            RequestInfos = listRI;
            //DeviceCode = deviceCode;
        }

        public IList<IRequestInfo> RequestInfos { get; set; }

        public string RemoteIP { get; set; }

        public int RemotePort { get; set; }

        // public string DeviceCode { get; set; }
    }
}

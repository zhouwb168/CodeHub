﻿using System;

namespace GEIIO.Service.Connector
{
    public class ServiceConnectorToken
    {
        public ServiceConnectorToken(string fromServiceKey, Func<IFromService, IServiceToDevice, object> deviceConnectorAsync)
        {
            FromServiceKey = fromServiceKey;
            DeviceConnectorAsync = deviceConnectorAsync;
        }

        public Func<IFromService, IServiceToDevice, object> DeviceConnectorAsync { get; private set; }

        public string FromServiceKey { get; private set; }
    }
}

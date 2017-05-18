namespace GEIIO.Service.Connector
{
    public class FromService : IFromService
    {
        public FromService(string serviceName, string serviceKey)
        {
            ServiceName = serviceName;
            ServiceKey = serviceKey;
        }

        public string ServiceName { get; private set; }
        public string ServiceKey { get; private set; }
    }
}

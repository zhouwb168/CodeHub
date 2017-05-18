namespace GEIIO.Server
{
    public abstract class ServerProvider : IServerProvider
    {
        protected ServerProvider()
        {
            Server = null;
        }

        public IServer Server { get; private set; }

        public virtual void Setup(IServer appServer)
        {
            Server = appServer;
        }
    }
}

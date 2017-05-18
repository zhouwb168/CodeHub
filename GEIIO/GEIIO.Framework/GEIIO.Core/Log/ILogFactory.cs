namespace GEIIO.Log
{
    public interface ILogFactory
    {
        ILog GetLog(string name, ILogContainer logContainer);
    }
}

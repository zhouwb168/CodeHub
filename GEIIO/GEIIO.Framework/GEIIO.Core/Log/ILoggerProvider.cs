namespace GEIIO.Log
{
    public interface ILoggerProvider
    {
        ILog Logger { get; }
    }
}

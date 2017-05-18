namespace GEIIO.Log
{
    public class LogFactory : ILogFactory
    {
        /// <summary>
        /// 创建日志实例
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logContainer"></param>
        /// <returns></returns>
        public ILog GetLog(string name, ILogContainer logContainer = null)
        {
            return new Log(name, logContainer);
        }
    }
}

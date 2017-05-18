using System;
using System.Configuration;
namespace Maticsoft.DBUtility
{
    
    public class PubConstant
    {        
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {           
            get 
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];       
                return _connectionString; 
            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return PubConstant.ConnectionString;
        }

        /// <summary>
        /// 得到web.config键值对。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetwebConfigValue(string configKey)
        {
            string configValue = ConfigurationManager.AppSettings[configKey];
            return configValue;
        }
    }
}

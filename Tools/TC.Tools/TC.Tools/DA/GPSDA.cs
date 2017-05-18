using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace TC.Tools
{
    public class GPSDA
    {
        /// <summary>
        /// 保存偏移数据
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="offsetlng"></param>
        /// <param name="offsetlat"></param>
        /// <param name="IsOk"></param>
        /// <returns></returns>
        public int SaveGpsOffset(int lng, int lat, int offsetlng, int offsetlat, int IsOk)
        {
            string strSql = @"IF EXISTS(SELECT 1 FROM GpsOffSet WHERE lng=" + lng + " AND lat=" + lat + @") RETURN;
                              INSERT INTO [GpsOffSet]([lng],[lat],[offsetlng],[offsetlat],[IsOk],[CreateDate])
                              VALUES (" + lng + "," + lat + "," + offsetlng + "," + offsetlat + "," + IsOk + ",GETDATE())";
            return new DbHelperSQLP(PubConstant.GetwebConfigValue("ConnectionStringGPS")).ExecuteSql(strSql);
        }
    }
}

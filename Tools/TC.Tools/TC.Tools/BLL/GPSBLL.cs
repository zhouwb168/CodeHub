using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TC.Tools
{
    public partial class GPSBLL
    {
        private readonly GPSDA dal = new GPSDA();
        public GPSBLL()
        {
        }

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
            return dal.SaveGpsOffset(lng, lat, offsetlng, offsetlat, IsOk);
        }
    }
}

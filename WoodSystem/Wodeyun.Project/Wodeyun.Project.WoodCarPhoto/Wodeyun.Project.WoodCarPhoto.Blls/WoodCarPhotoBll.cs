using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.WoodCarPhoto.Dals;
using Wodeyun.Project.WoodCarPhoto.Interfaces;

namespace Wodeyun.Project.WoodCarPhoto.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodCarPhotoBll : CommonBll, IWoodCarPhotoInterface
    {
        private WoodCarPhotoDal _Dal = new WoodCarPhotoDal();

        public WoodCarPhotoBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 根据记录ID获取该记录关联的照片列表
        /// </summary>
        /// <param name="barrierID">关联的关卡编号(来自于Barrier表）</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetCarPhotoListByRecordID(int barrierID, int start, int length)
        {
            start = length; // 这代码没作用，只是为了统一性书写

            return this._Dal.GetCarPhotoListByRecordID(barrierID);
        }

        /// <summary>
        /// 分页查询移动检查站的发卡记录
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="place">移动检查站名称</param>
        /// <returns>结果集</returns>
        public EntityCollection GetCarPhotoReportBySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string place)
        {
            return this._Dal.GetCarPhotoReportBySearchWithPaging(startDate, endDate, start, length, supplier.Trim(), license.Trim(), place.Trim());
        }
    }
}

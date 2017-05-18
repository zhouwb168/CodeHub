using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.WoodDataList.Dals;
using Wodeyun.Project.WoodDataList.Interfaces;

namespace Wodeyun.Project.WoodDataList.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodDataListBll : CommonBll, IWoodDataListInterface
    {
        private WoodDataListDal _Dal = new WoodDataListDal();

        public WoodDataListBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 分页获取化验室原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="key">料厂密码</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport04BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key)
        {
            return this._Dal.GetReport04BySearchWithPaging(startDate, endDate, start, length, supplier.Trim(), license.Trim(), key.Trim());
        }

        /// <summary>
        /// 分页获取料厂原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="key">料厂密码</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport03BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key)
        {
            return this._Dal.GetReport03BySearchWithPaging(startDate, endDate, start, length, supplier.Trim(), license.Trim(), key.Trim());
        }

        /// <summary>
        /// 分页获取地磅原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport02BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license)
        {
            return this._Dal.GetReport02BySearchWithPaging(startDate, endDate, start, length, supplier, license);
        }

        /// <summary>
        /// 分页获取移动检查站原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="place">移动检查站名称</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport01BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string place)
        {
            return this._Dal.GetReport01BySearchWithPaging(startDate, endDate, start, length, supplier.Trim(), license.Trim(), place.Trim());
        }

        /// <summary>
        /// 获取数据统计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity getDataStatistics(Entity entity)
        {
            string StartDate = entity.GetValue("StartDate").ToString();
            string EndDate = entity.GetValue("EndDate").ToString();
            string Supplier = entity.GetValue("Supplier").ToString();
            string License = entity.GetValue("License").ToString();
            return this._Dal.getDataStatistics(StartDate, EndDate, Supplier, License);
        }
    }
}

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.WoodReport.Dals;
using Wodeyun.Project.WoodReport.Interfaces;

namespace Wodeyun.Project.WoodReport.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodReportBll : CommonBll, IWoodReportInterface
    {
        private WoodReportDal _Dal = new WoodReportDal();

        public WoodReportBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 获取木片检验结果通知单的打印信息
        /// </summary>
        /// <param name="arrWoodID">木材记录ID集合</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitysForReportPrint(string arrWoodID)
        {
            EntityCollection resultCollection = null;
            try
            {
                this._Dal.BeginTransaction();

                int acts = this._Dal.UpdatePrintState(arrWoodID);
                resultCollection = this._Dal.GetEntitysForReportPrint(arrWoodID);

                this._Dal.CommitTransaction();
            }
            catch (System.Exception exception)
            {
                this._Dal.RollbackTransaction();

                throw new System.Exception("出错", exception);
            }

            return resultCollection;
        }

        /// <summary>
        /// 木片检验结果通知单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="key">料厂密码</param>
        /// <param name="printed">已打印标识（-1 - 全部， 0 - 否， 1 - 是）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport05BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key, int printed)
        {
            return this._Dal.GetReport05BySearchWithPaging(startDate, endDate, start, length, supplier.Trim(), license.Trim(), key.Trim(), printed);
        }

        public EntityCollection GetReport04ByDateAndStartAndLength(string date, int start, int length)
        {
            return this._Dal.GetReport04ByDateAndStartAndLength(date, start, length);
        }

        /// <summary>
        /// 分页查询木质原料来源地总表
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport03ByDateAndStartAndLength(string startDate, string endDate, int start, int length)
        {
            return this._Dal.GetReport03ByDateAndStartAndLength(startDate, endDate, start, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public EntityCollection GetReport02ByMonthAndStartAndLength(string month, int start, int length)
        {
            return this._Dal.GetReport02ByMonthAndStartAndLength(month, start, length);
        }

        /// <summary>
        /// 各区域木料平均水份统计
        /// </summary>
        /// <param name="month"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public EntityCollection GetReport06ByMonthAndStartAndLength(string month, int start, int length)
        {
            return this._Dal.GetReport06ByMonthAndStartAndLength(month, start, length);
        }

        /// <summary>
        /// 分页查询木片检验结果
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport01ByDateAndStartAndLength(string startDate, string endDate, int start, int length)
        {
            return this._Dal.GetReport01ByDateAndStartAndLength(startDate, endDate, start, length);
        }

        /// <summary>
        /// 分页高级查询木质原料来源地总表 zwb add
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="License">车号</param>
        /// <param name="Driver">送货员</param>
        /// <param name="PoundSupplier">卸货员</param>
        /// <param name="Area">区域</param>
        /// <param name="statistical">统计字段</param>
        /// <returns>结果集</returns>
        public EntityCollection AdvancedSearchReport(string startDate, string endDate, int start, int length,
            string License, string Driver, string PoundSupplier, string Area, string statistical)
        {
            return this._Dal.AdvancedSearchReport(startDate, endDate, start, length, License, Driver, PoundSupplier, Area, statistical);
        }

        /// <summary>
        /// 各区域进柴统计(重量)
        /// </summary>
        /// <param name="month"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public EntityCollection WoodAreaInFactoryReport(string month, int start, int length)
        {
            return this._Dal.WoodAreaInFactoryReport(month, start, length);
        }


        /// <summary>
        /// 各品种进柴统计(重量)
        /// </summary>
        /// <param name="month"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public EntityCollection WoodCropInFactoryReport(string month, int start, int length)
        {
            return this._Dal.WoodCropInFactoryReport(month, start, length);
        }
    }
}

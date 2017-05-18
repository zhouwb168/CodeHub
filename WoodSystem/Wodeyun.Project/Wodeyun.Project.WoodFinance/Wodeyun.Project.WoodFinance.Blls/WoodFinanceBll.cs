using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.WoodFinance.Dals;
using Wodeyun.Project.WoodFinance.Interfaces;

namespace Wodeyun.Project.WoodFinance.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodFinanceBll : CommonBll, IWoodFinanceInterface
    {
        private WoodFinanceDal _Dal = new WoodFinanceDal();

        public WoodFinanceBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 分页查询料厂民工卸木片汇总表
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="people">卸货人</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport02BySearchWithPaging(string startDate, string endDate, int start, int length, string people)
        {
            return this._Dal.GetReport02BySearchWithPaging(startDate, endDate, start, length, people.Trim());
        }

        /// <summary>
        /// 分页查询料厂民工卸木片报表
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="people">卸货人</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport01BySearchWithPaging(string startDate, string endDate, int start, int length, string people)
        {
            return this._Dal.GetReport01BySearchWithPaging(startDate, endDate, start, length, people.Trim());
        }
    }
}

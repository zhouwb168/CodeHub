using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.GsmReport.Dals;
using Wodeyun.Project.GsmReport.Interfaces;

namespace Wodeyun.Project.GsmReport.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmReportBll : CommonBll, IGsmReportInterface
    {
        private GsmReportDal _Dal = new GsmReportDal();

        public GsmReportBll()
        {
            this.Dal = this._Dal;
        }

        public EntityCollection GetReport01WithSupplierNameByDateAndStartAndLength(string date, int start, int length)
        {
            return this._Dal.GetReport01WithSupplierNameByDateAndStartAndLength(date, start, length);
        }

        public EntityCollection GetReport02WithMessageByMonthAndStartAndLength(string month, int start, int length)
        {
            return this._Dal.GetReport02WithMessageByMonthAndStartAndLength(month, start, length);
        }

        public EntityCollection GetReport03ByMonthAndSupplierAndStartAndLength(string month, string supplier, int start, int length)
        {
            return this._Dal.GetReport03ByMonthAndSupplierAndStartAndLength(month, supplier, start, length);
        }

        public EntityCollection GetReport04ByMonthAndStartAndLength(string month, int start, int length)
        {
            return this._Dal.GetReport04ByMonthAndStartAndLength(month, start, length);
        }

        public EntityCollection GetReport05ByMonthAndStartAndLength(string month, int start, int length)
        {
            return this._Dal.GetReport05ByMonthAndStartAndLength(month, start, length);
        }

        public EntityCollection GetReport06ByYearAndStartAndLength(string year, int start, int length)
        {
            return this._Dal.GetReport06ByYearAndStartAndLength(year, start, length);
        }

        public EntityCollection GetReport07ByYearAndStartAndLength(string year, int start, int length)
        {
            return this._Dal.GetReport07ByYearAndStartAndLength(year, start, length);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (this._Dal != null) this._Dal.Dispose();
        }

        #endregion
    }
}

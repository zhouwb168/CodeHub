using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.GsmOrigin.Dals;
using Wodeyun.Project.GsmOrigin.Interfaces;

namespace Wodeyun.Project.GsmOrigin.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmOriginBll : CommonBll, IGsmOriginInterface
    {
        private GsmOriginDal _Dal = new GsmOriginDal();

        public GsmOriginBll()
        {
            this.Dal = this._Dal;
        }

        public EntityCollection GetEntitiesWithAreaNameByStartAndLength(int start, int length)
        {
            return this._Dal.GetEntitiesWithAreaNameByStartAndLength(start, length);
        }

        public EntityCollection GetEntitiesWithAreaNameByAreaAndStartAndLength(int area, int start, int length)
        {
            return this._Dal.GetEntitiesWithAreaNameByAreaAndStartAndLength(area, start, length);
        }
    }
}

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.GsmArea.Dals;
using Wodeyun.Project.GsmArea.Interfaces;

namespace Wodeyun.Project.GsmArea.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmAreaBll : CommonBll, IGsmAreaInterface
    {
        private GsmAreaDal _Dal = new GsmAreaDal();

        public GsmAreaBll()
        {
            this.Dal = this._Dal;
        }
    }
}

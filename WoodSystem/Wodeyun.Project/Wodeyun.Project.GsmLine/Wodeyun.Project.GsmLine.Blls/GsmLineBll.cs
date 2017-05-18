using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.GsmLine.Dals;
using Wodeyun.Project.GsmLine.Interfaces;

namespace Wodeyun.Project.GsmLine.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmLineBll : CommonBll, IGsmLineInterface
    {
        private GsmLineDal _Dal = new GsmLineDal();

        public GsmLineBll()
        {
            this.Dal = this._Dal;
        }
    }
}

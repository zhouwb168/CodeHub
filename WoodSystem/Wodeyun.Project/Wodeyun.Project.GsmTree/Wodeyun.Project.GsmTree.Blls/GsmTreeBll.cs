using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.GsmTree.Dals;
using Wodeyun.Project.GsmTree.Interfaces;

namespace Wodeyun.Project.GsmTree.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmTreeBll : CommonBll, IGsmTreeInterface
    {
        private GsmTreeDal _Dal = new GsmTreeDal();

        public GsmTreeBll()
        {
            this.Dal = this._Dal;
        }
    }
}

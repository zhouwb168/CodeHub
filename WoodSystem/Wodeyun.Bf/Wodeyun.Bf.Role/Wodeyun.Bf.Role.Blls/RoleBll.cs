using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Role.Dals;
using Wodeyun.Bf.Role.Interfaces;

namespace Wodeyun.Bf.Role.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RoleBll : CommonBll, IRoleInterface
    {
        private RoleDal _Dal = new RoleDal();

        public RoleBll()
        {
            this.Dal = this._Dal;
        }
    }
}
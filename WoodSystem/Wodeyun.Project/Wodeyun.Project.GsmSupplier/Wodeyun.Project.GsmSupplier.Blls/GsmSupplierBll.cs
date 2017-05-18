using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.GsmSupplier.Dals;
using Wodeyun.Project.GsmSupplier.Interfaces;

namespace Wodeyun.Project.GsmSupplier.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmSupplierBll : CommonBll, IGsmSupplierInterface
    {
        private GsmSupplierDal _Dal = new GsmSupplierDal();

        public GsmSupplierBll()
        {
            this.Dal = this._Dal;
        }
    }
}

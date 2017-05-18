using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Esb.Dals;
using Wodeyun.Bf.Esb.Interfaces;

namespace Wodeyun.Bf.Esb.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceBll : CommonBll, IServiceInterface
    {
        private ServiceDal _Dal = new ServiceDal();

        public ServiceBll()
        {
            this.Dal = this._Dal;
        }
    }
}

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Function.Dals;
using Wodeyun.Bf.Function.Interfaces;

namespace Wodeyun.Bf.Function.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FunctionBll : CommonBll, IFunctionInterface
    {
        private FunctionDal _Dal = new FunctionDal();

        public FunctionBll()
        {
            this.Dal = this._Dal;
        }


    }
}

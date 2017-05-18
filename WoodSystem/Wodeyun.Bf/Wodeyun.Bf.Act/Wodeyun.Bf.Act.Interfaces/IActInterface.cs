using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.Act.Interfaces
{
    [ServiceContract]
    public interface IActInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithFunctionByAccount", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithFunctionByAccount(int account);
    }
}

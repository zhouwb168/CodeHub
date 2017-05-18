using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.Wood.Interfaces
{
    [ServiceContract]
    public interface IWoodInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByFieldWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByFieldWithOperator(string cardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntityByUniqueWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntityByUniqueWithOperator(int unique, int operatorID, string cardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByStartAndLengthWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

    }
}

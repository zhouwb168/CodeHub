using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.Barrier.Interfaces
{
    [ServiceContract]
    public interface IBarrierInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByFieldForWoodWhereComeFrom", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByFieldForWoodWhereComeFrom(string field, object value, string connect);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SavePhoto", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SavePhoto(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByStartAndLengthWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntityByUniqueWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntityByUniqueWithOperator(int unique, int operatorID, string greenCardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByFieldWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByFieldWithOperator(string greenCardNumber);

    }
}

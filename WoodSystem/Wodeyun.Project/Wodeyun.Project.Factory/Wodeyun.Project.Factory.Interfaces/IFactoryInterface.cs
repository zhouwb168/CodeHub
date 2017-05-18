using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.Factory.Interfaces
{
    [ServiceContract]
    public interface IFactoryInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByFieldWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByFieldWithOperator(string redCardNumbert);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntityByUniqueWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntityByUniqueWithOperator(int unique, int intWoodID, int operatorID, string redCardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByStartAndLengthWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity, string redCardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveBoxEntity", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveBoxEntity(Entity entity);
    }
}

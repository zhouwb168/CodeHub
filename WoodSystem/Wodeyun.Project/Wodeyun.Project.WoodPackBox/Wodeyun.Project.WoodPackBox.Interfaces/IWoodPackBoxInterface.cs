using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodPackBox.Interfaces
{
    [ServiceContract]
    public interface IWoodPackBoxInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntityByUniqueWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntityByUniqueWithOperator(int unique, int intWoodID, int operatorID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByStartAndLengthWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID);

    }
}

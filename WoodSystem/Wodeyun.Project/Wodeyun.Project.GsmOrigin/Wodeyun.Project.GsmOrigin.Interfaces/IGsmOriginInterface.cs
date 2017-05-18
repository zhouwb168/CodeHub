using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.GsmOrigin.Interfaces
{
    [ServiceContract]
    public interface IGsmOriginInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithAreaNameByStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithAreaNameByStartAndLength(int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithAreaNameByAreaAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithAreaNameByAreaAndStartAndLength(int area, int start, int length);
    }
}

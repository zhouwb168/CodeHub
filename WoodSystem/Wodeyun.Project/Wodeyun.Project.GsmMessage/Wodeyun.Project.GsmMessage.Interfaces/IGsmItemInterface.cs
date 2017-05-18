using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.GsmMessage.Interfaces
{
    [ServiceContract]
    public interface IGsmItemInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithAreaUniqueByMessage", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithAreaUniqueByMessage(int message);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByDateAndLicense", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByDateAndLicense(string date, string license);
    }
}

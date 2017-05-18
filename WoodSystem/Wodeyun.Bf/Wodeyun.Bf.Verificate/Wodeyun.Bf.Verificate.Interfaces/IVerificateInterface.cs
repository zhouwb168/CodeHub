using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.Verificate.Interfaces
{
    [ServiceContract]
    public interface IVerificateInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByLinkAndUsernameAndPassword", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByLinkAndUsernameAndPassword(int link, string username, string password);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithAccountAndLinkNameByStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithAccountAndLinkNameByStartAndLength(int start, int length);
    }
}

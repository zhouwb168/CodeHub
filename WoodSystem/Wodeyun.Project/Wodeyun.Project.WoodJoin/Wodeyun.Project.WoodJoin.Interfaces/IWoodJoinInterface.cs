using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodJoin.Interfaces
{
    [ServiceContract]
    public interface IWoodJoinInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/CutOffJoin", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity CutOffJoin(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateFilter", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity UpdateFilter(int bangid, int filter);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfJoinByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfJoinByDateAndStartAndLength(string date, int start, int length, string license);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfGsmByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfGsmByDateAndStartAndLength(string license, string date, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfWoodByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfWoodByDateAndStartAndLength(string license, string date, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfBangByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfBangByDateAndStartAndLength(string date, int start, int length, int isFiltered);

    }
}

using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.GsmJoin.Interfaces
{
    [ServiceContract]
    public interface IGsmJoinInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/CutOffJoin", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity CutOffJoin(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateFilter", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity UpdateFilter(int bangid, int filter);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfJoinByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfJoinByDateAndStartAndLength(string date, string enddate, int start, int length, string license, string area);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfJoinGsmByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfJoinGsmByDateAndStartAndLength(string date, int start, int length, string license);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfGsmByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfGsmByDateAndStartAndLength(string license, string driver, string supplier, string area, string date, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfWoodByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfWoodByDateAndStartAndLength(string license, string date, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetDataOfBangByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetDataOfBangByDateAndStartAndLength(string date, int start, int length, int isFiltered);

        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchJoinData", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string BatchJoinData(Entity entity);
    }
}

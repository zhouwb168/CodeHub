using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodDataList.Interfaces
{
    [ServiceContract]
    public interface IWoodDataListInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport04BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport04BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport03BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport03BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport02BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport02BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport01BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport01BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string place);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getDataStatistics", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity getDataStatistics(Entity entity);
    }
}

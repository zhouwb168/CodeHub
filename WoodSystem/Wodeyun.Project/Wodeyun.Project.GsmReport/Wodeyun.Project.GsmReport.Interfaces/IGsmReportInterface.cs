using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.GsmReport.Interfaces
{
    [ServiceContract]
    public interface IGsmReportInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport01WithSupplierNameByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport01WithSupplierNameByDateAndStartAndLength(string date, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport02WithMessageByMonthAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport02WithMessageByMonthAndStartAndLength(string month, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport03ByMonthAndSupplierAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport03ByMonthAndSupplierAndStartAndLength(string month, string supplier, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport04ByMonthAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport04ByMonthAndStartAndLength(string month, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport05ByMonthAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport05ByMonthAndStartAndLength(string month, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport06ByYearAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport06ByYearAndStartAndLength(string year, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport07ByYearAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport07ByYearAndStartAndLength(string year, int start, int length);
    }
}

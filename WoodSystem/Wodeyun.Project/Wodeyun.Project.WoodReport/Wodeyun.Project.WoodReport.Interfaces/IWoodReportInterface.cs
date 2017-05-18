using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodReport.Interfaces
{
    [ServiceContract]
    public interface IWoodReportInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitysForReportPrint", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitysForReportPrint(string arrWoodID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport05BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport05BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key, int printed);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport04ByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport04ByDateAndStartAndLength(string date, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport03ByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport03ByDateAndStartAndLength(string startDate, string endDate, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport02ByMonthAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport02ByMonthAndStartAndLength(string month, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport01ByDateAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport01ByDateAndStartAndLength(string startDate, string endDate, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/AdvancedSearchReport", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection AdvancedSearchReport(string startDate, string endDate, int start, int length, string License, string Driver, string PoundSupplier, string Area, string statistical);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport06ByMonthAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport06ByMonthAndStartAndLength(string month, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/WoodAreaInFactoryReport", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection WoodAreaInFactoryReport(string month, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/WoodCropInFactoryReport", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection WoodCropInFactoryReport(string month, int start, int length);
    }
}

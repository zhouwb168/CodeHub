using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.DataMaintenance.Interfaces
{
    [ServiceContract]
    public interface IDataMaintenanceInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByFieldForMatch", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByFieldForMatch(object license);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByFieldWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByFieldWithOperator(string cardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntityByUniqueWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntityByUniqueWithOperator(int unique, int intWoodID, int operatorID, string greenCardNumber, string redCardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByStartAndLengthWithOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID, string StartTime, string EndTime, string CarID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity, string greenCardNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetWoodBangPrintInfo", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetWoodBangPrintInfo(int start, int length, int operatorID, string startDate, string endDate, string License);


        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchSyncFullVolumeData", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity BatchSyncFullVolumeData(string StartTime, string EndTime, string CarID);
    }
}

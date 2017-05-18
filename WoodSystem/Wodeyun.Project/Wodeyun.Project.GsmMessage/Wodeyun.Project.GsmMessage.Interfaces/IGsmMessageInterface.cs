using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.GsmMessage.Interfaces
{
    [ServiceContract]
    public interface IGsmMessageInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithSupplierNameByDateAndSupplierAndMobileAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithSupplierNameByDateAndSupplierAndMobileAndStartAndLength(string date, string supplier, string mobile, int start, int length);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetItemsByText", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetItemsByText(string text);
    }
}

using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodLaboratory.Interfaces
{
    [ServiceContract]
    public interface IWoodLaboratoryInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLaboratoryDataByWoodID", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetLaboratoryDataByWoodID(int woodID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetGsmInfoByWoodID", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetGsmInfoByWoodID(int woodID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetWoodInfoByWoodID", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetWoodInfoByWoodID(int woodID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesBySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesBySearchWithPaging(string date, int start, int length, string key, string number);

    }
}

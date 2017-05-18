using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodPrice.Interfaces
{
    [ServiceContract]
    public interface IWoodPriceInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetWoodPriceData", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetWoodPriceData(string startDate, string endDate, int start, int length, string area, string tree);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getPriceDataList", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection getPriceDataList(string startDate, string endDate, int start, int length, string area, string tree, string IsCreate, string Supplier);

        [OperationContract]
        [WebInvoke(UriTemplate = "/InsertCostStatement", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection InsertCostStatement(object[] entities);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getCostDataList", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection getCostDataList(string startDate, string endDate, string bangstartDate, string bangendDate, int start, int length, string license, int printed, string groupid, string Supplier);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitysForCostPrint", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntitysForCostPrint(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/OnChangeArea", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string OnChangeArea(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchCheckCostState", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string BatchCheckCostState(Entity entity);


        [OperationContract]
        [WebInvoke(UriTemplate = "/ReviewPermissionsbyAccount", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int ReviewPermissionsbyAccount(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteWoodPriceByUnique", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteWoodPriceByUnique(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchDeleteCostState", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string BatchDeleteCostState(Entity entity);


        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveWoodPriceEntity", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveWoodPriceEntity(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchCheckWoodPrice", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string BatchCheckWoodPrice(Entity entity);

    }
}

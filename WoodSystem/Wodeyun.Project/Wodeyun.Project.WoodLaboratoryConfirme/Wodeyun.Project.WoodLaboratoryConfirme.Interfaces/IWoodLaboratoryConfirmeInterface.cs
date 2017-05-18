using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodLaboratoryConfirme.Interfaces
{
    [ServiceContract]
    public interface IWoodLaboratoryConfirmeInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchConfirme", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity BatchConfirme(object[] entities, int operatorID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchBackConfirme", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity BatchBackConfirme(object[] entities, int operatorID);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesBySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesBySearchWithPaging(string date, int start, int length, string key, string number, int confirme);
    }
}

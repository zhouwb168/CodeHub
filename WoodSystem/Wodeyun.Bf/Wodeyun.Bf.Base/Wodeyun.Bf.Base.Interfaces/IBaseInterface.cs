using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.Base.Interfaces
{
    [ServiceContract]
    public interface IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntity", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntity(Entity entity);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntities", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntities(object[] entities);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntityByUnique", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntityByUnique(int unique);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteEntitiesByUniques", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity DeleteEntitiesByUniques(int[] uniques);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByField", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByField(string field, object value, string connect);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntities", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntities();

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithDeletedAndUpdated", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithDeletedAndUpdated();

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByStartAndLength(int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithDeletedAndUpdatedByStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithDeletedAndUpdatedByStartAndLength(int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByField", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByField(string field, object value, string connect);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithDeletedAndUpdatedByField", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithDeletedAndUpdatedByField(string field, object value, string connect);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByFieldAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByFieldAndStartAndLength(string field, object value, string connect, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithDeletedAndUpdatedByFieldAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithDeletedAndUpdatedByFieldAndStartAndLength(string field, object value, string connect, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByFilter", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByFilter(Entity filter, Entity connector);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithDeletedAndUpdatedByFilter", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithDeletedAndUpdatedByFilter(Entity filter, Entity connector);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesByFilterAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesByFilterAndStartAndLength(Entity filter, Entity connector, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntitiesWithDeletedAndUpdatedByFilterAndStartAndLength", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetEntitiesWithDeletedAndUpdatedByFilterAndStartAndLength(Entity filter, Entity connector, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetGridByMethodAndFieldsAndUnique", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetGridByMethodAndFieldsAndUnique(string method, object[] args, IList fields, string unique);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetTreeByMethodAndFields", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetTreeByMethodAndFields(string method, object[] args, IList fields);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetGridByMethodAndParent", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetGridByMethodAndParent(string method, object[] args, string parent);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetTreeByMethodAndParentAndUniqueAndName", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetTreeByMethodAndParentAndUniqueAndName(string method, object[] args, string parent, string unique, string name);
    }
}

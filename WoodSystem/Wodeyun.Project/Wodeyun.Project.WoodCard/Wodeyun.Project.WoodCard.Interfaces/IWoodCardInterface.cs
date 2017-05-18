using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodCard.Interfaces
{
    [ServiceContract]
    public interface IWoodCardInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveEntityByOperator", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity SaveEntityByOperator(Entity entity);
    }
}

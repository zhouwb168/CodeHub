using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodPowerOfReadCard.Interfaces
{
    [ServiceContract]
    public interface IWoodPowerOfReadCardInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetEntityByAccount", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Entity GetEntityByAccount(int accountID);

    }
}

using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodFinance.Interfaces
{
    [ServiceContract]
    public interface IWoodFinanceInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport02BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport02BySearchWithPaging(string startDate, string endDate, int start, int length, string people);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetReport01BySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetReport01BySearchWithPaging(string startDate, string endDate, int start, int length, string people);
    }
}

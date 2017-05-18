using System.ServiceModel;
using System.ServiceModel.Web;

using Wodeyun.Bf.Base.Interfaces;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Project.WoodCarPhoto.Interfaces
{
    [ServiceContract]
    public interface IWoodCarPhotoInterface : IBaseInterface
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetCarPhotoListByRecordID", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetCarPhotoListByRecordID(int barrierID, int start, int length);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetCarPhotoReportBySearchWithPaging", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EntityCollection GetCarPhotoReportBySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string place);
    }
}

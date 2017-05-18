using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

using Wodeyun.Bf.Token.Manager;
using Wodeyun.Gf.Entities;

namespace Wodeyun.Bf.Token.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TokenService
    {
        private TokenManager _TokenManager = TokenManager.GetInstance();

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetTokenByLinkAndUsernameAndPassword", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Entity GetTokenByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            return this._TokenManager.GetTokenByLinkAndUsernameAndPassword(link, username, password);
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckToken", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Entity CheckToken(string token)
        {
            return this._TokenManager.CheckToken(token);
        }
    }
}

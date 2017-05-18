using System;

using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Execute.Wrappers;
using Wodeyun.Bf.Verificate.Dals;
using Wodeyun.Bf.Verificate.Interfaces;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Verificate.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class VerificateBll : CommonBll, IVerificateInterface
    {
        private VerificateDal _Dal = new VerificateDal();

        public VerificateBll()
        {
            this.Dal = this._Dal;
        }

        public override Entity ExecuteForSaveEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public override Entity ExecuteForSaveEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public Entity GetEntityByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            using (ExecutorClient client = new ExecutorClient())
            {
                int type = ((Entity)client.Execute("Link", "GetEntityByField", new object[] { "Unique", link, "=" })).GetValue("Type").ToInt32();

                if (type == 0) return this.GetEntityByLinkAndIdAndPassword(link, username, password);
                else if (type == 1) return this.GetEntityForUrlByLinkAndUsernameAndPassword(link, username, password);
                else return this.GetEntityForDomainByLinkAndUsernameAndPassword(link, username, password);
            }
        }

        private Entity GetEntityByLinkAndIdAndPassword(int link, string id, string password)
        {
            Entity entity = this._Dal.GetEntityByIdAndLink(id, link);

            if (entity.IsEmpty == true) return Helper.GetEntity(false, "用户名错误，请重新输入！", "Id");

            if (entity.GetValue("Value").ToString() == password) return Helper.GetEntity(true, "登录成功！", this.GetTokenByLinkAndUsernameAndPassword(link, id, password));
            else return Helper.GetEntity(false, "密码错误，请重新输入！", "Password");
        }

        private Entity GetEntityForUrlByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            return null;
        }

        private Entity GetEntityForDomainByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            using (ExecutorClient client = new ExecutorClient())
            {
                string domain = ((Entity)client.Execute("Link", "GetEntityByField", new object[] { "Unique", link, "=" })).GetValue("Value").ToString();

                try
                {
                    if ((new PrincipalContext(ContextType.Domain, domain)).ValidateCredentials(username, password) == true)
                    {
                        Entity filter = Helper.Deserialize("{\"Value\": \"" + username + "\", \"Link\": " + link.ToString() + "}");
                        Entity connector = Helper.Deserialize("{\"Value\": \"=\", \"Link\": \"=\"}");
                        EntityCollection verificates = (EntityCollection)client.Execute("Verificate", "GetEntitiesByFilter", new object[] { filter, connector });

                        if (verificates.Count == 0) return Helper.GetEntity(false, "该用户未在本系统授权！", "Link");
                        else return Helper.GetEntity(true, "登录成功！", this.GetTokenByLinkAndUsernameAndPassword(link, username, password));
                    }
                    else return Helper.GetEntity(false, "用户名/密码错误，请重新登录！", "Id");
                }
                catch (Exception exception)
                {
                    return Helper.GetEntity(false, exception.Message);
                }
            }
        }

        private Entity GetTokenByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            using (ExecutorClient client = new ExecutorClient())
            {
                return (Entity)client.Execute("Token", "GetTokenByLinkAndUsernameAndPassword", new object[] { link, username, password });
            }
        }

        public EntityCollection GetEntitiesWithAccountAndLinkNameByStartAndLength(int start, int length)
        {
            return this._Dal.GetEntitiesWithAccountAndLinkNameByStartAndLength(start, length);
        }
    }
}

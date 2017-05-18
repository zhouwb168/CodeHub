using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Execute.Wrappers;

namespace Wodeyun.Bf.Token.Manager
{
    public class TokenManager : IDisposable
    {
        private static TokenManager _Instance = new TokenManager();

        private Dictionary<string, Entity> _Tokens = new Dictionary<string, Entity>();
        private PropertyCollection _PropertyCollection = new PropertyCollection();
        private bool _State = true;
        private Thread _ClearThread = null;

        private TokenManager()
        {
            this._PropertyCollection.Add(new SimpleProperty("Token", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Account", typeof(Entity)));
            this._PropertyCollection.Add(new SimpleProperty("Verificate", typeof(Entity)));
            this._PropertyCollection.Add(new SimpleProperty("Functions", typeof(EntityCollection)));
            this._PropertyCollection.Add(new SimpleProperty("Date", typeof(DateTime)));

            this._ClearThread = new Thread(new ThreadStart(this.ClearThread));
            this._ClearThread.Start();
        }

        public static TokenManager GetInstance()
        {
            return _Instance;
        }

        private void ClearThread()
        {
            while (this._State == true)
            {
                foreach (var item in this._Tokens)
                {
                    if ((DateTime.Now - item.Value.GetValue("Date").ToDateTime()).TotalMinutes > 60 * 15)
                    {
                        this._Tokens.Remove(item.Key);
                        break;
                    }
                }

                Thread.Sleep(1000 * 60);
            }
        }

        public Entity GetTokenByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            using (ExecutorClient client = new ExecutorClient())
            {
                int type = ((Entity)client.Execute("Link", "GetEntityByField", new object[] { "Unique", link, "=" })).GetValue("Type").ToInt32();

                if (type == 0) return this.GetTokenByLinkAndIdAndPassword(link, username, password);
                else if (type == 1) return this.GetTokenForUrlByLinkAndUsernameAndPassword(link, username, password);
                else return this.GetTokenForDomainByLinkAndUsernameAndPassword(link, username, password);
            }
        }

        private Entity GetTokenByLinkAndIdAndPassword(int link, string id, string password)
        {
            using (ExecutorClient client = new ExecutorClient())
            {
                Entity account = (Entity)client.Execute("Account", "GetEntityByField", new object[] { "Id", id, "=" });

                if (account.IsEmpty == true) return Helper.GetEntity(false, "该用户名不存在！", "Id");

                Entity filter = Helper.Deserialize("{\"Account\": " + account.GetValue("Unique").ToString() + ", \"Link\": " + link.ToString() + "}");
                Entity connector = Helper.Deserialize("{\"Account\": \"=\", \"Link\": \"=\"}");
                EntityCollection verificates = (EntityCollection)client.Execute("Verificate", "GetEntitiesByFilter", new object[] { filter, connector });

                if (verificates.Count == 0) return Helper.GetEntity(false, "该用户不支持该登录方式！", "Link");
                if (verificates[0].GetValue("Value").ToString() != password) return Helper.GetEntity(false, "密码错误！", "Password");

                EntityCollection functions = (EntityCollection)client.Execute("Act", "GetEntitiesWithFunctionByAccount", new object[] { account.GetValue("Unique").ToInt32() });

                return this.GetToken(account, verificates[0], functions);
            }
        }

        private Entity GetTokenForUrlByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            return null;
        }

        private Entity GetTokenForDomainByLinkAndUsernameAndPassword(int link, string username, string password)
        {
            using (ExecutorClient client = new ExecutorClient())
            {
                string domain = ((Entity)client.Execute("Link", "GetEntityByField", new object[] { "Unique", link, "=" })).GetValue("Value").ToString();

                if ((new PrincipalContext(ContextType.Domain, domain)).ValidateCredentials(username, password) == true)
                {
                    Entity filter = Helper.Deserialize("{\"Value\": \"" + username + "\", \"Link\": " + link.ToString() + "}");
                    Entity connector = Helper.Deserialize("{\"Value\": \"=\", \"Link\": \"=\"}");
                    EntityCollection verificates = (EntityCollection)client.Execute("Verificate", "GetEntitiesByFilter", new object[] { filter, connector });
                    
                    if (verificates.Count == 0) return Helper.GetEntity(false, "该用户未在本系统授权！", "Link");

                    Entity account = (Entity)client.Execute("Account", "GetEntityByField", new object[] { "Unique", verificates[0].GetValue("Account").ToString(), "=" });
                    EntityCollection functions = (EntityCollection)client.Execute("Act", "GetEntitiesWithFunctionByAccount", new object[] { account.GetValue("Unique").ToInt32() });

                    return this.GetToken(account, verificates[0], functions);
                }
                else return Helper.GetEntity(false, "用户名/密码错误！", "Id");
            }
        }

        private Entity GetToken(Entity account, Entity verificate, EntityCollection functions)
        {
            Entity entity = new Entity(this._PropertyCollection);
            string token = Guid.NewGuid().ToString();

            entity.SetValue("Token", token);
            entity.SetValue("Account", account);
            entity.SetValue("Verificate", verificate);
            entity.SetValue("Functions", functions);
            entity.SetValue("Date", DateTime.Now);

            this._Tokens.Add(token, entity);

            return entity;
        }

        public Entity CheckToken(string token)
        {
            if (this._Tokens.ContainsKey(token) == true)
            {
                this._Tokens[token].SetValue("Date", DateTime.Now);

                return this._Tokens[token];
            }
            else return Helper.GetEntity(false, "该令牌不存在！");
        }

        public void Dispose()
        {
            this._State = false;

            try
            {
                this._ClearThread.Abort();
            }
            catch { }
        }
    }
}

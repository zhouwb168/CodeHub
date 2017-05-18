using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TC.Tools
{
    public partial class XmlBLL
    {
        private readonly XmlDA dal = new XmlDA();
        public XmlBLL() { }

        /// <summary>
        /// 获取证件号为空的用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable getUserInfo()
        {
            return dal.getUserInfo();
        }

        /// <summary>
        /// 更新证件号
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public int UpdateCardNo(int UserID, string CardNo)
        {
            return dal.UpdateCardNo(UserID, CardNo);
        }
    }
}

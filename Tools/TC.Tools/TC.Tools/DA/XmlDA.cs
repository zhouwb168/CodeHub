using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TC.Tools
{
    public class XmlDA
    {
        /// <summary>
        /// 获取证件号为空的用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable getUserInfo()
        {
            string strSql = @"SELECT UserID,UserName,UserType,CardNo,CustId FROM V_Users WHERE ISNULL(CustId,'')<>'' AND ISNULL(CardNo,'')=''";
            return Maticsoft.DBUtility.DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// 更新证件号
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public int UpdateCardNo(int UserID, string CardNo)
        {
            string strSql = @"IF EXISTS(SELECT 1 FROM PersonBasicInfo WHERE CardNo='" + CardNo + @"') RETURN;
                               UPDATE PersonBasicInfo SET CardNo='" + CardNo + "' WHERE UserID=" + UserID + "";
            return Maticsoft.DBUtility.DbHelperSQL.ExecuteSql(strSql);
        }
    }
}

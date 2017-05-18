using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace TC.Tools
{
    public class DA
    {
        /// <summary>
        /// 获取借款信息
        /// </summary>
        /// <param name="BorrowID"></param>
        /// <param name="BorrowUser"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        public static DataTable getBorrowInfo(string BorrowID, string BorrowUser, string Where)
        {
            try
            {
                string strSql = @"SELECT B.BorrowID,B.BorrowCode,B.UserID,B.BorrowUser,T.TypeID,T.TypeName AS TenderTypeName,B.Area,B.BorrowTitle,B.BorrowAmount
                            ,B.BorrowUse,B.BorrowValidate,B.AnnualRate,B.ServiceRate,B.InvestmentRate,B.Reimburse,B.EarlyRepayRate,B.ManagerRate
                            ,B.SecurityType,SecurityTypeName=BT2.TypeName,U.CustId,B.SuccessTime,B.ReleaseTime,B.OtherRate,B.LoanServiceRate
                            ,B.OverdueRate,B.GuaranRate,B.RiskRate,BT.TypeName AS BaseTypeName,B.ReimburseSource,B.PremiumRate,B.TransterRate 
                            ,CASE B.InterestType WHEN 0 THEN '自然月' ELSE  '固定日' END InterestTypeName,B.InterestType,B.LendTime,B.LendUser
                            ,CASE B.StartDay WHEN 0 THEN '当日计算' ELSE  '次日计算' END StartDayName,B.StartDay,B.EarlyRepayFlag,B.EarlyRepayPass
                            ,B.Validate,B.Remark,BT.TypeID AS BTypeID,B.FreezeDate,B.AreaID,B.GuaranType,B.BorrowStatus,VU.RealName FROM Borrow B
                            INNER JOIN TenderType T ON (T.TypeID=B.TypeID AND T.DataStatus=1)
                            INNER JOIN BaseType BT ON BT.TypeID=B.Reimburse
                            INNER JOIN BaseType BT2 ON BT2.TypeID=B.SecurityType
                            INNER JOIN Users U ON U.UserID=B.UserID
                            INNER JOIN V_Users VU ON VU.UserID=U.UserID
                            WHERE 1=1 {0}";

                string strWhere = string.Empty;
                if (BorrowID != "")
                {
                    strWhere += " AND B.BorrowID=" + BorrowID + "";
                }
                if (BorrowUser != "")
                {
                    strWhere += " AND B.BorrowUser=" + BorrowUser + "";
                }
                if (Where != "") strWhere += " AND " + Where;
                strSql = string.Format(strSql, strWhere);
                return Maticsoft.DBUtility.DbHelperSQL.Query(strSql).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取投标记录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="BorrowID"></param>
        /// <param name="BidRecordID"></param>
        /// <param name="BidStatusList"></param>
        /// <returns></returns>
        public static DataTable getBidRecords(string UserName, string BorrowID, string BidRecordID)
        {
            string strSql = @"SELECT B.[BidRecordID],B.[BorrowID],B.[UserID],B.[BidAmount],B.[BidTime]
                                  ,B.[BidType],B.[BidStatus],U.UserName,U.CustId,B.OrdId,B.FreezeOrdId,B.FreezeTrxId
                            FROM [BidRecords] B INNER JOIN Users U ON U.UserID=B.UserID 
                            WHERE 1=1 {0} ORDER BY BidRecordID DESC";
            string strWhere = string.Empty;
            ArrayList list = new ArrayList();
            if (UserName != "")
            {
                strWhere += " AND U.UserName='" + UserName + "'";
            }
            if (BorrowID != "")
            {
                strWhere += " AND B.BorrowID=" + BorrowID + "";
            }
            if (BidRecordID != "")
            {
                strWhere += " AND B.BidRecordID=" + BidRecordID + "";
            }
            strSql = string.Format(strSql, strWhere);

            return Maticsoft.DBUtility.DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// 生成收益计划表
        /// </summary>
        public static int InsertIncomePlan(int BorrowID, int BidRecordID, decimal Percentage)
        {
            string sql = @"
                    INSERT INTO [IncomePlan] (BorrowID ,BidRecordID,PeriodNo ,IncomeAmount ,IncomeInterest,IncomeTime,RealIncomeTime,IncomeStatus)
                    SELECT  
                    BorrowID,BidRecordID=" + BidRecordID + ",PeriodNo,RepayAmount=sum(RepayAmount)*" + Percentage + @",
                    RepayInterest=sum(RepayInterest)*" + Percentage + @",RepayTime,RealRepayTime,RepayStatus
                    FROM RepayPlan
                    where  BorrowID=" + BorrowID + " group by  BorrowID,PeriodNo,RepayTime,RealRepayTime,RepayStatus";

            return Maticsoft.DBUtility.DbHelperSQL.ExecuteSql(sql);
        }


        /// <summary>
        /// 清除收益计划
        /// </summary>
        public static int DelIncomePlan(int BorrowID)
        {
            string sql = @"DELETE FROM IncomePlan WHERE BorrowID=" + BorrowID + "";
            return Maticsoft.DBUtility.DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 获取收益列表
        /// </summary>
        /// <param name="BorrowID"></param>
        /// <returns></returns>
        public static DataTable getInComeInfo(int BorrowID)
        {
            string strSql = @"SELECT [IncomePlanID],IC.[BorrowID],IC.[BidRecordID],[PeriodNo]
                                  ,[IncomeAmount],[IncomeInterest],[IncomeTime],[RealIncomeTime]
                                  ,[IncomeStatus],U.UserName
                              FROM BidRecords BR LEFT JOIN [IncomePlan] IC ON IC.BidRecordID=BR.BidRecordID
                              INNER JOIN Users U ON U.UserID=BR.UserID
                              WHERE BR.[BorrowID]=" + BorrowID + " ORDER BY IncomeTime ASC";
            return Maticsoft.DBUtility.DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// 更新投标状态为回收中
        /// </summary>
        public static int UpdateBidStatus(int BorrowID)
        {
            string sql = @"UPDATE BidRecords SET BidStatus=1 WHERE BorrowID=" + BorrowID + " AND BidStatus=0";
            return Maticsoft.DBUtility.DbHelperSQL.ExecuteSql(sql);
        }
    }
}

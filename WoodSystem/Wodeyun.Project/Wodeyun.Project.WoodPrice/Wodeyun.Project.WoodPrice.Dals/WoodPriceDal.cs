using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodPrice.Dals
{
    public class WoodPriceDal : CommonDal
    {
        public WoodPriceDal()
        {
            this.Init();
        }

        public WoodPriceDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        private void Init()
        {
            this.Table = "WoodPrice";
            this.Inserts = "[Unique],[AreaID],[TreeID],[Price],[Unit],[ExeDate],[State],[Version],[Operator],[Remark],[Log],[WetPrice],[CubePrice],[IsConfirmed]";
            this.Updates = "[AreaID],[TreeID],[Price],[Unit],[ExeDate],[Remark],[WetPrice],[CubePrice],[IsConfirmed]";
            this.Selects = "[Unique],[AreaID],[TreeID],[Price],[Unit],[ExeDate],[Remark],[WetPrice],[CubePrice],[IsConfirmed]";
        }

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <param name="AreaID"></param>
        /// <param name="TreeID"></param>
        /// <param name="ExeDate"></param>
        /// <returns></returns>
        public bool isExists(int AreaID, int TreeID, string ExeDate)
        {
            string strSql = "SELECT [AreaID],[TreeID],[ExeDate] FROM [WoodPrice] WHERE [AreaID]=" + AreaID + " AND TreeID=" + TreeID + " AND ExeDate='" + ExeDate + "' AND State<>" + StateEnum.Deleted.ToDatabase() + "";
            return this.Execute.GetEntityCollection(strSql).Count > 0;
        }

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <param name="AreaID"></param>
        /// <param name="TreeID"></param>
        /// <param name="ExeDate"></param>
        /// <returns></returns>
        public bool isExists(int AreaID, int TreeID, string ExeDate, decimal Price, decimal WetPrice, decimal CubePrice)
        {
            string strSql = "SELECT [AreaID],[TreeID],[ExeDate] FROM [WoodPrice] WHERE [AreaID]=" + AreaID + " AND TreeID=" + TreeID + " AND ExeDate='" + ExeDate + "' AND Price=" + Price + " AND WetPrice=" + WetPrice + " AND CubePrice=" + CubePrice + " AND State<>" + StateEnum.Deleted.ToDatabase() + "";
            return this.Execute.GetEntityCollection(strSql).Count > 0;
        }

        /// <summary>
        /// 分页查询价格体系
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetWoodPriceData(string startDate, string endDate, int start, int length, string area, string tree)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(area))
            {
                strWhere += " AND GA.Name LIKE '%" + area + "%'";
            }
            if (!string.IsNullOrWhiteSpace(tree))
            {
                strWhere += " AND GT.Name LIKE '%" + tree + "%'";
            }
            string sql = @"WITH [WoodPriceData] AS
                           (
                               SELECT WP.[Unique],WP.[AreaID],WP.[TreeID],WP.[Price],WP.[Unit],WP.[ExeDate],WP.[State],
                                WP.[Version],WP.[Operator],WP.[Remark],WP.[Log],GA.Name AS AreaName,GT.Name AS TreeName,WP.[WetPrice],WP.[CubePrice],WP.[IsConfirmed]
                                  FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                                  INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                                  WHERE WP.[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                AND WP.[ExeDate] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                AND WP.[ExeDate] <= " + endDate.ToDateEnd().ToDatabase() + strWhere + @"
                           )
                           SELECT ROW_NUMBER() OVER (ORDER BY AreaName,TreeName ASC,[Version] DESC) AS [Number],* INTO #TEMP FROM [WoodPriceData]
                           SELECT TOP " + length.ToDatabase() + @" * FROM #TEMP where [Number] >= " + start.ToDatabase() + @"
                           SELECT COUNT([Unique]) AS [Total] FROM #TEMP
                           DROP TABLE #TEMP";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 木材收购价格统计明细表
        /// </summary>
        /// <param name="month"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="area"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public EntityCollection getPriceDataList(string startDate, string endDate, int start, int length, string area, string tree, string IsCreate, string Supplier)
        {
            string begin = startDate.ToDateBegin();
            string end = endDate.ToDateEnd();
            string strwhere = string.Empty;
            //区域
            if (!string.IsNullOrWhiteSpace(area))
            {
                strwhere += " AND GI.Area LIKE '%" + area + "%'";
            }
            //树种
            if (!string.IsNullOrWhiteSpace(tree))
            {
                strwhere += " AND FP.Tree LIKE '%" + tree + "%'";
            }
            //卸货员
            if (!string.IsNullOrWhiteSpace(Supplier))
            {
                strwhere += " AND FP.Supplier LIKE '%" + Supplier + "%'";
            }
            if (!string.IsNullOrWhiteSpace(IsCreate))
            {
                if (IsCreate == "已生成")
                {
                    strwhere += " AND ISNULL(CS.OrderNo,'')<>''";
                }
                else
                {
                    strwhere += " AND ISNULL(CS.OrderNo,'')=''";
                }
            }
            string strSql = @"WITH PriceData AS(
                            SELECT DISTINCT WJ.[Unique], WJ.[BangID], WB.[Bang_Time], FP.[License], FP.[Tree],GI.[Area]
                            , FP.[Driver], FP.[Supplier], FP.[WeighTime],FP.FullVolume
                            ,[dbo].[GetJvolume](WB.jWeight,FP.FullVolume,EP.HandVolume,EP.EmptyVolume,EP.RebateVolume,WLB.RebateSkin,WLB.RebateScrap,FP.Tree,WLB.DeductVolume) AS JVolume
                            , WB.jWeight,WLB.RebateWater,GJ.IsAdd
                            ,[dbo].[GetGweight](WB.jWeight,WLB.RebateWater,WLB.RebateSkin,WLB.RebateScrap,FP.Tree) AS GWeight,
                            --取最接近过磅日期的价格
                            (SELECT TOP 1 WP.[Price] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> 9 AND GA.Name=GI.Area AND GT.Name=FP.Tree AND WP.ExeDate<=WB.[Bang_Time]
                            ORDER BY WP.ExeDate DESC) AS Price,

                            (SELECT TOP 1 WP.[WetPrice] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> 9 AND GA.Name=GI.Area AND GT.Name=FP.Tree AND WP.ExeDate<=WB.[Bang_Time]
                            ORDER BY WP.ExeDate DESC) AS WetPrice,

                            (SELECT TOP 1 WP.[CubePrice] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> 9 AND GA.Name=GI.Area AND GT.Name=FP.Tree AND WP.ExeDate<=WB.[Bang_Time]
                            ORDER BY WP.ExeDate DESC) AS CubePrice,

                            GS.LinkMan,CASE ISNULL(CS.OrderNo,'') WHEN '' THEN '未生成' ELSE '已生成' END IsCreate
                            FROM [WoodJoin] WJ
                            INNER JOIN [WoodBang] WB ON (WJ.[BangID] = WB.[bangid] 
                            AND WB.[Bang_Time] >= " + begin.ToDatabase() + @" AND WB.[Bang_Time] <= " + end.ToDatabase() + @")
                            LEFT JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] =" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [EmptyPound] EP ON (WJ.[WoodID] = EP.[WoodID] AND EP.[State] =" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [WoodLaboratory] WLB ON (WLB.[WoodID] = FP.[WoodID] AND WLB.[IsConfirmed] = 1 AND WLB.[State]=" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN GsmSupplier GS ON (GS.Name=FP.Supplier AND GS.[State]=" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [CostStatement] CS ON CS.[Unique]=WJ.[Unique] AND CS.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN [GsmJoin] GJ ON (GJ.[WoodID] = FP.[WoodID] AND GJ.[State] =" + StateEnum.Default.ToDatabase() + @")
                            INNER JOIN [GsmItem] GI ON (GJ.GsmID = GI.[Unique] AND GI.[State]=0) WHERE 1=1 AND WJ.[State]=0 AND GJ.GsmID > 0 " + strwhere + @"
                            --取不需要短信报备
                            UNION
                            SELECT DISTINCT WJ.[Unique], WJ.[BangID], WB.[Bang_Time], FP.[License], FP.[Tree],GA1.[Name] AS [Area]
                            , FP.[Driver], FP.[Supplier], FP.[WeighTime],FP.FullVolume
                            ,[dbo].[GetJvolume](WB.jWeight,FP.FullVolume,EP.HandVolume,EP.EmptyVolume,EP.RebateVolume,WLB.RebateSkin,WLB.RebateScrap,FP.Tree,WLB.DeductVolume) AS JVolume
                            , WB.jWeight,WLB.RebateWater,GJ.IsAdd
                            ,[dbo].[GetGweight](WB.jWeight,WLB.RebateWater,WLB.RebateSkin,WLB.RebateScrap,FP.Tree) AS GWeight,
                            --取最接近过磅日期的价格
                            (SELECT TOP 1 WP.[Price] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> 9 AND GA.Name=GA1.[Name] AND GT.Name=FP.Tree AND WP.ExeDate<=WB.[Bang_Time]
                            ORDER BY WP.ExeDate DESC) AS Price,

                            (SELECT TOP 1 WP.[WetPrice] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> 9 AND GA.Name=GA1.[Name] AND GT.Name=FP.Tree AND WP.ExeDate<=WB.[Bang_Time]
                            ORDER BY WP.ExeDate DESC) AS WetPrice,

                            (SELECT TOP 1 WP.[CubePrice] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> 9 AND GA.Name=GA1.[Name] AND GT.Name=FP.Tree AND WP.ExeDate<=WB.[Bang_Time]
                            ORDER BY WP.ExeDate DESC) AS CubePrice,

                            GS.LinkMan,CASE ISNULL(CS.OrderNo,'') WHEN '' THEN '未生成' ELSE '已生成' END IsCreate
                            FROM [WoodJoin] WJ
                            INNER JOIN [WoodBang] WB ON (WJ.[BangID] = WB.[bangid] 
                            AND WB.[Bang_Time] >= " + begin.ToDatabase() + @" AND WB.[Bang_Time] <= " + end.ToDatabase() + @")
                            LEFT JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] =" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [EmptyPound] EP ON (WJ.[WoodID] = EP.[WoodID] AND EP.[State] =" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [WoodLaboratory] WLB ON (WLB.[WoodID] = FP.[WoodID] AND WLB.[IsConfirmed] = 1 AND WLB.[State]=" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN GsmSupplier GS ON (GS.Name=FP.Supplier AND GS.[State]=" + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [CostStatement] CS ON CS.[Unique]=WJ.[Unique] AND CS.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN [GsmJoin] GJ ON (GJ.[WoodID] = FP.[WoodID] AND GJ.[State] = 0 AND GJ.[GsmID] = 0)
                            INNER JOIN GsmArea GA1 ON (GA1.[Unique]=GJ.AreaID AND GA1.[State]=0)
                            WHERE WJ.[State]=0  " + strwhere.Replace("GI.Area", "GA1.Name") + @")

                            SELECT ROW_NUMBER() OVER (ORDER BY [Bang_Time] ASC) AS [Number],[Unique],[BangID],[Bang_Time],FullVolume, 
                            [License],[Tree],[Area],[Driver],[Supplier],[WeighTime],JVolume,jWeight,RebateWater,GWeight,Price,WetPrice,CubePrice,LinkMan,IsCreate
                            ,GweightPrice=(ISNULL(Price,0) * ISNULL(GWeight,0)),VolumePrice=(ISNULL(CubePrice,0) * ISNULL(JVolume,0)),IsAdd
                            INTO #TEMP FROM PriceData

                            SELECT TOP " + length.ToDatabase() + @" [Number],[Unique],[BangID],[Bang_Time], 
                            [License],[Tree],[Area],[Driver],[Supplier],[WeighTime],JVolume,jWeight,FullVolume,
                            RebateWater,GWeight,Price,WetPrice,CubePrice,LinkMan,IsCreate,GweightPrice,VolumePrice,IsAdd FROM #TEMP WHERE [Number] >= " + start.ToDatabase() + @";

                            SELECT COUNT(*) AS [Total] FROM #TEMP

                            DROP TABLE #TEMP";
            IList collections = this.Execute.GetEntityCollections(strSql);
            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();
            return results;
        }

        /// <summary>
        /// 生成结算单
        /// </summary>
        /// <param name="Unique"></param>
        /// <param name="License">车号</param>
        /// <param name="LinkMan">联系人</param>
        /// <param name="GWeight">干吨重量</param>
        /// <param name="GPrice">干吨单价</param>
        /// <param name="Amount">干吨金额</param>
        /// <param name="account">操作人员</param>
        /// <returns></returns>
        public string InsertCostStatement(int Unique, string License, string LinkMan, decimal JWeight, string Tree, string Area,
            decimal GWeight, decimal GPrice, decimal Amount, int account, string Bang_Time, decimal JVolume, decimal FullVolume, decimal CubePrice, string groupid)
        {
            string orderno = GenerateCheckCodeNum(10);
            string log = "{\"Date\":\"" + DateTime.Now + "\",\"People\":\"" + account + "\"}";
            string strSql = @"
                            IF NOT EXISTS(SELECT 1 FROM [CostStatement] WHERE [Unique]=" + Unique + @" AND [State]=0)
                            BEGIN
                            INSERT INTO [CostStatement]([OrderNo],[Unique],[License],[LinkMan],[JWeight],[Tree],[Area],[GWeight],[GPrice]
                                       ,[Amount],[State],[Operator],[OperatorDate],[Auditor],[IsPrint],[Bang_Time],[JVolume],[FullVolume],[CubePrice],[Log],[GroupID])
                                 VALUES ('" + orderno + "'," + Unique + ",'" + License + "','" + LinkMan + "'," + JWeight + ",'" + Tree + "','" + Area + "'," + GWeight + "," + GPrice + @"
                                       ," + Amount + ",0," + account + @",GETDATE(),0,0,'" + Bang_Time + "'," + JVolume + "," + FullVolume + "," + CubePrice + ",'" + log + "','" + groupid + @"')
                            END";
            return this.Execute.ExecuteNonQuery(strSql) > 0 ? orderno : "";
        }

        private int rep = 0;
        /// <summary> 
        /// 生成随机数字字符串 
        /// </summary> 
        /// <param name="codeCount">待生成的位数</param> 
        /// <returns>生成的数字字符串</returns> 
        private string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="license"></param>
        /// <param name="printed"></param>
        /// <returns></returns>
        public EntityCollection getCostDataList(string startDate, string endDate, string bangstartDate, string bangendDate, int start, int length, string license, int printed, string groupid, string Supplier)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(license))
            {
                strWhere += " AND CS.License LIKE '%" + license + "%'";
            }
            if (!string.IsNullOrWhiteSpace(groupid))
            {
                strWhere += " AND CS.GroupID LIKE '%" + groupid + "%'";
            }
            if (printed != -1)
            {
                strWhere += " AND CS.IsPrint = " + printed;
            }
            if (!string.IsNullOrWhiteSpace(Supplier))
            {
                strWhere += " AND FP.Supplier LIKE '%" + Supplier + "%'";
            }
            if (!string.IsNullOrWhiteSpace(bangstartDate) && !string.IsNullOrWhiteSpace(bangendDate))
            {
                strWhere += " AND CS.Bang_Time >= " + bangstartDate.ToDateBegin().ToDatabase() + " AND CS.Bang_Time <= " + bangendDate.ToDateEnd().ToDatabase() + "";
            }

            string strSql = @"WITH T AS(
                                SELECT DISTINCT CS.OrderNo,CS.Bang_Time,CS.License,CS.LinkMan,CS.JWeight,CS.Area,CS.Tree,CS.GWeight,CS.GPrice,CS.Amount,CS.IsPrint,FP.Supplier,FP.[Unique],
                                CS.OperatorDate,CS.GroupID,CS.IsConfirmed,CS.JVolume,CS.FullVolume,CS.CubePrice,VolumePrice=(ISNULL(CS.CubePrice,0) * ISNULL(CS.JVolume,0)) FROM CostStatement CS 
                                INNER JOIN WoodJoin WJ ON WJ.[Unique]=CS.[Unique] AND WJ.[State]=0
                                INNER JOIN FullPound FP ON FP.WoodID=WJ.WoodID AND FP.[State]=0
                                WHERE CS.[State]=0 AND CS.OperatorDate >= " + startDate.ToDateBegin().ToDatabase() + @" AND CS.OperatorDate <= " + endDate.ToDateEnd().ToDatabase() + @" 
                                " + strWhere + @"
                                )
                                SELECT ROW_NUMBER() OVER (ORDER BY [OperatorDate] DESC) AS [Number],OrderNo,Bang_Time,GroupID,IsConfirmed,JVolume,FullVolume,CubePrice,
                                License,LinkMan,JWeight,Area,Tree,GWeight,GPrice,Amount,IsPrint,OperatorDate,Supplier,[Unique],VolumePrice INTO #TEMP FROM T

                                SELECT TOP " + length.ToDatabase() + @"  [Number],OrderNo,Bang_Time,GroupID,IsConfirmed,JVolume,FullVolume,CubePrice,
                                License,LinkMan,JWeight,Area,Tree,GWeight,GPrice,Amount,IsPrint,OperatorDate,Supplier,[Unique],VolumePrice 
                                FROM #TEMP WHERE [Number] >= " + start.ToDatabase() + @";

                                SELECT COUNT(*) AS [Total] FROM #TEMP
                                DROP TABLE #TEMP";
            IList collections = this.Execute.GetEntityCollections(strSql);
            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();
            return results;
        }


        /// <summary>
        /// 标注结算单已打印过状态
        /// </summary>
        /// <param name="arrWoodID">结算单号</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdatePrintState(string ordernos, int Account, int Ismegre)
        {
            string groupid = string.Empty;
            if (Ismegre == 1)
            {
                groupid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            string sql = @"UPDATE [CostStatement] SET [IsPrint] = 1 , Auditor=" + Account + @"
                           WHERE [State] = " + StateEnum.Default.ToDatabase() + @"
                               " + string.Format(" AND [OrderNo] IN ({0})", "SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + ordernos + "',',')");

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取木片检验结果通知单的打印信息
        /// </summary>
        /// <param name="arrWoodID">结算单号ID集合</param>
        /// <returns>结果集</returns>
        public IList GetEntitysForCostPrint(string ordernos)
        {
            string sql = @"WITH T AS(
                            SELECT DISTINCT CS.OrderNo,CS.Bang_Time,CS.License,CS.LinkMan,CS.JWeight,CS.Area,CS.Tree,CS.GWeight,CS.GPrice,CS.Amount,CS.IsPrint,
                            CS.OperatorDate,CS.JVolume,CS.FullVolume,CS.CubePrice FROM CostStatement CS 
                            WHERE CS.[State]=0 AND CS.OrderNo IN(
                            SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + ordernos + @"',','))
                            )
                            SELECT * INTO #TEMP FROM T;
                            --重量结算
                            SELECT Tree=MAX(Tree),GPrice,JWeight=SUM(JWeight),GWeight=SUM(GWeight),Amount=SUM(Amount) FROM #TEMP GROUP BY GPrice
                            --体积结算
                            SELECT Tree=MAX(Tree),CubePrice,JVolume=SUM(JVolume),JWeight=SUM(JWeight),Amount=SUM(ISNULL(JVolume,0)*ISNULL(CubePrice,0)) FROM #TEMP GROUP BY CubePrice
                            SELECT MINBang_Time=MIN(Bang_Time),MAXBang_Time=MAX(Bang_Time),CarCount=COUNT(OrderNo) FROM #TEMP
                            DROP TABLE #TEMP";
            return this.Execute.GetEntityCollections(sql);
        }

        /// <summary>
        /// 修结算单
        /// </summary>
        /// <param name="OrderNo">结算单号</param>
        /// <param name="Area">区域</param>
        /// <param name="Account">操作人员</param>
        /// <param name="price">木片单价</param>
        /// <returns></returns>
        public int UpdateCostOrder(string OrderNo, string Area, int Account, decimal price, decimal CubePrice)
        {
            string log = "{\"Date\":\"" + DateTime.Now + "\",\"ModifyAreaPeople\":\"" + Account + "\"}";
            string strSql = "UPDATE [CostStatement] SET Area='" + Area + "',GPrice=" + price + ",CubePrice=" + CubePrice + ",Amount=(" + price + " * (SELECT GWeight FROM [CostStatement] WHERE OrderNo='" + OrderNo + "')) ";
            strSql += ",[Log]+='" + log + "'  WHERE OrderNo='" + OrderNo + "'";
            return this.Execute.ExecuteNonQuery(strSql);
        }


        /// <summary>
        /// 修结卸货代码
        /// </summary>
        /// <param name="Unique"></param>
        /// <param name="Account"></param>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public int UpdateSupplier(int Unique, int Account, string Supplier)
        {
            string log = "{\"Date\":\"" + DateTime.Now + "\",\"ModifySupplierPeople\":\"" + Account + "\"}";
            string strSql = "UPDATE [FullPound] SET Supplier='" + Supplier + "',[Log]+='" + log + "'  WHERE [Unique]=" + Unique + " AND [State]=0";
            return this.Execute.ExecuteNonQuery(strSql);
        }

        /// <summary>
        /// 根据区域和品种获取木片价格
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Bang_Time"></param>
        /// <param name="Tree"></param>
        /// <returns></returns>
        public Entity getWoodPrice(string Area, string Bang_Time, string Tree)
        {
            string strSql = "SELECT TOP 1 WP.[Price],WP.[WetPrice],WP.[CubePrice] FROM [WoodPrice] WP INNER JOIN GsmArea GA ON GA.[Unique]=WP.[AreaID] AND GA.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN GsmTree GT ON GT.[Unique]=WP.[TreeID] AND GT.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE WP.[State] <> " + StateEnum.Deleted.ToDatabase() + @" AND GA.Name='" + Area + "' AND GT.Name='" + Tree + "' AND WP.ExeDate<='" + Bang_Time + @"'
                            ORDER BY WP.ExeDate DESC";
            return this.Execute.GetEntity(strSql);
        }


        /// <summary>
        /// 标注结算单审核状态
        /// </summary>
        /// <param name="ordernos">结算单号</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateIsConfirmedState(string ordernos, int Account)
        {

            string sql = @"UPDATE [CostStatement] SET [IsConfirmed] = 1 ,ConfirmTime='" + System.DateTime.Now + "', Confirmor=" + Account + @"
                           WHERE [State] = " + StateEnum.Default.ToDatabase() + @"
                               " + string.Format(" AND [OrderNo] IN ({0})", "SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + ordernos + "',',')");

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 结算单反审核
        /// </summary>
        /// <param name="unique">记录号</param>
        /// <param name="operatorID">审核人身份识别</param>
        /// <returns>数据库表受影响的行数</returns>
        public int BackCheckCostState(string ordernos, int Account)
        {
            string log = "{\"BackConfirmeDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"BackConfirmePeople\":\"" + Account + "\"}";
            string sql = @"update [CostStatement] set [IsConfirmed] = 0, [Log] += '" + log + @"'
                           where [IsConfirmed] = 1 and [State] = " + StateEnum.Default.ToDatabase() + @"
                                " + string.Format(" AND [OrderNo] IN ({0})", "SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + ordernos + "',',')");

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取用户成本结算的审核权限
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public int ReviewPermissionsbyAccount(int Account, int role)
        {
            string strSql = "SELECT COUNT([Account]) AS Total  FROM [Act] WHERE [Account]=" + Account + " AND [Role]=" + role + " AND [State]=0";
            return this.Execute.GetEntity(strSql).GetValue("Total").TryInt32();
        }

        /// <summary>
        /// 删除数据价格体系
        /// </summary>
        /// <param name="unique"></param>
        /// <returns></returns>
        public int DeleteWoodPriceByUnique(int unique, int Version)
        {

            string sql = @"update [WoodPrice]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                           where [Unique] = " + unique.ToDatabase() + " and [Version]=" + Version + @"
                           and [State] <> " + StateEnum.Deleted.ToDatabase();
            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除结算单
        /// </summary>
        /// <param name="ordernos"></param>
        /// <returns></returns>
        public int DeleteCostList(string ordernos, int Account)
        {
            string log = "{\"Date\":\"" + DateTime.Now + "\",\"DeletePeople\":\"" + Account + "\"}";
            string sql = @"UPDATE [CostStatement] SET [State] = " + StateEnum.Deleted.ToDatabase() + @",[Log]+='" + log + @"'
                           WHERE [State] = " + StateEnum.Default.ToDatabase() + @"
                               " + string.Format(" AND [OrderNo] IN ({0})", "SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + ordernos + "',',')");

            return this.Execute.ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 获取当前生成结算单列表
        /// </summary>
        /// <returns></returns>
        public EntityCollection getCurrentCostDataList(string ordernos)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(ordernos))
            {
                strWhere += " AND CS.OrderNo IN(" + ordernos + ")";
            }
            else
                strWhere += " AND CS.OrderNo IN('')";
            string strSql = @"SELECT DISTINCT CS.OrderNo,CS.Bang_Time,CS.License,CS.LinkMan,CS.JWeight,CS.Area,CS.Tree,CS.GWeight,CS.GPrice,CS.Amount,CS.IsPrint,FP.Supplier,
                                CS.OperatorDate,CS.GroupID,CS.IsConfirmed,CS.JVolume,CS.FullVolume,CS.CubePrice,VolumePrice=(ISNULL(CS.CubePrice,0) * ISNULL(CS.JVolume,0)) FROM CostStatement CS 
                                INNER JOIN WoodJoin WJ ON WJ.[Unique]=CS.[Unique] AND WJ.[State]=0
                                INNER JOIN FullPound FP ON FP.WoodID=WJ.WoodID AND FP.[State]=0
                                WHERE CS.[State]=0 " + strWhere + @"";
            return this.Execute.GetEntityCollection(strSql);
        }

        /// <summary>
        /// 修改价格体系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateWoodPrice(Entity entity)
        {
            string Inserts = "[Unique],[AreaID],[TreeID],[Price],[Unit],[ExeDate],[State],[Version],[Operator],[Remark],[Log],[WetPrice],[CubePrice],[IsConfirmed]";
            string Updates = "[AreaID],[TreeID],[Price],[Unit],[ExeDate],[Remark],[WetPrice],[CubePrice],[IsConfirmed]";
            string sql = @"update [WoodPrice]
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + " AND [Version]=" + entity.GetValue("Version1").ToDatabase() + @"
                           and [State] <> " + StateEnum.Deleted.ToDatabase() + @";
                           insert into [WoodPrice]
                           select top 1 " + entity.ToUpdate(Inserts, Updates).Replace("[State]", StateEnum.Default.ToDatabase()).Replace("[Version]", "(select [Version]=MAX([Version]) from WoodPrice where [Unique]=" + entity.GetValue("Unique").ToDatabase() + " and [State] <> " + StateEnum.Deleted.ToDatabase() + ") + 1") + @"
                           from [WoodPrice]
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + " AND [Version]=" + entity.GetValue("Version1").ToDatabase() + @"
                           and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           order by [Version] desc";

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 审核、反审价格体系
        /// </summary>
        /// <param name="unique"></param>
        /// <param name="version"></param>
        /// <param name="Account"></param>
        /// <returns></returns>
        public int UpdateWoodPriceState(int unique, int version, int Account, int checkType)
        {
            string log = string.Empty;
            int IsConfirmed = checkType == 0 ? 1 : 0;
            log = checkType == 0 ? "{\"Date\":\"" + DateTime.Now + "\",\"CheckPeople\":\"" + Account + "\"}" : "{\"Date\":\"" + DateTime.Now + "\",\"BackCheckPeople\":\"" + Account + "\"}";
            string strSql = "UPDATE [WoodPrice] SET IsConfirmed=" + IsConfirmed + ",[Log]+='" + log + "' WHERE [Unique]=" + unique + " AND [Version]=" + version + " AND [State]<>" + StateEnum.Deleted.ToDatabase() + "";
            return this.Execute.ExecuteNonQuery(strSql);
        }

    }
}

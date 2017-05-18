namespace Wodeyun.Bf.Base.Enums
{
    /// <summary>
    /// RFID电子卡原始发卡地
    /// </summary>
    public enum CardComeFrom
    {
        /// <summary>
        /// 工厂门口
        /// </summary>
        Factry = 1,

        /// <summary>
        /// 移动服务站
        /// </summary>
        Station = 2,

        /// <summary>
        /// 地磅
        /// </summary>
        Weighbridge = 3
    }

    /// <summary>
    /// RFID电子卡颜色类型
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// 绿卡
        /// </summary>
        Green = 1,

        /// <summary>
        /// 红卡
        /// </summary>
        Red = 2
    }

    /// <summary>
    /// RFID电子卡的状态
    /// </summary>
    public enum CardState
    {
        /// <summary>
        /// 未使用，可以发卡
        /// </summary>
        UnUse = 0,

        /// <summary>
        /// 检查站已登记，等待入厂验收
        /// </summary>
        Station = 1,

        /// <summary>
        /// 入厂已登记，等待首磅
        /// </summary>
        Door = 2,

        /// <summary>
        /// 首磅已登记，等待取样
        /// </summary>
        Balance = 3,

        /// <summary>
        /// 取样已登记，等待回皮
        /// </summary>
        Sample = 4,

        /// <summary>
        /// 回皮已登记，等待离厂回收
        /// </summary>
        EmptyBalance = 5
    }

    public enum StateEnum
    {
        Default = 0,
        Updated = 8,
        Deleted = 9
    }
}

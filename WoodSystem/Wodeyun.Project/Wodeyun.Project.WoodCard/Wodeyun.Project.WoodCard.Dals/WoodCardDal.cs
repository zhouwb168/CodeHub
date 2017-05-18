using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodCard.Dals
{
    public class WoodCardDal : CommonDal
    {
        private void Init()
        {
            this.Table = "WoodCard";
            this.Inserts = "[Unique], [CID], [CardNumber], [Operator], [State], [Version], [Log]";
            this.Updates = "[CID], [CardNumber], [Operator]";
            this.Selects = "[Unique], [CID], [CardNumber]";
            this.Order = "[CardNumber] asc";
        }

        public WoodCardDal()
        {
            this.Init();
        }

        public WoodCardDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}

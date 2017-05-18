using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodGps.Dals
{
    public class WoodGpsDal : CommonDal
    {
        private void Init()
        {
            this.Table = "WoodGps";
            this.Inserts = "[Unique], [StationName], [Lat], [Lng], [Operator], [State], [Version], [Log]";
            this.Updates = "[StationName], [Lat], [Lng], [Operator]";
            this.Selects = "[Unique], [StationName], [Lat], [Lng]";
            this.Order = "[Unique] asc";
        }

        public WoodGpsDal()
        {
            this.Init();
        }

        public WoodGpsDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

    }
}

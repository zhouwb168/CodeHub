using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodMachine.Dals
{
    public class WoodMachineDal : CommonDal
    {

        private void Init()
        {
            this.Table = "WoodMachine";
            this.Inserts = "[Unique], [Name], [MachineNumber], [Operator], [State], [Version], [Log]";
            this.Updates = "[Name], [MachineNumber]";
            this.Selects = "[Unique], [Name], [MachineNumber]";
            this.Order = "[Unique] asc";
        }

        public WoodMachineDal()
        {
            this.Init();
        }

        public WoodMachineDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }
    }
}

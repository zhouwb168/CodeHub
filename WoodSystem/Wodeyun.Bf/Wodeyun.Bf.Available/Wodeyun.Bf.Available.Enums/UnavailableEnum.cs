using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wodeyun.Bf.Available.Enums
{
    public enum UnavailableEnum
    {
        Offline,    // 因系统故障导致服务不可用
        Security,   // 因安全原因等设置为不可用
        Deny        // 因权限问题导致不可用
    }
}

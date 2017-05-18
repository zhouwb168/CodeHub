using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TC.Tools.Model
{
    [Serializable]
    public class Users
    {
        public int UserID { get; set; }
        public string NickName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string CardNo { get; set; }
        public string CustId { get; set; }
    }
}

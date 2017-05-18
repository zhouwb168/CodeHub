using System;

namespace TestLog
{
    [Serializable]
    public class Class1 : GEIIO.Device.DeviceParameter
    {
        public Class1()
        {
            mystring = "string";
        }

        public string mystring { get; set; }

        public override object Repair()
        {
            throw new NotImplementedException();
        }
    }
}

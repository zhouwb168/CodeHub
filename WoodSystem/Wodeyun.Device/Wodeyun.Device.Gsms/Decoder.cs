using System;

using Wodeyun.Gf.Entities;

namespace Wodeyun.Device.Gsms
{
    public class Decoder
    {
        private PropertyCollection _PropertyCollection = new PropertyCollection();

        public Decoder()
        {
            this._PropertyCollection.Add(new SimpleProperty("Key", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Count", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Index", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Text", typeof(string)));
        }

        public string GetOa(string pdu)
        {
            int sca = Convert.ToInt32(pdu.Substring(0, 2), 16) * 2 - 2;
            int length = Convert.ToInt32(pdu.Substring(4 + sca + 2, 2), 16) + (Convert.ToInt32(pdu.Substring(4 + sca + 2, 2), 16) % 2 == 1 ? 1 : 0);
            string oa = pdu.Substring(4 + sca + 2 + 4, length);

            string parity = this.Parity(oa);

            if (parity.StartsWith("86") == true) return parity.TrimEnd('F', 'f').Substring(2);
            else return parity.TrimEnd('F', 'f');
        }

        public string GetScts(string pdu)
        {
            int sca = Convert.ToInt32(pdu.Substring(0, 2), 16) * 2 - 2;
            int oa = Convert.ToInt32(pdu.Substring(4 + sca + 2, 2), 16) + (Convert.ToInt32(pdu.Substring(4 + sca + 2, 2), 16) % 2 == 1 ? 1 : 0);
            int length = 14;
            string scts = pdu.Substring(4 + sca + 2 + 4 + oa + 4, length);

            string parity = this.Parity(scts);

            return string.Format("20{0}-{1}-{2} {3}:{4}:{5}", parity.Substring(0, 2), parity.Substring(2, 2), parity.Substring(4, 2), parity.Substring(6, 2), parity.Substring(8, 2), parity.Substring(10, 2));
        }

        public Entity GetUd(string pdu)
        {
            Entity entity = new Entity(this._PropertyCollection);

            int sca = Convert.ToInt32(pdu.Substring(0, 2), 16) * 2 - 2;
            int oa = Convert.ToInt32(pdu.Substring(4 + sca + 2, 2), 16) + (Convert.ToInt32(pdu.Substring(4 + sca + 2, 2), 16) % 2 == 1 ? 1 : 0);
            int scts = 14;
            string pdut = pdu.Substring(4 + sca, 2);
            string dcs = pdu.Substring(4 + sca + 2 + 4 + oa + 2, 2);

            if (pdut == "00" || pdut == "04" || pdut == "24")
            {
                string ud = pdu.Substring(4 + sca + 2 + 4 + oa + 4 + scts + 2);

                entity.SetValue("Text", this.GetText(ud));
            }
            else
            {
                if (dcs == "08")
                {
                    string ud = pdu.Substring(4 + sca + 2 + 4 + oa + 4 + scts + 2 + 12);

                    entity.SetValue("Key", pdu.Substring(4 + sca + 2 + 4 + oa + 4 + scts + 2 + 6, 2));
                    entity.SetValue("Count", pdu.Substring(4 + sca + 2 + 4 + oa + 4 + scts + 2 + 8, 2));
                    entity.SetValue("Index", pdu.Substring(4 + sca + 2 + 4 + oa + 4 + scts + 2 + 10, 2));
                    entity.SetValue("Text", this.GetText(ud));
                }
                else entity.SetValue("Text", pdu);
            }

            return entity;
        }

        private string GetText(string value)
        {
            string result = string.Empty;

            for (int i = 0; i < value.Length; i += 4)
            {
                try
                {
                    result = result + ((char)Convert.ToInt16(value.Substring(i, 4), 16)).ToString();
                }
                catch { }
            }

            return result;
        }

        private string Parity(string value)
        {
            string result = string.Empty;

            if (value.Length % 2 != 0) value = value + "F";

            for (int i = 0; i < value.Length; i += 2)
            {
                result = result + value[i + 1];
                result = result + value[i];
            }

            return result;
        }
    }
}

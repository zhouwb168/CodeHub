using System.Text;

namespace Wodeyun.Device.Gsms
{
    public class Encoder
    {
        public string GetPdu(string sca, string mobile, string text)
        {
            string length = (this.GetUd(text).Length / 2).ToString("X2");

            return string.Format("{0}1100{1}000800{2}{3}", this.GetSca(sca), this.GetDa(mobile), length, this.GetUd(text));
        }

        private string GetSca(string mobile)
        {
            string sca = "91" + this.Parity("86" + mobile);

            return (sca.Length / 2).ToString("X2") + sca;
        }

        private string GetDa(string mobile)
        {
            int length = ("86" + mobile).Length;

            return length.ToString("X2") + "91" + this.Parity("86" + mobile);
        }

        private string GetUd(string text)
        {
            string result = string.Empty;

            byte[] bytes = Encoding.Unicode.GetBytes(text);

            for (int i = 0; i < bytes.Length; i += 2)
            {
                result = result + bytes[i + 1].ToString("X2");
                result = result + bytes[i].ToString("X2");
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

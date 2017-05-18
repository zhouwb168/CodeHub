using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TC.Tools
{
    public static class Util
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入INI
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Writeini(string section, string key, string val)
        {
            WritePrivateProfileString(section, key, val, Application.StartupPath + "\\mcs.ini");
        }

        /// <summary>
        /// 读取INI
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Readini(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, Application.StartupPath + "\\mcs.ini");
            return temp.ToString();
        }

        public static string ToJosnDatabase(this string value, int UserID, string key)
        {
            string ret = string.Empty;
            bool flg = false;
            if (value == null) return "";
            value = value.Replace("{", "").Replace("}", "");
            value = value.Replace("\"", "");
            string[] strArr = value.Split(new char[] { ',' });
            foreach (string s in strArr)
            {
                string[] values = s.Split(new char[] { ':' });
                if (values[0] == "UserID" && UserID == Convert.ToInt32(values[1]))
                {
                    flg = true;
                }
                if (flg && key == values[0])
                {
                    ret = values[1];
                    break;
                }
            }
            return ret;
        }
    }
}

using System;

using System.Collections;

using Wodeyun.Gf.System.Exceptions;

namespace Wodeyun.Gf.System.Utilities
{
    public static class Extensions
    {
        public static string[] Add(this string[] values, string value)
        {
            string[] results = new string[values.Length + 1];

            Array.Copy(values, results, values.Length);
            results[values.Length] = value;

            return results;
        }

        public static string[] Left(this string[] values, int length)
        {
            if (length <= 0) throw new IndexOutOfRangeException();
            if (length > values.Length) throw new IndexOutOfRangeException();

            string[] results = new string[length];

            for (int i = 0; i < length; i++)
                results[i] = values[i];

            return results;
        }

        public static IList ToList(this string value, string connect)
        {
            string[] items = value.Split(connect.ToCharArray());

            IList results = items.ToList();
            results.Remove(string.Empty);

            return results;
        }

        public static IList ToList(this string[] values)
        {
            IList results = new ArrayList();

            for (int i = 0; i < values.Length; i++)
                results.Add(values[i]);

            return results;
        }

        public static string ToDatabase(this object value)
        {
            if (value is string) return "'" + value.ToString().Replace("'", "''") + "'";
            if (value is DateTime) return "'" + value.ToString() + "'";
            if (value is Enum) return ((int)value).ToString();
            if (value == null) return "null";

            return value.ToString();
        }

        public static string ToLike(this object value)
        {
            return string.Format("%{0}%", value.ToString());
        }

        public static string ToDateBegin(this object value)
        {
            return string.Format("{0} 00:00:00", value.ToString());
        }

        public static string ToDateEnd(this object value)
        {
            return string.Format("{0} 23:59:59", value.ToString());
        }

        public static object ToVariable(this object value)
        {
            if (value is DBNull) return null;
            if (value.ToString().IndexOf("''") != -1) return value.ToString().Replace("''", "'");
            
            return value;
        }

        public static string ToHtml(this object value)
        {
            return value.ToString().Replace("\"", "\\\"");
        }

        public static int ToInt32(this object value)
        {
            if (value == null) throw new NullReferenceException();
            
            return Convert.ToInt32(value);
        }

        public static bool IsInt32(this object value)
        {
            int result;

            return int.TryParse(value.ToString(), out result);
        }

        public static int TryInt32(this object value)
        {
            if (value == null) return 0;
            if (value.IsInt32() == false) return 0;

            return Convert.ToInt32(value);
        }

        public static bool ToBoolean(this object value)
        {
            return Convert.ToBoolean(value);
        }

        public static DateTime ToDateTime(this object value)
        {
            return Convert.ToDateTime(value);
        }

        public static Decimal ToDecimal(this object value)
        {
            if (value == null) throw new NullReferenceException();

            return Convert.ToDecimal(value);
        }

        public static bool IsDecimal(this object value)
        {
            decimal result;

            return decimal.TryParse(value.ToString(), out result);
        }

        public static Decimal TryDecimal(this object value)
        {
            if (value == null) return 0;
            if (value.IsDecimal() == false) return 0;

            return Convert.ToDecimal(value);
        }

        public static T ToEnum<T>(this int value)
        {
            if (typeof(T).IsEnum == false) throw new TypeNotMatchException();

            try
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            catch
            {
                throw new ValueNotFoundException();
            }
        }

        public static T ToEnum<T>(this object value) 
        {
            if (typeof(T).IsEnum == false) throw new TypeNotMatchException();
            if (Enum.IsDefined(typeof(T), value) == false) throw new ValueNotFoundException();

            return (T)Enum.Parse(typeof(T), value.ToString()); 
        }

        public static string ToString(this IList values, string separator)
        {
            if (values.Count == 0) return string.Empty;

            string result = string.Empty;

            for (int i = 0; i < values.Count; i++)
                result = result + values[i].ToString() + separator;

            return result.Remove(result.Length - separator.Length);
        }

        public static string TryString(this object value)
        {
            if (value == null) return string.Empty;

            return value.ToString();
        }
    }
}

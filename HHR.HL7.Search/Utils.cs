using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
    internal static class Utils
    {
        internal static void UIThread(this ISynchronizeInvoke control, MethodInvoker code)
        {
            if (control != null && control.InvokeRequired)
            {
                if (!(control is Control) || (control as Control).IsHandleCreated)
                {
                    control.Invoke(code, null);
                }
                return;
            }
            else
                code.Invoke();
        }

        internal static V Maybe<T, V>(this T t, Func<T, V> selector)
        {
            return t != null ? selector(t) : default(V);
        }

        internal static DateTime ToDatetime(this string value, string format, DateTime defaultDate)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultDate;

            DateTime ret;
            if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret))
                return ret;

            return defaultDate;
        }

        internal static DateTime? ToNullableDatetime(this string value, params string[] formats)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            DateTime ret;
            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ret))
                    return ret;
            }

            return null;
        }

        internal static decimal ToDecimal(this string value)
        {
            decimal ret = decimal.Zero;

            if (string.IsNullOrWhiteSpace(value))
                return ret;

            if (decimal.TryParse(value, out ret))
                return ret;

            return ret;
        }

        internal static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        internal static int ToInt32(this string value)
        {
            Int32 result = 0;

            if (!value.IsNullOrEmpty())
                Int32.TryParse(value, out result);

            return result;
        }

        internal static string RegexReplace(this string source, string pattern, string replacement)
        {
            return Regex.Replace(source, pattern, replacement);
        }

        internal static string TrimStart(this string target, string trimString)
        {
            var result = target;
            while (result.StartsWith(trimString))
                result = result.Substring(trimString.Length);

            return result;
        }

        internal static string TrimEnd(this string target, string trimString)
        {
            var result = target;
            while (result.EndsWith(trimString))
                result = result.Substring(0, result.Length - trimString.Length);

            return result;
        }
    }

    internal static class UriExtensions
    {
        internal static string GetAbsoluteUriExceptUserInfo(this Uri uri)
        {
            return uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped);
        }

        internal static string GetBasicAuthString(this Uri uri)
        {
            if (string.IsNullOrWhiteSpace(uri.UserInfo))
                return null;

            var parts = GetUserInfoParts(uri);

            var credentialsBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", parts[0], parts[1]));
            return Convert.ToBase64String(credentialsBytes);
        }

        internal static string[] GetUserInfoParts(this Uri uri)
        {
            return uri.UserInfo
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => Uri.UnescapeDataString(p))
                .ToArray();
        }
    }
}

using System;

namespace RasmiOnline.Business
{
    public static class Utils
    {
        public static string FormatString(string source, params string[] placeholders)
        {
            try
            {
                for (int i = 0; i < placeholders.Length; i++)
                {
                    var p = placeholders[i];
                    source = source.Replace("{" + i.ToString() + "}", (string.IsNullOrEmpty(p) ? " - " : p));
                }
                return source;
            }
            catch (Exception e)
            {
                return source;
            }
        }
    }
}

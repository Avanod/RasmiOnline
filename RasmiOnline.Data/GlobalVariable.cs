namespace RasmiOnline.Data
{
    using System;

    public class GlobalVariable
    {
        public static string LogPath { get { return $"{AppDomain.CurrentDomain.BaseDirectory}\\Log"; } }
        public static string SiteRootUrl { get { return "Index"; } }
    }
}

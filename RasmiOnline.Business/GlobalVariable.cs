namespace RasmiOnline.Business
{
    using System;

    public class GlobalVariable
    {
        public static string LogPath { get { return $"{AppDomain.CurrentDomain.BaseDirectory}\\Log"; } }
        public static string ObserverConfig { get { return $"{AppDomain.CurrentDomain.BaseDirectory}\\ObserverConfig.json"; } }

        public static string AdminInstanceId = "-1001128896026";
    }
}

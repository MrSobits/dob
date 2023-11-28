namespace Bars.Gkh.Gis.Utils
{
    public static class BillingConnection
    {
#if !DEBUG
        // Подключения для продакшена
        public const string ConnectStringCommon =
           "Server=10.14.6.61;CommandTimeout=3600;Database=kp5;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Release);";

        public const string ConnectStringForGis1 =
            "Server=10.14.6.60;CommandTimeout=3600;Database=kp5;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Release);";

        public const string ConnectStringForPgu =
            "Server=10.14.6.61;CommandTimeout=3600;Database=portal;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Release);";

        public const string ConnectStringForOlapReport =
            "Server=10.14.6.60;CommandTimeout=3600;Database=gis_olap_report;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Release);";

        public const string ConnectStringForOtherSystem =
         "Server=10.14.6.61;CommandTimeout=3600;Database=gkh_rt;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Release);";
#else
        //Подключения для теста 

        //загрузка инкрементальных данных ПО ЕРЦ РТ
        public const string ConnectStringCommon =
            "Server=192.168.229.21;CommandTimeout=3600;Database=kp5;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Debug);";
        
        //загрузка инкрементальных данных ПО ЕРЦ Казани
        public const string ConnectStringForGis1 =
            "Server=192.168.229.20;CommandTimeout=3600;Database=kp5;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Debug);";

        //public const string ConnectStringForPgu = "Server=192.168.229.20;CommandTimeout=3600;Database=portal_test;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Debug);";

        //загрузка инкрементальных данных для отчетов МСА РТ 
        //public const string ConnectStringForOlapReport =
        //    "Server=192.168.229.20;CommandTimeout=3600;Database=gis_olap_report_test;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Debug);";

        //загрузка инкрементальных данных в формате 3.0
        //public const string ConnectStringForOtherSystem =
       // "Server=192.168.229.21;CommandTimeout=3600;Database=gkh_rt_test;User ID=postgres;Password=postgres;Timeout=1024;MaxPoolSize=500;ApplicationName=Bars.Gkh.Main(Debug);";
#endif

    }
}
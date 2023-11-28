namespace Bars.B4.Modules.Analytics.Reports.Sti
{
    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;

    /// <summary>
    /// Соединение Stimul к БД
    /// </summary>
    public class StiConnection
    {
        private readonly string _name;
        private readonly string _alias;
        private readonly string _connectionString;


        public static StiConnection PostgreConnection = new StiConnection("GkhDb", "Server=localhost;CommandTimeout=120;Database=dbname;User ID=user;Password=passw;");

        /// <summary>
        /// Создает новый экземпляр
        /// </summary>
        /// <param name="connectionString">Connection string сединения</param>
        /// <param name="alias">Псевдоним соединения</param>
        /// <param name="name">Имя соединения</param>
        public StiConnection(string name, string @alias, string connectionString)
        {
            _connectionString = connectionString;
            _alias = alias;
            _name = name;
        }

        /// <summary>
        /// Создает новый экземпляр
        /// </summary>
        /// <param name="name">Имя соединения</param>
        /// <param name="connectionString">Connection string сединения</param>
        public StiConnection(string name, string connectionString)
            : this(name, name, connectionString)
        {

        }

        /// <summary>
        /// Добавить соединение к отчету
        /// </summary>
        /// <param name="stiReport">Stimul-отчет</param>
        public void AddTo(StiReport stiReport)
        {
            stiReport.Dictionary.Databases.Add(
                new StiPostgreSQLDatabase(_name, _alias, _connectionString));
        }
    }
}

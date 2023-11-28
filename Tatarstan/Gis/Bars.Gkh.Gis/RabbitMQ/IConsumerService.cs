namespace Bars.Gkh.Gis.RabbitMQ
{
    /// <summary>
    /// Прослушиватель очереди
    /// </summary>
    public interface IConsumerService
    {
        /// <summary>
        /// Начать прослушивание
        /// </summary>
        /// <param name="serverAddress">Адрес сервера RabbitMQ</param>
        /// <param name="port">Порт</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="queueName">Имя очереди, в которую будет публиковаться сообщения</param>
        void StartConsuming(string serverAddress, int port, string login, string password, string queueName);
    }
}

namespace Bars.Gkh.Gis.RabbitMQ
{
    /// <summary>Публикатор сообщений в очередь</summary>
    public interface IProducerService
    {
        /// <summary>Метод публикации сообщения в очередь.</summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="serverAddress">Адрес сервера RabbitMQ</param>
        /// <param name="port">Порт</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="queueName">Имя очереди, в которую будет публиковаться сообщения</param>
        /// <param name="message">Сообщение</param>
        void SendMessage<T>(string serverAddress, int port, string login, string password, string queueName, T message)
            where T : BaseTask;
    }
}

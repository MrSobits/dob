namespace Bars.Gkh.Gis.RabbitMQ.Impl
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using global::RabbitMQ.Client;

    /// <summary>Реализация публикатора сообщений в очередь</summary>
    public class ProducerService : IDisposable, IProducerService
    {
        /// <summary></summary>
        protected IModel Model { get; set; }

        /// <summary></summary>
        protected IConnection Connection { get; set; }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
            }

            if (Model != null)
            {
                Model.Abort();
            }
        }

        /// <summary>Метод публикации сообщения в очередь.</summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="serverAddress">Адрес сервера RabbitMQ</param>
        /// <param name="port">Порт</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="queueName">Имя очереди, в которую будет публиковаться сообщения (Пример: "reports")</param>
        /// <param name="message">Сообщение</param>
        public void SendMessage<T>(string serverAddress, int port, string login, string password, string queueName, T message) where T : BaseTask
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = serverAddress,
                Port = port,
                UserName = login,
                Password = password
            };
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(queueName, true, false, false, null);

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, message);
                IBasicProperties basicProperties = Model.CreateBasicProperties();
                basicProperties.SetPersistent(true);
                Model.BasicPublish(string.Empty, queueName, basicProperties, ms.ToArray());
            }
        }
    }
}
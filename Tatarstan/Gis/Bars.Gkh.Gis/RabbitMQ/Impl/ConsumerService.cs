namespace Bars.Gkh.Gis.RabbitMQ.Impl
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Castle.Windsor;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Exceptions;

    /// <summary>Подписчик</summary>
    public class ConsumerService : IConsumerService
    {
        protected IWindsorContainer Container;

        ///// <summary>used to pass messages back to for processing</summary>
        ///// <param name="message">Сообщение</param>
        ///// <param name="queue">Очередь</param>
        //public delegate void OnReceiveMessage(byte[] message, string queue);

        /// <summary>internal delegate to run the consuming queue on a seperate thread</summary>
        private delegate void ConsumeDelegate();

        /// <summary>Событие получения сообщения</summary>
        //public event OnReceiveMessage OnMessageReceived;

        protected IModel Model;

        protected IConnection Connection;

        protected string QueueName;

        protected bool IsConsuming;

        public ConsumerService(IWindsorContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Запуск примеки сообщений
        /// </summary>
        public void StartConsuming(string hostName, int port, string login, string password, string queueName)
        {
            QueueName = queueName;
            var connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = login,
                Password = password
            };
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.BasicQos(0, 1, false);

            Model.QueueDeclare(QueueName, true, false, false, null);

            IsConsuming = true;
            var consumeDelegate = new ConsumeDelegate(Consume);
            consumeDelegate.BeginInvoke(null, null);
        }

        /// <summary>Метод извлечения сообщений из очереди</summary>
        public void Consume()
        {
            var consumer = new QueueingBasicConsumer(Model);
            var consumerTag = Model.BasicConsume(QueueName, false, consumer);
            while (IsConsuming)
            {
                try
                {
                    var e = consumer.Queue.Dequeue();
                    byte[] body = e.Body;

                    // ... process the message
                    OnMessageReceived(body);
                    Model.BasicAck(e.DeliveryTag, false);
                }
                catch (OperationInterruptedException exc)
                {
                    Model.BasicCancel(consumerTag);
                    Dispose();
                    break;
                }
                catch (EndOfStreamException)
                {
                    Dispose();
                    break;
                }
                catch (Exception ex)
                {
                    Model.BasicCancel(consumerTag);
                    Dispose();
                    break;
                }
            }
        }

        private void OnMessageReceived(byte[] body)
        {
            BaseTask task;

            using (var memoryStream = new MemoryStream(body))
            {
                var deserializer = new BinaryFormatter();
                task = (BaseTask)deserializer.Deserialize(memoryStream);
            }

            var genericType = typeof(ITaskHandler<>);
            var taskHandlerType = genericType.MakeGenericType(new[] { task.Type });
            var taskHandler = Container.Resolve(taskHandlerType);
            var method = taskHandler.GetType().GetMethod("Run");
            method.Invoke(taskHandler, new object[] { task });
        }

        private void Dispose()
        {
            try
            {
                IsConsuming = false;
                if (Connection != null)
                {
                    Connection.Close();
                }

                if (Model != null)
                {
                    Model.Abort();
                }
            }
            catch { }
        }
    }
}
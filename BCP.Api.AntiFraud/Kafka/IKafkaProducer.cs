namespace BCP.Api.AntiFraud.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceMessage(string topic, string message);
    }
}
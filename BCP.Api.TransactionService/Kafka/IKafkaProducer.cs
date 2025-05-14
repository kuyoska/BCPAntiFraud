namespace BCP.Api.TransactionService.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceMessage(string topic, string message);
    }
}
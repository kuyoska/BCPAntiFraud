using Confluent.Kafka;

namespace BCP.Api.AntiFraud.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private ProducerConfig _config;

        public KafkaProducer()
        {
            _config = new ProducerConfig()
            {
                BootstrapServers = "kafka:9092",
                ClientId = "bcpDemo",

            };
        }

        public async Task ProduceMessage(string topic, string message)
        {
            try
            {
                using (var producer = new ProducerBuilder<Null, string>(_config).Build())
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

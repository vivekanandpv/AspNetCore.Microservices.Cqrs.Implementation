namespace CommandService.MQ;

public interface IRabbitMQManager
{
    void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
        where T : class;
}
namespace API.RabbitMQ
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
        EventTypeEnum RecognizeEventType(string message);
    }
}

using ids.Quickstart.Account;

namespace ids.RabbitMQ
{
    public interface IEventBus
    {
        public void PublishNewMessage(UserPublishViewModel model);
    }
}

using API.Data.UserRepo;
using API.Mapping.Dtos.Event;
using API.Mapping.Dtos.User;
using API.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace API.RabbitMQ
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        
        public async void ProcessEvent(string message)
        {
            var recognizedEventType = RecognizeEventType(message);

            if (recognizedEventType == EventTypeEnum.UserCreated)
              AddPlatform(message);
        }
        public async void AddPlatform(string message)
        {
            var userCreatedDto = JsonSerializer.Deserialize<UserCreatedDto>(message);

            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var user = _mapper.Map<User>(userCreatedDto);
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                if (!await userRepository.IsUserExists(user.ExternalId))
                    await userRepository.AddUser(user);
            }
        }
        public EventTypeEnum RecognizeEventType(string message)
        {
            var eventType = JsonSerializer.Deserialize<EventDto>(message);

            if (eventType.EventName == EventTypeEnum.UserCreated.ToString())
                return EventTypeEnum.UserCreated;

            return EventTypeEnum.Unrecognized;
        }
    }
}

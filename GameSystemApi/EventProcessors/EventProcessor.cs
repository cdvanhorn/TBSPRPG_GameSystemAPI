using System;
using System.Threading.Tasks;

using TbspRpgLib.EventProcessors;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Settings;
using TbspRpgLib.Events;
using TbspRpgLib.Entities;

using GameSystemApi.Repositories;
using GameSystemApi.Services;

using Microsoft.Extensions.DependencyInjection;

namespace GameSystemApi.EventProcessors
{
    public class EventProcessor : MultiEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(
            IEventStoreSettings eventStoreSettings,
            IServiceScopeFactory scopeFactory) :
                base(
                    "gamesystem",
                    new string[] {
                        Event.ENTER_LOCATION_EVENT_TYPE
                    },
                    eventStoreSettings
                )
        {
            _scopeFactory = scopeFactory;
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameSystemContext>();
            InitializeStartPosition(context);
        }

        protected override async Task HandleEvent(Aggregate aggregate, Event evnt) {
            GameAggregate gameAggregate = (GameAggregate)aggregate;
            EventType eventType = GetEventTypeByName(evnt.Type);

            using(var scope = _scopeFactory.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<GameSystemContext>();
                var gameService = scope.ServiceProvider.GetRequiredService<IGameSystemService>();
                
                var transaction = context.Database.BeginTransaction();
                try {
                    //check if we've already processed the event
                    if(await gameService.HasBeenProcessed(evnt.EventId))
                        return;

                    //figure out what handler to call based on event type
                    IEventHandler handler = null;
                    if(eventType.TypeName == Event.ENTER_LOCATION_EVENT_TYPE) {
                        handler = scope.ServiceProvider.GetRequiredService<IEnterLocationEventHandler>();
                    }
                    if(handler != null)
                        await handler.HandleEvent(gameAggregate, evnt);

                    //update the event type position and this event is processed
                    await gameService.UpdatePosition(eventType.Id, gameAggregate.GlobalPosition);
                    await gameService.EventProcessed(evnt.EventId);
                    //save the changes
                    context.SaveChanges();
                    transaction.Commit();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    transaction.Rollback();
                    throw new Exception("event processor error");
                }
            }
        }
    }
}

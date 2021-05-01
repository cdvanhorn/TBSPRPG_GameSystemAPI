using System;
using System.Threading.Tasks;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;

namespace GameSystemApi.EventProcessors {
    public interface IEnterLocationEventHandler : IEventHandler {

    }

    public class EnterLocationEventHandler : EventHandler, IEnterLocationEventHandler {

        private readonly IAggregateService _aggregateService;

        public EnterLocationEventHandler(IAggregateService aggregateService) : base() {
            _aggregateService = aggregateService;
        }

        public async Task HandleEvent(GameAggregate gameAggregate, Event evnt) {
            //Game game = _gameAdapter.ToEntity(gameAggregate);
            
            //get what checks need to be done from the adventure api
            Console.WriteLine("handling new location event");

            //create an enter_location_check event, default to success
            Event enterLocationCheckEvent = new LocationEnterCheckEvent(
                new LocationEnterCheck() {
                    Id = gameAggregate.Id,
                    Result = true
                }
            );

            //oncomplete send enter_location_check event
            await _aggregateService.AppendToAggregate(
                AggregateService.GAME_AGGREGATE_TYPE,
                enterLocationCheckEvent,
                gameAggregate.StreamPosition);
        }
    }
}
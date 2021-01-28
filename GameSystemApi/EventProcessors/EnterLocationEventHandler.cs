using System;
using System.Threading.Tasks;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;

namespace GameSystemApi.EventProcessors {
    public interface IEnterLocationEventHandler : IEventHandler {

    }

    public class EnterLocationEventHandler : EventHandler, IEnterLocationEventHandler {

        public EnterLocationEventHandler() : base() {

        }

        public async Task HandleEvent(GameAggregate gameAggregate, Event evnt) {
            //Game game = _gameAdapter.ToEntity(gameAggregate);
            
            //get what checks need to be done from the adventure api

            //oncomplete send enter_location_check_result event
        }
    }
}
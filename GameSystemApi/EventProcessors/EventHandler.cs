using System.Threading.Tasks;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using GameSystemApi.Adapters;

namespace GameSystemApi.EventProcessors {
    public interface IEventHandler {
        Task HandleEvent(GameAggregate gameAggregate, Event evnt);
    }

    public class EventHandler {
        protected IGameAggregateAdapter _gameAdapter;

        public EventHandler() {
            _gameAdapter = new GameAggregateAdapter();
        }
    }
}
using TbspRpgLib.Services;
using GameSystemApi.Repositories;

namespace GameSystemApi.Services {
    public interface IGameSystemService : IServiceTrackingService {

    }

    public class GameSystemService : ServiceTrackingService, IGameSystemService {
        private readonly IGameSystemRepository _repository;

        public GameSystemService(IGameSystemRepository repository) : base(repository) {
            _repository = repository;
        }
    }
}
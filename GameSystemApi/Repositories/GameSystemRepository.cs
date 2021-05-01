using TbspRpgLib.Repositories;

namespace GameSystemApi.Repositories {
    public interface IGameSystemRepository : IServiceTrackingRepository {

    }

    public class GameSystemRepository : ServiceTrackingRepository, IGameSystemRepository {
        private readonly GameSystemContext _context;

        public GameSystemRepository(GameSystemContext context) : base(context) {
            _context = context;
        }
    }
}
using TbspRpgLib.Repositories;

namespace GameSystemApi.Repositories {
    public interface IGameSystemRepository : IServiceTrackingRepository {

    }

    public class GameSystemRepository : ServiceTrackingRepository, IGameSystemRepository {
        private GameSystemContext _context;

        public GameSystemRepository(GameSystemContext context) : base(context) {
            _context = context;
        }
    }
}
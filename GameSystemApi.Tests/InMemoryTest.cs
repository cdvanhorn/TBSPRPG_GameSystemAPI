using GameSystemApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameSystemApi.Tests
{
    public class InMemoryTest
    {
        protected readonly DbContextOptions<GameSystemContext> _dbContextOptions;

        protected InMemoryTest(string dbName)
        {
            _dbContextOptions = new DbContextOptionsBuilder<GameSystemContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using GameSystemApi.EventProcessors;
using GameSystemApi.Repositories;
using GameSystemApi.Services;
using Moq;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using Xunit;

namespace GameSystemApi.Tests.EventProcessors
{
    public class EnterLocationEventHandlerTests : InMemoryTest
    {
        #region Setup

        public EnterLocationEventHandlerTests() : base("EnterLocationEventHandlerTests")
        {
            Seed();
        }

        private void Seed()
        {
            using var context = new GameSystemContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.SaveChanges();
        }

        private static EnterLocationEventHandler CreateHandler(GameSystemContext context, ICollection<Event> events)
        {
            // var repository = new GameSystemRepository(context);
            // var service = new GameSystemService(repository);
            
            var aggregateService = new Mock<IAggregateService>();
            aggregateService.Setup(service =>
                service.AppendToAggregate(It.IsAny<string>(), It.IsAny<Event>(), It.IsAny<ulong>())
            ).Callback<string, Event, ulong>((type, evnt, n) =>
            {
                if (n <= 100)
                    events.Add(evnt);
            });

            return new EnterLocationEventHandler(aggregateService.Object);
        }

        #endregion
        
        #region HandleEvent

        [Fact]
        public async void HandleEvent_EventGenerated()
        {
            //arrange
            await using var context = new GameSystemContext(_dbContextOptions);
            var events = new List<Event>();
            var handler = CreateHandler(context, events);
            var agg = new GameAggregate()
            {
                Id = Guid.NewGuid().ToString()
            };
            
            //act
            await handler.HandleEvent(agg, null);
            
            //assert
            context.SaveChanges();
            Assert.Single(events);
            var evnt = events.First();
            Assert.Equal(Event.LOCATION_ENTER_CHECK_EVENT_TYPE, evnt.Type);
        }
        
        #endregion
    }
}
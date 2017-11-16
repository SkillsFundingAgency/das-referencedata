using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using SFA.DAS.ReferenceData.Infrastructure.Caching;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Caching.CacheProviderTests
{
    public class WhenICacheAnItem
    {
        private Mock<ICache> _cache;
        private CacheProvider _provider;

        [SetUp]
        public void Arrange()
        {
            _cache = new Mock<ICache>();
            _provider = new CacheProvider(_cache.Object);

            _cache.Setup(x => x.SetCustomValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.Delay(0));
        }

        [Test]
        public async Task ThenTheItemShouldBeCachedForARelativeTime()
        {
            //Arrange
            const string key = "test";
            const string item = "payload";

            var expectedCacheTime = new TimeSpan(0, 2, 200);
            
            //Act
            await _provider.SetAsync(key, item, expectedCacheTime);

            //Assert
            _cache.Verify(x => x.SetCustomValueAsync<object>(key, item, (int)expectedCacheTime.TotalSeconds), Times.Once);
        }

        [Test]
        public async Task TheTheItemShouldBeCachedForAnAbsoluteTime()
        {
            //Arrange
            const string key = "test";
            const string item = "payload";
            const int expectedSeconds = 200;

            var absoluteTime = DateTime.Now.AddSeconds(expectedSeconds);

            //Act
            await _provider.SetAsync(key, item, absoluteTime);

            //Assert
            //As the timing may not be exact for the test we add a 2 second threshold
            _cache.Verify(x => x.SetCustomValueAsync<object>(key, item, It.Is<int>(s => s < expectedSeconds + 2)), Times.Once);
        }
    }
}

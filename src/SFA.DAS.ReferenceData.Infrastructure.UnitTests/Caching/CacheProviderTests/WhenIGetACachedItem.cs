using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using SFA.DAS.ReferenceData.Infrastructure.Caching;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Caching.CacheProviderTests
{
    public class WhenIGetACachedItem
    {
        private CacheProvider _provider;
        private Mock<ICache> _cache;

        [SetUp]
        public void Arrange()
        {
            _cache = new Mock<ICache>();
            _provider = new CacheProvider(_cache.Object);
        }

        [Test]
        public async Task ThenIShouldGetTheCachedItem()
        {
            //Arrange
            const string key = "test";
            const string item = "payload";
            _cache.Setup(x => x.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _cache.Setup(x => x.GetCustomValueAsync<string>(key)).ReturnsAsync(item);
            
            //Act
            var result = await _provider.GetAsync<string>(key);

            //Assert
            Assert.AreEqual(item, result);
            _cache.Verify(x => x.ExistsAsync(key), Times.Once);
            _cache.Verify(x => x.GetCustomValueAsync<string>(key), Times.Once);
        }

        [Test]
        public async Task ThenIShouldNotGetAnItemThatIsntCached()
        {
            //Arrange
            const string key = "test";
            _cache.Setup(x => x.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var result = await _provider.GetAsync<string>(key);

            //Assert
            Assert.AreEqual(default(string), result);
            _cache.Verify(x => x.ExistsAsync(key), Times.Once);
            _cache.Verify(x => x.GetCustomValueAsync<string>(It.IsAny<string>()), Times.Never);
        }
    }
}

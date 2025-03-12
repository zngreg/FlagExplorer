using Microsoft.Extensions.Caching.Memory;

[TestFixture]
public class MemoryCacheServiceTests
{
    private ICacheService _cacheService;
    private IMemoryCache _memoryCache;

    [SetUp]
    public void Setup()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheService = new MemoryCacheService(_memoryCache);
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
    }

    [Test]
    public void Set_ShouldStoreItemInCache()
    {
        // Arrange
        var key = "test_key";
        var value = "test_value";

        // Act
        _cacheService.Set(key, value);
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Get_ShouldReturnDefaultIfKeyDoesNotExist()
    {
        // Arrange
        var key = "non_existing_key";

        // Act
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void TryGetValue_ShouldReturnTrueIfItemExists()
    {
        // Arrange
        var key = "existing_key";
        var value = "cached_value";
        _cacheService.Set(key, value);

        // Act
        var result = _cacheService.TryGetValue(key, out string cachedValue);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(cachedValue, Is.EqualTo(value));
    }

    [Test]
    public void TryGetValue_ShouldReturnFalseIfItemDoesNotExist()
    {
        // Arrange
        var key = "missing_key";

        // Act
        var result = _cacheService.TryGetValue(key, out string cachedValue);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(cachedValue, Is.Null);
    }

    [Test]
    public void Remove_ShouldRemoveItemFromCache()
    {
        // Arrange
        var key = "key_to_remove";
        var value = "value";
        _cacheService.Set(key, value);

        // Act
        _cacheService.Remove(key);
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Set_ShouldUseCustomExpiration()
    {
        // Arrange
        var key = "expiring_key";
        var value = "expiring_value";
        var expiration = TimeSpan.FromSeconds(1);

        _cacheService.Set(key, value, expiration);

        // Act
        Thread.Sleep(1500);
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.That(result, Is.Null);
    }
}
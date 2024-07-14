namespace RadisCacheImplementation.Configurations
{
    public static class CacheConfiguration
    {
        public static DistributedCacheEntryOptions EmployeeCacheOptions => new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        // Add other cache configurations here as needed
    }
}
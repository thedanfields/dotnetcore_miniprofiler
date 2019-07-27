using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using StackExchange.Profiling.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace profiler.common
{
    public class LoggingMemoryCacheStorage : IAsyncStorage
    {
        private readonly ILogger<LoggingMemoryCacheStorage> _logger;
        private readonly MemoryCacheStorage _delegateStorage;
        public LoggingMemoryCacheStorage(ILogger<LoggingMemoryCacheStorage> logger,
            IMemoryCache cache, TimeSpan cacheDuration)
        {
            _logger = logger;
            _delegateStorage = new MemoryCacheStorage(cache, cacheDuration);
        }

        public List<Guid> GetUnviewedIds(string user)
        {
            return _delegateStorage.GetUnviewedIds(user);
        }

        public Task<List<Guid>> GetUnviewedIdsAsync(string user)
        {
            return _delegateStorage.GetUnviewedIdsAsync(user);
        }

        public IEnumerable<Guid> List(int maxResults, DateTime? start = null, DateTime? finish = null, ListResultsOrder orderBy = ListResultsOrder.Descending)
        {
            return _delegateStorage.List(maxResults, start, finish, orderBy);
        }

        public Task<IEnumerable<Guid>> ListAsync(int maxResults, DateTime? start = null, DateTime? finish = null, ListResultsOrder orderBy = ListResultsOrder.Descending)
        {
            return _delegateStorage.ListAsync(maxResults, start, finish, orderBy);
        }

        public MiniProfiler Load(Guid id)
        {
            return _delegateStorage.Load(id);
        }

        public Task<MiniProfiler> LoadAsync(Guid id)
        {
            return _delegateStorage.LoadAsync(id);
        }

        public void Save(MiniProfiler profiler)
        {
            Log(profiler);
            _delegateStorage.Save(profiler);
        }

        public Task SaveAsync(MiniProfiler profiler)
        {
            Log(profiler);
            return _delegateStorage.SaveAsync(profiler);
        }

        private void Log(MiniProfiler profiler)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Profiling Information: {@ProfileInfo}", JsonConvert.DeserializeObject<object>(profiler.ToJson()));

        }

        public void SetUnviewed(string user, Guid id)
        {
            _delegateStorage.SetUnviewed(user, id);
        }

        public Task SetUnviewedAsync(string user, Guid id)
        {
            return _delegateStorage.SetUnviewedAsync(user, id);
        }

        public void SetViewed(string user, Guid id)
        {
            _delegateStorage.SetViewed(user, id);
        }

        public Task SetViewedAsync(string user, Guid id)
        {
            return _delegateStorage.SetViewedAsync(user, id);
        }
    }
}

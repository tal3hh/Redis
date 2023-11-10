namespace Distributed.Cache.Service
{
    public interface IRepository<T> where T : class, new()
    {
        Task CreateAsync(string key, T value, int minute = 1);
        Task<T?> GetAsync(string key);
        Task DeleteAsync(string key);
    }
}
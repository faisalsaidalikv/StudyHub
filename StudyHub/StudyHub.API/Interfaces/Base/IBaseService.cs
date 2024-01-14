namespace StudyHub.API.Interfaces.Base
{
    public interface IBaseService<T> where T : class
    {
        bool IsExists(string id);
        Task<string> AddAsync(T entity);
        Task<T?> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindAsync(Dictionary<string, object> keyValuePairs);
        IEnumerable<T>? FindAllAsync(Func<T, bool> predicate);
        Task<string> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
    }
}

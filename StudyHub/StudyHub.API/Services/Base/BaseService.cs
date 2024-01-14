using Microsoft.EntityFrameworkCore;
using StudyHub.API.Data;
using StudyHub.API.Interfaces.Base;
using StudyHub.Models.Base;

namespace StudyHub.API.Services.Base
{
    public class BaseService<T>(StudyHubAPIContext context) : IBaseService<T> where T : BaseDto
    {
        protected StudyHubAPIContext Context { get; private set; } = context;
        protected DbSet<T>? Model { get; set; }

        public virtual async Task<T?> GetAsync(string id) => await Model.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await Model.ToListAsync();

        public virtual async Task<T?> FindAsync(Dictionary<string, object> keyValuePairs) 
            => await Model.FindAsync(keyValuePairs.Cast<object>().ToArray());

        public virtual IEnumerable<T>? FindAllAsync(Func<T, bool> predicate)
            => Model?.Where(predicate);

        public virtual async Task<string> AddAsync(T entity)
        {
            Model?.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task<string> UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            var dto = await Model.FindAsync(id);
            if (dto == null) return false;

            Model.Remove(dto);
            await Context.SaveChangesAsync();
            return true;
        }

        public virtual bool IsExists(string id) => Model?.Any(e => e.Id == id) ?? false;
    }
}

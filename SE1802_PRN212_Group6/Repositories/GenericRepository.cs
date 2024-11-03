using Microsoft.EntityFrameworkCore;
using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext _dbContext) where T : class
    {
        // Function Get
        public List<T> GetAll(string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return typeof(TrackableEntity).IsAssignableFrom(typeof(T))
                ? query.AsNoTracking().Where(x => !(x as TrackableEntity)!.IsDeleted).ToList()
                : query.AsNoTracking().ToList();
        }

        public List<T> GetAllWithDeleted(string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.AsNoTracking().ToList();
        }

        public T? GetById(int id, string[]? includes = null)
        {
            if (!typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return null;
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefault(x => (x as BaseEntity)!.Id == id);
        }

        // Function Process
        public void Add(T entity)
        {
            if (entity is TrackableEntity trackableEntity)
            {
                trackableEntity.CreationDate = DateTime.Now;
            }

            _dbContext.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            if (entity is TrackableEntity trackableEntity)
            {
                trackableEntity.DeletionDate = DateTime.Now;
                trackableEntity.IsDeleted = true;
                _dbContext.Set<T>().Update(entity);
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public void Restore(T entity)
        {
            if (entity is TrackableEntity trackableEntity)
            {
                trackableEntity.DeletionDate = null;
                trackableEntity.IsDeleted = false;
                _dbContext.Set<T>().Update(entity);
            }
        }

        public void Update(T entity)
        {
            if (entity is TrackableEntity trackableEntity)
            {
                trackableEntity.ModificationDate = DateTime.Now;
            }

            _dbContext.Set<T>().Update(entity);
        }
    }
}

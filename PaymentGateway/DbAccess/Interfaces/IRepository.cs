using System.Linq;

namespace DbAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        // TODO: Check if this works in EF Core. I think it does not.
        //void ValidateModel(object entity);

        int SaveChanges();
    }
}

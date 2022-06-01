using DbAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DbAccess.Repositories
{
    class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly PaymentGatewayDbContext _context;

        private readonly DbSet<T> _dbSet;

        public GenericRepository(PaymentGatewayDbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this._dbSet;
        }

        public T Find(object id)
        {
            return this._dbSet.Find(id);
        }

        public TResult SearchAndSelect<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, TResult>> selector)
        {
            return this._dbSet.Where(condition).Select(selector).SingleOrDefault();
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void Delete(object id)
        {
            this.Delete(this.Find(id));
        }

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this._context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this._dbSet.Attach(entity);
            }

            entry.State = state;
        }

        //public void ValidateModel(object entity)
        //{
        //    foreach (var property in entity.GetType().GetProperties())
        //    {
        //        var firstInvalidAttribute = property.GetCustomAttributes(typeof(ValidationAttribute), true)
        //                            .OfType<ValidationAttribute>()
        //                            .FirstOrDefault(attr => !attr.IsValid(property.GetValue(entity)));

        //        if (firstInvalidAttribute != null)
        //        {
        //            throw new ValidationException(firstInvalidAttribute.ErrorMessage);
        //        }
        //    }
        //}
    }
}

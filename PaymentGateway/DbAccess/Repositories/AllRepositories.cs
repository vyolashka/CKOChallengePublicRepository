using DbAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Repositories
{
    public class AllRepositories : IDisposable, IAllRepositories
    {
        private readonly PaymentGatewayDbContext _context;

        private readonly IDictionary<Type, object> _repositories;

        private bool _disposed = false;

        public AllRepositories(PaymentGatewayDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<Payment> PaymentRepository
        {
            get
            {
                return this.GetRepository<Payment>();
            }
        }

        public IDbContextTransaction GetTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this._repositories.Add(typeof(T), Activator.CreateInstance(type, _context));
            }

            return (IRepository<T>)this._repositories[typeof(T)];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this._context != null)
                    {
                        this._context.Dispose();
                    }
                }
            }

            this._disposed = true;
        }    
    }
}

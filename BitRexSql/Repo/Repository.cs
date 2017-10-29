using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRexSql.ModelBitRex;
using System.Data.Entity;
using System.Linq.Expressions;

namespace BitRexSql.Repo
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        protected BitRexSql.ModelBitRex.ModelBitRex dbContext;

        //DbSet usable with any of our entity types
        protected DbSet<T> dbSet;

        //constructor taking the database context and getting the appropriately typed data set from it
        public Repository(BitRexSql.ModelBitRex.ModelBitRex  context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }


        //Implementation of IRepository methods
        public virtual IEnumerable<T> DataSet { get { return dbSet; } }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual T GetById(int? id)
        {
            return dbSet.Find(id);
        }

        public virtual void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        //IDisposable implementation
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

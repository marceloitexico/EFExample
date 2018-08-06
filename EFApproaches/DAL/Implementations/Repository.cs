using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Implementations
{
    //I am specifying how School Database's repositories will behave
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        //Dbcontext for School Database
        protected SchoolContext dbContext;

        //DbSet usable with any of our entity types
        protected DbSet<T> dbSet;

        //constructor taking the database context and getting the appropriately typed data set from it
        public Repository(SchoolContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        //Implementation of IRepository methods
        public virtual  IEnumerable<T> DataSet { get { return dbSet; } }

        public virtual void Create(T entity)
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

        public void Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //remove method receives an entity object, not an id
           
        }
        
        //IDisposable implementation
        private bool disposed = false;

        protected virtual void Dispose (bool disposing)
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
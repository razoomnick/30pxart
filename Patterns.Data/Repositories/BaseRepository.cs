using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public abstract class BaseRepository<T>: IBaseRepository<T> where T: DatabaseObject
    {
        private readonly PatternsContext context = ContextManager.GetCurrentContext<PatternsContext>();
        protected const int MaxCount = 500;

        protected PatternsContext Context
        {
            get { return context; }
        }

        public void Save(T entity)
        {
            Collection.Add(entity);
            context.SaveChanges();
        }

        public T GetById(Guid id)
        {
            var entity = Collection.FirstOrDefault(u => u.Id == id);
            return entity;
        }

        public IList<T> List()
        {
            var entities = Collection.ToList();
            return entities;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        protected abstract DbSet<T> Collection { get; }
    }
}

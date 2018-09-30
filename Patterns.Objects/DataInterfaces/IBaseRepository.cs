using System;
using System.Collections.Generic;
using Patterns.Objects.Entities;

namespace Patterns.Objects.DataInterfaces
{
    public interface IBaseRepository<T> where T: DatabaseObject
    {
        void Save(T entity);
        T GetById(Guid id);
        IList<T> List();
        void SaveChanges();
    }
}

using BlackLagoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? IncludeProperties = null);
        T Get(Expression<Func<T, bool>>? filter, string? IncludeProperties = null);
        void Add(T entity);
        bool Any(Expression<Func<T,bool>> filter);

        void Delete(T entity);

    }
}

using BlackLagoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Application.Common.Interfaces
{
    public interface IVillaRepository : IRepository<Villa>
    {

      /*  IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? IncludeProperties = null);
        Villa Get(Expression<Func<Villa, bool>>? filter, string? IncludeProperties = null);
        void Add(Villa entity);*/
        void Update(Villa entity);
        //void Delete(Villa entity);
      


    }
}

using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
           dbSet.Add(entity);
        }

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public void Delete(T entity)
        {
           dbSet.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>>? filter, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                /*// foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))*/
                foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? IncludeProperties = null)
        {
            IQueryable<T> query =dbSet;
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                /*// foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))*/
                foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
    }
}

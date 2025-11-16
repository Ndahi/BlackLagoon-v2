using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
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
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }
        /*public void Add(Villa entity)
        {
            _context.Add(entity);
        }

        public void Delete(Villa entity)
        {
            _context.Remove(entity);
        }*/

 /*       public Villa Get(Expression<Func<Villa, bool>>? filter, string? IncludeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                *//*// foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))*//*
                foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }*/

 /*       public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? IncludeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                *//*// foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))*//*
                foreach (var includeProp in IncludeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }*/

       

        public void Update(Villa entity)
        {
            _context.Update(entity);
        }
    }
}

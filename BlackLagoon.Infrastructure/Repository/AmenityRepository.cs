using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity> , IAmenityRepository
    {
        private readonly ApplicationDbContext _context;
        public AmenityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Amenity entity)
        {
            _context.Amenities.Update(entity);
        }

     
    }
}

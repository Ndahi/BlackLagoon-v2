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
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(VillaNumber entity)
        {
           _context.VillaNumbers.Update(entity);
        }
    }
}

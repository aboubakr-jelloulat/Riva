using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;

namespace Riva.API.Data.Repository.Repositories
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

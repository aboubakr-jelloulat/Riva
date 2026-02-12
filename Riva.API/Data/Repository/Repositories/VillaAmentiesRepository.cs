using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;

namespace Riva.API.Data.Repository.Repositories;

public class VillaAmentiesRepository : Repository<VillaAmenties>, IVillaAmentiesRepository
{
    public VillaAmentiesRepository(ApplicationDbContext db) : base(db)
    {
    }


}


using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;

namespace Riva.API.Data.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IVillaRepository Villa { get; private set; }

        public IUserRepository Users { get; private set; }

        public IVillaAmentiesRepository VillaAmenties { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;


            Villa = new VillaRepository(_db);
            Users = new UserRepository(_db);
        }

        public void Save() => _db.SaveChanges();

        public Task Saveasync() => _db.SaveChangesAsync();

    }
}

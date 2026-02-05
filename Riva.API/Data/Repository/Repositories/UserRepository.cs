using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;

namespace Riva.API.Data.Repository.Repositories;




public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext db) : base(db) {}

}


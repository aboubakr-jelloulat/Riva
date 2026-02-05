namespace Riva.API.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IUserRepository Users { get; }

        void Save();
        Task Saveasync();
    }
}

namespace Riva.API.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }

        void Save();
        Task Saveasync();
    }
}

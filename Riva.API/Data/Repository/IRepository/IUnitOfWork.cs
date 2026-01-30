namespace Riva.API.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository villaRepository { get; }

        void Save();
        Task Saveasync();
    }
}

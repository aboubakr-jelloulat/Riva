namespace Riva.API.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IUserRepository Users { get; }
        IVillaAmentiesRepository VillaAmenties { get; }



        void Save();
        Task Saveasync();
    }
}

namespace Riva.API.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IUserRepository Users { get; }
        IVillaAmentiesRepository VillaAmenities { get; }



        void Save();
        Task SaveAsync();
    }
}

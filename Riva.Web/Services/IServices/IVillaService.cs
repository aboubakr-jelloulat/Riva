using Riva.DTO;

namespace Riva.Web.Services.IServices;


public interface IVillaService
{
    Task<T?> CreateAsync<T>(VillaCreateDTO model);
    Task<T?> GetAllAsync<T>();
    Task<T?> GetTAsync<T>(int id);
    Task<T?> UpdateAsync<T>(VillaUpdateDTO model);
    Task<T?> DeleteAsync<T>(int id);

}

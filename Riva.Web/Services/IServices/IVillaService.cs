using Riva.DTO;

namespace Riva.Web.Services.IServices;


public interface IVillaService
{
    Task<T?> CreateAsync<T>(VillaCreateDTO model, string token);
    Task<T?> GetAllAsync<T>(string token);
    Task<T?> GetTAsync<T>(int id, string token);
    Task<T?> UpdateAsync<T>(VillaUpdateDTO model, string token);
    Task<T?> DeleteAsync<T>(int id, string token);

}

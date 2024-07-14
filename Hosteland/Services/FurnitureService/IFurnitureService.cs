using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace Hosteland.Services.FurnitureService
{
    public interface IFurnitureService
    {
        Task<ServiceResponse<List<Furniture>>> GetFurnitures();
        Task<ServiceResponse<Furniture>> AddFurniture(Furniture furniture);
        Task<ServiceResponse<bool>> SaveAsync();
        Task<ServiceResponse<int>> GetFurnituresTotal();
    }
}

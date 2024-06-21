using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace HostelandOData.Services.FurnitureService
{
    public interface IFurnitureService
    {
        Task<ServiceResponse<List<Furniture>>> GetFurnitures();
        Task<ServiceResponse<Furniture>> AddFurniture(Furniture furniture);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}

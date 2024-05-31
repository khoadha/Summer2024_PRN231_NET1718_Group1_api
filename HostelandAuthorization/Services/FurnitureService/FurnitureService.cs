using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.FurnitureRepository;

namespace HostelandAuthorization.Services.FurnitureService
{
    public class FurnitureService : IFurnitureService
    {
        private readonly IFurnitureRepository _repo;

        public FurnitureService(IFurnitureRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResponse<Furniture>> AddFurniture(Furniture furniture)
        {
            var serviceResponse = new ServiceResponse<Furniture>();
            try
            {
                var addedFurniture = await _repo.AddFurniture(furniture);
                serviceResponse.Data = addedFurniture;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<Furniture>>> GetFurnitures()
        {
            var serviceResponse = new ServiceResponse<List<Furniture>>();
            try
            {
                var list = await _repo.GetFurnitures();
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> SaveAsync()
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _repo.SaveAsync();
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}

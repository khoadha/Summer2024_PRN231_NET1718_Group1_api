using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.RoomCategoryRepository;

namespace HostelandOData.Services.RoomCategoryService
{
    public class RoomCategoryService : IRoomCategoryService
    {
        private readonly IRoomCategoryRepository _repo;

        public RoomCategoryService(IRoomCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResponse<RoomCategory>> AddRoomCategory(RoomCategory RoomCategory)
        {
            var serviceResponse = new ServiceResponse<RoomCategory>();
            try
            {
                var addedCate = await _repo.AddRoomCategory(RoomCategory);
                serviceResponse.Data = addedCate;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

 

        public async Task<ServiceResponse<List<RoomCategory>>> GetRoomCategories()
        {
            var serviceResponse = new ServiceResponse<List<RoomCategory>>();
            try
            {
                var list = await _repo.GetRoomCategories();
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

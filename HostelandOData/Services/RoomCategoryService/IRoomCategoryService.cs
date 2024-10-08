﻿using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace HostelandOData.Services.RoomCategoryService
{
    public interface IRoomCategoryService
    {
        Task<ServiceResponse<List<RoomCategory>>> GetRoomCategories();
        Task<ServiceResponse<RoomCategory>> AddRoomCategory(RoomCategory category);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}

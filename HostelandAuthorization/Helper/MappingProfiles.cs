using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;

namespace HostelandAuthorization.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            //AUTH
            CreateMap<UserLoginRequestDto, ApplicationUser>();

            //ROOM CATEGORY
            CreateMap<RoomCategory, GetRoomCategoryDto>();
            CreateMap<AddRoomCategoryDto, RoomCategory>();

            //ROOM
            CreateMap<Room, GetRoomDTO>()
                .ForMember(des => des.ImgPath, act => act.MapFrom(src => src.RoomImages.First().Url))
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName));

            CreateMap<Room, GetRoomDetailDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName))
                .ForMember(des => des.RoomImages, act => act.MapFrom(src => src.RoomImages))
                .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));


            // FURNITURE
            CreateMap<RoomFurniture, RoomFurnitureDTO>()
                .ForMember(des => des.Furniture, act => act.MapFrom(src => src.Furniture));

            CreateMap<Furniture, FurnitureDTO>();

            //CreateMap<AddProductDto, Product>();

            //CreateMap<UpdateProductDto, Product>();
            //CreateMap<Product, UpdateProductImageDto>();
        }
    }
}

using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;

namespace HostelandAuthorization.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            // AUTH
            CreateMap<UserLoginRequestDto, ApplicationUser>();

            // ROOM CATEGORY
            CreateMap<RoomCategory, GetRoomCategoryDto>();
            CreateMap<AddRoomCategoryDto, RoomCategory>();

            // FURNITURE
            CreateMap<RoomFurniture, RoomFurnitureDTO>();

            CreateMap<Furniture, FurnitureDTO>();
            CreateMap<AddFurnitureDTO, Furniture>();

            // ROOM
            CreateMap<Room, GetRoomDTO>()
                .ForMember(des => des.ImgPath, act => act.MapFrom(src => src.RoomImages.First().Url))
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName));

            CreateMap<Room, GetRoomDetailDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName))
                .ForMember(des => des.RoomImages, act => act.MapFrom(src => src.RoomImages))
                .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));

            CreateMap<AddRoomDTO, Room>();
                //.ForMember(dest => dest.RoomFurniture, opt => opt.MapFrom(src => src.Furnitures.Select(f => new RoomFurniture
                //{
                //    FurnitureId = f.FurnitureId,
                //    Quantity = f.Quantity
                //})));

            //CreateMap<UpdateProductDto, Product>();
            //CreateMap<Product, UpdateProductImageDto>();


            

            
        }
    }
}

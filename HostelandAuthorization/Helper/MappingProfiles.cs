using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;
using HostelandAuthorization.Shared;

namespace HostelandAuthorization.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            // AUTH
            CreateMap<UserLoginRequestDto, ApplicationUser>();
            CreateMap<GetPersonalUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetPersonalUserDto>();

            // ROOM CATEGORY
            CreateMap<RoomCategory, GetRoomCategoryDto>();
            CreateMap<AddRoomCategoryDto, RoomCategory>();

            // FURNITURE
            CreateMap<RoomFurniture, RoomFurnitureDTO>();
            CreateMap<RoomFurnitureDTO, RoomFurniture>();

            CreateMap<Furniture, FurnitureDTO>();

            CreateMap<AddFurnitureDTO, Furniture>();

            // ROOM IMAGE
            CreateMap<ImageDTO, RoomImage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImgPath));
            CreateMap<RoomImage, ImageDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ImgPath, opt => opt.MapFrom(src => src.Url));

            // ROOM
            CreateMap<Room, GetRoomDTO>()
                .ForMember(des => des.ImgPath, act => act.MapFrom(src => src.RoomImages.First().Url))
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName));

            CreateMap<Room, GetRoomDetailDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName != null ? src.Category.CategoryName : string.Empty))
                .ForMember(des => des.RoomImages, act => act.MapFrom(src => src.RoomImages))
                .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));

            CreateMap<AddRoomDTO, Room>();
            CreateMap<UpdateRoomDTO, Room>();

            //CreateMap<UpdateProductDto, Product>();
            //CreateMap<Product, UpdateProductImageDto>();


            

            
            // ORDER
            CreateMap<GuestDto, Guest>();
            CreateMap<CreateOrderDto, Order>()
                        .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests));
            CreateMap<CreateOrderDto, Contract>();

            //CreateMap<AddProductDto, Product>();

            //CreateMap<UpdateProductDto, Product>();
            //CreateMap<Product, UpdateProductImageDto>();
        }
    }
}

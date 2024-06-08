using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;

namespace Hosteland.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            // PROFILE
            CreateMap<GetPersonalUserDto, ApplicationUser>();

            // ROOM CATEGORY
            CreateMap<RoomCategory, GetRoomCategoryDto>();
            CreateMap<AddRoomCategoryDto, RoomCategory>();

            // SERVICE
            CreateMap<Service, GetServiceDto>();
            CreateMap<Service, GetServiceNewestPriceDto>()
                .ForMember(dest => dest.ServicePriceNumber, opt => opt.MapFrom(src => src.ServicePrice.OrderByDescending(sp => sp.EndDate).FirstOrDefault().Amount));

            CreateMap<AddServiceDto, Service>();

            CreateMap<ServicePrice, ServicePriceDto>();

            CreateMap<AddServicePriceDto, ServicePrice>();


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
                .ForMember(des => des.ImgPath, act => act.MapFrom(src => src.RoomImages.Count() > 0 ? src.RoomImages.First().Url : string.Empty))
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName != null ? src.Category.CategoryName : string.Empty))
                .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));

            CreateMap<Room, GetRoomDetailDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName != null ? src.Category.CategoryName : string.Empty))
                .ForMember(des => des.RoomImages, act => act.MapFrom(src => src.RoomImages))
                .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));
            CreateMap<RoomFurniture, RoomFurnitureDTO>()
                .ForMember(dest => dest.FurnitureName, opt => opt.MapFrom(src => src.Furniture.Name));

            CreateMap<AddRoomDTO, Room>();
            CreateMap<UpdateRoomDTO, Room>();

            //CreateMap<UpdateProductDto, Product>();
            //CreateMap<Product, UpdateProductImageDto>();


            

            
            // ORDER
            CreateMap<GuestDto, Guest>();
            CreateMap<CreateOrderDto, Order>()
                        .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests));
            CreateMap<CreateOrderDto, Contract>();
            CreateMap<Order, GetOrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name));


            //CreateMap<AddProductDto, Product>();

            //CreateMap<UpdateProductDto, Product>();
            //CreateMap<Product, UpdateProductImageDto>();
        }
    }
}

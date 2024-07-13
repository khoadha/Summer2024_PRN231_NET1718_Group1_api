using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;

namespace Hosteland.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            // AUTH
            CreateMap<UserLoginRequestDto, ApplicationUser>();
            CreateMap<GetPersonalUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetPersonalUserDto>();

            // ROOM CATEGORY
            CreateMap<RoomCategory, GetRoomCategoryDto>();
            CreateMap<AddRoomCategoryDto, RoomCategory>();

            //REPORT
            CreateMap<Report, GetReportDto>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));
            CreateMap<AddReportDto, Report>();
            CreateMap<UpdateReportDto, Report>();
            // SERVICE
            CreateMap<Service, GetServiceDto>();
            CreateMap<Service, GetServiceNewestPriceDto>()
                .ForMember(dest => dest.ServicePriceNumber, opt => opt.MapFrom(src => src.ServicePrice.OrderByDescending(sp => sp.StartDate).FirstOrDefault().Amount));

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
            //CreateMap<Room, GetRoomDTO>()
            //    .ForMember(des => des.ImgPath, act => act.MapFrom(src => src.RoomImages.Count() > 0 ? src.RoomImages.First().Url : string.Empty))
            //    .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName ?? string.Empty))
            //    .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));

            CreateMap<Room, GetRoomDetailDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName ?? string.Empty))
                .ForMember(des => des.RoomImages, act => act.MapFrom(src => src.RoomImages))
                .ForMember(des => des.RoomFurniture, act => act.MapFrom(src => src.RoomFurniture));
            CreateMap<RoomFurniture, RoomFurnitureDTO>()
                .ForMember(dest => dest.FurnitureName, opt => opt.MapFrom(src => src.Furniture.Name));

            CreateMap<AddRoomDTO, Room>();
            CreateMap<UpdateRoomDTO, Room>();

            // ORDER
            CreateMap<GuestDto, Guest>();
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests));
            CreateMap<RoomServiceDto, RoomService>();
            CreateMap<GetOrderDto, Order>();
            CreateMap<CreateOrderDto, Contract>();

            CreateMap<Order, GetOrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name));

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.ContractTypeName, opt => opt.MapFrom(src => src.Type.ContractName));
            CreateMap<RoomService, GetRoomServiceDto>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name));
            CreateMap<Guest, GuestDto>();

            // CONTRACT TYPE
            CreateMap<ContractType, GetContractTypeDto>();
            CreateMap<AddContractTypeDto, ContractType>();

            // FEE CATEGORY
            CreateMap<FeeCategory, GetFeeCateDto>()
                .ForMember(dest => dest.FeeCategoryName, opt => opt.MapFrom(src => src.Name));
            CreateMap<AddFeeCateDto, FeeCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FeeCategoryName));

            // FEE
            CreateMap<Fee, GetFeeDto>()
                .ForMember(dest => dest.FeeCategoryName, opt => opt.MapFrom(src => src.FeeCategory.Name ?? string.Empty))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? string.Empty));
            // FEE
            CreateMap<Fee, GetDeferredElectricityFeeDto>()
                .ForMember(dest => dest.FeeCategoryName, opt => opt.MapFrom(src => src.FeeCategory.Name ?? string.Empty))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Order.Room.Name ?? string.Empty));

            // GLOBAL RATE
            CreateMap<GlobalRate, GlobalRateDTO>();
            CreateMap<AddGlobalRateDTO, GlobalRate>();

        }
    }
}

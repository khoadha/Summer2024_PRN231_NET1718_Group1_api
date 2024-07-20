using AutoMapper.Configuration.Annotations;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.DTOs
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<GetRoomServiceDto> RoomServices { get; set; }
        public List<ContractDto> Contracts { get; set; }
        public List<GuestDto> Guests { get; set; }
        public string UserName { get; set; }
        public string RoomName { get; set; }
    }
    public class ContractDto
    {
        public double Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        public string Name { get; set; }
        public int ServiceId { get; set; }
    }
    public class GetRoomServiceDto
    {
        public string ServiceName { get; set; }
    }
    public class CreateOrderDto
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public double Cost { get; set; }
        public List<GuestDto> Guests { get; set; }
        public List<RoomServiceDto> RoomServices { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public bool IsMonthly { get; set; }
    }

    public class GuestDto
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
    }

    public class RoomServiceDto
    {
        public int ServiceId { get; set; }
    }

    public class GetContractTypeDto
    {
        public int Id { get; set; }
        public string ContractName { get; set; }
    }
    public class AddContractTypeDto
    {
        [Required]
        public string ContractName { get; set; }
    }
    
    public class GetFeeCateDto
    {
        public int Id { get; set; }
        public string FeeCategoryName { get; set; }
    }
    public class AddFeeCateDto
    {
        [Required]
        public string FeeCategoryName { get; set; }
    }

    public class GetFeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeeCategoryName { get; set; }
        public double Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public FeeStatus FeeStatus { get; set; }
    }

    public class GetDeferredElectricityFeeDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeeCategoryName { get; set; }
        public double Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public FeeStatus FeeStatus { get; set; }

        //MAP
        public int OrderId { get; set; }
        public string RoomName { get; set; }
    }
}

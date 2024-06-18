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
        public string UserName { get; set; }
        public string RoomName { get; set; }
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
        public List<RoomService> RoomServices { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }

    public class GuestDto
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
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
}

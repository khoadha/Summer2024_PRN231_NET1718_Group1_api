using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class GetServiceDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ServiceType ServiceType { get; set; }
        public string? ImgPath { get; set; }
        public bool? IsCountPerCapita { get; set; }
    }
    public class GetServiceNewestPriceDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ServicePriceNumber { get; set; }
        public string? ImgPath { get; set; }
        public ServiceType? ServiceType { get; set; }
        public bool? IsCountPerCapita { get; set; }
    }
    public class AddServiceDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public ServiceType? ServiceType { get; set; }
        public bool? IsCountPerCapita { get; set; }
    }

    public class ServicePriceDto
    {
        public double Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetServiceWithPriceDto
    {
        public int ServiceId { get; set; }
        public List<ServicePriceDto> ServicePrices { get; set; } = new List<ServicePriceDto>();
    }

    public class AddServicePriceDto
    {
        public int ServiceId { get; set; }
        public double Amount { get; set; }
        //public DateTime StartDate { get; set; } start date  = now
        //public DateTime? EndDate { get; set; } khong can enddate
    }


}

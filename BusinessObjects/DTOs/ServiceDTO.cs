using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GetServiceDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class AddServiceDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
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
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }


}

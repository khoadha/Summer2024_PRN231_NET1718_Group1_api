using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GetReportDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? AuthorId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? AuthorName { get; set; }
        public string? RoomName { get; set; }
        public string? Reply { get; set; }
        public MaintainanceStatus Status { get; set; }
    }


    public class AddReportDto
    {
        public int RoomId { get; set; }
        public string? AuthorId { get; set; }
        public string? Description { get; set; }
    }


    public class UpdateReportDto
    {
        public string? Reply { get; set; }
        public MaintainanceStatus Status { get; set; }
    }
}



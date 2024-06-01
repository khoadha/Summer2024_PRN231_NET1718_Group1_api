using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GetRoomCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
    public class AddRoomCategoryDto
    {
        [Required]
        public string CategoryName { get; set; }
    }
}

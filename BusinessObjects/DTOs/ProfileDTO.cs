using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GetPersonalUserDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? ImgPath { get; set; }
        public decimal? AccountBalance { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    public class UpdateUsernameDto
    {
        [Required]
        public string Username { get; set; }
    }
    
    public class UpdateBankDto
    {
        [Required]
        public string BankName { get; set; }
        
        [Required]
        public string BankAccountNumber { get; set; }
    }
}

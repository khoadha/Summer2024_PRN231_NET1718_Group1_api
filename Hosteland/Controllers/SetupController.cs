using AutoMapper;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.Constants;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Context;
using Hosteland.EmailTemplate;
using Hosteland.Services.ApplicationUserService;
using Hosteland.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace Hosteland.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRole.ADMIN)]
    public class SetupController : ControllerBase {

        private readonly IApplicationUserService _userService;
        private readonly IEmailService _emailSender;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public SetupController(
            UserManager<ApplicationUser> userManager,
            IUserContext userContext,
            IEmailService emailSender,
            IMapper mapper,
            IApplicationUserService userService) {
            _userContext = userContext;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() {

            var users = await _userService.GetUsers();
            var response = _mapper.Map<List<GetPersonalUserDto>>(users.Data);

            foreach (var userDto in response) {
                var user = users.Data.First(u => u.Id == userDto.Id);
                var userRoles = await _userManager.GetRolesAsync(user);
                userDto.Role = userRoles.FirstOrDefault();
            }

            return Ok(response);
        }

        [HttpPut("users")]
        public async Task<IActionResult> UpdateRole([FromQuery] string userId, [FromQuery] int action) {

            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if(currentUser.IsStaff=="True") {
                return Forbid();
            }

            var user = await _userService.GetUserById(userId);
            if (user == null) {
                return NotFound($"User with ID {userId} not found.");
            }

            var role = GetRoleFromAction(action);
            if (string.IsNullOrEmpty(role)) {
                return BadRequest("Invalid role action.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user.Data);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user.Data, currentRoles);
            if (!removeRolesResult.Succeeded) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error removing user roles.");
            }

            if(role=="Admin") {
                user.Data.IsStaff = true;
                var addRoleResult = await _userManager.AddToRoleAsync(user.Data, role);
                if (!addRoleResult.Succeeded) {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error adding user role.");
                }
            } else {
                user.Data.IsStaff = false;
                var addRoleResult = await _userManager.AddToRoleAsync(user.Data, role);
                if (!addRoleResult.Succeeded) {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error adding user role.");
                }
            }

            _userService.Save();
            return Ok(new { data = "User role updated successfully." });
        }

        [HttpPost("send-email")]
        public IActionResult SendEmail([FromBody] SendEmailRequestDto dto) {
            foreach(var email in dto.Emails) {
                var receiver = new string[] { email };

                string subject = dto.Subject;

                string htmlBody = dto.Content;

                var message = new Message(receiver, subject, htmlBody);

                _emailSender.SendEmail(message);
            }
            return NoContent();
        }

        private string GetRoleFromAction(int action) {
            return action switch {
                1 => "Admin",
                2 => "User",
                _ => throw new ArgumentException("Invalid action", nameof(action)),
            };
        }
    }
}

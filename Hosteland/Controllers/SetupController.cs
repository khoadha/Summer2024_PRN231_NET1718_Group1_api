using AutoMapper;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.Constants;
using BusinessObjects.DTOs;
using Hosteland.EmailTemplate;
using Hosteland.Services.ApplicationUserService;
using Hosteland.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRole.ADMIN)]
    public class SetupController : ControllerBase {

        private readonly IApplicationUserService _userService;
        private readonly IEmailService _emailSender;
        private readonly IMapper _mapper;

        public SetupController(
            IEmailService emailSender,
            IMapper mapper,
            IApplicationUserService userService) {
            _emailSender = emailSender;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() {
            var users = await _userService.GetUsers();
            var response = _mapper.Map<List<GetPersonalUserDto>>(users.Data);
            return Ok(response);
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
    }
}

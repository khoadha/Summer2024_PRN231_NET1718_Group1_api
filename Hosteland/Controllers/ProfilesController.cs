using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Context;
using Hosteland.Services.ApplicationUserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers.Profiles
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IBlobService _blobService;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilesController(
            IApplicationUserService applicationUserService,
            IBlobService blobService,
            IMapper mapper,
            IUserContext userContext,
            UserManager<ApplicationUser> userManager)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _blobService = blobService;
            _userContext = userContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "PLease sign in." }
                });
            }

            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId)
            {
                return Forbid();
            }

            var user = await _applicationUserService.GetUserById(userId);
            if (user.Data == null)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "User not found." }
                });
            }
            var response = _mapper.Map<GetPersonalUserDto>(user.Data);

            return Ok(response);
        }

        [HttpGet]
        [Route("check-login-method/{userId}")]
        public async Task<IActionResult> CheckLoginMethod(string userId)
        {
            // Get the current logged-in user
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Please sign in." }
                });
            }

            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId)
            {
                return Forbid();
            }

            var user = await _applicationUserService.GetUserById(userId);

            if (user.Data == null)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "User not found." }
                });
            }
            var appUser = await _userManager.FindByIdAsync(userId);

            // Get the logins associated with the user
            var logins = await _userManager.GetLoginsAsync(appUser);

            var isGoogleLogin = logins.Any(l => l.LoginProvider == "Google");

            var loginMethod = isGoogleLogin ? "Google" : "Normal";

            return Ok(new { LoginMethod = loginMethod });
        }


        [HttpPost]
        [Route("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid model state." }
                });
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "PLease sign in." }
                });
            }

            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId)
            {
                return Forbid();
            }

            var user = await _applicationUserService.GetUserById(userId);

            if (user.Data == null)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "User not found." }
                });
            }
            var appUser = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(appUser, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });
            }

            return Ok(new { Result = true, Message = "Password changed!" });
        }

        [HttpPut("update-username/{userId}")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto model, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid model state." }
                });
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Please sign in." }
                });
            }

            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId)
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            user.UserName = model.Username;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Name changedd!" });
            }
            else
            {
                return BadRequest(new { message = "Changing name failed!" });
            }
        }
        
        [HttpPut("update-bank/{userId}")]
        public async Task<IActionResult> UpdateBank([FromBody] UpdateBankDto model, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid model state." }
                });
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Please sign in." }
                });
            }

            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId)
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            user.BankName = model.BankName;
            user.BankAccountNumber = model.BankAccountNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Bank info changedd!" });
            }
            else
            {
                return BadRequest(new { message = "Changing Bank info failed!" });
            }
        }

        [HttpPut("update-avatar/{userId}")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile avatar, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid model state." }
                });
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Please sign in." }
                });
            }

            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId)
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            if (avatar == null || avatar.Length == 0)
            {
                return BadRequest(new { message = "Image required." });
            }

            var isDeleted = await _blobService.DeleteBlobsByUrlAsync(user.ImgPath);

            if (isDeleted)
            {
                var imageUrl = await _blobService.UploadFileAsync(avatar);
                user.ImgPath = imageUrl;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Avatar changed!" });
            }
            else
            {
                return BadRequest(new { message = "Changing avatar failed!" });
            }
        }

        [HttpGet]
        [Route("profile-img/{email}")]
        public async Task<IActionResult> GetUserProfileByMail(string email)
        {
            var user = await _applicationUserService.GetUserByEmail(email);
            if (user == null)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "User not found." }
                });
            }
            var response = _mapper.Map<GetPersonalUserDto>(user.Data);

            return Ok(response);
        }
    }
}

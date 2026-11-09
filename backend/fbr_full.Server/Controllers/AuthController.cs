using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using fbr_full.Server.DTOs;
using fbr_full.Server.Models;
using fbr_full.Server.Services;

namespace fbr_full.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", request.Email);
                
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Email}", request.Email);
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                _logger.LogInformation("User found: {UserId}, Email: {Email}", user.Id, user.Email);
                
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                _logger.LogInformation("Password check result: {Succeeded}, {LockedOut}, {RequiresTwoFactor}", 
                    result.Succeeded, result.IsLockedOut, result.RequiresTwoFactor);
                
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Password check failed for user: {Email}", request.Email);
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                if (!user.IsActive)
                {
                    return Unauthorized(new { message = "Account is deactivated" });
                }

                var token = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                return Ok(new AuthResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(8),
                    User = new UserProfile
                    {
                        Id = user.Id,
                        Email = user.Email ?? string.Empty,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        StoreName = user.StoreName,
                        StoreAddress = user.StoreAddress,
                        NTN = user.NTN,
                        STRN = user.STRN
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _userManager.FindByEmailAsync(request.Email) != null)
                {
                    return BadRequest(new { message = "Email already registered" });
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    StoreName = request.StoreName,
                    StoreAddress = request.StoreAddress,
                    NTN = request.NTN,
                    STRN = request.STRN,
                    EmailConfirmed = true // For demo purposes
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Registration failed", errors = result.Errors.Select(e => e.Description) });
                }

                var token = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                return Ok(new AuthResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(8),
                    User = new UserProfile
                    {
                        Id = user.Id,
                        Email = user.Email ?? string.Empty,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        StoreName = user.StoreName,
                        StoreAddress = user.StoreAddress,
                        NTN = user.NTN,
                        STRN = user.STRN
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfile>> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new UserProfile
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    StoreName = user.StoreName,
                    StoreAddress = user.StoreAddress,
                    NTN = user.NTN,
                    STRN = user.STRN
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return StatusCode(500, new { message = "An error occurred while getting profile" });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Password change failed", errors = result.Errors.Select(e => e.Description) });
                }

                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new { message = "An error occurred while changing password" });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { message = "An error occurred during logout" });
            }
        }

        [HttpGet("test-user/{email}")]
        public async Task<ActionResult> TestUser(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    HasPassword = await _userManager.HasPasswordAsync(user)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing user: {Email}", email);
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                // Remove existing password if any
                if (await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.RemovePasswordAsync(user);
                }

                // Set new password
                var result = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Failed to set password", errors = result.Errors.Select(e => e.Description) });
                }

                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for: {Email}", request.Email);
                return StatusCode(500, new { message = "An error occurred" });
            }
        }
    }
} 
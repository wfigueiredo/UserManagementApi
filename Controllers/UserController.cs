using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Dto;
using UserManagement.Domain.Enum;
using UserManagement.Domain.Error;
using UserManagement.Domain.Model;
using UserManagement.Extensions;
using UserManagement.Interfaces.Service;
using UserManagement.Interfaces.Services;
using UserManagement.Translators;
using UserManagement.Util;

namespace UserManagement.Controllers
{
    [Authorize]
    [Route("api/usermanagement/v1")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILoginService _loginService;
        private readonly IAuditLogService _auditLogService;

        public UserController(ILogger<UserController> logger, IUserService userService, IRoleService roleService, ILoginService loginService, IAuditLogService auditLogService)
        {
            _logger = logger;
            _userService = userService;
            _roleService = roleService;
            _loginService = loginService;
            _auditLogService = auditLogService;
        }

        [AllowAnonymous]
        [HttpPost("user/login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] UserDto dto)
        {
            try
            {
                var user = await _userService.FindByUsernameAndSecretAsync(dto.username, dto.secret);
                if (user == null)
                    return NotFound(new { message = "Invalid username or password" });

                var token = _loginService.GenerateJwtToken(user);
                var AuthenticatedDto = user.ToAuthenticatedDto(token);
                
                return Ok(AuthenticatedDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error in UserManagementApi [Login]", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin)]
        [HttpPost("user")]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            try
            {
                if (!IsAuthenticated())
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Unauthenticated user" });

                if (dto == null)
                    return BadRequest(new { message = "Payload has not a valid format. Please, check for potential errors." });

                if (!_roleService.IsRoleValid(dto.role))
                    return BadRequest(new { message = "Invalid Role Type" });

                var Role = await _roleService.FindByCodeAsync(dto.role.GetDisplayName());
                var CurrentUser = await GetLoggedUser();

                var User = await _userService.FindByUsernameAsync(dto.username);
                if (User != null)
                    return Conflict(new { message = "Username already in use" });

                if (!IsAllowed(CurrentUser.Role, Role))
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Permission denied to this action" });

                var UserToCreate = dto.ToDomain(Role);
                await _userService.CreateAsync(UserToCreate);
                await CreateAuditLog(ActionType.Create, CurrentUser, UserToCreate);

                return Created("api/usermanagement/v1/user", UserToCreate.ToSavedDto());
            }
            catch (DuplicateEntryException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(new { message = "Operation error", reason = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error in UserManagementApi [Create]", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin)]
        [HttpPatch("user/password-reset")]
        [Consumes("application/json")]
        public async Task<IActionResult> PasswordReset([FromBody] UserDto dto)
        {
            try
            {
                if (!IsAuthenticated())
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Unauthenticated user" });

                if (dto == null)
                    return BadRequest(new { message = "Payload has not a valid format. Please, check for potential errors." });

                var existingUser = await _userService.FindByUsernameAsync(dto.username);
                if (existingUser == null)
                    return NotFound("User not found");

                var CurrentUser = await GetLoggedUser();
                if (!IsAllowed(CurrentUser.Role, existingUser.Role))
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Permission denied to this action" });

                existingUser.Secret = HashUtil.SHA1(dto.secret);
                await _userService.UpdateAsync(existingUser);
                await CreateAuditLog(ActionType.Update, CurrentUser, existingUser);

                return Ok();
            }
            catch (DuplicateEntryException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(new { message = "Operation error", reason = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error in UserManagementApi [PasswordReset]", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin, RoleTypeDesc.User)]
        [HttpGet("user/{RequestedUsername}")]
        [Produces("application/json")]
        public async Task<IActionResult> ViewUserDetails([FromRoute] string RequestedUsername)
        {
            try
            {
                if (!IsAuthenticated())
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Unauthenticated user" });

                var RequestedUser = await _userService.FindByUsernameAsync(RequestedUsername);
                if (RequestedUser == null)
                    return NotFound("User not found");

                var CurrentUsername = User.Identity.Name;
                if (RequestedUsername == CurrentUsername)
                    return Ok(RequestedUser.ToFullDetailsDto());

                var CurrentUser = await GetLoggedUser();
                if (!IsAllowed(CurrentUser.Role, RequestedUser.Role))
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Permission denied to this action" });

                return Ok(RequestedUser.ToFullDetailsDto());
            }
            catch (NotAuthenticatedException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Forbidden action: user not authenticated" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error in UserManagementApi [ViewUserDetails]", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin)]
        [HttpDelete("user/{username}")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete([FromRoute] string username)
        {
            try
            {
                if (!IsAuthenticated())
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Unauthenticated user" });

                var RequestedUser = await _userService.FindByUsernameAsync(username);
                if (RequestedUser == null)
                    return NotFound("User not found");

                var CurrentUser = await GetLoggedUser();
                if (!IsAllowed(CurrentUser.Role, RequestedUser.Role))
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Permission denied to this action" });

                await _userService.DeleteAsync(RequestedUser);
                await CreateAuditLog(ActionType.Delete, CurrentUser, RequestedUser);

                return NoContent();
            }
            catch (NotAuthenticatedException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Forbidden action: user not authenticated" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error in UserManagementApi [ViewUserDetails]", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin)]
        [HttpGet("user")]
        [Produces("application/json")]
        public async Task<IActionResult> FindByRole([FromQuery] string role)
        {
            try
            {
                if (!IsAuthenticated())
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Unauthenticated user" });

                var CurrentUsername = User.Identity.Name;
                var CurrentUser = await _userService.FindByUsernameAsync(CurrentUsername);
                var Role = await _roleService.FindByCodeAsync(role);
                if (!IsAllowed(CurrentUser.Role, Role))
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Permission denied to this action" });

                var userList = await _userService.FindByRoleAsync(Role);
                if (!userList.Any())
                    return NotFound();

                return Ok(userList.ToDetailsDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error in UserManagementApi [FindByRole]", reason = ex.Message });
            }
        }

        private Task<User> GetLoggedUser()
        {
            var Username = User.Identity.Name;
            return _userService.FindByUsernameAsync(Username);
        }

        private bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        private bool IsAllowed(Role CurrentRole, Role RequestedRole)
        {
            return _userService.HasActPermission(CurrentRole, RequestedRole);
        }

        private async Task CreateAuditLog(ActionType actionType, User user, User target)
        {
            await _auditLogService.CreateAsync(new AuditLog()
            {
                Action = actionType.GetDisplayName(),
                Entity = EntityType.User.GetDisplayName(),
                User = user,
                Target = target
            });
        }
    }
}

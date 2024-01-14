using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyHub.API.Data;
using StudyHub.API.Interfaces.Accounts;
using StudyHub.API.Interfaces.AWS;
using StudyHub.API.Services.AWS;
using StudyHub.Models.Accounts;

namespace StudyHub.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IAwsS3Service _awsS3Service;

        public UserController(ILogger<UserController> logger,
            IUserService userService,
            IAwsS3Service awsS3Service)
        {
            _logger = logger;
            _userService = userService;
            _awsS3Service = awsS3Service;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        //{
        //    try
        //    {
        //        _logger.LogTrace("Get all users");
        //        var items = await _userService.GetAllAsync();
        //        return items?.Any() ?? false ? Ok(items) : NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}

        //// GET: api/User/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserDto>> GetUser(string id)
        //{
        //    try
        //    {
        //        var userDto = await _userService.GetAsync(id);
        //        if (userDto == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(userDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}

        //// PUT: api/User/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(string id, UserDto userDto)
        //{
        //    try
        //    {
        //        if (id != userDto.Id)
        //        {
        //            return BadRequest();
        //        }

        //        var userId = await _userService.UpdateAsync(userDto);
        //        return string.IsNullOrEmpty(userId) ? NoContent() : Ok(userId);
        //    }
        //    catch (DbUpdateConcurrencyException dcEx)
        //    {
        //        if (!_userService.IsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            _logger.LogError(dcEx, dcEx.Message);
        //            return Problem(dcEx.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}

        //// POST: api/User
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserDto>> PostUser(UserDto user)
        //{
        //    try
        //    {
        //        var userId = await _userService.AddAsync(user);
        //        return string.IsNullOrEmpty(userId) ? NoContent() : CreatedAtAction("GetUser", new { id = userId }, user); ;
        //    }
        //    catch (DbUpdateConcurrencyException dcEx)
        //    {
        //        //if (!UserDtoExists(dcEx.))
        //        //{
        //        //    return NotFound();
        //        //}
        //        //else
        //        //{
        //        _logger.LogError(dcEx, dcEx.Message);
        //        return Problem(dcEx.Message);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}

        //// DELETE: api/User/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(string id)
        //{
        //    try
        //    {
        //        var response = await _userService.DeleteAsync(id);
        //        return response ? Ok(response) : NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}

        [HttpPost("register")]
        public IActionResult Register(RegisterDto register)
        {
            try
            {
                var userId = _userService.AddAsync(new UserDto 
                { 
                    Id = Guid.NewGuid().ToString(),
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Password = register.Password
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message);
            }
            return Ok(register);
        }
        
        [HttpPost("{userId}/profilephoto")]
        public async Task<IActionResult> UploadProfilePhoto(string userId, IFormFile profilePhoto)
        {
            if (profilePhoto == null) return BadRequest();

            try
            {
                var user = await _userService.GetAsync(userId);
                if (user == null) return NotFound("User doesn't found with given user id.");

                var fileKey = await _awsS3Service.UploadFile(profilePhoto, "PhotoBucket", "Profiles");
                if (string.IsNullOrEmpty(fileKey)) return Problem("Profile photo not uploaded in S3 bucket.");

                user.PhotoId = fileKey;
                userId = await _userService.UpdateAsync(user);
                return string.IsNullOrEmpty(userId) ? Problem("User table doesn't updated") : Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}

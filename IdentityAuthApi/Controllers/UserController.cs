using IdentityAuthApi.DTOs;
using IdentityAuthApi.Filters;
using IdentityAuthApi.Models;
using IdentityAuthApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly SignInManager<AppUser> _signInManager;
        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                FullName = registerDTO.FullName,
                UserName=registerDTO.Email,
                Email = registerDTO.Email,
                age = registerDTO.age,
                Status = registerDTO.Status

            };


            var result = await _userManager.CreateAsync(user,registerDTO.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }



            foreach (var role in registerDTO.Roles)
            {
                await _userManager.CreateAsync(user,role);
            }

            return Ok(result);
        }



        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Something went wrong");
            }


            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if(user is null)
            {
                return Unauthorized("Email not found wir this emil");

            }

            if (user.IsDeleted == true)
                throw new Exception("Not found");

            var tokenDTO = await _authService.GenerateToken(user);


            if (tokenDTO.IsSuccess == false || tokenDTO.Token == "" || tokenDTO.Token is null)
            {
                throw new Exception("Something went wrong!!");
            }

            HttpContext.Response.Cookies.Append("accessToken", tokenDTO.Token);

            return Ok(tokenDTO);




        }



        [HttpGet]
        public async Task<ActionResult<string>> GetAllUsers()
        {
            var result = await _userManager.Users.ToListAsync();
            return Ok(result);
        }


        [HttpGet]
        [AuthorizeFilter]
        public async Task<ActionResult<string>> GetAllUsers1()
        {
            try
            {
                return Ok(await _userManager.Users.Where(x => x.IsDeleted == false).ToListAsync());
            }
            catch
            {
                return NotFound("Users are not found");
            }
        }

        [HttpGet("{accountId}")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var result = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);


                if (result is null)
                {
                    return NotFound("Not found");
                }
                if (result.IsDeleted == true)
                    throw new Exception("Not found");
                return Ok(result);
            }
            catch
            {
                return NotFound("User is not found");
            }

        }


        [HttpDelete("{accountId}")]
        [DeleteActionFilter]
        [MyResultFilter]
        public async Task<IActionResult> DeleteAccount(string accountId)
        {
            var user = await _userManager.FindByIdAsync(accountId);

            if (user is null)
                throw new Exception("Not found");

            if (user.IsDeleted == true)
                throw new Exception("Not found");
            //var deleteUser = await _userManager.DeleteAsync(user);
            user.IsDeleted = true;
            user.DeleteDate = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("No deleted");
            return Ok(result);
        }



       



    }



}


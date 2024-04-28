using IdentityAuthApi.DTOs;
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
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if(User is null)
            {
                return Unauthorized("User not found wir this emil");

            }

            var test = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!test)
            {
                return Unauthorized("Password invalid");
            }

            return Ok("welcome to world");



        }



        [HttpGet]
        public async Task<ActionResult<string>> GetAllUsers()
        {
            var result = await _userManager.Users.ToListAsync();
            return Ok(result);
        }


        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<string>> GetAllUsers1()
        {
            var result = await _userManager.Users.ToListAsync();

            return Ok("Student keldi");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, Student")]
        public async Task<ActionResult<string>> GetAllUsers2()
        {
            var result = await _userManager.Users.ToListAsync();

            return Ok("Admin, Student ishladi");
        }
    }



}


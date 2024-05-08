using IdentityAuthApi.DTOs;
using IdentityAuthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }



        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateRole(RoleDTO role)
        {

            var result = await _roleManager.FindByNameAsync(role.RoleName);

            if (result == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role.RoleName));

                return Ok(new ResponseDTO
                {
                    Message = "Role Created",
                    IsSuccess = true,
                    StatusCode = 201
                });
            }

            return Ok(new ResponseDTO
            {
                Message = "Role cann not created",
                StatusCode = 403
            });
        }


        [HttpGet]
        public async Task<ActionResult<List<IdentityRole>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDTO>> UpdateRole(string id, RoleDTO role)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);

            if (existingRole == null)
            {
                return NotFound(new ResponseDTO
                {
                    Message = "Role not found",
                    StatusCode = 404
                });
            }

            existingRole.Name = role.RoleName; 

            var result = await _roleManager.UpdateAsync(existingRole);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO
                {
                    Message = "Role updated successfully",
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO
            {
                Message = "Failed to update role",
                StatusCode = 500
            });
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDTO>> DeleteRole(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);

            if (existingRole == null)
            {
                return NotFound(new ResponseDTO
                {
                    Message = "Role not found",
                    StatusCode = 404
                });
            }

            var result = await _roleManager.DeleteAsync(existingRole);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO
                {
                    Message = "Role deleted successfully",
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO
            {
                Message = "Failed to delete role",
                StatusCode = 500
            });
        }




    }
}

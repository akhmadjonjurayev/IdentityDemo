using IdentityDemo.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDemo.Service
{
    public interface IUserService
    {
        Task<UserManagerResponce> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponce> LoginUserAsync(LoginViewModel model);
    }
    public class UserService : IUserService
    {
        private RoleManager<IdentityUser> _role;
        private IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        public UserService(UserManager<IdentityUser> userManager,IConfiguration configuration,RoleManager<IdentityUser> role)
        {
            _role = role;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<UserManagerResponce> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponce
                {
                    Message = "Invalid email address",
                    IsSuccess = false,
                };
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if(!result)
                return new UserManagerResponce
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            var claims = new[]
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Role,await _role.GetRoleNameAsync(user))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            return new UserManagerResponce
            {
                Message=new JwtSecurityTokenHandler().WriteToken(token),
                IsSuccess=true,
                ExireDate=token.ValidTo
            };
        }

        public async Task<UserManagerResponce> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register model is null");
            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponce
                {
                    Message = "ConfirmPassword doesn't match the password",
                    IsSuccess = false,
                };
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, Role.Admin);
                await _role.CreateAsync(identityUser);
                return new UserManagerResponce
                {
                    Message = "User created successfully",
                    IsSuccess = true,
                };
            }
            return new UserManagerResponce
            {
                Message = "User didn't created",
                IsSuccess = false,
                Errors = result.Errors.Select(op => op.Description),
            };
        }
    }
}

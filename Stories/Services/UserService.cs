using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TEK.Core.UoW;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using Stories.Models;
using Stories.VM.Request;

namespace Stories.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateUser(LogInRequest login);
        Task<User> GoogleAuthenticateUser(string email);
        Task<User> GetUserInfo(string username);
        Task<User> RegisterAccount(RegisterAccountRequest request);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        public async Task<User> AuthenticateUser(LogInRequest login)
        {
            var user = await _unitOfWork.GetRepository<User>().FindAsync(x => x.Username == login.Username && x.Password == login.Password);

            return user;
        }

        public async Task<User> GoogleAuthenticateUser(string email)
        {
            var user = await _unitOfWork.GetRepository<User>().FindAsync(x => x.Email == email);

            return user;
        }

        public async Task<User> GetUserInfo(string username)
        {
            var user = await _unitOfWork.GetRepository<User>().FindAsync(x => x.Username == username);

            return user;
        }

        public async Task<User> RegisterAccount(RegisterAccountRequest request)
        {
            var chk = await _unitOfWork.GetRepository<User>().FindAsync(x => x.Username == request.Username || x.Email == request.Email);
            if (chk == null)
            {
                var user = new User();
                user = _mapper.Map<User>(request);

                _unitOfWork.GetRepository<User>().Add(user);
                await _unitOfWork.CommitAsync();
                return user;
            }
            return null;
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Username", userInfo.Username),
                new Claim("IsAuthor", userInfo.IsAuthor.ToString()),
                new Claim("Name", userInfo.Name)
            };

            var token = new JwtSecurityToken(_config["AppSetting:Issuer"],
                _config["AppSetting:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

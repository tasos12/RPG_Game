using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using _NET_Course.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace _NET_Course.Data
{
    public class AuthRepository : IAuthRepository
    {
        public readonly DataContext _context;
        public readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var _response = new ServiceResponse<string>();
            var _user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));

            if(username == null)
            {
                _response.Success = false;
                _response.Message = "User not found.";
            }
            else if(!VerifyPasswordHash(password, _user.PasswordHash, _user.PasswordSalt))
            {
                _response.Success = false;
                _response.Message = "Wrong password.";
            }
            else
            {
                _response.Data = CreateToken(_user);
            }

            return _response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> _response = new ServiceResponse<int>();

            if(await UserExists(user.Username))
            {
                _response.Success = false;
                _response.Message = "User already esxists";

                return _response;
            }

            CreatePasswordHash(password, out byte[] _passwordHash, out byte[] _passwordSalt);
            user.PasswordHash = _passwordHash;
            user.PasswordSalt = _passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();     
            _response.Data = user.ID;

            return _response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(User user) 
        {
            var _claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            
            var _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(_claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = _creds
            };

            var _tokenHandler = new JwtSecurityTokenHandler();
            var _token = _tokenHandler.CreateToken(_tokenDescriptor);

            return _tokenHandler.WriteToken(_token);
        }
    }
}
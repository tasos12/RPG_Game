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
    /// <summary>
    /// An implementation of the interface that connects with the database to do User based functions.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        public readonly DataContext _context;
        public readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Authentificates a user and creates a token for his seassion.
        /// </summary>
        /// <param name="username">The username specified to find.</param>
        /// <param name="password">The password to verify the user.</param>
        /// <returns>A token for the user session if succesfull, otherwise User not found when the username is wrong or Wrong password if the password is false.</returns>
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

        /// <summary>
        /// Register's the user with its information and password on the database.
        /// </summary>
        /// <param name="user">The user information.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A service response with the user's id.</returns>
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

        /// <summary>
        /// Searches the database for a user with the specified username.
        /// </summary>
        /// <param name="username">The username to be searched.</param>
        /// <returns>True if username found, false if not found.</returns>
        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generates the password hash.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies the given password hash.
        /// </summary>
        /// <param name="password">The password to generate the hash.</param>
        /// <param name="passwordHash">The hash to compare with the generated hash</param>
        /// <param name="passwordSalt"></param>
        /// <returns>True if the computed hash and the given hash is identical, False if not.</returns>
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

        /// <summary>
        /// Creates the session token depending on the credentials and the description given.
        /// </summary>
        /// <param name="user">User information used to create the token.</param>
        /// <returns>A JwtSecurityToken as string.</returns>
        private string CreateToken(User user) 
        {
            // Represent attributes of the subject that are useful in the context of authentication and authorization operations. 
            var _claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Create Security key using private key from appsettings.
            var _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            // Represents the cryptographic key and security algorithms that are used to generate a digital signature.
            var _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Information about the token.
            var _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(_claims),      //Information about the user corresponding to the token.
                Expires = System.DateTime.Now.AddDays(1),   //Token expiration time.
                SigningCredentials = _creds
            };

            var _tokenHandler = new JwtSecurityTokenHandler();
            // Generate the token.
            var _token = _tokenHandler.CreateToken(_tokenDescriptor);

            return _tokenHandler.WriteToken(_token);
        }
    }
}
using System.Threading.Tasks;
using _NET_Course.Data;
using _NET_Course.Dto.User;
using _NET_Course.Models;
using Microsoft.AspNetCore.Mvc;

namespace _NET_Course.Cotrollers
{
    /// <summary>
    /// An API endpoint for authentification.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Register a user and their info to the database.
        /// </summary>
        /// <param name="request">A dto with user information.</param>
        /// <returns>OK status code if successfull, otherwise Bad request status code.</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDto request)
        {
            var response = await _authRepository.Register(
                new User { Username = request.Username }, request.Password
            );

            if(!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Authentificates the user information.
        /// </summary>
        /// <param name="request">A dto with user login information.</param>
        /// <returns>OK status code if successfull, otherwise Bad request status code.</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(
                request.Username, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
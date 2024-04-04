using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RubricaTelefonicaAziendale.Dtos;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;
using RubricaTelefonicaAziendale.Services;

namespace RubricaTelefonicaAziendale.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("web/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService authService)
        {
            service = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", [.. errors]));
            }
            if (String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid Username or Password!");
            }
            Users? user = await service.Login(model.Username, model.Password);
            if (user != null)
            {
                Roles? role = await service.GetRoleByUserId(user?.Id ?? "");
                if (role == null) return BadRequest("Username and password are not valid. Please contact the administrator!");
                string jwt = service.GenerateToken(user, role);
                return Ok(new AuthDto()
                {
                    User = UserDto.ConvertToDto(user ?? new Users(), role),
                    Token = jwt,
                });
            }
            return Problem("Username and password are not valid. Please contact the administrator!");
        }


        [HttpGet("whoami")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthDto>> WhoAmI()
        {
            JwtTokenClaims? jtc = service.WhoAmI();
            if (jtc == null)
                return Problem("Token is not valid or expired");
            Users? user = await service.GetUserById(jtc!.UserId ?? "");
            if (user != null)
            {
                Roles? role = await service.GetRoleByUserId(user?.Id ?? "");
                if (role == null) return BadRequest("Username and password are not valid. Please contact the administrator!");
                string jwt = service.GenerateToken(user, role);
                return Ok(new AuthDto()
                {
                    User = UserDto.ConvertToDto(user ?? new Users(), role),
                    Token = jwt,
                });
            }
            return Problem("Username and password are not valid. Please contact the administrator!");
        }


        [HttpPost("register")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthDto>> Register([FromBody] UserDto model)
        {
            await Task.Delay(1000);
            // if (!ModelState.IsValid)
            // {
            //     var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
            //     return BadRequest(new InfoMessage(MessageType.Error, 400, "Problems with received data!", String.Join(";", errors.ToArray())));
            // }
            // if (String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Password))
            // {
            //     return BadRequest(new InfoMessage(MessageType.Error, 400, "Error", "Invalid Username or Password"));
            // }
            // if (String.IsNullOrEmpty(model.Email))
            // {
            //     return BadRequest(new InfoMessage(MessageType.Error, 400, "Error", "Invalid email address"));
            // }
            // bool usernameexist = await service.ExistUserWithUsername(model.Username);
            // if (usernameexist)
            // {
            //     return BadRequest(new InfoMessage(MessageType.Error, 400, "Error", "Username not available"));
            // }
            // bool emailexist = await service.ExistUserWithEmail(model.Email);
            // if (emailexist)
            // {
            //     return BadRequest(new InfoMessage(MessageType.Error, 400, "Error", "Email already in use! Try to recover your credentials instead of creating a new user"));
            // }
            // Users newuser = new()
            // {
            //     Id = new Guid().ToString(),
            //     Firstname = model.Firstname,
            //     Lastname = model.Lastname,
            //     Username = model.Username,
            //     Password = model.Password,
            //     Email = model.Email,
            // };
            // if (model?.Role == null) return BadRequest(new InfoMessage(MessageType.Error, 400, "Error", "Role not valid!"));
            // Roles? role = await service.GetRoleByDesc(model.Role);
            // if (role == null)
            // {
            //     return BadRequest(new InfoMessage(MessageType.Error, 400, "Error", "Role not valid!"));
            // }
            // Users? user = await service.Register(newuser, role);
            // if (user != null)
            // {
            //     var jwt = service.GenerateToken(user, role);
            //     return Ok(new LoginResponse()
            //     {
            //         Username = user.Username,
            //         Fullname = user.Firstname + " " + user!.Lastname,
            //         Email = user.Email ?? "",
            //         Picture = DefaultPicture,
            //         Role = role.Description ?? "",
            //         Token = jwt,
            //     });
            // }
            // else 
            return Problem("Username and password are not valid. Please contact the administrator!");
        }


        [HttpPost("passwordforgot")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PasswordForgot([FromBody] String email)
        {
            await System.Threading.Tasks.Task.Delay(1000);
            return BadRequest();
        }


        [HttpGet("passwordreset")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> PasswordReset(String token)
        {
            if (String.IsNullOrEmpty(token))
                return BadRequest("Token not received");

            await System.Threading.Tasks.Task.Delay(1000); ;
            return BadRequest();
        }


        [HttpGet("setusers")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> SetUsers()
        {
            GeneratePasswordHash("tjf", out string salt, out string hash);
            Users user = new Users()
            {
                Id = Guid.NewGuid().ToString(),
                Firstname = "TJF-Challenge",
                Lastname = "Admin",
                Username = "tjf",
                Password = hash,
                Salt = salt,
                Picture = null
            };
            Roles role = await service.GetRoleByDesc("Admin");

            await service.Register(user, role);

            user = new Users()
            {
                Id = Guid.NewGuid().ToString(),
                Firstname = "TJF-Challenge",
                Lastname = "User",
                Username = "tjf",
                Password = hash,
                Salt = salt,
                Picture = null
            };
            role = await service.GetRoleByDesc("User");

            await service.Register(user, role);

            return Ok();
        }

        private void GeneratePasswordHash(String password, out String passwordSalt, out String passwordHash)
        {
            // generate a 512-bit salt using a secure PRNG
            byte[] salt = new byte[512 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(salt);
            }
            passwordSalt = Convert.ToBase64String(salt);
            passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                                                                        salt: salt,
                                                                        prf: KeyDerivationPrf.HMACSHA512,
                                                                        iterationCount: 10000,
                                                                        numBytesRequested: 1024 / 8));
        }

    }
}
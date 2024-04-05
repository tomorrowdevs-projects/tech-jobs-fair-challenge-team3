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
                string jwt = service.GenerateToken(user);
                return Ok(new AuthDto()
                {
                    User = UserDto.ConvertToDto(user ?? new Users()),
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
            if (jtc == null) return Problem("Token is not valid or expired");
            Users? user = await service.GetUserById(jtc!.UserId ?? "");
            if (user != null)
            {
                string jwt = service.GenerateToken(user);
                return Ok(new AuthDto()
                {
                    User = UserDto.ConvertToDto(user ?? new Users()),
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
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", errors.ToArray()));
            }
            if (String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid Username or Password");
            }
            bool usernameexist = await service.ExistUserWithUsername(model.Username);
            if (usernameexist)
            {
                return BadRequest("Username not available");
            }
            try
            {
                // calcolo la password
                GeneratePasswordHash(model.Password, out string salt, out string hash);
                // creo un nuovo utente
                Users newuser = new Users()
                {
                    Id = Guid.NewGuid(),
                    Firstname = model.Firstname ?? "",
                    Lastname = model.Lastname ?? "",
                    Username = model.Username,
                    Password = hash,
                    Salt = salt
                };
                // individuo il ruolo da assegnare all'utente
                if (model?.RoleDesc == null) return BadRequest("Role not valid!");
                Roles? role = await service.GetRoleByDesc(model.RoleDesc);
                if (role == null) return BadRequest("Role not found!");
                await service.Register(newuser, role);
                return Ok("User created!");
            }
            catch (Exception ex)
            {
                return Problem("Error creating user! " + ex.Message);
            }
        }


        [HttpPost("update")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthDto>> Update([FromBody] UserDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", errors.ToArray()));
            }
            if (String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid Username or Password");
            }
            bool usernameexist = await service.ExistUserWithUsername(model.Username);
            if (usernameexist)
            {
                return BadRequest("Username not available");
            }
            try
            {
                // cerco l'utente
                Users? updateuser = await service.GetUserById(model.Id);
                if (updateuser == null)
                {
                    return BadRequest("User not found!");
                }
                //updateuser.Id = Guid.NewGuid().ToString();
                updateuser.Firstname = model.Firstname ?? "";
                updateuser.Lastname = model.Lastname ?? "";
                updateuser.Username = model.Username;
                // calcolo la password
                if (!String.IsNullOrEmpty(model.Password))
                {
                    GeneratePasswordHash(model.Password, out string salt, out string hash);
                    updateuser.Password = hash;
                    updateuser.Salt = salt;
                }
                // individuo il ruolo da assegnare all'utente
                if (model?.RoleDesc == null) return BadRequest("Role not valid!");
                Roles? role = await service.GetRoleByDesc(model.RoleDesc);
                if (role == null) return BadRequest("Role not found!");
                await service.Register(updateuser, role);
                return Ok("User updated!");
            }
            catch (Exception ex)
            {
                return Problem("Error creating user! " + ex.Message);
            }
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
using System.Threading.Tasks;
using HomeView.Models.Account;
using HomeView.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeView.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;

        public AccountController(ITokenService tokenService, UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserCreate userCreate)
        {
            var userIdentity = new UserIdentity
            {
                Username = userCreate.Username,
                Firstname = userCreate.Firstname,
                Lastname = userCreate.Lastname,
                Dateofbirth = userCreate.Dateofbirth,
                Addressline1 = userCreate.Addressline1,
                Addressline2 = userCreate.Addressline2,
                Addressline3 = userCreate.Addressline3,
                Town = userCreate.Town,
                City = userCreate.City,
                Postcode = userCreate.Postcode,
                Accounttype = userCreate.Accounttype,
                Email = userCreate.Email,
                Phone = userCreate.Phone,
                MarketingEmail = userCreate.MarketingEmail,
                MarketingPhone = userCreate.MarketingPhone,
                ProfilepictureUrl = userCreate.ProfilepictureUrl

            };

            var result = await _userManager.CreateAsync(userIdentity, userCreate.Password);

            if (result.Succeeded)
            {
                User user = new User()
                {
                    UserId = userIdentity.UserId,
                    Username = userIdentity.Username,
                    Firstname = userIdentity.Firstname,
                    Lastname = userIdentity.Lastname,
                    Dateofbirth = userIdentity.Dateofbirth,
                    Addressline1 = userIdentity.Addressline1,
                    Addressline2 = userIdentity.Addressline2,
                    Addressline3 = userIdentity.Addressline3,
                    Town = userIdentity.Town,
                    City = userIdentity.City,
                    Postcode = userIdentity.Postcode,
                    Accounttype = userIdentity.Accounttype,
                    Email = userIdentity.Email,
                    Phone = userIdentity.Phone,
                    MarketingEmail = userIdentity.MarketingEmail,
                    MarketingPhone = userIdentity.MarketingPhone,
                    ProfilepictureUrl = userIdentity.ProfilepictureUrl,
                    Token = _tokenService.CreateToken(userIdentity)
                };

                return Ok(user);
                
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLogin userLogin)
        {
            var userIdentity = await _userManager.FindByNameAsync(userLogin.Username);

            if (userIdentity != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(
                    userIdentity,
                    userLogin.Password,
                    false);

                if (result.Succeeded)
                {
                    User user = new User
                    {
                        UserId = userIdentity.UserId,
                        Username = userIdentity.Username,
                        Firstname = userIdentity.Firstname,
                        Lastname = userIdentity.Lastname,
                        Dateofbirth = userIdentity.Dateofbirth,
                        Addressline1 = userIdentity.Addressline1,
                        Addressline2 = userIdentity.Addressline2,
                        Addressline3 = userIdentity.Addressline3,
                        Town = userIdentity.Town,
                        City = userIdentity.City,
                        Postcode = userIdentity.Postcode,
                        Accounttype = userIdentity.Accounttype,
                        Email = userIdentity.Email,
                        Phone = userIdentity.Phone,
                        MarketingEmail = userIdentity.MarketingEmail,
                        MarketingPhone = userIdentity.MarketingPhone,
                        ProfilepictureUrl = userIdentity.ProfilepictureUrl,
                        Token = _tokenService.CreateToken(userIdentity)
                    };

                    return Ok(user);
                }
            }

            return BadRequest("Invalid login attempt.");
        }
    }
}

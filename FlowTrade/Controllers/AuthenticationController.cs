using FlowTrade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FlowTrade.Interfaces;
using FlowTrade.Authentication.DTOs;
using FlowTrade.Authentication.Helpers;
using FlowTrade.Authentication.Services;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<UserCompany> _userManager;
        private readonly SignInManager<UserCompany> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IProductionPossibilityRepository _productionPossibilityRepository;

        public AuthenticationController(
            UserManager<UserCompany> userManager,
            SignInManager<UserCompany> signInManager, 
            IConfiguration configuration, 
            IEmailService emailService,
            IProductionPossibilityRepository productionPossibilityRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _productionPossibilityRepository = productionPossibilityRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "A user with the same email already exists.");
                return BadRequest(ModelState);
            }

            var productionPossibilities = await _productionPossibilityRepository.GetProductionPossibilitiesByIds(model.ProductionPossibilityIds);

            var user = new UserCompany
            {
                UserName = model.Username,
                Email = model.Email,
                CompanyType = model.CompanyType,
                AuthorizedPerson = model.AuthorizedPerson,
                NIP = model.NIP,
                REGON = model.REGON,
                Phone = model.Phone,
                ProductionPossibilities = productionPossibilities
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var confirmationLink = Url.Action("ConfirmEmail", "Authentication",
                    new { userId = user.Id, token = encodedToken }, Request.Scheme);

                // Send the email confirmation link to the user's email address
                var emailService = new EmailService(_configuration); // Inject IConfiguration into the controller's constructor
                var subject = "Email Confirmation";
                var body = $"Please confirm your email by clicking the link: {confirmationLink}";

                emailService.SendEmail(user.Email, subject, body, isBodyHtml: true);

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to sign in the user
            var user = await _userManager.FindByNameAsync(model.Identifier) ??
                       await _userManager.FindByEmailAsync(model.Identifier);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return BadRequest("Email is not confirmed.");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, lockoutOnFailure: false, isPersistent: false);

                if (result.Succeeded)
                {
                    var secretKey = _configuration["Jwt:SecretKey"];
                    var issuer = _configuration["Jwt:Issuer"];
                    var audience = _configuration["Jwt:Audience"];

                    var token = JwtHelper.GenerateJwtToken((user.Id).ToString(), user.UserName, secretKey, issuer, audience, 60);

                    return Ok(new { token });
                }
            }

            return Unauthorized();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // Decode the token
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Confirm the email
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found, return an error
                return BadRequest("Invalid user.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }


        [HttpGet("possibilities-types")]
        public async Task<ActionResult<IEnumerable<object>>> GetPossibilities()
        {
            var possibilities = await _productionPossibilityRepository.GetAllPossibilities();

            var possibilityObjects = possibilities.Select(p => new { p.Id, p.Name }).ToList();

            return Ok(possibilityObjects);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
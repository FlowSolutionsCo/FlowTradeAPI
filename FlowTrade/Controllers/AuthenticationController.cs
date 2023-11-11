using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FlowTrade.Interfaces;
using FlowTrade.Authentication.DTOs;
using FlowTrade.Authentication.Helpers;
using FlowTrade.Authentication.Services;
using FlowTrade.ProductionPossibility.Models;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<UserCompany> userManager;
        private readonly SignInManager<UserCompany> signInManager;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;
        private readonly IProductionPossibilityRepository productionPossibilityRepository;

        public AuthenticationController(
            UserManager<UserCompany> userManager,
            SignInManager<UserCompany> signInManager, 
            IConfiguration configuration, 
            IEmailService emailService,
            IProductionPossibilityRepository productionPossibilityRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.emailService = emailService;
            this.productionPossibilityRepository = productionPossibilityRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "A user with the same email already exists.");
                return BadRequest(ModelState);
            }

            var productionPossibilities = await productionPossibilityRepository.GetProductionPossibilitiesByIds(model.ProductionPossibilityIds);

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

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var confirmationLink = Url.Action("ConfirmEmail", "Authentication",
                    new { userId = user.Id, token = encodedToken }, Request.Scheme);

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
            var user = await userManager.FindByNameAsync(model.Identifier) ??
                       await userManager.FindByEmailAsync(model.Identifier);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return BadRequest("Email is not confirmed.");
                }

                var result = await signInManager.PasswordSignInAsync(user, model.Password, lockoutOnFailure: false, isPersistent: false);

                if (result.Succeeded)
                {
                    var secretKey = configuration["Jwt:SecretKey"];
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];

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
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found, return an error
                return BadRequest("Invalid user.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }


        [HttpGet("possibilities-types")]
        public async Task<ActionResult<IEnumerable<object>>> GetPossibilities()
        {
            var possibilities = await productionPossibilityRepository.GetAllPossibilities();

            var possibilityObjects = possibilities.Select(p => new { p.Id, p.Type }).ToList();

            return Ok(possibilityObjects);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }
    }
}
using Api.Features.Authentication.Entities;
using Api.Features.Authentication.Models;
using Api.Features.Users.Services;
using Api.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Data.Entities.Authentication
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IUserService userService,
            IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._logger = logger;
            _userService = userService;
            this._emailSender = emailSender;
        }

        //// GET: web/Account/providers
        //[AllowAnonymous]
        //[HttpGet("providers", Name = "web-account-external-providers")]
        //public async Task<ActionResult<IEnumerable<string>>> Providers()
        //{
        //    var providers = await _signInManager.GetExternalAuthenticationSchemesAsync();
        //    var result = providers.Select(s => s.DisplayName);

        //    return Ok(result);
        //}

        // GET: web/Account/current-user
        [Authorize]
        [HttpGet("current-user", Name = "web-account-currentuser")]
        public async Task<ActionResult<ApplicationUser>> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(user);
        }

        // GET: web/Account/connect/{provider}
        [AllowAnonymous]
        [HttpGet("connect/{medium}/{provider}", Name = "web-account-external-connect-challenge")]
        public async Task<ActionResult> ExternalLogin([FromRoute]string medium, [FromRoute]string provider)
        {
            //var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { medium, provider });
            var redirectUrl = Url.RouteUrl("web-account-external-connect-callback", new { medium, provider });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: web/Account/connect/{provider}/callback
        [HttpGet("connect/{medium}/{provider}/callback", Name = "web-account-external-connect-callback")]
        public async Task<ActionResult> ExternalLoginCallback([FromRoute]string medium, [FromRoute]string provider)
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                    throw new UnauthorizedAccessException();

                // Check if the login is known in our database
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    string username = info.Principal.FindFirstValue(ClaimTypes.Name);
                    string email = info.Principal.FindFirstValue(ClaimTypes.Email);

                    var new_user = new ApplicationUser
                    {
                        FirstName = username.Split(" ").First(),
                        LastName = username.Split(" ").LastOrDefault(),
                        UserName = Regex.Replace(username, @"\s+", "_"),
                        Email = email
                    };
                    var id_result = await _userManager.CreateAsync(new_user);
                    if (id_result.Succeeded)
                    {
                        user = new_user;
                    }
                    else
                    {
                        // User creation failed, probably because the email address is already present in the database
                        if (id_result.Errors.Any(e => e.Code == "DuplicateEmail"))
                        {
                            var existing = await _userManager.FindByEmailAsync(email);
                            var existing_logins = await _userManager.GetLoginsAsync(existing);

                            if (existing_logins.Any())
                            {
                                throw new OtherAccountException(existing_logins);
                            }
                            else
                            {
                                throw new Exception("Could not create account from social profile");
                            }
                        }
                        else
                        {
                            throw new Exception("Could not create account from social profile");
                        }
                    }

                    await _userManager.AddLoginAsync(user, new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName));
                }

                await _signInManager.SignInAsync(user, true);

                var login_result = new LoginResult
                {
                    Status = true,
                    Platform = info.LoginProvider,
                    User = user
                };

                if (login_result.Status)
                {
                    var model = new LoginResultVM
                    {
                        Status = true,
                        Medium = medium,
                        Platform = login_result.Platform,
                        User = user,
                        Token = GetToken(user)
                    };
                    return View(model);
                }
                else
                {
                    var model = new LoginResultVM
                    {
                        Status = false,
                        Medium = medium,
                        Platform = login_result.Platform,

                        Error = login_result.Error,
                        ErrorDescription = login_result.ErrorDescription
                    };
                    return View(model);
                }
            }
            catch (OtherAccountException otherAccountEx)
            {
                var model = new LoginResultVM
                {
                    Status = false,
                    Medium = medium,
                    Platform = provider,

                    Error = "Could not login",
                    ErrorDescription = otherAccountEx.Message
                };
                return View(model);
            }
            catch (Exception ex)
            {
                var model = new LoginResultVM
                {
                    Status = false,
                    Medium = medium,
                    Platform = provider,

                    Error = "Could not login",
                    ErrorDescription = "There was an error with your social login"
                };
                return View(model);
            }
        }

        //[HttpPost]
        //[Route("external-login")]
        //public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //{
        //    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        //[HttpPost]
        //[Route("external-login")]
        //public IActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    var redirectUrl = Url.Action("HandleExternalLogin", "Auth", new { ReturnUrl = returnUrl });
        //    var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);


        //    //return new ChallengeResult(provider, authenticationProperties);
        //}

        //public async Task<IActionResult> HandleExternalLogin(string returnUrl = null, string remoteError = null)
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();

        //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        //    if (!result.Succeeded) //user does not exist yet
        //    {
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        var newUser = new ApplicationUser
        //        {
        //            UserName = email,
        //            Email = email,
        //            EmailConfirmed = true
        //        };
        //        var createResult = await _userManager.CreateAsync(newUser);
        //        if (!createResult.Succeeded)
        //            throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

        //        await _userManager.AddLoginAsync(newUser, info);
        //        var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
        //        await _userManager.AddClaimsAsync(newUser, newUserClaims);
        //        await _signInManager.SignInAsync(newUser, isPersistent: false);
        //        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        //    }

        //    return Ok();
        //}

        //[HttpGet]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Login));
        //    }

        //    var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //    if (signInResult.Succeeded)
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    if (signInResult.IsLockedOut)
        //    {
        //        return RedirectToAction(nameof(ForgotPassword));
        //    }
        //    else
        //    {
        //        ViewData["ReturnUrl"] = returnUrl;
        //        ViewData["Provider"] = info.LoginProvider;
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        return View("ExternalLogin", new ExternalLoginModel { Email = email });
        //    }
        //}

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> CreateToken([FromBody] UserLogin loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: false, lockoutOnFailure: false);

                if (!loginResult.Succeeded)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByNameAsync(loginModel.Email);

                return Ok(GetToken(user));
            }
            return BadRequest(ModelState);

        }

        [Authorize]
        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
                );
            return Ok(GetToken(user));

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    //var callbackUrl = Url.Action(
                    //    controller: "Auth",
                    //    action: "login",
                    //    values: null,
                    //    protocol: Request.Scheme,
                    //    host: "localhost:8100");

                    //await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(true);

                    var authResponse = new AuthResponse
                    {
                        Token = GetToken(user)
                    };

                    return Ok(authResponse);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
            Debug.WriteLine(messages);
            return StatusCode(406);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return StatusCode(205);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (result.Succeeded)
                {
                    var authResponse = new AuthResponse
                    {
                        Token = GetToken(user)
                    };

                    return Ok(authResponse);
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return StatusCode(403); // implicit failure, failed login, includes bad credentials/permissions
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (ModelState.IsValid)
            {
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return StatusCode(404);
                }

                var code = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);
                var codeGenerated = code.First();
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                // var callbackUrl = Url.Page(
                //     "/change-password",
                //     pageHandler: null,
                //     values: new { area = "Identity", code },
                //     protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Your code for reset password: {codeGenerated}");

                return StatusCode(404);
            }

            return StatusCode(404);
        }

        private String GetToken(ApplicationUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.GivenName, $"{user.FirstName} {user.LastName}"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime")),
                audience: this._configuration.GetValue<String>("Tokens:Audience"),
                issuer: this._configuration.GetValue<String>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }
    }
}

using Cinema.Models;
using Cinema.Models.ViewModel;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IApplicationUserOTPRepository applicationUserOTPRepository;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , IEmailSender emailSender, IApplicationUserOTPRepository applicationUserOTPRepository, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.applicationUserOTPRepository = applicationUserOTPRepository;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Register()
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.SuperAdmin));
                await roleManager.CreateAsync(new(SD.Admin));
                await roleManager.CreateAsync(new(SD.Customer));
                await roleManager.CreateAsync(new(SD.Company));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            ApplicationUser applicationUser = new()
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Addrress = registerVM.Address,
            };
            var result = await userManager.CreateAsync(applicationUser, registerVM.Password);
            if (result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var ConfirmationLink = Url.Action("ConfirmEmail", "Account", new { area = "Identity", applicationUser.Id, token }, Request.Scheme);

                await emailSender.SendEmailAsync(applicationUser.Email, "Confirmation Email", $"<h1> Confirm Your Email By Click <a href='{ConfirmationLink}'>Here</a></h1>");
                TempData["Notfication"] = "Add Account Successfuly";
                return RedirectToAction("Index", "Home", new { area = "Spectator" });
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(registerVM);
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var applicationUser = await userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (applicationUser is null)
            {
                applicationUser = await userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            }
            if (applicationUser is not null)
            {
                var result = await userManager.CheckPasswordAsync(applicationUser, loginVM.Password);
                if (result)
                {
                    await signInManager.SignInAsync(applicationUser, loginVM.RememberMe);
                    TempData["Notfication"] = "Login Successfuly";
                    return RedirectToAction("Index", "Home", new { area = "Spectator" });
                }
                ModelState.AddModelError("Password", "Invalid Password");
                return View(loginVM);

            }
            ModelState.AddModelError("Password", "Invalid Email OR UserName");

            return View(loginVM);
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            TempData["Notfication"] = "LogOut Successfuly";
            return RedirectToAction("Index", "Home", new { area = "Spectator" });
        }
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var applicationuser = await userManager.FindByIdAsync(id);
            if (applicationuser is not null)
            {
                if (applicationuser.EmailConfirmed)
                {
                    TempData["Notfication"] = "Alredy Confirmation";
                    return RedirectToAction("Index", "Home", new { area = "Spectator" });


                }
                else
                {
                    var result = await userManager.ConfirmEmailAsync(applicationuser, token);
                    if (result.Succeeded)
                    {

                        TempData["Notfication"] = "Confirmation Account Successfuly";
                        return RedirectToAction("Index", "Home", new { area = "Spectator" });
                    }
                    else
                    {
                        TempData["NotficationError"] = string.Join(" ", result.Errors.Select(e => e.Description));
                    }
                }

            }
            return BadRequest();
        }

        public IActionResult ResendEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmail(ResendEmailVM resendEmailVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resendEmailVM);
            }
            var applicationUser = await userManager.FindByEmailAsync(resendEmailVM.UserNameOrEmail);
            if (applicationUser is null)
            {
                applicationUser = await userManager.FindByNameAsync(resendEmailVM.UserNameOrEmail);
            }
            if (applicationUser is not null)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var ConfirmationLink = Url.Action("ConfirmEmail", "Account", new { area = "Identity", applicationUser.Id, token }, Request.Scheme);

                await emailSender.SendEmailAsync(applicationUser.Email, "Confirmation Email", $"<h1> Confirm Your Email By Click <a href='{ConfirmationLink}'>Here</a></h1>");
                TempData["Notfication"] = "Resned Email Successfuly";
                return RedirectToAction("Index", "Home", new { area = "Spectator" });
            }
            ModelState.AddModelError("UserNameOrEmail", "Invalid Email OR UserName");
            return View(resendEmailVM);
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {

            if (!ModelState.IsValid)
            {
                return View(forgetPasswordVM);

            }
            var applicationUser = await userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail);
            if (applicationUser == null)
            {
                applicationUser = await userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail);
            }
            if (applicationUser is not null)
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(applicationUser);
                var OTPNumber = new Random().Next(1000, 9999);

                //var PasswordResetLink = Url.Action("ResetPassword", "Account", new { area = "Identity", ApplicationUserId = applicationUser.Id, Token = token }, Request.Scheme);
                //await emailSender.SendEmailAsync(applicationUser.Email, "Reset Password", $"<h1>Reset Your Password  Account By Click <a href='{PasswordResetLink}'>Here</a></h1>");
                //return RedirectToAction("Index", "Home", new { area = "Customer" });
                var OTPInDb = applicationUserOTPRepository.Get(e => e.ApplicationUserId == applicationUser.Id).LastOrDefault();
                if (OTPInDb is null || OTPInDb.ExpirationDate < DateTime.UtcNow)
                {
                    await applicationUserOTPRepository.CreateAsync(new()
                    {
                        ApplicationUserId = applicationUser.Id,
                        OTP = OTPNumber,
                        ReleaseDate = DateTime.UtcNow,
                        ExpirationDate = DateTime.UtcNow.AddMinutes(2),
                    });

                    await applicationUserOTPRepository.CommitAsync();
                    await emailSender.SendEmailAsync(applicationUser.Email, "OTP Number", $"<h1>The OTP Number : {OTPNumber}</h1>");

                    TempData["Notification"] = "Check Your Email, find OTP Number";
                    TempData["_Validation"] = Guid.NewGuid().ToString();

                    return RedirectToAction("ResetPassword", "Account", new
                    {
                        area = "Identity",
                        ApplicationUserId = applicationUser.Id,
                        Token = token
                    });
                }

                ModelState.AddModelError(string.Empty, "You have already requested an OTP recently. Please check your email or try again later.");
                return View(forgetPasswordVM);


            }
            ModelState.AddModelError("UserNameOrEmail", "Invalid User Name Or Email");
            return View(forgetPasswordVM);



        }



        public IActionResult ResetPassword()
        {
            if (TempData[key: "_Validation"] is not null)
            {
                return View();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var applicationUser = await userManager.FindByIdAsync(resetPasswordVM.ApplicationUserId);

            if (applicationUser is not null)
            {
                var OTPInDb = applicationUserOTPRepository
                    .Get(e => e.ApplicationUserId == resetPasswordVM.ApplicationUserId && e.OTP == resetPasswordVM.OTP)
                    .OrderByDescending(o => o.ReleaseDate)
                    .FirstOrDefault();

                // تحقق من أن OTP صحيحة وغير منتهية
                if (OTPInDb is not null && OTPInDb.ExpirationDate >= DateTime.UtcNow)
                {
                    var result = await userManager.ResetPasswordAsync(applicationUser, resetPasswordVM.Token, resetPasswordVM.Password);

                    if (result.Succeeded)
                    {
                        TempData["Notification"] = "Password reset successfully.";

                        // حذف الـOTP بعد استخدامها (اختياري لكن جيد من ناحية الأمان)
                        applicationUserOTPRepository.Remove(OTPInDb);
                        await applicationUserOTPRepository.CommitAsync();

                        return RedirectToAction("Index", "Home", new { area = "Spectator" });
                    }
                    else
                    {
                        TempData["NotificationError"] = string.Join(",", result.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    ModelState.AddModelError("OTP", "Invalid or expired OTP.");
                }

                return View(resetPasswordVM);
            }

            return BadRequest();
        }

    }
}

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models.ViewModels;
using ToDoApp.Services;

namespace ToDoApp.Controllers;

public class AccountController : Controller
{
    //  сервис отвечает за создание-удаление пользователей,
    // управление паролями, токенами, ролями, блокировка и тд
    private UserManager<IdentityUser> userManager;
    private SignInManager<IdentityUser> signInManager;
    private IConfiguration configuration;

    public AccountController(UserManager<IdentityUser> userManager,
     SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel,
    [FromServices] RecaptchaService recaptchaService)
    {
        bool check = ModelState.IsValid;

        if (check == true)
        {
            // проверка на робота
            var recaptchaToken = Request.Form["g-recaptcha-response"];

            var isRecaptchaValid = await recaptchaService.VerifyAsync(recaptchaToken);
            if (isRecaptchaValid == false)
            {
                ModelState.AddModelError("", "CAPTCHA error! Try again!");
                return View(viewModel);
            }

            IdentityUser identityUser = new IdentityUser()
            {
                UserName = viewModel.Email,
                Email = viewModel.Email
            };

            // попытка создать пользователя
            IdentityResult result = await userManager.CreateAsync(identityUser, viewModel.Password);
            if (result.Succeeded)
            {
                // добавляем роль для пользователя
                await userManager.AddToRoleAsync(identityUser, "customer");

                return RedirectToAction("Index");
            }

            // делаем модель невалидной
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }

        return View(viewModel);
    }
    [HttpGet]
    public IActionResult Register()
    {
        var siteKey = configuration["ReCaptchaSettings:SiteKey"];

        ViewBag.SiteKey = siteKey;

        // var viewModel = new RegisterViewModel(){SiteKey = siteKey}; //?
        /*
        Вопрос:
        Почему при передедачи SiteKey как поля RegisterViewModel 
        в HttpPOST Register() Model.IsValid = false ?
        Хотя во view данные точно приходят и они верные 100%,
        потому что я просто их вывел в теге h1 и посмотрел
        их верность(значение SiteKey совпало с исходным(то есть нужным мне))
        */

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginViewModel viewModel,
    [FromServices] RecaptchaService recaptchaService)
    {
        if (ModelState.IsValid)
        {
            // проверка на робота
            var recaptchaToken = Request.Form["g-recaptcha-response"];
            var isRecaptchaValid = await recaptchaService.VerifyAsync(recaptchaToken);
            if (!isRecaptchaValid)
            {
                ModelState.AddModelError("", "Captcha error! Try again!");
                return View(viewModel);
            }

            var result = await signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password,
             viewModel.RememberMe, false);
            if (result.Succeeded)
            {
                return Content("Ok");
            }
            return Content("Error");
        }
        return View(viewModel);
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return View();
    }
}

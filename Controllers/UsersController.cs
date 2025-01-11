using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models.ViewModels;

namespace ToDoApp.Controllers;

[Authorize(Roles = "admin")]
public class UsersController : Controller
{
    private UserManager<IdentityUser> userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }
    public ActionResult Index() // 
    {
        // identity core получить всех польщователей,
        // которые у нас есть в бд
        var users = userManager.Users.ToList();

        return View(users);
    }
    public async Task<IActionResult> AllUsers()
    {
        var users = userManager.Users.ToList();

        var usersForView = new List<UserWithRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            usersForView.Add(new UserWithRolesViewModel(){ UserId = user.Id,
                UserName = user.UserName, Email = user.Email, Roles =string.Join(", ", roles)});
        }

        return View(usersForView);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityUser identityUser = await userManager.FindByIdAsync(id);

        // проверка админ или нет
        bool isAdmin = await userManager.IsInRoleAsync(identityUser, "admin");

        // если мы - админ когда авторизованы
        // проверка пользователя который только что открыл метод Delete
        if(User.IsInRole("isAdmin"))

        if(identityUser != null){
        await userManager.DeleteAsync(identityUser);

        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if(user != null)
        {
            EditUserViewModel viewModel = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(viewModel);
        }
        return  RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Edits(EditUserViewModel viewModel)
    {
        IdentityUser identityUser = await userManager.FindByIdAsync(viewModel.Id);

        identityUser.UserName = viewModel.UserName;
        identityUser.Email = viewModel.Email;

        await userManager.UpdateAsync(identityUser); // обновление(сохранение) изменений manager'а

        // работа с паролем
        if(!string.IsNullOrEmpty(viewModel.NewPassword))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(identityUser);
            var passwordRes = await userManager.ResetPasswordAsync(identityUser, token, viewModel.NewPassword);

            if(!passwordRes.Succeeded)
            {
                // ModelState изменение
            }
        }

        return RedirectToAction("Index");
    }
}

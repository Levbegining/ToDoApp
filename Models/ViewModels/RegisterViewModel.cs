using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models.ViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    // public string SiteKey { get; set; }

}

using System;

namespace ToDoApp.Models.ViewModels;

public class UserWithRolesViewModel
{
public string UserId { get; set; }
public string UserName { get; set; }
public string Email { get; set; } // user'а
public string Roles { get; set; }
}

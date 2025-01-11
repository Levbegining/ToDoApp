using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Configure<ReCaptchaSettings>(builder.Configuration.GetSection("ReCaptchaSettings"));

builder.Services.AddScoped<RecaptchaService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
// для бд
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("MyDataBase")));

#region CoreIdentity
// builder.Services.AddSingleton(TimeProvider.System);

// // сервис для CoreIdentity
// builder.Services.AddIdentityCore<IdentityUser>().
//     AddEntityFrameworkStores<AppIdentityDbContext>().
//     AddSignInManager(). // для входа в систему
//     AddDefaultTokenProviders(); // для генерации токенов

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//     options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//     options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
// }).AddCookie(IdentityConstants.ApplicationScheme, options =>
// {
//     options.LoginPath = "/Account/Login";
//     options.LogoutPath = "/Account/Logout";
//     options.ExpireTimeSpan = TimeSpan.FromDays(14);
// });
#endregion



builder.Services.AddIdentity<IdentityUser, IdentityRole>().
    AddEntityFrameworkStores<AppIdentityDbContext>().
    AddDefaultTokenProviders(); // для генерации токенов



// // добавление ролей
// async Task SeedRoles(IServiceProvider serviceProvider)
// {
//     var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//     var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

//     if(!await roleManager.RoleExistsAsync("admin")){
//         await roleManager.CreateAsync(new IdentityRole("adimin"));
//     }
//     if(!await roleManager.RoleExistsAsync("customer")){
//         await roleManager.CreateAsync(new IdentityRole("customer"));
//     }

//     // создание админ юзера
//     var adminEmail = "admin@todo.com";
//     var adminPassword = "Admin@123";
//     var adminUser = userManager.FindByEmailAsync(adminEmail);
    
//     if(adminUser == null)
//     {
//         var user = new IdentityUser(){ UserName = adminEmail, Email = adminEmail };
//         var result = await userManager.CreateAsync(user);

//         if(result.Succeeded){
//             await userManager.AddToRoleAsync(user, "admin");
//         }
//     }
// }

async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider) 
{ 
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); 
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); 
 
    if(!await roleManager.RoleExistsAsync("admin")) 
    { 
        await roleManager.CreateAsync(new IdentityRole("admin")); 
    } 
 
    if(!await roleManager.RoleExistsAsync("customer")) 
    { 
        await roleManager.CreateAsync(new IdentityRole("customer")); 
    } 
 
    // создание админ юзера 
    var adminEmail = "admin@todo.net"; 
    var adminPassword = "Admin@123"; 
    var adminUser = await userManager.FindByEmailAsync(adminEmail); 
 
    if (adminUser == null) 
    { 
        var user = new IdentityUser() {UserName = adminEmail, Email = adminEmail}; 
        var result = await userManager.CreateAsync(user, adminPassword); 
 
        if (result.Succeeded) 
        { 
            await userManager.AddToRoleAsync(user, "admin"); 
        } 
    } 
}

 var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;

//     await SeedRoles(services);
// }
 
using(var scope = app.Services.CreateScope()) 
{ 
    var services = scope.ServiceProvider; 
    await SeedRolesAndAdminUser(services); 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();

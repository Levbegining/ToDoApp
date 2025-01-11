using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Data;

// 1. Наследуем от IdentityDbContext
public class AppIdentityDbContext : IdentityDbContext
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) :
    base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // убираем таблицы
        //builder.Ignore<IdentityRole>();
        //builder.Ignore<IdentityUserRole<string>>();
        //builder.Ignore<IdentityUserClaim<string>>();
        //builder.Ignore<IdentityRoleClaim<string>>();

        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityUserToken<string>>();
    }
}

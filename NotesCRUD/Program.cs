using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesCRUD.Data.DbContexts;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.Repository;
using NotesCRUD.Data.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<NotesCRUDDbContext>();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();



builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.Configure<CookiePolicyOptions>(options =>
{
options.CheckConsentNeeded = context => true;
options.MinimumSameSitePolicy = SameSiteMode.Strict;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<NotesCRUDDbContext>(options =>
   options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<NotesCRUDDbContext>()
.AddDefaultTokenProviders();


//builder.Services.AddDbContext<NotesCRUDDbContext>();


builder.Services.AddMvc(options =>
{
var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
options.Filters.Add(new AuthorizeFilter(policy));
});

//builder.Services.AddTransient<MyViewComponent>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
// Cookie settings
options.Cookie.HttpOnly = true;
options.ExpireTimeSpan = TimeSpan.FromDays(150);
options.LoginPath = "/Auth/Index";
options.LogoutPath = "/Auth/SignOut";
options.AccessDeniedPath = new PathString("/Auth/Index");
options.SlidingExpiration = true;
});
builder.Services.Configure<IISOptions>(options =>
{
options.AutomaticAuthentication = false;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BKUNotes API", Version = "v1" });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("83=)Aa6L\\zhjN4DT*VFnAfo9(}RhHpC{//\\X5Y%KEx#NfwEWf0_i%ut<%*J(\"aK"))
            };
        });

builder.Services.AddAuthorization();

builder.Services.AddScoped<INoteRepo, NoteRepo>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();

var app = builder.Build();
var serviceProvider = app.Services;

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await SeedRoles.CreateRole(services);
//}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NotesCRUD API v1");
});

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("X-Frame-Options", "DENY");
//    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
//    //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; " +
//    //                             "script-src 'self'; object-src 'none'; base-uri 'self'; frame-src 'none'");
//    await next();
//});



//builder.Services.AddAuthorization();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
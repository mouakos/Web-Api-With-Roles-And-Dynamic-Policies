using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiWithRolesAmdDynamicPolicies.ActionsFilters;
using WebApiWithRolesAmdDynamicPolicies.Authorization;
using WebApiWithRolesAmdDynamicPolicies.Data;
using WebApiWithRolesAmdDynamicPolicies.Entities;
using WebApiWithRolesAmdDynamicPolicies.ExceptionHandling;
using WebApiWithRolesAmdDynamicPolicies.Interfaces;
using WebApiWithRolesAmdDynamicPolicies.JwtFeatures;
using WebApiWithRolesAmdDynamicPolicies.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Suppress automatically API controller model validation
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// Add Validation filter
builder.Services.AddScoped<ModelValidationFilterAttribute>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<JwtHandler>();

/* Database */
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ??
                      throw new InvalidOperationException("Sorry, your connection is not found"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetRequiredSection("JwtSettings");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtSettings.GetRequiredSection("ValidIssuer").Value,
            ValidIssuer = jwtSettings.GetRequiredSection("ValidAudience").Value,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetRequiredSection("Key").Value!))
        };
    });

// Roles policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, DynamicRoleHandler>();

var app = builder.Build();

// Add exception middleware
app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication before Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
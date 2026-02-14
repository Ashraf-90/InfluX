using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using InfluX.Hubs;
using Infrastructure.ExtraServies;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Reposities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// App DB (Domain entities)
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CONSTR")));

// Auth DB (Identity)
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CONSTR")));

// Identity GUID
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// =======================
// JWT (For Mobile APIs)
// =======================
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

// JWT service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Auth/Login";
    opt.LogoutPath = "/Auth/Logout";
    opt.AccessDeniedPath = "/Auth/AccessDenied";
});

// UoW uses AppDBContext only
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Existing services
builder.Services.AddScoped<IMetaPagesServices, MetaPagesServices>();
builder.Services.AddScoped<IKeyWordsServices, KeyWordsServices>();
builder.Services.AddScoped<IPixelsServices, PixelsServices>();

// Identity service
builder.Services.AddScoped<IIdentityUserServices, IdentityUserServices>();

// CRUD services
builder.Services.AddScoped<INicheServices, NicheServices>();
builder.Services.AddScoped<IUserNicheServices, UserNicheServices>();
builder.Services.AddScoped<IUserKeyWordServices, UserKeyWordServices>();
builder.Services.AddScoped<ISocialAccountServices, SocialAccountServices>();
builder.Services.AddScoped<IInfluencerProfileServices, InfluencerProfileServices>();
builder.Services.AddScoped<IUserProfileServices, UserProfileServices>();
builder.Services.AddScoped<IVerificationRequestServices, VerificationRequestServices>();
builder.Services.AddScoped<IServiceListingServices, ServiceListingServices>();
builder.Services.AddScoped<IServicePricingOptionServices, ServicePricingOptionServices>();
builder.Services.AddScoped<IInfluencerMediaServices, InfluencerMediaServices>();
builder.Services.AddScoped<IInfluencerAssetServices, InfluencerAssetServices>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/EN/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/ChatHub");

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

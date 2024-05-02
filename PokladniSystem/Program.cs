using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.Implementation;
using PokladniSystem.Application.Validations;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Domain.Validations;
using PokladniSystem.Infrastructure.Database;
using PokladniSystem.Infrastructure.Identity;
using PokladniSystem.Infrastructure.Identity.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
builder.Services.AddScoped<IValidator<LoginViewModel>, LoginViewModelValidator>();
builder.Services.AddScoped<IValidator<AccountAdminEditViewModel>, AccountAdminEditViewModelValidator>();
builder.Services.AddScoped<IValidator<AccountUserEditViewModel>, AccountUserEditViewModelValidator>();
builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
builder.Services.AddScoped<IValidator<VATRate>, VATRateValidator>();
builder.Services.AddScoped<IValidator<StoreViewModel>, StoreViewModelValidator>();
builder.Services.AddScoped<IValidator<CompanyViewModel>, CompanyViewModelValidator>();
builder.Services.AddScoped<IValidator<ProductViewModel>, ProductViewModelValidator>();
builder.Services.AddScoped<IValidator<SupplyViewModel>, SupplyViewModelValidator>();

string connectionString = builder.Configuration.GetConnectionString("MySQL");
ServerVersion serverVersion = new MySqlServerVersion("8.0.34");
builder.Services.AddDbContext<CRSDbContext>(optionsBuilder => optionsBuilder.UseMySql(connectionString, serverVersion));

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<User, Role>()
     .AddEntityFrameworkStores<CRSDbContext>()
     .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.LoginPath = "/Security/Account/Login";
    options.LogoutPath = "/Security/Account/Logout";
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IAccountService, AccountIdentityService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IVATService, VATService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>(serviceProvider =>
{
    var webRootPath = serviceProvider.GetService<IWebHostEnvironment>().WebRootPath;
    var dbContext = serviceProvider.GetService<CRSDbContext>();
    var saleService = serviceProvider.GetService<ISaleService>();
    var companyService = serviceProvider.GetService<ICompanyService>();
    var storeService = serviceProvider.GetService<IStoreService>();
    var productService = serviceProvider.GetService<IProductService>();

    return new ReceiptService(webRootPath, dbContext, saleService, companyService, storeService, productService);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    var roles = Enum.GetNames(typeof(Roles));
    foreach (var role in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new Role(role));
        }
    }

    var userName = "admin";
    // Default password that must be changed due to security reasons!
    var userPassword = "admin";

    var user = await userManager.FindByNameAsync(userName);
    if (user == null)
    {
        user = new User
        {
            UserName = userName,
            Active = true
        };

        var result = await userManager.CreateAsync(user, userPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
        }
    }
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

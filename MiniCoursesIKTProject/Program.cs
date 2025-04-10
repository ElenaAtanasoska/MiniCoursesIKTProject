using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
using (var scope = app.Services.CreateScope())
{
    var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[]
    {
        "Admin",
        "Editor",
        "Student"
    };
    foreach (var role in roles)
        if (!await RoleManager.RoleExistsAsync(role))
            await RoleManager.CreateAsync(new IdentityRole(role));
}

using (var scope = app.Services.CreateScope())
{
    var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    var email = "admin@admin.com";
    var sitename = "admin@admin.com";
    var password = "Kai.123Kai.123";
    //////////////////////////////
    var email2 = "editor@edit.com";
    var sitename2 = "editor@edit.com";
    var passwrod2 = "Kai.123";
    ///////////////////////////////
    var student1 = "211065@student.minicourses.com";
    var studentname = "211065@student.minicourses.com";
    var Name = "Kire";
    var Prezime = "Smilkov";
    var indeks = "211065";
    if (await UserManager.FindByEmailAsync(email) == null)
    {
        var user = new User();
        user.Email = email;
        user.UserName = sitename;
        await UserManager.CreateAsync(user, password);
        await UserManager.AddToRoleAsync(user, "Admin");
    }

    if (await UserManager.FindByEmailAsync(email2) == null)
    {
        var user2 = new User();
        user2.Email = email2;
        user2.UserName = sitename2;
        await UserManager.CreateAsync(user2, passwrod2);
        await UserManager.AddToRoleAsync(user2, "Editor");
    }

    if (await UserManager.FindByEmailAsync(student1) == null)
    {
        var user3 = new User();
        user3.Email = student1;
        user3.UserName = studentname;
        user3.Name = Name;
        user3.LastName = Prezime;
        user3.Indeks = indeks;
        await UserManager.CreateAsync(user3, passwrod2);
        await UserManager.AddToRoleAsync(user3, "Student");
    }
}

await app.RunAsync();
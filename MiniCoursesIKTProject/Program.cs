using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<Student>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Student>(options => options.SignIn.RequireConfirmedAccount = false)
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
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
    {
        if (!await RoleManager.RoleExistsAsync(role))
            await RoleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<Student>>();

    string email = "admin@admin.com";
    string sitename = "admin@admin.com";
    string password = "Kai.123";
    //////////////////////////////
    string email2 = "editor@edit.com";
    string sitename2 = "editor@edit.com";
    string passwrod2 = "Kai.123";
    ///////////////////////////////
    string student1 = "211065@student.minicourses.com";
    string studentname = "211065@student.minicourses.com";
    string Name = "Kire";
    string Prezime = "Smilkov";
    string indeks = "211065";
    if (await UserManager.FindByEmailAsync(email) == null)
    {
        var user = new Student();
        user.Email = email;
        user.UserName = sitename;
        await UserManager.CreateAsync(user, password);
        await UserManager.AddToRoleAsync(user, "Admin");
    }

    if (await UserManager.FindByEmailAsync(email2) == null)
    {
        var user2 = new Student();
        user2.Email = email2;
        user2.UserName = sitename2;
        await UserManager.CreateAsync(user2, passwrod2);
        await UserManager.AddToRoleAsync(user2, "Editor");
    }

    if (await UserManager.FindByEmailAsync(student1) == null)
    {
        var user3 = new Student();
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
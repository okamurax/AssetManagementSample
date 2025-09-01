using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --------------------------------------------------------
// �ǉ�

builder.Services.AddDbContext<TestDbContext>(options =>
{
    // SQLite�g���ꍇ
    // ���s�t�@�C���Ɠ����K�w�ɁAdat.sqlite���쐬
    // NuGet��EntityFrameworkCore.Sqlite���C���X�g�[���B

    options.UseSqlite("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dat.sqlite");

    // ���̂悤��appsettings.json�̒����Ăяo�����Ƃ��\�B
    //options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"));

    // SQL Server�̏ꍇ
    //options.UseSqlServer(builder.Configuration.GetConnectionString("xxx"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        //options.SlidingExpiration = true;
        //options.AccessDeniedPath = "/Account/Login/";
        options.LoginPath = "/account/login/";
        //options.LoginPath = CookieAuthenticationDefaults.LoginPath.ToString().ToLower();

    });

builder.Services.AddControllers(options =>
{
    // �S�ẴR���g���[���[�ŔF�؂��K�v
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

builder.Services.AddSingleton<SelectItems>();

// �����܂�
// --------------------------------------------------------

var app = builder.Build();

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

// --------------------------------------------------------
// �ǉ�

app.UseAuthentication();

// �����܂�
//--------------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TestTable33}/{action=Index}/{id?}");

app.Run();

// add-migration add_pass
// update-database

// model��ύX����update-database�����ꍇ�Ŏ��s������model�̕ύX�_��߂��Ȃ��ƃG���[

// --------------------------------------------------------

//if (User.Identity.IsAuthenticated) ���F����Ă��邩�ǂ����̔��f�͂��̂悤��

//--------------------------------------------------------


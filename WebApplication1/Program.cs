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
// 追加

builder.Services.AddDbContext<TestDbContext>(options =>
{
    // SQLite使う場合
    // 実行ファイルと同じ階層に、dat.sqliteを作成
    // NuGetでEntityFrameworkCore.Sqliteをインストール。

    options.UseSqlite("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\dat.sqlite");

    // このようにappsettings.jsonの中を呼び出すことも可能。
    //options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"));

    // SQL Serverの場合
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
    // 全てのコントローラーで認証が必要
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

builder.Services.AddSingleton<SelectItems>();

// ここまで
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
// 追加

app.UseAuthentication();

// ここまで
//--------------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TestTable33}/{action=Index}/{id?}");

app.Run();

// add-migration add_pass
// update-database

// modelを変更してupdate-databaseした場合で失敗したらmodelの変更点を戻さないとエラー

// --------------------------------------------------------

//if (User.Identity.IsAuthenticated) 承認されているかどうかの判断はこのように

//--------------------------------------------------------


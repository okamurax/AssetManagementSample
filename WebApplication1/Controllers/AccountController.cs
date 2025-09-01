using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        List<ApplicationUser> users = new List<ApplicationUser> {
            new ApplicationUser{UserName = "管理ユーザー", Password = "NcZzBxPkBub48"},
            new ApplicationUser{UserName = "閲覧ユーザー", Password = "BPIM6buDQi77s"}
        };

        [AllowAnonymous] // 追加
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "TestTable33");
            }

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AllowAnonymous] // 追加
        public async Task<IActionResult> Login(ApplicationUser user, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "TestTable33");
            }

            // ログイン後の処理

            //if (!ModelState.IsValid) return View(user);

            //const string badUserNameOrPasswordMessage = "Username or password is incorrect.";

            //if (user == null)
            //{
                //ModelState.AddModelError(string.Empty, badUserNameOrPasswordMessage);
                //return View(user);

                // このように書いている場合もあった
                // この場合、ApplicationUserクラスに[Required]をつけていなかった
                //return BadRequest(badUserNameOrPasswordMessage);
            //}

            // ユーザー名が一致するユーザーを抽出
            var lookupUser = users.Where(u => u.UserName == user.UserName).FirstOrDefault();

            //if (lookupUser == null)
            //{
                //ModelState.AddModelError(string.Empty, badUserNameOrPasswordMessage);
                //return View(user);

                // このように書いている場合もあった
                // この場合、ApplicationUserクラスに[Required]をつけていなかった
                //return BadRequest(badUserNameOrPasswordMessage);

            //}

            // パスワードの比較
            if (lookupUser?.Password != user.Password)
            {
                //return BadRequest(badUserNameOrPasswordMessage);
                return View(user);
            }

            // Cookies 認証スキームで新しい ClaimsIdentity を作成し、ユーザー名を追加します。
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, lookupUser.UserName));

            // クッキー認証スキームと、上の数行で作成されたIDから作成された新しい ClaimsPrincipal を渡します。
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "TestTable33");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}

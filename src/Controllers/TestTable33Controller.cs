using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using ZXing.SkiaSharp;
using Microsoft.Data.SqlClient.Server;
using SkiaSharp;
using System.Net.NetworkInformation;
using System.Numerics;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class TestTable33Controller : Controller
    {
        private readonly TestDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TestTable33Controller(TestDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> search(string Number, string Name, string Model, string Maker, string Location)
        {
            if (Number == null && Name == null && Model == null && Maker == null && Location == null)
            {
                return View("index", await _context.TestTable33s.ToListAsync());
            }

            List<TestTable33> result = new List<TestTable33>();

            if (Number != null)
            {
                var temp = await _context.TestTable33s.Where(
                    m => m.管理番号.ToLower().Contains(Number.ToLower())
                    ).ToListAsync();
                result.AddRange(temp);
            }

            if (Name != null)
            {
                var temp = await _context.TestTable33s.Where(
                    m => m.一般名称.ToLower().Contains(Name.ToLower())
                    ).ToListAsync();
                result.AddRange(temp);
            }

            if (Model != null)
            {
                var temp = await _context.TestTable33s.Where(
                    m => m.モデル名.ToLower().Contains(Model.ToLower())
                    ).ToListAsync();
                result.AddRange(temp);
            }

            if (Maker != null)
            {
                var temp = await _context.TestTable33s.Where(
                    m => m.メーカー.ToLower().Contains(Maker.ToLower())
                    ).ToListAsync();
                result.AddRange(temp);
            }

            if (Location != null)
            {
                var temp = await _context.TestTable33s.Where(
                    m => m.設置保存場所.ToLower().Contains(Location.ToLower())
                    ).ToListAsync();
                result.AddRange(temp);
            }

            //else
            //{
            //    return BadRequest("");
            //}

            return View("index", result.Distinct().OrderBy(m => m.Id));
        }

        // public async Task<IActionResult> Index(string staff)
        // 引数だけが違うオーバーロードはできない

        // GET: TestTable33
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestTable33s.ToListAsync());
        }

        // GET: TestTable33/Details/5

        // 認証書き方の一つ
        //[Authorize(Roles = "Administrators")]
        //

        public async Task<IActionResult> CreateImage(string? id)
        {
            if (id == null) id = "";
            var number = id;

            using SkiaSharp.SKBitmap bitmap = new SkiaSharp.SKBitmap(490, 100);
            using SkiaSharp.SKCanvas canvas = new SkiaSharp.SKCanvas(bitmap);

            //背景描画1
            canvas.Clear(SKColors.White);

            //背景描画2
            SkiaSharp.SKPaint paint1 = new SkiaSharp.SKPaint();
            paint1.Style = SkiaSharp.SKPaintStyle.Fill;
            paint1.Color = SkiaSharp.SKColors.Blue;

            // 文字描画
            SkiaSharp.SKPaint paint2 = new SkiaSharp.SKPaint();
            paint2.Style = SkiaSharp.SKPaintStyle.Fill;
            paint2.Color = SkiaSharp.SKColors.Black;
            paint2.TextSize = 36;
            paint2.Typeface = SkiaSharp.SKTypeface.FromFamilyName("Yu Gothic UI");
            paint2.IsAntialias = true;
            canvas.DrawText("管理番号：", 110, 45, paint2);
            canvas.DrawText(number, 110, 85, paint2); ;

            //QRコード描画
            var writer = new BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 90,
                    Height = 90,
                    Margin = 0,
                }
            };

            var ms = new MemoryStream();
            var qr = writer.Write(Request.Headers["Referer"]);
            canvas.DrawImage(SkiaSharp.SKImage.FromBitmap(qr), new SkiaSharp.SKPoint(5, 5));
            bitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
            ms.Position = 0;
            return new FileStreamResult(ms, "image/png");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TestTable33s == null)
            {
                //return NotFound();
                return RedirectToAction("Index", "TestTable33");
            }

            var testTable33 = await _context.TestTable33s
                .FirstOrDefaultAsync(m => m.Id == id);

            if (testTable33 == null)
            {
                return NotFound();
            }

            ViewData["number"] = testTable33.管理番号;

            if (User.Identity.Name == "閲覧ユーザー")
            {
                testTable33.パスワード１ = "*****";
                testTable33.パスワード２ = "*****";
                testTable33.パスワード３ = "*****";
            }

            if(System.IO.File.Exists(_environment.WebRootPath + "/Image/" + id.ToString() + ".png"))
            {
                ViewData["exists"] = true;
            }
            else
            {
                ViewData["exists"] = false;
            }

            return View(testTable33);
        }

        // GET: TestTable33/Create
        public IActionResult Create()
        {
            if (User.Identity.Name != "管理ユーザー")
            {
                return RedirectToAction("Index", "TestTable33");
            }

            var tableRow = _context.TestTable33s.OrderByDescending(m => m.Id).FirstOrDefault();
            int maxId = 1;
            if (tableRow != null) maxId = tableRow.Id + 1;
            var testTable33 = new TestTable33();
            testTable33.Id = maxId;
            testTable33.管理番号 = maxId.ToString().PadLeft(5, '0');
            testTable33.備考 = "ログイン、ライセンス更新方法等";

            return View(testTable33);
        }

        public async Task SaveImage(string id)
        {
            var formFile = Request.Form.Files["furniture"];

            if (formFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    await formFile.CopyToAsync(ms);
                    var byteArray = ms.ToArray();
                    SKBitmap srcBitmap = SKBitmap.Decode(byteArray);

                    ms.Position = 0;
                    SKCodec codec = SKCodec.Create(ms);
                    var origin = codec.EncodedOrigin;

                    int w = srcBitmap.Width;
                    int h = srcBitmap.Height;

                    SKRect src;

                    if (w > h)
                    {
                        int tmp = (w - h) / 2;
                        src = new SKRect(tmp, 0, tmp + h, h); // left, top, right, bottom
                    }
                    else if (w < h)
                    {
                        int tmp = (h - w) / 2;
                        src = new SKRect(0, tmp, w, tmp + w);
                    }
                    else // w == h
                    {
                        src = new SKRect(0, 0, w, h);
                    }

                    SKBitmap croppedBitmap = new SKBitmap(500, 500);
                    SKCanvas canvas = new SKCanvas(croppedBitmap);

                    // Exif Orientation
                    switch (origin)
                    {
                        case SKEncodedOrigin.TopLeft: // 1
                            break;
                        case SKEncodedOrigin.TopRight: // 2
                            canvas.Scale(-1, 1, 250, 250);
                            break;
                        case SKEncodedOrigin.BottomRight: // 3
                            canvas.RotateDegrees(180, 250, 250);
                            break;
                        case SKEncodedOrigin.BottomLeft: // 4
                            canvas.Scale(1, -1, 250, 250);
                            break;
                        case SKEncodedOrigin.LeftTop: // 5
                            canvas.RotateDegrees(90, 250, 250);
                            canvas.Scale(1, -1, 250, 250);
                            break;
                        case SKEncodedOrigin.RightTop: // 6
                            canvas.RotateDegrees(90, 250, 250);
                            break;
                        case SKEncodedOrigin.RightBottom: // 7
                            canvas.RotateDegrees(90, 250, 250);
                            canvas.Scale(-1, 1, 250, 250);
                            break;
                        case SKEncodedOrigin.LeftBottom: // 8
                            canvas.RotateDegrees(270, 250, 250);
                            break;
                    }

                    SKRect dst = new SKRect(0, 0, 500, 500);
                    canvas.DrawBitmap(srcBitmap, src, dst);

                    using var fs = new FileStream(_environment.WebRootPath + "/Image/" + id + ".png", FileMode.Create); //同名ファイルがあると上書きされる
                    croppedBitmap.Encode(fs, SKEncodedImageFormat.Png, 100);
                }
            }
        }

        // POST: TestTable33/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,管理番号,関連番号,導入年月,担当者１,担当者２,一般名称,モデル名,メーカー,設置保存場所,所有形態,Idユーザー名,メール,パスワード,パスワード１,パスワード２,パスワード３,備考")] TestTable33 testTable33)
        //public async Task<IActionResult> Create([Bind("管理番号,関連番号,導入年月,担当者１,担当者２,一般名称,モデル名,メーカー,設置保存場所,所有形態,Idユーザー名,メール,パスワード,備考")] TestTable33 testTable33)
        {
            await SaveImage(testTable33.Id.ToString());

            if (ModelState.IsValid)
            {
                _context.Add(testTable33);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testTable33);
        }

        // GET: TestTable33/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.Name != "管理ユーザー")
            {
                return RedirectToAction("Index", "TestTable33");
            }

            if (id == null || _context.TestTable33s == null)
            {
                return NotFound();
            }

            var testTable33 = await _context.TestTable33s.FindAsync(id);
            if (testTable33 == null)
            {
                return NotFound();
            }

            if (System.IO.File.Exists(_environment.WebRootPath + "/Image/" + id.ToString() + ".png"))
            {
                ViewData["exists"] = true;
            }
            else
            {
                ViewData["exists"] = false;
            }

            return View(testTable33);
        }

        // POST: TestTable33/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,管理番号,関連番号,導入年月,担当者１,担当者２,一般名称,モデル名,メーカー,設置保存場所,所有形態,Idユーザー名,メール,パスワード,パスワード１,パスワード２,パスワード３,備考")] TestTable33 testTable33)
        {
            if (User.Identity.Name != "管理ユーザー")
            {
                return RedirectToAction("Index", "TestTable33");
            }

            if (id != testTable33.Id)
            {
                return NotFound();
            }

            await SaveImage(testTable33.Id.ToString());

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testTable33);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestTable33Exists(testTable33.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testTable33);
        }

        // GET: TestTable33/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity.Name != "管理ユーザー")
            {
                return RedirectToAction("Index", "TestTable33");
            }

            if (id == null || _context.TestTable33s == null)
            {
                return NotFound();
            }

            var testTable33 = await _context.TestTable33s
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testTable33 == null)
            {
                return NotFound();
            }

            return View(testTable33);
        }

        // POST: TestTable33/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity.Name != "管理ユーザー")
            {
                return RedirectToAction("Index", "TestTable33");
            }

            if (_context.TestTable33s == null)
            {
                return Problem("Entity set 'TestDbContext.TestTable33s'  is null.");
            }

            string imagePath = _environment.WebRootPath + "/Image/" + id.ToString() + ".png";
            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

            var testTable33 = await _context.TestTable33s.FindAsync(id);
            if (testTable33 != null)
            {
                _context.TestTable33s.Remove(testTable33);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestTable33Exists(int id)
        {
            return _context.TestTable33s.Any(e => e.Id == id);
        }
    }
}

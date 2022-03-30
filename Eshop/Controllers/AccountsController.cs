using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Http;

namespace Eshop.Controllers
{
    public class AccountsController : Controller
    {
        private readonly EshopContext _context;

        public AccountsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            //var result = _context.Accounts
            //                      .Where(a => a.Email.Contains("@gmail.com"))
            //                      .OrderByDescending(a => a.FullName);


            //ViewData["result"] = result.FirstOrDefault();

            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                Account user = _context.Accounts.Where(_ => _.Username == Username && _.Password == Password).FirstOrDefault();
                if (user != null)
                {
                    // Giả sử muốn lưu cookie xuống client
                    // append: lưu thêm 1 cookie vào cuối list cookie
                    // vì sao lại .Response ? Vì cookie lưu ở client, server muốn client lưu
                    // thêm cookie nào thì gửi yêu cầu trong Response
                    // Khi người dùng gửi Request sẽ gửi kèm cookie

                    //CookieOptions cookieOptions = new CookieOptions
                    //{
                    //    Expires = DateTime.Now.AddDays(7),
                    //    Domain = ".eshop.com",   // tên miền có www hay không đều được
                    //    Path = "/Accounts/"      // chỉ có hiệu lực với Accounts (mặc định là /)
                    //};
                    //HttpContext.Response.Cookies.Append("Cookie_AccountId", user.Id.ToString(), cookieOptions);
                    //HttpContext.Response.Cookies.Append("Cookie_AccountUsername", user.Username, cookieOptions);

                    // Kiểm tra  ở các action khác: (kiểm tra request gửi lên có cookie không)
                    //if (HttpContext.Request.Cookies.ContainsKey("Cookie_AccountUsername"))
                    //{
                    // ViewBag.AccountUsername = HttpContext.Request.Cookies["Cookie_AccountUsername"];
                    //}

                    HttpContext.Session.SetInt32("Account_Id", user.Id);
                    HttpContext.Session.SetString("Account_Username", user.Username);
                    HttpContext.Session.SetString("Account_IsAdmin", user.IsAdmin.ToString());

                    if (TempData["prevController"] != null && TempData["prevAction"] != null)
                    {
                        var _prevController = TempData["prevController"].ToString();
                        var _prevAction = TempData["prevAction"].ToString();

                        TempData["prevController"] = null;
                        TempData["prevAction"] = null;

                        return RedirectToAction(_prevAction, _prevController);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.isFailed = 1;
                    return View();
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            // ghi cookie cùng tên xuống cline và ho ngày expire là quá khứ
            //HttpContext.Response.Cookies.Append("Cookie_AccountId", "", new CookieOptions{Expires = DateTime.Now.AddDays(-1)});
            //HttpContext.Response.Cookies.Append("Cookie_AccountUsername", "", new CookieOptions{Expires = DateTime.Now.AddDays(-1));

            //HttpContext.Session.Remove("Account_Username");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,Status")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,Status")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}

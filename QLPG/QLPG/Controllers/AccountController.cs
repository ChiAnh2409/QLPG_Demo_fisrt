using QLPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QLPG.Controllers
{
    public class AccountController : Controller 
    {
        private QLPG1Entities db = new QLPG1Entities(); // Tạo biến database để lấy dữ liệu
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(Account acc) 
        {
            var usr = db.Account.SingleOrDefault(a => a.Username == acc.Username && a.Pass == acc.Pass);
            if (usr != null)
            {
                // Kiểm tra và gán Session["Role"] sau khi xác thực người dùng thành công
                var role = (from ru in db.Account
                            join r in db.Roles on ru.id_Role equals r.id_Role
                            where ru.Username == acc.Username  
                            select new { Account = ru, Roles = r }).FirstOrDefault();

                if (role != null)
                {
                    Session["Role"] = role.Roles.Roles1.ToString();
                }

                Session["Username"] = usr.Username.ToString();
                Session["Pass"] = usr.Pass.ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu nhập sai!");
            }

            return View(acc);
        }

        public ActionResult DangXuat() 
        {
            // Xóa session đăng nhập
            Session.Abandon();

            // Chuyển hướng đến trang chủ
            return RedirectToAction("Index", "Account");
        }


    }
}
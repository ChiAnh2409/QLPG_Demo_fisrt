using QLPG.Models;
using QLPG.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLPG.Controllers
{
    public class UserController: Controller
    {
        private QLPG1Entities db = new QLPG1Entities();
        //tạo biến database để lấy dữ liệu
        // GET: User
        public ActionResult NguoiDung()
        {
            var list = new MultipleData();
            list.accounts = db.Account.Include("Roles"); //tham chiếu khóa ngoại bảng Roles
            list.roles = db.Roles.ToList();
            return View(list);
        }
        public ActionResult ThemND()
        {
            var list = new MultipleData();
            list.accounts = db.Account.Include("Roles");
            list.roles = db.Roles.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ThemND(Account nd)
        {
            db.Account.Add(nd);
            db.SaveChanges();
            return RedirectToAction("NguoiDung");
        }
        public ActionResult SuaND(int id)
        {
            var viewmodel = new MultipleData();
            viewmodel.accounts = db.Account.Where(nd => nd.id == id).ToList();
            viewmodel.roles = db.Roles.ToList();
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult SuaND(Account nd)
        {
            var existingND = db.Account.FirstOrDefault(item => item.id == nd.id);
            if (existingND != null)
            {
                existingND.id_Role = nd.id_Role;
                existingND.Username = nd.Username;
                existingND.TenNV = nd.TenNV;
                existingND.SDT = nd.SDT;
                existingND.Email = nd.Email;
                existingND.Pass = nd.Pass;

                db.SaveChanges();
            }

            return RedirectToAction("NguoiDung");
        }
        public ActionResult XoaND(int id)
        {
            var nguoidung = db.Account.Find(id);
            if (nguoidung != null)
            {
                db.Account.Remove(nguoidung);
                db.SaveChanges();

            }
            return RedirectToAction("NguoiDung");
        }
        [HttpPost]
        public ActionResult TimKiemND(string search)
        {
            var list = new MultipleData();

            // Thực hiện tìm kiếm dựa trên chuỗi `search`
            list.accounts = db.Account.Where(nd => nd.TenNV.Contains(search)).ToList();

            // Lấy danh sách vai trò (Roles) để sử dụng trong view
            list.roles = db.Roles.ToList();

            ViewBag.Search = search; // Đặt chuỗi tìm kiếm vào ViewBag để hiển thị trong view

            return View("NguoiDung", list);
        }

    }
}
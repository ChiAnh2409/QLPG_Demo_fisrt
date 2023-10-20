using QLPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLPG.Controllers
{
    public class GoiTapController : Controller
    {
        private QLPG1Entities db = new QLPG1Entities();
        //tạo biến database để lấy dữ liệu
        // GET: GoiTap
        
        public ActionResult GoiTap()
        {
            List<GoiTap> list = new List<GoiTap>();
            list = db.GoiTap.ToList();
            return View(list);
        }
        public ActionResult ThemGT() 
        {
            // Kiểm tra quyền của người dùng từ phiên và ủy quyền "Admin" mới có thể thêm mới
            if (Session["Role"] != null && Session["Role"].ToString().Equals("Admin"))
            {
                // Người dùng có quyền "Admin", cho phép thêm mới
                return View();
            }
            else
            {
                // Người dùng không có quyền "Admin", không cho phép thêm mới
                return RedirectToAction("GoiTap"); // Hoặc chuyển hướng đến trang khác
            }
        }
        [HttpPost]
        public ActionResult ThemGT(GoiTap gt) 
        {
            db.GoiTap.Add(gt); 
            db.SaveChanges();
            return RedirectToAction("GoiTap");
        }
        public ActionResult SuaGT(int id)
        {
            GoiTap gt = db.GoiTap.Find(id);
              if (Session["Role"] != null && Session["Role"].ToString().Equals("Admin"))
                 {  return View(gt); }
              else
                 { return RedirectToAction("GoiTap"); }
        }
        [HttpPost]
        public ActionResult SuaGT(GoiTap gt)
        {
            db.Entry(gt).State = System.Data.Entity.EntityState.Modified; 
            db.SaveChanges();
            return RedirectToAction("GoiTap");
        }

        [HttpPost]
        public ActionResult XoaGT(int id)
        {
              if (Session["Role"] != null && Session["Role"].ToString().Equals("Admin"))
                {
                  GoiTap gt = db.GoiTap.Find(id);
                  if (gt != null)
                   {
                      db.GoiTap.Remove(gt);
                      db.SaveChanges();
                   }

                  return RedirectToAction("GoiTap");
            }
              else
                { return RedirectToAction("GoiTap"); }
        }


    }
}
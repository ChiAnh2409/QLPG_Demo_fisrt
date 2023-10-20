using QLPG.Models;
using QLPG.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QLPG.Controllers
{
    public class HoiVienController : Controller
    {
        private QLPG1Entities db = new QLPG1Entities();
        //tạo biến database để lấy dữ liệu
        // GET: HoiVien
        public ActionResult HoiVien()
        {
            var list = new MultipleData();
            list.hoiViens = db.HoiVien.Include("ThanhVien"); //tham chiếu khóa ngoại bảng Thành viên
            list.vien =db.ThanhVien.ToList();
            return View(list);

        }
        public ActionResult ThemHV() 
        {
            var list = new MultipleData();
            list.hoiViens = db.HoiVien.Include("ThanhVien");
            list.vien = db.ThanhVien.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ThemHV(HoiVien hv)
        {
            String HinhAnh = "";

            HttpPostedFileBase file = Request.Files["HinhAnh"];
            if (file != null && file.FileName != "")
            {
                String serverPath = HttpContext.Server.MapPath("~/assets/img/team");
                String filePath = serverPath + "/" + file.FileName;
                file.SaveAs(filePath);
                HinhAnh = file.FileName;
            }
            hv.HinhAnh = HinhAnh;
            db.HoiVien.Add(hv);
            hv.TinhTrang = true;
            DateTime now = DateTime.Now;
            hv.NgayGiaNhap = now;
            db.SaveChanges();
            return RedirectToAction("HoiVien");
        }
        public ActionResult SuaHV(int id)
        {
            var viewmodel = new MultipleData();
            viewmodel.hoiViens = db.HoiVien.Where(hv => hv.id_HV == id).ToList();
            viewmodel.vien = db.ThanhVien.ToList();
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult SuaHV(HoiVien hv)
        {
            HoiVien existingHoiVien = db.HoiVien.Find(hv.id_HV);
            if (existingHoiVien != null)
            {
                existingHoiVien.id_TV = hv.id_TV;
                existingHoiVien.NgaySinh = hv.NgaySinh;
                existingHoiVien.CCCD = hv.CCCD;
                existingHoiVien.TinhTrang = hv.TinhTrang;

                // Kiểm tra và lưu hình ảnh nếu có
                HttpPostedFileBase file = Request.Files["HinhAnh"];
                if (file != null && file.FileName != "")
                {
                    String HinhAnh = file.FileName;
                    String serverPath = HttpContext.Server.MapPath("~/assets/img/team");
                    String filePath = serverPath + "/" + HinhAnh;
                    file.SaveAs(filePath);
                    existingHoiVien.HinhAnh = HinhAnh;
                }

                db.SaveChanges();
            }

            return RedirectToAction("HoiVien");
        }
        public ActionResult XoaHV(int id)
        {
            var HoiVien = db.HoiVien.Find(id);
            if (HoiVien != null)
            {
                db.HoiVien.Remove(HoiVien);
                db.SaveChanges();

            }
            return RedirectToAction("HoiVien");
        }
        [HttpPost] //Tìm kiếm bằng tên trong bảng hội viên nhưng tham chiếu bằng id_TV trong bảng thành viên
        public ActionResult TimKiemHV(string search)
        {
            var list = new MultipleData();
            list.hoiViens = db.HoiVien.Include("ThanhVien").Where(hv => hv.ThanhVien.TenTV.Contains(search)).ToList();
            list.vien = db.ThanhVien.ToList();

            return View("HoiVien", list);
        }


    }
}
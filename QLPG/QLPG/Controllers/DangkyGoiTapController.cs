using QLPG.Models;
using QLPG.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLPG.Controllers
{
    public class DangkyGoiTapController : Controller
    {
        private QLPG1Entities db = new QLPG1Entities();
        //tạo biến database để lấy dữ liệu
        // GET: DangkyGoiTap
        public ActionResult DKGT()
        {
            var list = new MultipleData();
            list.chiTietDK_= db.ChiTietDK_GoiTap.Include("GoiTap");  //tham chiếu khóa ngoại 2 bảng gói tập và hội viên
            list.chiTietDK_ = db.ChiTietDK_GoiTap.Include("HoiVien");
            list.goiTap = db.GoiTap.ToList();
            list.hoiViens = db.HoiVien.ToList();
            return View(list);
        }
        public ActionResult ThemDKGT()
        {
            var list = new MultipleData();
            list.chiTietDK_ = db.ChiTietDK_GoiTap.Include("GoiTap");
            list.chiTietDK_ = db.ChiTietDK_GoiTap.Include("HoiVien");
            list.goiTap = db.GoiTap.ToList();
            list.hoiViens = db.HoiVien.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ThemDKGT(ChiTietDK_GoiTap dkgt)
        {
            db.ChiTietDK_GoiTap.Add(dkgt);
            db.SaveChanges();
            return RedirectToAction("DKGT");
        }
        public ActionResult SuaDKGT(int id)
        {
            var viewmodel = new MultipleData();
            viewmodel.chiTietDK_ = db.ChiTietDK_GoiTap.Where(dkgt => dkgt.id_CTDKGoiTap == id).ToList();
            viewmodel.goiTap = db.GoiTap.ToList();
            viewmodel.hoiViens = db.HoiVien.ToList();
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult SuaDKGT(ChiTietDK_GoiTap dkgt)
        {
            var existingDangkyGoiTap = db.ChiTietDK_GoiTap.FirstOrDefault(item => item.id_CTDKGoiTap == dkgt.id_CTDKGoiTap);
            if (existingDangkyGoiTap != null)
            {
                existingDangkyGoiTap.id_GT = dkgt.id_GT;
                existingDangkyGoiTap.id_HV = dkgt.id_HV;
                existingDangkyGoiTap.NgayBatDau = dkgt.NgayBatDau;
                existingDangkyGoiTap.NgayKetThuc= dkgt.NgayKetThuc;
                existingDangkyGoiTap.ThanhTien = dkgt.ThanhTien;

                db.SaveChanges();
            }

            return RedirectToAction("DKGT");
        }
        public ActionResult XoaDKGT(int id)
        {
            var DangkyGoiTap = db.ChiTietDK_GoiTap.Find(id);
            if (DangkyGoiTap != null)
            {
                db.ChiTietDK_GoiTap.Remove(DangkyGoiTap);
                db.SaveChanges();

            }
            return RedirectToAction("DKGT");
        }
        [HttpPost]
        public ActionResult TimKiemDKGT(string search)
        {
            var list = new MultipleData();

            // Tìm kiếm theo tên thành viên trong bảng hội viên
            var hoiViensResults = db.HoiVien.Where(hv => hv.ThanhVien.TenTV.Contains(search)).ToList();
            var hoiVienIds = hoiViensResults.Select(hv => hv.id_HV).ToList();

            // Lấy danh sách ChiTietDK_GoiTap dựa trên các kết quả tìm kiếm
            list.chiTietDK_ = db.ChiTietDK_GoiTap
                .Include("HoiVien")
                .Include("GoiTap")
                .Where(dkgt => hoiVienIds.Contains(dkgt.HoiVien.id_HV))
                .ToList();

            list.goiTap = db.GoiTap.ToList();
            list.hoiViens = db.HoiVien.ToList();
            ViewBag.Search = search; // Đặt tên cần tìm kiếm vào ViewBag để hiển thị trong view
            return View("DKGT", list);
        }

    }
}
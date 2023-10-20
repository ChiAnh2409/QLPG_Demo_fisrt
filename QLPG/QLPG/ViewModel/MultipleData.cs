using QLPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLPG.ViewModel
{
    public class MultipleData
    {
        // Các thuộc tính tương ứng với các bảng dữ liệu
        public IEnumerable<HoiVien> hoiViens { get; set; }
        public IEnumerable<ThanhVien> vien { get; set; }
        public IEnumerable<BuoiTap> buoiTap { get; set; } 
        public IEnumerable<ChiTietDK_GoiTap> chiTietDK_ { get; set; }
        public IEnumerable<GoiTap> goiTap { get; set; }
        public IEnumerable<Account> accounts { get; set; }
        public IEnumerable<Roles> roles { get; set; } 

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webphukien.Models
{
    
    public class Giohang
    {
        DangkyDataContext data = new DangkyDataContext();
        public int iMaSP { set; get; }
        public string sTenSanPham { set; get; }
        public string sAnhbia { set; get; }
        public Double dGiaban { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dGiaban; }

        }
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;   
            SANPHAM sanpham = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            sTenSanPham = sanpham.TenSP;
            sAnhbia = sanpham.Anhbia;
            dGiaban = double.Parse(sanpham.Giaban.ToString());
            iSoluong = 1;
        }
    }
}
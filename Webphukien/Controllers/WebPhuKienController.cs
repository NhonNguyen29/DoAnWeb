using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Webphukien.Models;

namespace Webphukien.Controllers
{
    public class WebPhuKienController : Controller
    {
        DangkyDataContext db = new DangkyDataContext();
       
        private List<SANPHAM> laygiasanpham(int count)
        {
            // sap xep dan theo gia
            return db.SANPHAMs.OrderByDescending(gia => gia.Giaban).Take(count).ToList();

        }
        public ActionResult Index()
        {
            /// lay 5 sp gia cao nhat
            var giacaotoithap = laygiasanpham(5);
            return View(giacaotoithap);
        }
        public ActionResult cacloaiphukien()
        {
            var loaiphukien = from TenPhuKien in db.LOAIPHUKIENs select TenPhuKien; 

            return PartialView(loaiphukien);
        }

        public ActionResult thuonghieu()
        {
            var tenthuonghieu = from TenTH in db.THUONGHIEUs select TenTH;

            return PartialView(tenthuonghieu);
        }

        public ActionResult SanPhamThuongHieu(int id)
        {
            var spth = from sp in db.SANPHAMs where sp.MaTH==id select sp;
            return View(spth);
        }   

        public ActionResult SanPhamTheoPhuKien(int id)
        {
            var sppk = from sp in db.SANPHAMs where sp.MaLPK==id select sp; 
            return View(sppk);
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var ctsp = from sp in db.SANPHAMs where sp.MaSP == id select sp;
            return View(ctsp.Single());
        }
    }
}
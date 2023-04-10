using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Webphukien.Models;

using PagedList;
using PagedList.Mvc;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace Webphukien.Controllers
{

    public class WebPhuKienController : Controller
    {
        DangkyDataContext db = new DangkyDataContext();
        public ActionResult Index(int? page)
        {
         
                int pageSize = 3;
                int pageNum = (page ?? 1);



                ///// lay 5 sp gia cao nhat
                var giacaotoithap = laygiasanpham(10);
                return View(giacaotoithap.ToPagedList(pageNum, pageSize));
            
         
        }
        private List<SANPHAM> laygiasanpham(int count)
        {
            // sap xep dan theo gia
            return db.SANPHAMs.OrderByDescending(gia => gia.Giaban).Take(count).ToList();

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
            var sanpham = from sp in db.SANPHAMs where sp.MaSP == id select sp;
            return View(sanpham.Single());
        }

        public ActionResult Search(string searchString)
        {

            var links = from l in db.SANPHAMs
                        select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.TenSP.Contains(searchString));
            }

            return View(links);
        }

    }
}
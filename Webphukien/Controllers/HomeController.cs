using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Schema;
using Webphukien.Models;

using PagedList;
using PagedList.Mvc;
using System.Net.NetworkInformation;

namespace Webphukien.Controllers
{
    public class HomeController : Controller
    {
        DangkyDataContext db = new DangkyDataContext();
        
        public ActionResult Index(int ? page)
        {
           
            int pageSize = 3;
            int pageNum = (page ?? 1);
            var giacaotoithap = laygiasanpham(10);
            return View(giacaotoithap.ToPagedList(pageNum, pageSize));
        } 

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private List<SANPHAM> laygiasanpham(int count)
        {
            // sap xep dan theo gia
            return db.SANPHAMs.OrderByDescending(gia => gia.Giaban).Take(count).ToList();

        }


        public ActionResult SanPhamTheoPhuKien(int id)
        {
            var sppk = from sp in db.SANPHAMs where sp.MaLPK == id select sp;
            return View(sppk);
        }

       
    }
}
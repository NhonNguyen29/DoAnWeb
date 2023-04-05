using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Schema;
using Webphukien.Models;

namespace Webphukien.Controllers
{
    public class HomeController : Controller
    {
        DangkyDataContext db = new DangkyDataContext();
        public ActionResult Index(/*int MaSP=0, string TimTuKhoa=""*/)
        {
            return View();
        } /*return View(db.SANPHAMs.Where(x => x.TenSP.Contains(TimTuKhoa) || TimTuKhoa == null).ToList());*/

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
    }
}
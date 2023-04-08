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
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace Webphukien.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DangkyDataContext db = new DangkyDataContext();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SANPHAM(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult QLHDT(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.HANGDIENTHOAIs.ToList().OrderBy(n => n.MaHDT).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult QLPK(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.LOAIPHUKIENs.ToList().OrderBy(n => n.MaLPK).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult QLTH(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.THUONGHIEUs.ToList().OrderBy(n => n.MaTH).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult QLDH(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection cl)
        {
            var tendn = cl["username"];
            var matkhau = cl["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = " Phải nhập mật khẩu";

            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewData["LoiTK"] = " Tên đăng nhập hoặc mật khẩu không đúng !";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Admin");
        }
        // THEM
        [HttpGet]
        public ActionResult ThemHDT()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemHDT(HANGDIENTHOAI hdt)
        {
            db.HANGDIENTHOAIs.InsertOnSubmit(hdt);
            db.SubmitChanges();
            return RedirectToAction("QLHDT");
        }
        [HttpGet]
        public ActionResult ThemLPK()
        {
            ViewBag.MaHDT = new SelectList(db.HANGDIENTHOAIs.ToList().OrderBy(n => n.TenHDT), "MaHDT", "TenHDT");
            return View();
        }
        [HttpPost]
        public ActionResult ThemLPK(LOAIPHUKIEN pk)
        {
            ViewBag.MaHDT = new SelectList(db.HANGDIENTHOAIs.ToList().OrderBy(n => n.TenHDT), "MaHDT", "TenHDT");
            db.LOAIPHUKIENs.InsertOnSubmit(pk);
            db.SubmitChanges();
            return RedirectToAction("QLPK");
        }
        [HttpGet]
        public ActionResult ThemTH()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemTH(THUONGHIEU th)
        {
            db.THUONGHIEUs.InsertOnSubmit(th);
            db.SubmitChanges();
            return RedirectToAction("QLTH");
        }
        [HttpGet]
        public ActionResult ThemDH()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            return View();
        }
        [HttpPost]
        public ActionResult ThemDH(DONDATHANG dh)
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            db.DONDATHANGs.InsertOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("QLDH");
        }
        [HttpGet]
        public ActionResult Themmoi()
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLPK = new SelectList(db.LOAIPHUKIENs.ToList().OrderBy(n => n.TenPhuKien), "MaLPK", "TenPhuKien");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoi(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLPK = new SelectList(db.LOAIPHUKIENs.ToList().OrderBy(n => n.TenPhuKien), "MaLPK", "TenPhuKien");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại !";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.Anhbia = filename;
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham");
            }
        }
        //CHI TIET
        public ActionResult Chitiet(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        public ActionResult ChitietDH(int id)
        {
            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dh.MaDonHang;
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }
        // XOA
        public ActionResult XoaHDT(int id)
        {
            HANGDIENTHOAI hdt = db.HANGDIENTHOAIs.SingleOrDefault(n => n.MaHDT == id);
            ViewBag.MaHDT = hdt.MaHDT;
            if (hdt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hdt);
        }
        [HttpPost, ActionName("XoaHDT")]
        public ActionResult XacNhanXoaHDT(int id)
        {
            HANGDIENTHOAI hdt = db.HANGDIENTHOAIs.SingleOrDefault(n => n.MaHDT == id);
            ViewBag.MaHDT = hdt.MaHDT;
            if (hdt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.HANGDIENTHOAIs.DeleteOnSubmit(hdt);
            db.SubmitChanges();
            return RedirectToAction("QLHDT");
        }
        public ActionResult XoaLPK(int id)
        {
            LOAIPHUKIEN pk = db.LOAIPHUKIENs.SingleOrDefault(n => n.MaLPK == id);
            ViewBag.MaLPK = pk.MaLPK;
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(pk);
        }
        [HttpPost, ActionName("XoaLPK")]
        public ActionResult XacNhanXoaLPK(int id)
        {

            LOAIPHUKIEN pk = db.LOAIPHUKIENs.SingleOrDefault(n => n.MaLPK == id);
            ViewBag.MaLPK = pk.MaLPK;
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LOAIPHUKIENs.DeleteOnSubmit(pk);
            db.SubmitChanges();
            return RedirectToAction("QLPK");
        }
        public ActionResult XoaTH(int id)
        {
            THUONGHIEU th = db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = th.MaTH;
            if (th == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(th);
        }
        [HttpPost, ActionName("XoaTH")]
        public ActionResult XacNhanXoaTH(int id)
        {

            THUONGHIEU th = db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = th.MaTH;
            if (th == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.THUONGHIEUs.DeleteOnSubmit(th);
            db.SubmitChanges();
            return RedirectToAction("QLTH");
        }

        public ActionResult XoaDH(int id)
        {
            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dh.MaDonHang;
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }
        [HttpPost, ActionName("XoaDH")]
        public ActionResult XacNhanXoaDH(int id)
        {

            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dh.MaDonHang;
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DONDATHANGs.DeleteOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("QLDH");
        }

        public ActionResult Xoa(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoaSP(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Sanpham");
        }
        // SUA

        [HttpGet]
        public ActionResult SuaHDT(int id)
        {
            HANGDIENTHOAI hdt = db.HANGDIENTHOAIs.SingleOrDefault(n => n.MaHDT == id);
            if (hdt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hdt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaHDT(int id, HANGDIENTHOAI hdt)
        {
            hdt = db.HANGDIENTHOAIs.SingleOrDefault(n => n.MaHDT == id);
            UpdateModel(hdt);
            db.SubmitChanges();
            return RedirectToAction("QLHDT");
        }


        [HttpGet]
        public ActionResult SuaLPK(int id)
        {
            LOAIPHUKIEN pk = db.LOAIPHUKIENs.SingleOrDefault(n => n.MaLPK == id);
            if (pk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaHDT = new SelectList(db.HANGDIENTHOAIs.ToList().OrderBy(n => n.TenHDT), "MaHDT", "TenHDT");
            return View(pk);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaLPK(int id, LOAIPHUKIEN pk)
        {
            ViewBag.MaHDT = new SelectList(db.HANGDIENTHOAIs.ToList().OrderBy(n => n.TenHDT), "MaHDT", "TenHDT");
            pk = db.LOAIPHUKIENs.SingleOrDefault(n => n.MaLPK == id);
            UpdateModel(pk);
            db.SubmitChanges();
            return RedirectToAction("QLPK");
        }

        public ActionResult SuaTH(int id)
        {
            THUONGHIEU th = db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            if (th == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(th);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaTH(int id, THUONGHIEU th)
        {
            th = db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            UpdateModel(th);
            db.SubmitChanges();
            return RedirectToAction("QLTH");
        }


        public ActionResult SuaDH(int id)
        {
            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            return View(dh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaDH(int id, DONDATHANG dh)
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs.ToList().OrderBy(n => n.HoTen), "MaKH", "HoTen");
            dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            UpdateModel(dh);
            db.SubmitChanges();
            return RedirectToAction("QLDH");
        }

        [HttpGet]
        public ActionResult Sua(int id)
        {

            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);

            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLPK = new SelectList(db.LOAIPHUKIENs.ToList().OrderBy(n => n.TenPhuKien), "MaLPK", "TenPhuKien");
            return View(sp);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(HttpPostedFileBase fileupload, int id)

        {

            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLPK = new SelectList(db.LOAIPHUKIENs.ToList().OrderBy(n => n.TenPhuKien), "MaLPK", "TenPhuKien");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại !";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
                    sp.Anhbia = filename;
                    UpdateModel(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham");
            }
        }
    }
}
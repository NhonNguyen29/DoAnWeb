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
     
        public ActionResult SANPHAM(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber,pageSize));
        }
      
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login (FormCollection cl)
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
                    ViewData["LoiTK"]= " Tên đăng nhập hoặc mật khẩu không đúng !";
            }
            return View();
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
            if(fileupload==null)
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
        public ActionResult Chitiet(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        public ActionResult Xoa(int id)
        {
            SANPHAM sp =db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost,ActionName("Xoa")]
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
        public ActionResult Sua( HttpPostedFileBase fileupload, int id)

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



        ////
        ///  [HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    if (Session["Taikhoanadmin"] == null)
        //    {
        //        ViewBag.ThongBao = "Bạn cần đăng nhập trước khi sử dụng chức năng chỉnh sửa!";
        //        return RedirectToAction("Login", "Admin");
        //    }
        //    else
        //    {
        //        XE xe = data.XEs.SingleOrDefault(n => n.MaXe == id);
        //        ViewBag.MaLoai = new SelectList(data.LOAIXEs.OrderBy(n => n.TenLoaiXe).ToList(), "MaLoai", "TenLoaiXe", xe.MaLoai);
        //        ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.OrderBy(n => n.TenNCC).ToList(), "MaNCC", "TenNCC", xe.MaNCC);
        //        ViewBag.MaMau = new SelectList(data.MAUXEs.OrderBy(n => n.TenMauXe).ToList(), "MaMau", "TenMauXe", xe.MaMau);
        //        return View(xe);
        //    }
        //}

        //[HttpPost, ActionName("Edit")]

        //public ActionResult ConfirmEdit(int id)
        //{
        //    if (Session["Taikhoanadmin"] == null)
        //    {
        //        ViewBag.ThongBao = "Bạn cần đăng nhập trước khi sử dụng chức năng chỉnh sửa!";
        //        return RedirectToAction("Login", "Admin");
        //    }
        //    else
        //    {
        //        XE xe = data.XEs.SingleOrDefault(n => n.MaXe == id);
        //        UpdateModel(xe);
        //        data.SubmitChanges();
        //        return RedirectToAction("Xe", "QLXe");
        //    }
        //}


    }
}
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
    public class UserController : Controller
       
    {

        DangkyDataContext db = new DangkyDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection cl, KHACHHANG kh)
        {
            var hoten = cl["HoTen"];
            var taikhoan = cl["Taikhoan"];
            var matkhau = cl["Matkhau"];
            var nhaplaimatkhau = cl["Nhaplaimatkhau"];
            var dienthoai = cl["Dienthoai"];
            var email = cl["Email"];
            var diachi = cl["Diachi"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", cl["Ngaysinh"]);
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi_1"]= "Bạn chưa nhập họ tên";
            }
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi_2"] = "Bạn chưa nhập tài khoản";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi_3"] = "Bạn chưa nhập mật khẩu";
            }
            if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi_4"] = "Bạn chưa nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi_5"] = "Bạn chưa nhập số điện thoại";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi_6"] = "Bạn chưa nhập email";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi_7"] = "Bạn chưa nhập địa chỉ";
            }
            else
            {
                kh.HoTen=hoten;
                kh.Taikhoan=taikhoan;
                kh.Matkhau=matkhau;
                kh.DienthoaiKH=dienthoai;
                kh.Email=email;
                kh.DiachiKH=diachi;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
               
            }
            return DangKy();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection cl)
        {
            var taikhoan = cl["Taikhoan"];
            var matkhau = cl["Matkhau"];
            if(String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi_1"] ="Bạn chưa nhập tên tài khoản!";
            }
            else if(String.IsNullOrEmpty(matkhau))
                {
                    ViewData["Loi_2"] = "Bạn chưa nhập mật khẩu!";
                }
                else
                {
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n=>n.Taikhoan == taikhoan && n.Matkhau== matkhau);
                    if(kh != null)
                    {
                        ViewBag.Thongbao = "Đăng nhập thành công";
                        Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                    else
                    
                        ViewBag.Thongbao = "Tên tài khoản hoặc mật khẩu không đúng!";
                    
                }
            return View();
        }

        
             
    }
}
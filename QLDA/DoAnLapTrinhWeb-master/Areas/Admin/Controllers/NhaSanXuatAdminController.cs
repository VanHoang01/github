using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Electro.Models;
using PagedList;
namespace Electro.Areas.Admin.Controllers
{
    public class NhaSanXuatAdminController : Controller
    {
        // GET: Admin/NhaSanXuatAdmin
        ElectroDbContext db = new ElectroDbContext();
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            int iPageNum = (page ?? 1);
            var lst = db.NhaSanXuats.ToList().OrderBy(n => n.MaNSX).ToPagedList(iPageNum, 10);
            return View(lst);
        }
        [HttpGet]
        public ActionResult Sua(int MaNSX, string url)
        {
            ViewBag.Url = Url;
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == MaNSX);
            return View(nsx);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(FormCollection f)
        {
            int MaNSX = int.Parse(f["MaNSX"]);
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == MaNSX);
            nsx.TenNSX = f["TenNSX"];
            nsx.ThongTin = f["ThongTin"].Replace("<p>", "").Replace("</p>", "\n");

            db.SaveChanges();
            return RedirectToAction("Index", "NhaSanXuatAdmin");
        }
        public ActionResult ChiTietDonHang(int MaNSX, string url)
        {
            ViewBag.Url = url;
            var lstChiTietDonHang = db.NhaSanXuats.Select(n => n).Where(n => n.MaNSX == MaNSX);
            return View(lstChiTietDonHang);
        }
        public ActionResult SanPham(int MaNSX, int ? page, string url)
        {
            ViewBag.Url = url;
            ViewBag.MaNSX = MaNSX;
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            var lst = db.SanPhams.Where(n => n.MaNSX == MaNSX).ToList().OrderBy(n => n.MaSP).ToPagedList(iPageNum, iPageSize);
            return View(lst);
        }

        public ActionResult Them(string url)
        {
            ViewBag.Url = url;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Them(FormCollection f)
        {
            var nsx = new NhaSanXuat();
            nsx.TenNSX = f["TenNSX"];
            nsx.ThongTin = f["ThongTin"].Replace("<p>", "").Replace("</p>", "\n");
            db.NhaSanXuats.Add(nsx);
            db.SaveChanges();
            return RedirectToAction("Index", "NhaSanXuatAdmin");
        }
    }

    }
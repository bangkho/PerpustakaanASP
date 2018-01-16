using Perpustakaan.Models.EntityManager;
using Perpustakaan.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Perpustakaan.Security;

namespace Perpustakaan.Controllers
{
    public class BukuController : Controller
    {
        // GET: Buku
        public ActionResult AddBuku()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddBuku(BukuView bv)
        {
            if (ModelState.IsValid)
            {
                BukuManager BK = new BukuManager();
                BK.AddBuku(bv);
                return RedirectToAction("Welcome", "Home");
            }
            return View();
        }
        public ActionResult ManageBukuPartial(string status = "")
        {
            //if (User.Identity.IsAuthenticated)
            //{
            string loginName = User.Identity.Name;
            BukuManager BM = new BukuManager();
            BukuDataView BDV = new BukuDataView();
            BDV.BukuProfile = BM.GetBukuData();
            string message = string.Empty;
            if (status.Equals("update"))
                message = "Update Successful";
            else if (status.Equals("delete"))
                message = "Delete Successful";
            ViewBag.Message = message;
            return PartialView(BDV);
            //}
            // return RedirectToAction("Index", "Home");
        }
        public ActionResult UpdateBukuData(int bukuID, string isbn, string
judul, string penulis, string penerbit, int tahun,int stok)
        {
            BukuView BV = new BukuView();
            BV.id_buku = bukuID;
            BV.isbn = isbn;
            BV.judul = judul;
            BV.penulis = penerbit;
            BV.penerbit = penerbit;
            BV.tahun = tahun;
            BV.stok = stok;
            BukuManager BM = new BukuManager();
            BM.UpdateBuku(BV);
            return Json(new { success = true });
        }
        public ActionResult DeleteBuku(int bukuID)
        {
            BukuManager BM = new BukuManager();
            BM.DeleteBuku(bukuID);
            return Json(new { success = true });
        }
        public ActionResult Perubahan()
        {
            return View();
        }
        [AuthorizeRoles("Librarian")]
        public ActionResult DaftarBuku()
        {
            return View();
        }
    }
}
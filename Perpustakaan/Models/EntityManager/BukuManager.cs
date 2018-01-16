using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Perpustakaan.Models.DB;
using Perpustakaan.Models.ViewModel;


namespace Perpustakaan.Models.EntityManager
{
    public class BukuManager
    {
        public void AddBuku(BukuView bv)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                buku bk = new buku();
                bk.isbn = bv.isbn;
                bk.judul = bv.judul;
                bk.penulis = bv.penulis;
                bk.penerbit = bv.penerbit;
                bk.tahun = bv.tahun;
                bk.stok = bv.stok;
                db.bukus.Add(bk);
                db.SaveChanges();
            }
        }
        public void UpdateBuku(BukuView bv)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                buku bk = db.bukus.Find(bv.id_buku);
                bk.isbn = bv.isbn;
                bk.judul = bv.judul;
                bk.penulis = bv.penulis;
                bk.penerbit = bv.penerbit;
                bk.tahun = bv.tahun;
                bk.stok = bv.stok;
                db.bukus.Add(bk);
                db.SaveChanges();
            }
        }
        public List<BukuView> GetBukuData()
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                var buku = db.bukus.Select(o => new BukuView
                {
                    id_buku = o.id_buku,
                    isbn = o.isbn,
                    judul = o.judul,
                    penulis = o.penulis,
                    penerbit = o.penerbit,
                    tahun = o.tahun,
                    stok = o.stok

                }).ToList();
                return buku;
            }
        }
        public void DeleteBuku(int bukuID)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var bk = db.bukus.Where(o => o.id_buku == bukuID);
                        if (bk.Any())
                        {
                            db.bukus.Remove(bk.FirstOrDefault());
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}
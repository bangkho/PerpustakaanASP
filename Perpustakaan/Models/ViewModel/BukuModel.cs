using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Perpustakaan.Models.ViewModel
{
    public class BukuView
    {
        [Key]
        public int id_buku { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "ISBN")]
        public string isbn { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Judul")]
        public string judul { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Penulis")]
        public string penulis { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Penerbit")]
        public string penerbit { get; set; }
        [Display(Name = "Tahun")]
        public int? tahun { get; set; }
        [Display(Name = "Stok")]
        public int? stok { get; set; }
    }
    public class BukuDataView
    {
        public IEnumerable<BukuView> BukuProfile { get; set; }
    }
}
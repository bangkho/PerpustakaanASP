//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Perpustakaan.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class laporan
    {
        public int id_laporan { get; set; }
        public Nullable<int> id_buku { get; set; }
        public Nullable<int> id_pelanggan { get; set; }
        public string keterangan { get; set; }
        public Nullable<System.DateTime> tgl_pinjam { get; set; }
        public Nullable<System.DateTime> tgl_kembali { get; set; }
        public Nullable<int> saldo { get; set; }
    
        public virtual buku buku { get; set; }
        public virtual pelanggan pelanggan { get; set; }
    }
}

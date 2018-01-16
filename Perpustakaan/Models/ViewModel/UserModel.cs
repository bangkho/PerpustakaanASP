using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.ViewModel
{
    public class UserSignUpView
    {
        [Key]
        public int SYSUserID { get; set; }
        public int LookUpRoleID { get; set; }
        [Display(Name = "Member Type")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "*Isi Username")]
        [Display(Name = "Username")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*Isi Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*Isi Nama Depan")]
        [Display(Name = "Nama Depan")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Isi Nama Belakang")]
        [Display(Name = "Nama Belakang")]
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
    public class UserLoginView
    {
        [Key]
        public int SYSUserID { get; set; }
        [Required(ErrorMessage = "*Isikan Username")]
        [Display(Name = "Username")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*Isikan Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }    public class UserProfileView
    {
        [Key]
        public int SYSUserID { get; set; }
        public int LookUpRoleID { get; set; }
        [Display(Name = "Member Type")]
        public string RoleName { get; set; }
        public bool? IsRoleActive { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Username")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Nama Depan")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Nama Belakang")]
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
    public class LookUpAvailableRole
    {
        [Key]
        public int LookUpRoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
    public class Gender
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class UserRoles
    {
        public int? SelectedRoleID { get; set; }
        public IEnumerable<LookUpAvailableRole> UserRoleList { get; set; }
    }
    public class UserGender
    {
        public string SelectedGender { get; set; }
        public IEnumerable<Gender> Gender { get; set; }
    }
    public class UserDataView
    {
        public IEnumerable<UserProfileView> UserProfile { get; set; }
        public UserRoles UserRoles { get; set; }
        public UserGender UserGender { get; set; }
    }
}
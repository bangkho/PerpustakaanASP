﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Perpustakaan.Models.DB;
using Perpustakaan.Models.ViewModel;
using System.Collections.Generic;

namespace Perpustakaan.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                SYSUser SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowMOdifiedDateTime = DateTime.Now;
                db.SYSUsers.Add(SU);
                db.SaveChanges();
                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;
                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();
                if (user.LookUpRoleID > 0)
                {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LookUpRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID :
                   1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID :
                   1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;
                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }
        public bool IsLoginNameExist(string loginName)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }
        public string GetUserPassword(string loginName)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                var user = db.SYSUsers.Where(o =>
               o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }
        public bool IsUserInRole(string loginName, string roleName)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                SYSUser SU = db.SYSUsers.Where(o =>
               o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (SU != null)
                {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LookUpRoles on q.LOOKUPRoleID equals
                               r.LookUpRoleID
                                where r.RoleName.Equals(roleName) &&
                               q.SYSUserID.Equals(SU.SYSUserID)
                                select r.RoleName;
                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }
                return false;
            }
        }
        public List<LookUpAvailableRole> GetAllRoles()
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                var roles = db.LookUpRoles.Select(o => new LookUpAvailableRole
                {
                    LookUpRoleID = o.LookUpRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();
                return roles;
            }
        }
        public int GetUserID(string loginName)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().SYSUserID;
            }
            return 0;
        }
        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                UserProfileView UPV;
                var users = db.SYSUsers.ToList();
                foreach (SYSUser u in db.SYSUsers)
                {
                    UPV = new UserProfileView();
                    UPV.SYSUserID = u.SYSUserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;
                    var SUP = db.SYSUserProfiles.Find(u.SYSUserID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }
                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LookUpRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LookUpRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }
                    profiles.Add(UPV);
                }
            }
            return profiles;
        }
        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<LookUpAvailableRole> roles = GetAllRoles();
            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;
            userID = GetUserID(loginName);
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID ==
               userID)?.FirstOrDefault().LOOKUPRoleID;
                userGender = db.SYSUserProfiles.Where(o => o.SYSUserID ==
               userID)?.FirstOrDefault().Gender;
            }
            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender { Text = "Male", Value = "M" });
            genders.Add(new Gender { Text = "Female", Value = "F" });
            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles
            {
                SelectedRoleID = userAssignedRoleID,
                UserRoleList
           = roles
            };
            UDV.UserGender = new UserGender
            {
                SelectedGender = userGender,
                Gender = genders
            };
            return UDV;
        }
        public void UpdateUserAccount(UserProfileView user)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        SYSUser SU = db.SYSUsers.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowMOdifiedDateTime = DateTime.Now;
                        db.SaveChanges();
                        var userProfile = db.SYSUserProfiles.Where(o => o.SYSUserID ==
                       user.SYSUserID);
                        if (userProfile.Any())
                        {
                            SYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.SYSUserID;
                            SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            SUP.Gender = user.Gender;
                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;
                            db.SaveChanges();
                        }
                        if (user.LookUpRoleID > 0)
                        {
                            var userRole = db.SYSUserRoles.Where(o => o.SYSUserID ==
                           user.SYSUserID);
                            SYSUserRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LookUpRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new SYSUserRole();
                                SUR.LOOKUPRoleID = user.LookUpRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRoles.Add(SUR);
                            }
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
        public void DeleteUser(int userID)
        {
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var SUR = db.SYSUserRoles.Where(o => o.SYSUserID == userID);
                        if (SUR.Any())
                        {
                            db.SYSUserRoles.Remove(SUR.FirstOrDefault());
                            db.SaveChanges();
                        }
                        var SUP = db.SYSUserProfiles.Where(o => o.SYSUserID == userID);
                        if (SUP.Any())
                        {
                            db.SYSUserProfiles.Remove(SUP.FirstOrDefault());
                            db.SaveChanges();
                        }
                        var SU = db.SYSUsers.Where(o => o.SYSUserID == userID);
                        if (SU.Any())
                        {
                            db.SYSUsers.Remove(SU.FirstOrDefault());
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
        public UserProfileView GetUserProfile(int userID)
        {
            UserProfileView UPV = new UserProfileView();
            using (PerpustakaanEntities db = new PerpustakaanEntities())
            {
                var user = db.SYSUsers.Find(userID);
                if (user != null)
                {
                    UPV.SYSUserID = user.SYSUserID;
                    UPV.LoginName = user.LoginName;
                    UPV.Password = user.PasswordEncryptedText;
                    var SUP = db.SYSUserProfiles.Find(userID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }
                    var SUR = db.SYSUserRoles.Find(userID);
                    if (SUR != null)
                    {
                        UPV.LookUpRoleID = SUR.LOOKUPRoleID;
                        UPV.RoleName = SUR.LookUpRole.RoleName;
                        UPV.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return UPV;
        }
    }
}
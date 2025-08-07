using Dapper;
using HSEMS.Models;
using Microsoft.Office.Interop.Word;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSEMS.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult ListUser()
        {
            var sessionuser = (Person)Session["user"];
            if (Session["user"] == null)
            {
                return RedirectToAction("Logout", "User", new { ReturnURL = Url.Action("UploadData", "SubmitForm") });
            }
            var getforms = Person.GetListOfUsers().OrderBy(x=>x.username);
            return View(getforms);
        }

        public ActionResult UserForm(int user_id = 0)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Logout", "User", new { ReturnURL = Url.Action("UploadData", "SubmitForm") });
            }
            
            var userHsems = new Person();

            if (user_id != 0)
            {
                userHsems = Person.GetDetailsOfUserForm(user_id);
            }

            var roleUser = Roles.GetRoles();
            var unitUser = UnitUser.GetUnit();
            var subunitUser = SubUnitUser.GetSubunit();
            var teamUser = TeamUser.GetTeam();
            ViewBag.roleUser = roleUser.Select(a => new SelectListItem
            {
                Text = a.role_name,
                Value = a.role_id.ToString(),
            });
            ViewBag.unitUser = unitUser.Select(a => new SelectListItem
            {
                Text = a.unit_name,
                Value = a.unit_id.ToString(),
            });
            ViewBag.subunitUser = subunitUser.Select(a => new SelectListItem
            {
                Text = a.subunit_name,
                Value = a.subunit_id.ToString(),
            });

            ViewBag.teamUser = teamUser.Select(a => new SelectListItem
            {
                Text = a.team_name,
                Value = a.team_id.ToString(),
            });
            return View(userHsems);
        }

        public ActionResult GetDetailsDocs()
        {
            return Json(new { message = "ok" });
        }

        [HttpPost]
        public ActionResult SaveUser(Person newuser)
        {
           
            if (newuser.user_id == 0)
            {
                var gettheexistinguser = Person.GetExistingUser(newuser.username);
                Person.AddUser(newuser);
            }
            else
            {
                Person.UpdateUser(newuser);
            }

            return RedirectToAction("ListUser");
        }

        public ActionResult login(string ReturnURL = "")
        {

            var getdetails = new Person();
            if (string.IsNullOrEmpty(ReturnURL))
            {
                getdetails.ReturnURL = "";
            }
            else
            {
                getdetails.ReturnURL = ReturnURL;
            }
            return View(getdetails);

        }

        public ActionResult Authenticate(Person data)
        {
            if (AuthenticateUser(data.username, data.password))
            {
                var user = GetUserDetails(data.username);
                Session["user"] = new
                {
                    Username = user.SamAccountName,
                    DisplayName = user.DisplayName,
                    Email = user.EmailAddress
                };

                if (string.IsNullOrEmpty(data.ReturnURL))
                {
                    var link = Url.Action("Index", "Home");
                    var result = new { link = link };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var result = new { link = data.ReturnURL };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }


            //var getdetailsofuser = Person.GetDetailsOfUser(data.username, data.password);
            //if (getdetailsofuser != null)
            //{
            //    Session["user"] = getdetailsofuser;

            //    if (string.IsNullOrEmpty(data.ReturnURL))
            //    {
            //        var link = Url.Action("Index", "Home");
            //        var result = new { link = link };
            //        return Json(result, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {

            //        var result = new { link = data.ReturnURL };
            //        return Json(result, JsonRequestBehavior.AllowGet);
            //    }



            //}
            //else
            //{
            //    var result = new { link = "" };
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}

        }




        public ActionResult Logout(string ReturnURL = "", string stop = "")
        {
            if (!String.IsNullOrEmpty(stop))
            {
                Session.RemoveAll();
                return RedirectToAction("login", new { ReturnURL });
            }

            Session.RemoveAll();
            return RedirectToAction("login", new { ReturnURL });
        }

        public bool AuthenticateUser(string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, "tnb.my"))
            {
                return context.ValidateCredentials(username, password);
            }
        }

        public UserPrincipal GetUserDetails(string username)
        {
            using (var context = new PrincipalContext(ContextType.Domain, "tnb.my"))
            {
                return UserPrincipal.FindByIdentity(context, username);
            }
        }

        //public void SignIn()
        //{
        //    if (!Request.IsAuthenticated)
        //    {

        //        HttpContext.GetOwinContext()
        //            .Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
        //                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        //    }
        //}

        //public void SignOut()
        //{

        //    HttpContext.GetOwinContext().Authentication.SignOut(
        //    OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        //}
    }
}
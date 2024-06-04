using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSEMS.Models;
using System.Data.Entity;
using Dapper;

namespace HSEMS.Controllers
{
    public class FormController : Controller
    {
        // GET: Form
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PEE_Form()
        {
            List<PEEForm> data = new List<PEEForm>();
            return View(data);

        }

        public ActionResult ListForms()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Logout", "User", new { ReturnURL = Url.Action("ListForms", "Form") });
            }


            return View();

        }

        public ActionResult DetailsOfForms(int listofforms)
        {
            ViewBag.FormID = listofforms;
            return View();
        }

        public ActionResult DownloadFile(string fileName)
        {
            // Get the full path to the file on the server
            string filePath = Server.MapPath("~/Template/" + fileName);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Return the file as a file result
                return File(filePath, MimeMapping.GetMimeMapping(fileName), fileName);
            }

            // If the file does not exist, you may want to handle the error differently (e.g., show a custom error page)
            return HttpNotFound("File not found");
        }

    }
}
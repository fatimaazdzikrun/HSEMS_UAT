using HSEMS.Models;
//using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSEMS.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			if (Session["user"] == null)
			{
				return RedirectToAction("Logout", "User", new { ReturnURL = Url.Action("Index", "Home") });
			}

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult OpenPdf(string fileName)
		{
			try
			{
				string pdfFilePath = Server.MapPath("~/Template/"+fileName);

				//Check if the file exists
				if (!System.IO.File.Exists(pdfFilePath))
				{
					return HttpNotFound("File error"); // Or another appropriate action
				}

				string mimeType = "application/pdf";
				//string fileName = "PIC.pdf";
				string ReportURL = "../Template/PIC.pdf";

				ViewBag.filess = pdfFilePath;
				byte[] FileBytes = System.IO.File.ReadAllBytes(pdfFilePath);

				//using (var stream = new MemoryStream())
				//{

				//	FileBytes = stream.ToArray();
				//}

				// use 2 parameters
				return File(FileBytes, "application/pdf");
				//return File(pdfFilePath, MimeMapping.GetMimeMapping(fileName), fileName);
			}
			catch (Exception ex)
			{
				return HttpNotFound(ex.Message);
			}

			



		}

		public ActionResult OpenPowerBI()
		{
			return View();
		}
	}
}

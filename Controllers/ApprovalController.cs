using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HSEMS.Models;
using Dapper;
using System.IO;
using OfficeOpenXml;

namespace HSEMS.Controllers
{
    public class ApprovalController : Controller
    {
        // GET: Approval
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListSubmittedForm()
        {

            var sessionuser = (Person)Session["user"];
            var getforms = new List<ApprovalForm>();
            if (sessionuser.employee_id == 10084975)
            {
                getforms = ApprovalForm.GetAllFormsSubmitted(sessionuser.dept_id, sessionuser.subunit_id);
            }
            else
            {
                getforms = ApprovalForm.GetAllFormsSubmitted(sessionuser.dept_id, sessionuser.subunit_id, sessionuser.assg_team);
            }
            
            return View(getforms);
        }

        public ActionResult AuditorListSubmittedForm()
        {

            var sessionuser = (Person)Session["user"];
            var getforms = ApprovalForm.GetAllFormsSubmitted();
            return View(getforms);
        }

        public ActionResult DownloadFile(int file_id)
        {
            var getfilename = FileUploads.GetFileDetails(file_id);


            // Get the full path to the file on the server
            string filePath = Server.MapPath("~/SavedFile/" + getfilename.FileNames);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Return the file as a file result
                return File(filePath, MimeMapping.GetMimeMapping(getfilename.FileNames), getfilename.FileNames);
            }

            // If the file does not exist, you may want to handle the error differently (e.g., show a custom error page)
            return HttpNotFound("File not found");
        }

        public ActionResult SaveUploadedFile(HttpPostedFileBase file, int file_id)
        {
            return RedirectToAction("ListSubmittedForm", "Approval");
        }

        public ActionResult UploadModal(int file_id, int doc_id)
        {

            ViewData["FILEID"] = file_id;
            ViewData["DOCID"] = doc_id;
            return View();
        }

        public ActionResult GetDetailsDocs()
        {
            return Json(new { message = "ok" });
        }

        public ActionResult ConfirmApproval(int doc_id)
        {

            var sessionUser = (Person)Session["user"];
            FileUploads.ConfirmApproval(doc_id, sessionUser.user_id);
            return Json(new { message = "" });
            //return RedirectToAction("ListSubmittedForm", "Approved");
        }


        [HttpPost]
        public ActionResult SaveUploadedFile(HttpPostedFileBase file, int file_id, int ID)
        {
            var sessionUser = (Person)Session["user"];
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                try
                {

                    var path = Path.Combine(Server.MapPath("~/SavedFile"), fileName);
                    file.SaveAs(path);

                    FileUploads.UpdateFileIntoDB(sessionUser, fileName, 1, path, file_id, ID);
                    //FileUploads.InsertFileIntoDB(sessionUser, fileName, 1, path);

                }
                catch (Exception ex)
                {
                    FileUploads.InsertFileIntoDB(sessionUser, fileName, 0, "");
                }

                // Perform any additional logic or database operations here
                // For example, you can save the file path to the database
            }

            return RedirectToAction("ListSubmittedForm", "Approval"); // Redirect to the appropriate action
        }

        public ActionResult DownloadExcel()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Logout", "User", new { ReturnURL = Url.Action("UploadData", "SubmitForm") });
            }
            // Get your table data (replace this with your actual data retrieval logic)
            var sessionUser = (Person)Session["user"];
            List<ApprovalForm> tableData = GetTableData(sessionUser);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Add a worksheet
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Set the header row
                int rowIndex = 1;
                int colIndex = 1;
                foreach (var property in typeof(ApprovalForm).GetProperties())
                {
                    if(property.Name == "ID"|| property.Name == "Approval_Status" || property.Name == "detailsFile")
                    {
                        continue;
                    }
                    if(property.Name == "File_ID")
                    {
                        worksheet.Cells[1, colIndex].Value = "File Name";
                    }
                    else if(property.Name == "Uploaded_by")
                    {
                        worksheet.Cells[1, colIndex].Value = "Uploaded By";
                    }
                    else if(property.Name == "Approved_by")
                    {
                        worksheet.Cells[1, colIndex].Value = "Approved By";
                    }
                    else if (property.Name == "Dept_id")
                    {
                        worksheet.Cells[1, colIndex].Value = "Unit";
                    }
                    else if (property.Name == "Approval_desc")
                    {
                        worksheet.Cells[1, colIndex].Value = "Status";
                    }
                    else
                    {
                        worksheet.Cells[1, colIndex].Value = "Approved On";
                    }
                    colIndex++;
                }
                worksheet.Cells[1, colIndex].Value = "Extract By";
                //dt.ToString("dd/MM/yy");
                // Populate the data
                rowIndex = 2;
                foreach (var dataRow in tableData)
                {
                    colIndex = 1;
                    foreach (var property in typeof(ApprovalForm).GetProperties())
                    {
                        if (property.Name == "ID" || property.Name == "Approval_Status" || property.Name == "detailsFile")
                        {
                            continue;
                        }
                        worksheet.Cells[rowIndex, colIndex].Value = property.GetValue(dataRow, null);
                        colIndex++;
                    }
                    worksheet.Cells[rowIndex, colIndex].Value = sessionUser.username;
                    rowIndex++;
                }

                // Set the content type and file name for the response
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=TableData.xlsx");

                // Write the Excel package to the response stream
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
            }

            return new EmptyResult();
        }

        // Replace this method with your actual data retrieval logic
        private List<ApprovalForm> GetTableData(Person sessionuser)
        {
            // Sample data - replace this with your actual data retrieval logic

            var getforms = new List<ApprovalForm>();
            if(sessionuser.employee_id == 10084975) // currently Ir. Hidayat is admin so can have all data
            {
                getforms = ApprovalForm.GetAllFormsSubmittedExcel(sessionuser.dept_id, sessionuser.subunit_id);
            }
            else
            {
                getforms = ApprovalForm.GetAllFormsSubmittedExcel(sessionuser.dept_id, sessionuser.subunit_id,sessionuser.assg_team);

            }

            return getforms;
        }
    }

    

    //public ActionResult ListSubmittedForm()
    //{

    //    var sessionuser = (Person)Session["user"];
    //    var getforms = new ApprovalForm() ;
    //    return View(getforms);
    //}

    //public ActionResult PEEForm(int ID)
    //{
    //    HSEMSEntities conn = new HSEMSEntities();
    //    var connection = conn.Database.Connection;
    //    //var getforms = new PEE_Form();
    //    //getforms = PEE_Form.GetAllForms(ID, getforms);
    //    var getforms = connection.Query<PEE_Form>("SELECT * FROM PEE_Form  WHERE form_id = @formid", new { formid = ID }).ToList();
    //    var getdate = connection.Query<List_forms>("SELECT * FROM List_forms WHERE ID = @formid", new { formid = ID }).FirstOrDefault();

    //    ViewData["date"] = getdate.created_on;
    //    ViewData["dateapproved"] = getdate.approved_on;
    //    ViewData["statusform"] = getdate.status;
    //    ViewData["formId"] = ID;

    //    return View(getforms);
    //}

    //[HttpPost]
    //public JsonResult save_approverForm(List<PEE_Form> formdetails)
    //{
    //    //HSEMSEntities conn = new HSEMSEntities(); 
    //    using (HSEMSEntities conn = new HSEMSEntities())
    //    {
    //        var connection = conn.Database.Connection;
    //        string formname = "3A.04(3)_Portable Electrical Equipment";
    //        int createdby = 123;
    //        DateTime createdon = DateTime.Now;

    //        connection.Execute("INSERT INTO List_Forms (form_name, created_by, created_on, status) VALUES (@formname, @createdby, @createdon, 1)", new { formname = formname, createdby = createdby, createdon = createdon });

    //        var formId = connection.Query<List_forms>("SELECT * FROM List_forms WHERE form_name = @formname AND created_by = @createdby AND created_on = @createdon", new { formname = formname, createdby = createdby, createdon = createdon }).Select(m => m.ID).FirstOrDefault();
    //        for (int i = 0; i < formdetails.Count; i++)
    //        {
    //            //formdetails[i].submitted_date=DateTime.Now;
    //            //formdetails[i].user_id = 123;

    //            connection.Execute("INSERT INTO PEE_Form (form_id, item_description, brand, registration_no, spec1, spec2, spec3, spec4, spec5, others, action) " +
    //                                "VALUES(@formid , @item_description, @brand, @registration_no, @spec1, @spec2, @spec3, @spec4, @spec5, @others, @action)", new { formid = formId, item_description = formdetails[i].item_description, brand = formdetails[i].brand, registration_no = formdetails[i].registration_no, spec1 = formdetails[i].spec1, spec2 = formdetails[i].spec2, spec3 = formdetails[i].spec3, spec4 = formdetails[i].spec4, spec5 = formdetails[i].spec5, others = formdetails[i].others, action = formdetails[i].action });
    //            //conn.PEE_Form.Add(formdetails[i]);
    //            //conn.SaveChanges(); 
    //        }
    //    }
    //    var result = new { };
    //    return Json(result);
    //}
}

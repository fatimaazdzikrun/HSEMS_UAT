using Aspose.Words;
using DocumentFormat.OpenXml.Packaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HSEMS.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HSEMS.Controllers
{
    public class SubmitFormController : Controller
    {
        // GET: SubmitForm
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ()
        //{
        //    var getthheuploadedfile = new FileUploads();
        //    return View();
        //}

        public ActionResult UploadData()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Logout", "User", new { ReturnURL = Url.Action("UploadData", "SubmitForm") });
            }
            var sessionUser = (Person)Session["user"];
            ViewBag.selectFrequency = FileUploads.GetFreqType().ToList().Select(r => new SelectListItem { Text = r.FrequencyType, Value = r.ID.ToString() }).ToList();
            var getthheuploadedfile = FileUploads.GetAllFileUploaded(sessionUser.username).ToList();
            return View(getthheuploadedfile);
        }

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // Set SharePoint details
        //        string siteUrl = "https://liveutm.sharepoint.com/sites/HSEMSDigital";
        //        string username = "fatimaazdzikrun@live.utm.my";
        //        string password = "M@laysia2080";
        //        string libraryName = "Documents";

        //        // Connect to SharePoint
        //        using (ClientContext context = SharePointManager.ConnectToSharePoint(siteUrl, username, password))
        //        {
        //            try
        //            {
        //                // Get the SharePoint library
        //                List library = context.Web.Lists.GetByTitle(libraryName);
        //                context.Load(library, l => l.RootFolder);

        //                // Prepare the file information
        //                string fileName = Path.GetFileName(file.FileName);
        //                byte[] fileBytes;
        //                using (var binaryReader = new BinaryReader(file.InputStream))
        //                {
        //                    fileBytes = binaryReader.ReadBytes(file.ContentLength);
        //                }

        //                // Upload the file to SharePoint
        //                var fileCreationInformation = new FileCreationInformation
        //                {
        //                    Content = fileBytes,
        //                    Url = fileName,
        //                    Overwrite = true
        //                };
        //                Microsoft.SharePoint.Client.File newFile = library.RootFolder.Files.Add(fileCreationInformation);
        //                context.Load(newFile);
        //                context.ExecuteQuery();

        //                // File uploaded successfully
        //                ViewBag.Message = "File uploaded successfully.";
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle any exceptions
        //                ViewBag.Message = "Error uploading file: " + ex.Message;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // No file selected
        //        ViewBag.Message = "Please select a file to upload.";
        //    }

        //    return View();
        //}

        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        try
        //        {
        //            // Set Google Drive details
        //            string applicationName = "HSEMS";
        //            string credentialsFilePath = "C:/Users/User/Desktop/ClientSecret/client_secret_242198987768 - qb7nakdqa2jf0267iqsi2edftff7477e.apps.googleusercontent.com.json";
        //            string folderId = "1g3wQ0BrywM14bedM0xRbaibVWX-n_NtD"; // ID of the Google Drive folder where you want to save the file

        //            // Create the Google Drive service
        //            DriveService service = CreateDriveService(applicationName, credentialsFilePath);

        //            // Prepare the file metadata
        //            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        //            {
        //                Name = Path.GetFileName(file.FileName),
        //                Parents = new List<string>() { folderId }
        //            };

        //            // Upload the file to Google Drive
        //            FilesResource.CreateMediaUpload request;
        //            using (var stream = new MemoryStream())
        //            {
        //                file.InputStream.CopyTo(stream);
        //                request = service.Files.Create(fileMetadata, stream, file.ContentType);
        //                request.Upload();
        //            }

        //            // Handle the response
        //            var uploadedFile = request.ResponseBody;
        //            if (uploadedFile != null)
        //            {
        //                string fileId = uploadedFile.Id;
        //                string fileName = uploadedFile.Name;

        //                // File uploaded successfully
        //                ViewBag.Message = "File uploaded successfully.";
        //            }
        //            else
        //            {
        //                // Error occurred during file upload
        //                ViewBag.Message = "Error uploading file.";
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            ViewBag.Message = "Error uploading file: " + ex.Message;

        //        }

        //    }
        //    else
        //    {
        //        // No file selected
        //        ViewBag.Message = "Please select a file to upload.";
        //    }

        //    return View();
        //}

        //private DriveService CreateDriveService(string applicationName, string credentialsFilePath)
        //{
        //    UserCredential credential;

        //    using (var stream = new FileStream(credentialsFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        string credPath = "path/to/store/tokens";

        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.Load(stream).Secrets,
        //            new[] { DriveService.Scope.Drive },
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //    }

        //    // Create the DriveService with the provided credential
        //    var service = new DriveService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = applicationName
        //    });

        //    return service;
        //}

        //[HttpPost]
        //public ActionResult UploadAndInsertData(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // Parse the Word document and extract the column data
        //        List<string> columnData = WordDocumentParser.ParseColumnData(file.InputStream);

        //        // Create a list of model objects
        //        List<YourModel> dataList = new List<YourModel>();
        //        foreach (var data in columnData)
        //        {
        //            // Map the extracted column data to your model
        //            YourModel model = new YourModel();
        //            model.ColumnName = data;
        //            dataList.Add(model);
        //        }

        //        // Connect to the database
        //        using (var connection = new SqlConnection("your_connection_string"))
        //        {
        //            connection.Open();

        //            // Insert the data into the database using SQL
        //            foreach (var model in dataList)
        //            {
        //                string query = $"INSERT INTO YourTable (ColumnName) VALUES ('{model.ColumnName}')";
        //                using (var command = new SqlCommand(query, connection))
        //                {
        //                    command.ExecuteNonQuery();
        //                }
        //            }
        //        }

        //        // Redirect or return a success message
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // If no file is uploaded, redirect or return an error message
        //    return RedirectToAction("Error", "Home");
        //}

        //[HttpPost]
        //public ActionResult UploadAndInsertData(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // Open the Word document using OpenXML SDK
        //        using (var document = WordprocessingDocument.Open(file.InputStream, false))
        //        {
        //            // Select the table and column to extract data from
        //            Table table = document.MainDocumentPart.Document.Body.Elements<Table>().FirstOrDefault();
        //            int columnIndex = 0; // Change this to the appropriate column index

        //            // Extract the column data
        //            List<string> columnData = new List<string>();
        //            foreach (TableRow row in table.Elements<TableRow>())
        //            {
        //                TableCell cell = row.Elements<TableCell>().ElementAt(columnIndex);
        //                string data = cell.InnerText;
        //                columnData.Add(data);
        //            }

        //            // Create a list of model objects
        //            List<YourModel> dataList = new List<YourModel>();
        //            foreach (var data in columnData)
        //            {
        //                // Map the extracted column data to your model
        //                YourModel model = new YourModel();
        //                model.ColumnName = data;
        //                dataList.Add(model);
        //            }

        //            // Connect to the database
        //            using (var connection = new SqlConnection("your_connection_string"))
        //            {
        //                connection.Open();

        //                // Insert the data into the database using SQL
        //                foreach (var model in dataList)
        //                {
        //                    string query = $"INSERT INTO YourTable (ColumnName) VALUES ('{model.ColumnName}')";
        //                    using (var command = new SqlCommand(query, connection))
        //                    {
        //                        command.ExecuteNonQuery();
        //                    }
        //                }
        //            }

        //            // Redirect or return a success message
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    // If no file is uploaded, redirect or return an error message
        //    return RedirectToAction("Error", "Home");
        //}



        private static string[] Scopes = { DriveService.Scope.Drive };
        private static string ApplicationName = "Your Application Name";

        // ...

        public ActionResult UploadToGoogleDrive(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                UserCredential credential;

                // Specify the path to your client secrets file
                string clientSecretsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClientSecret", "client_secret_242198987768 - qb7nakdqa2jf0267iqsi2edftff7477e.apps.googleusercontent.com.json");

                using (var stream = new FileStream(clientSecretsPath, FileMode.Open, FileAccess.Read))
                {
                    // Create the credential object
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore("DriveServiceCredentials")).Result;
                }

                // Create the Drive service using the credentials
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Create the metadata for the file
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(file.FileName),
                    MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };

                // Upload the file to Google Drive
                FilesResource.CreateMediaUpload request;
                using (var stream = new MemoryStream())
                {
                    file.InputStream.CopyTo(stream);
                    request = service.Files.Create(fileMetadata, stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    request.Fields = "id";
                    request.Upload();
                }

                // Get the ID of the uploaded file
                var uploadedFile = request.ResponseBody;
                string fileId = uploadedFile.Id;

                // Redirect or return a success message
                return RedirectToAction("UploadData", "SubmitForm");
            }

            // If no file is uploaded, redirect or return an error message
            return RedirectToAction("Error", "Home");
        }

        // ...

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var sessionUser = (Person)Session["user"];
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                try
                {
                    // Load the Word document using Aspose.Words
                    //Document doc = new Document(file.InputStream);

                    // Save the document as PDF using Aspose.Words
                    //var pdfStream = new MemoryStream();
                    //doc.Save(pdfStream, SaveFormat.Pdf);

                    // Save the PDF file to the file system
                    //var pdfFilePath = Path.Combine(Server.MapPath("~/SavedFile"), Path.GetFileNameWithoutExtension(file.FileName) + ".pdf");
                    //var _filename = Path.GetFileNameWithoutExtension(file.FileName) + ".pdf";

                    //using (var fileStream = new FileStream(pdfFilePath, FileMode.Create))
                    //{
                    //    pdfStream.Seek(0, SeekOrigin.Begin);
                    //    pdfStream.CopyTo(fileStream);
                    //}
                    var path = Path.Combine(Server.MapPath("~/SavedFile"), fileName);
                    file.SaveAs(path);

                    FileUploads.InsertFileIntoDB(sessionUser, fileName, 1, path);

                }
                catch (Exception ex)
                {
                    FileUploads.InsertFileIntoDB(sessionUser, fileName, 0, "");
                }

                // Perform any additional logic or database operations here
                // For example, you can save the file path to the database
            }

            return RedirectToAction("UploadData", "SubmitForm"); // Redirect to the appropriate action
        }
    }

}

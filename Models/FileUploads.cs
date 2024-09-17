using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class FileUploads
    {
        public int ID { get; set; }

        public string FileNames { get; set; }

        public string Uploaded_by { get; set; }

        public DateTime Date_uploaded { get; set; }

        public int File_status { get; set; }

        public string File_path { get; set; }

        public string FrequencyType { get; set; }

        public static List<FileUploads> GetAllFileUploaded(string username ="")
        {
            
            using(var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getallfile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE uploaded_by = @username ORDER by Date_uploaded desc", new {username}).ToList();
                return getallfile;
            }
        }

        public static void InsertFileIntoDB(Person user, string FileName, int status, string path, int freq=0)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var freqfile = con.Query<List_forms>("SELECT * FROM List_forms").ToList();
                int frequencyFile = 0;

                foreach (var form in freqfile)
                {
                    if(FileName.Contains(form.form_name))
                    {
                        freq = form.freq;
                        break;
                    }
                }
                con.Execute("INSERT INTO tbl_uploadedfile (FileNames, Uploaded_by, Date_uploaded, File_status, File_path, Freq) VALUES (@FileName, @username, GETDATE(), @status, @path, @freq )", new {FileName, user.username, status, path, freq});
                var getfileid = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE Uploaded_by = @username AND FileNames = @FileName", new {user.username, FileName}).FirstOrDefault();
                con.Execute("INSERT INTO tbl_document_approval(File_ID, Uploaded_by, Dept_id, Approval_status, subunit_id,team_id) VALUES (@ID, @user_id, @dept_id, 0, @subunit_id, @assg_team)", new { getfileid.ID, user.user_id, user.dept_id, user.subunit_id, user.assg_team});
                 
            }
        }

        public static void UpdateFileIntoDB(Person user, string FileName, int status, string path, int file_id, int app_id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                con.Execute("UPDATE tbl_uploadedfile SET FileNames= @FileName , File_path= @path WHERE ID = @file_id", new {FileName, path, file_id});
                con.Execute("UPDATE tbl_document_approval SET Approval_Status = @status, Edited_on = GETDATE() WHERE ID = @app_id", new { app_id, status });

            }
        }

        public static void ConfirmApproval(int id, int approved_by)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                con.Execute("UPDATE tbl_document_approval SET Approval_Status = 2, Approved_by = @approved_by, Edited_on = GETDATE() WHERE ID = @id", new { id, approved_by });
            }
        }

        public static List<FileUploads> GetFreqType()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getallfrequency = con.Query<FileUploads>("SELECT * FROM freq_type").ToList();
                return getallfrequency;

            }
        }

        public static FileUploads GetFileDetails(int file_id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getfiledetails = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @file_id", new {file_id}).FirstOrDefault();
                return getfiledetails;

            }
        }


    }
}
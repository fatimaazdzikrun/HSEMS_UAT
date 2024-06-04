using Dapper;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class ApprovalForm
    {
        public int ID { get; set; }

        public string File_ID { get; set; }

        [DisplayName("Uploaded By")]
        public string Uploaded_by { get; set; }

        public int Approval_Status { get; set; }

        [DisplayName("Approved By")]
        public string Approved_by { get; set; }

        [DisplayName("Unit")]
        public string Dept_id { get; set; }

        [DisplayName("Status")]
        public string Approval_desc { set; get; }
        public FileUploads detailsFile { get; set; }
        
        [DisplayName("Approved On")]
        public DateTime Edited_on { get; set; }

        public int team_id { get; set; }

        public static List<ApprovalForm> GetAllFormsSubmitted(int dept_id = 0, int subunit_id = 0, int assg_team = 0)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                if(dept_id != 0)
                {

                    if(assg_team != 0)
                    {
                        var getallfrequency = con.Query<ApprovalForm>("SELECT * FROM tbl_document_approval FROM tbl_document_approval WHERE A.Dept_id = @dept_id AND A.subunit_id = @subunit_id AND A.assg_team = @assg_team", new { dept_id, subunit_id, assg_team }).ToList();

                        foreach (var item in getallfrequency)
                        {
                            item.detailsFile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @ID", new { ID = item.File_ID }).FirstOrDefault();
                            item.File_ID = item.detailsFile.FileNames;
                        }
                        return getallfrequency;
                    }
                    else
                    {
                        var getallfrequency = con.Query<ApprovalForm>("SELECT * FROM tbl_document_approval WHERE tbl_document_approval.Dept_id = @dept_id AND subunit_id = @subunit_id", new { dept_id, subunit_id }).ToList();

                        foreach (var item in getallfrequency)
                        {
                            item.detailsFile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @ID", new { ID = item.File_ID }).FirstOrDefault();

                        }
                        return getallfrequency;
                    }
                   
                }
                else
                {
                    var getallfrequency = con.Query<ApprovalForm>("SELECT * FROM tbl_document_approval", new { dept_id }).ToList();

                    foreach (var item in getallfrequency)
                    {
                        item.detailsFile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @ID", new { ID = item.File_ID }).FirstOrDefault();

                    }
                    return getallfrequency;
                }
                

            }
        }

        public static List<ApprovalForm> GetAllFormsSubmittedExcel(int dept_id = 0, int subunit_id = 0, int assg_team = 0)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                if (dept_id != 0)
                {

                    if (assg_team != 0)
                    {
                        var getallfrequency = con.Query<ApprovalForm>("SELECT A.File_ID, B.username as Uploaded_by, C.username as Approved_by, D.type_name as Approval_desc, E.unit_name as Dept_id FROM tbl_document_approval A INNER JOIN  Person B on B.user_id = A.Uploaded_by INNER JOIN Person C on C.user_id = A.Approved_by INNER JOIN  tbl_approvalType D on D.type_id = A.Approval_Status INNER JOIN tbl_unit E on E.unit_id = A.Dept_id WHERE A.Dept_id = @dept_id AND A.subunit_id = @subunit_id AND A.assg_team = @assg_team", new { dept_id, subunit_id, assg_team }).ToList();

                        foreach (var item in getallfrequency)
                        {
                            item.detailsFile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @ID", new { ID = item.File_ID }).FirstOrDefault();
                            item.File_ID = item.detailsFile.FileNames;
                        }
                        return getallfrequency;
                    }
                    else
                    {
                        var getallfrequency = con.Query<ApprovalForm>("SELECT A.File_ID, B.username as Uploaded_by, C.username as Approved_by, D.type_name as Approval_desc, E.unit_name as Dept_id FROM tbl_document_approval A INNER JOIN  Person B on B.user_id = A.Uploaded_by INNER JOIN Person C on C.user_id = A.Approved_by INNER JOIN  tbl_approvalType D on D.type_id = A.Approval_Status INNER JOIN tbl_unit E on E.unit_id = A.Dept_id WHERE A.Dept_id = @dept_id AND A.subunit_id = @subunit_id", new { dept_id, subunit_id }).ToList();

                        foreach (var item in getallfrequency)
                        {
                            item.detailsFile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @ID", new { ID = item.File_ID }).FirstOrDefault();
                            item.File_ID = item.detailsFile.FileNames;
                        }
                        return getallfrequency;
                    }

                }
                else
                {
                    var getallfrequency = con.Query<ApprovalForm>("SELECT * FROM tbl_document_approval", new { dept_id }).ToList();

                    foreach (var item in getallfrequency)
                    {
                        item.detailsFile = con.Query<FileUploads>("SELECT * FROM tbl_uploadedfile WHERE ID = @ID", new { ID = item.File_ID }).FirstOrDefault();

                    }
                    return getallfrequency;
                }


            }
        }

    }

}
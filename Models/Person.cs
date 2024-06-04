using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class Person
    {
        [DisplayName("User Id")]
        public int user_id { get; set; }

        [DisplayName("Username")]
        public string username { get; set; }

        [DisplayName("Password")]
        public string password { get; set; }

        [DisplayName("Employee Id")]
        public Nullable<int> employee_id { get; set; }

        [DisplayName("Unit")]
        public int dept_id { get; set; }

        [DisplayName("Name")]
        public string displayname { get; set; }

        [DisplayName("Sub Unit")]
        public int subunit_id { get; set; }

        [DisplayName("Team")]
        public int assg_team { get; set; }

        [DisplayName("Username")]
        public string signature { get; set; }

        [DisplayName("Role")]
        public int role_id { get; set; }

        public string ReturnURL { get; set; }

        [DisplayName("Role")]
        public string roleName { get; set; }

        [DisplayName("Sub Unit")]
        public string subUnit { get; set; }

        [DisplayName("Unit")]
        public string unitName { get; set; }

        [DisplayName("Team")]
        public string teamName { get; set; }

        public static Person GetDetailsOfUser(string username, string password)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getsessionuser = con.Query<Person>("SELECT * FROM Person WHERE employee_id = @username AND password = @password", new { username, password }).FirstOrDefault();
                return getsessionuser;
            }
        }

        public static Person GetExistingUser(string username)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getsessionuser = con.Query<Person>("SELECT * FROM Person WHERE username = @username ", new { username }).FirstOrDefault();
                return getsessionuser;
            }
        }

        public static Person GetDetailsOfUserForm(int user_id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getsessionuser = con.Query<Person>("SELECT * FROM Person WHERE user_id = @user_id ", new { user_id }).FirstOrDefault();
                return getsessionuser;
            }
        }

        public static List<Person> GetListOfUsers()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var getsessionuser = con.Query<Person>("Select a.user_id, a.username, a.password, a.employee_id, c.unit_name as unitName, d.role_name AS roleName, b.subunit_name AS subUnit FROM Person a " +
                    "LEFT JOIN tbl_subunit b on a.subunit_id = b.subunit_id " + 
                    "LEFT JOIN tbl_unit c on a.dept_id = c.unit_id " +
                    "LEFT JOIN tbl_roles d on a.role_id = d.role_id").ToList();
                return getsessionuser;
            }
        }

        public static void AddUser(Person person)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
               con.Execute("INSERT INTO Person ([username],[password],[employee_id],[dept_id],[role_id],[subunit_id],[assg_team]) VALUES (@username, @password,@employee_id,@dept_id,@role_id,@subunit_id,@assg_team)", new { person.username, person.password, person.employee_id, person.dept_id, person.role_id, person.subunit_id,person.assg_team });
                
            }
        }

        public static void UpdateUser(Person person)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                con.Execute("UPDATE Person SET username = @username ,password = @password ,employee_id = @employee_id,dept_id = @dept_id,role_id= @role_id,subunit_id= @subunit_id, assg_team=@assg_team WHERE user_id = @user_id", new { person.username, person.password, person.employee_id, person.dept_id, person.role_id, person.subunit_id, person.user_id, person.assg_team });

            }
        }
    }
}
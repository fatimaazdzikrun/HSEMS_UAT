using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class Roles
    {
        public int role_id { get; set; }

        public string role_name { get; set; }

       public static List<Roles> GetRoles()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var roleUser = con.Query<Roles>("SELECT * FROM tbl_roles ").ToList();
                return roleUser;
            }
        }


           
    }

}
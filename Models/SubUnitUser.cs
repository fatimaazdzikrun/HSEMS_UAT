using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class SubUnitUser
    {
        public int subunit_id { get; set; }

        public string subunit_name { get; set; }

       public static List<SubUnitUser> GetSubunit()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var subunitUser = con.Query<SubUnitUser>("SELECT * FROM tbl_subunit ").ToList();
                return subunitUser;
            }
        }


           
    }

}
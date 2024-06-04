using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class UnitUser
    {
        public int unit_id { get; set; }

        public string unit_name { get; set; }

       public static List<UnitUser> GetUnit()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var unitUser = con.Query<UnitUser>("SELECT * FROM tbl_unit ").ToList();
                return unitUser;
            }
        }


           
    }

    public class TeamUser
    {
        public int team_id { get; set; }

        public string team_name { get; set; }

        public static List<TeamUser> GetTeam()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSEMS"].ToString()))
            {
                var teamUser = con.Query<TeamUser>("SELECT * FROM tbl_team ").ToList();
                return teamUser;
            }
        }



    }

}
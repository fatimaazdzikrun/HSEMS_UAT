using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{
    public class List_forms
    {
        public int ID { get; set; }

        public string form_name { get; set; }

        

        public int freq { get; set; }

        

        //public List<PEE_Form> forms{ get; set;}

        //public static List_forms getformdetails (int ID, List_forms getdetails)
        //{
        //    HSEMSEntities conn = new HSEMSEntities();
        //    var connection = conn.Database.Connection;

        //    var getdetails2 = connection.Query<List_forms>("SELECT * FROM List_forms WHERE ID = @id", new { id = ID }).ToList();

        //    foreach(var form in getdetails2)
        //    {
        //        form.forms = connection
        //    }

        //}
    }
}
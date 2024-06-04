using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HSEMS.Models
{

    [Table("PEE_Form")]
    public class PEEForm
    {
        public int ID { get; set; }

        public int user_id { get; set; }

        public DateTime submitted_date { get; set; }

        public string item_description { get; set; }

        public string brand { get; set; }

        public string registration_no { get; set; }

        public int spec1 { get; set; }

        public int spec2 { get; set; }

        public int spec3 { get; set; }

        public int spec4 { get; set; }

        public int spec5 {get; set;}

        public int other { get; set; }

        public string action { get; set; }

        public int approved { get; set; }

        public int approved_by { get; set; }

        public DateTime approved_date { get; set;}
    }
}
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace Store.Models.Models
{
    public class Company
    {

        public int Id { get; set; }
      
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
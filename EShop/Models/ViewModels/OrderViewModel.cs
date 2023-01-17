using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Total { get; set; }
        public int Status { get; set; }

        public int? OrderNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm:ss tt}")]
        public DateTime? OrderDate { get; set; }
        public string UserFirstName { get; set; }

        public string UserLasttName { get; set; }

        public string? Phone { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public string? Country { get; set; }


    }
}

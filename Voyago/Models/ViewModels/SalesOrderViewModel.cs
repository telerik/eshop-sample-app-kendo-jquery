using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class SalesOrderViewModel
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal Total { get; set; }
        public byte Status { get; set; }
        public DateTime? ShipDate { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public int ContactId { get; set; }
        public decimal LineTotal { get; set; }
        public int? ProductId { get; set; }       
    }
}

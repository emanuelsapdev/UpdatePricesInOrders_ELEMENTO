using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePricesInOrders_Interface.Models
{
    public class OrderModel
    {
        public int vDocEntry { get; set; }
        public string vItemCode { get; set; }
        public double vNewPrice { get; set; }
        public double vCurrPrice { get; set; }
        public double vQuantityPending { get; set; }
        public double vQuantity { get; set; }
        public int vLineNum { get; set; }
        public string vListName { get; set; }
        public DateTime vDateUpdate { get; set; }
    }
}

using UpdatePricesInOrders_Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePricesInOrders_Interface.Mapped
{
    public class OrderMapped
    {
        public static OrderModel FromRecordset(SAPbobsCOM.Recordset pRec)
        {
            var data = new OrderModel();
            data.vDocEntry = pRec.Fields.Item("DocEntry").Value;
            data.vItemCode = pRec.Fields.Item("ItemCode").Value;
            data.vNewPrice = pRec.Fields.Item("NewPrice").Value;
            data.vCurrPrice = pRec.Fields.Item("CurrPrice").Value;
            data.vQuantityPending = pRec.Fields.Item("OpenQty").Value;
            data.vQuantity = pRec.Fields.Item("Quantity").Value;
            data.vLineNum = pRec.Fields.Item("LineNum").Value;
            data.vListName = pRec.Fields.Item("ListName").Value;
            data.vDateUpdate = DateTime.Now;
            return data;
        }

    }
}

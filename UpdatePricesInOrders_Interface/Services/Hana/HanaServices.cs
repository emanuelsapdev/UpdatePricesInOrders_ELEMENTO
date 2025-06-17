using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePricesInOrders_Interface.Services.Hana
{
    public class HanaServices
    {
        protected readonly IDbConnection dbConnection;

        public HanaServices(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        //public IList<StockItem> GetStockItem()
        //{
        //    if (dbConnection.State != ConnectionState.Open)
        //        dbConnection.Open();

        //    using IDbCommand command = dbConnection.CreateCommand();
        //    command.CommandText = $@"SELECT 
        //                                T0.""ItemCode"",
        //                                IFNULL((T0.""OnHand"" - T0.""IsCommited""),0) / IFNULL(""BaseQty"",1)  as ""Disponible"" 
        //                                FROM OITW T0
        //                                INNER JOIN OITM T1 ON T0.""ItemCode"" = T1.""ItemCode""
        //                                LEFT JOIN UGP1 T2 ON T2.""UgpEntry"" = T1.""UgpEntry""  AND T2.""UomEntry"" = T1.""U_DefaultUNVtex""
        //                                WHERE ""WhsCode"" = 'EC' AND T1.""QryGroup1"" = 'Y'";

        //    // Establecer un timeout de 30 segundos
        //    command.CommandTimeout = 2;
        //    using IDataReader reader = command.ExecuteReader();
        //    var list = new List<StockItem>();
        //    try
        //    {
        //        while (reader.Read())
        //        {
        //            list.Add(new StockItem
        //            {
        //                ItemCode = reader.GetString(reader.GetOrdinal("ItemCode")),
        //                OnHand = reader.GetDouble(reader.GetOrdinal("Disponible"))
        //            });
        //        }

        //        return list;
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }
        //}

        //public IList<ListaPrecio> GetListPrice()
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open)
        //            dbConnection.Open();

        //        using IDbCommand command = dbConnection.CreateCommand();
        //        command.CommandText = $@"SELECT T0.""ItemCode"",T0.""Price"",T0.""Currency"" 
        //                                    FROM ITM1 T0
        //                                    INNER JOIN OITM T1 ON T0.""ItemCode"" = T1.""ItemCode"" 
        //                                    WHERE T0.""PriceList"" = 16 AND T0.""Price"" > 0 AND T1.""QryGroup1"" = 'Y' ";

        //        using IDataReader reader = command.ExecuteReader();

        //        var listSN = new List<ListaPrecio>();

        //        while (reader.Read())
        //        {
        //            listSN.Add(new ListaPrecio
        //            {
        //                ItemCode = reader.GetString(reader.GetOrdinal("ItemCode")),
        //                Price = reader.GetDouble(reader.GetOrdinal("Price")),
        //                Currency = reader.GetString(reader.GetOrdinal("Currency"))
        //            });
        //        }

        //        return listSN;
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }
        //}

        //public IList<FormaEnvio> GetFormaEnvio()
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open)
        //            dbConnection.Open();

        //        using IDbCommand command = dbConnection.CreateCommand();
        //        command.CommandText = $@"SELECT ""TrnspCode"",""TrnspName"" FROM OSHP";

        //        using IDataReader reader = command.ExecuteReader();

        //        var listSN = new List<FormaEnvio>();

        //        while (reader.Read())
        //        {
        //            listSN.Add(new FormaEnvio
        //            {
        //                id = reader.GetInt32(reader.GetOrdinal("TrnspCode")),
        //                name = reader.GetString(reader.GetOrdinal("TrnspName"))
        //            });
        //        }

        //        return listSN;
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }
        //}

        //public int GetIdVtex(string code)
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open)
        //            dbConnection.Open();

        //        using IDbCommand command = dbConnection.CreateCommand();
        //        command.CommandText = @"SELECT ""DocEntry"" FROM ORDR WHERE ""U_Interfaz"" = 'VTEX' AND ""U_Code"" = ? AND CANCELED = 'N'";

        //        IDbDataParameter docEntryParameter = command.CreateParameter();
        //        docEntryParameter.ParameterName = "@Code";
        //        docEntryParameter.Value = code;
        //        command.Parameters.Add(docEntryParameter);

        //        using IDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            return reader.GetInt32(0);
        //        }
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }

        //    return 0;
        //}

        //public int GetUnDefault(string itemCode)
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open)
        //            dbConnection.Open();

        //        using IDbCommand command = dbConnection.CreateCommand();
        //        command.CommandText = @"SELECT ""U_DefaultUNVtex"" FROM OITM WHERE IFNULL(""U_DefaultUNVtex"",0) <> 0 AND ""QryGroup1"" = 'Y' AND ""ItemCode"" = ?";

        //        IDbDataParameter docEntryParameter = command.CreateParameter();
        //        docEntryParameter.ParameterName = "@ItemCode";
        //        docEntryParameter.Value = itemCode;
        //        command.Parameters.Add(docEntryParameter);

        //        using IDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            return reader.GetInt32(0);
        //        }
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }

        //    return 0;
        //}

        //public List<ResultGetOrdersVtexByStatus> GetOrdersVtexByStatus(string statusvtex = "PP")
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open)
        //            dbConnection.Open();

        //        using IDbCommand command = dbConnection.CreateCommand();
        //        command.CommandText = $@"SELECT 
        //                                T0.""DocEntry"", 
        //                                T0.""U_Code"",
        //                                COUNT(T1.""LineNum"") AS ""QuantityLines""
        //                                FROM ORDR T0 
        //                                INNER JOIN RDR1 T1 
        //                                ON T1.""DocEntry"" = T0.""DocEntry"" 
        //                                WHERE T0.CANCELED = 'N'
        //                                AND T0.""U_Interfaz"" = 'VTEX' 
        //                                AND IFNULL(T0.""U_Code"", '') <> '' 
        //                                AND T0.""U_Vtex_Status"" = ?
        //                                GROUP BY T0.""DocEntry"", T0.""U_Code""";

        //        IDbDataParameter param = command.CreateParameter();
        //        param.ParameterName = "@U_Vtex_Status";
        //        param.Value = statusvtex;
        //        command.Parameters.Add(param);

        //        using IDataReader reader = command.ExecuteReader();

        //        var orders = new List<ResultGetOrdersVtexByStatus>();

        //        while (reader.Read())
        //        {
        //            orders.Add(new ResultGetOrdersVtexByStatus
        //            {
        //                DocEntry = reader.GetInt32(reader.GetOrdinal("DocEntry")),
        //                U_Code = reader.GetString(reader.GetOrdinal("U_Code")),
        //                QuantityLines = reader.GetInt32(reader.GetOrdinal("QuantityLines"))
        //            });
        //        }

        //        return orders;
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }

        //}

        //public ResultGetOrderVtexByDocEntry GetOrderVtexByDocEntry(int? DocEntry)
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open)
        //            dbConnection.Open();

        //        using IDbCommand command = dbConnection.CreateCommand();
        //        command.CommandText = $@"SELECT 
        //                                T0.""DocEntry"", 
        //                                T0.""U_Code"",
        //                                COUNT(T1.""LineNum"") AS ""QuantityLines""
        //                                FROM ORDR T0 
        //                                INNER JOIN RDR1 T1 
        //                                ON T1.""DocEntry"" = T0.""DocEntry"" 
        //                                WHERE T0.CANCELED = 'N'
        //                                AND T0.""U_Interfaz"" = 'VTEX' 
        //                                AND IFNULL(T0.""U_Code"", '') <> '' 
        //                                AND T0.""DocEntry"" = ?
        //                                GROUP BY T0.""DocEntry"", T0.""U_Code""";

        //        IDbDataParameter param = command.CreateParameter();
        //        param.ParameterName = "@DocEntry";
        //        param.Value = DocEntry;
        //        command.Parameters.Add(param);

        //        using IDataReader reader = command.ExecuteReader();

        //        reader.Read();

        //        var order = new ResultGetOrderVtexByDocEntry
        //        {
        //            DocEntry = reader.GetInt32(reader.GetOrdinal("DocEntry")),
        //            U_Code = reader.GetString(reader.GetOrdinal("U_Code")),
        //            QuantityLines = reader.GetInt32(reader.GetOrdinal("QuantityLines"))
        //        };

        //        return order;
        //    }
        //    finally
        //    {
        //        if (dbConnection.State == ConnectionState.Open)
        //            dbConnection.Close();
        //    }

        //}
    }
}

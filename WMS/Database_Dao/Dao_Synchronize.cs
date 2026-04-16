using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Database_Dao
{
    public class Dao_Synchronize
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        public int Insert(Synchronize synchronize)
        {
            //inset , delete , update   
            //sqlconn.Open();
            OpenSQLConnection();
            string query = "Insert into [dbo].[Synchronize]( " + 
                               "[MAC Address]                 " + "," + 
                               "[Date Time]                   " + "," + 
                               "[User]                        " + "," + 
                               "[Customer Group]              " + "," + 
                               "Item                          " + "," + 
                               "Mapping                       " + "," + 
                               "Printer                       " + "," + 
                               "[Packing Header]              " + "," + 
                               "[Packing Line]                " + "," + 
                               "[Outer Carton]                " + "," + 
                               "[Inner Carton]                " + "," + 
                               "[Scan Label String]           " + "," + 
                               "Prescan                       " + "," + 
                               "[Prescan Outer Carton]        " + "," + 
                               "[Prescan Inner Carton]        " + "," + 
                               "DirectPrint                   " + "," + 
                               "[DirectPrint Outer Carton]    " + "," + 
                               "[DirectPrint Inner Carton]    " + "," + 
                               "[Packing Mapping]             " + "," + 
                               "[Scanned Packing Header]      " + "," + 
                               "[Scanned Packing Line]        " + "," + 
                               "[Scanned Packing Mapping]     " + "," + 
                               "[Closed Prescan]              " + "," + 
                               "[Closed Prescan Outer Carton] " + "," + 
                               "[Closed Prescan Inner Carton] " + "," + 
                               "[Label Header]                " + "," + 
                               "[Label Line]                  " + "," + 
                               "[Entries Process]             " + "," + 
                               ") VALUES( '" + synchronize.MACAddress + "',@datetime1," + synchronize.user + "," + synchronize.customerGroup + "," +
                               synchronize.item + "," + synchronize.mapping + "," + synchronize.printer + "," + synchronize.packingHeader + "," +
                               synchronize.packingLine + "," + synchronize.outerCarton + "," + synchronize.innerCarton + "," + synchronize.scanLabelString + "," +
                               synchronize.prescan + "," + synchronize.prescanOuterCarton + "," + synchronize.prescanInnerCarton + "," + synchronize.directPrint + "," +
                               synchronize.directPrintOuterCarton + "," + synchronize.directPrintInnerCarton + "," + synchronize.packingMapping + "," + synchronize.scannedPackingHeader + "," +
                               synchronize.scannedPackingLine + "," + synchronize.scannedPackingMapping + "," + synchronize.closedPrescan + "," + synchronize.closedPrescanOuterCarton + "," +
                               synchronize.closedPrescanInnerCarton + "," + synchronize.labelHeader + "," + synchronize.labelLine + "," + synchronize.entriesProcess + ")";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(synchronize.dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public List<Synchronize> Select()
        {
            //read
            List<Synchronize> data = new List<Synchronize>();

            OpenSQLConnection();
            string query = "Select * from [dbo].[Synchronize]";
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                Synchronize item = GetItem<Synchronize>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }

        private static Synchronize GetItem<T>(DataRow dr)
        {
            Synchronize synchronize = new Synchronize();
            Type temp = typeof(Synchronize);
            Synchronize obj = Activator.CreateInstance<Synchronize>();
            foreach (DataColumn column in dr.Table.Columns) 
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToUpper() == string.Join("", column.ColumnName.Split('@', ',', ' ', '.', ';', '_', '\'')).ToUpper())
                    {
                        if ("System.Byte[]" == column.DataType.ToString())
                        {
                            pro.SetValue(obj, Encoding.ASCII.GetBytes(dr[column.ColumnName].ToString()), null);
                        }
                        else if (dr[column.ColumnName] != DBNull.Value)
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
        public Synchronize SelectByMac(String MACAddress)
        {
            //read
            OpenSQLConnection();
            string query = "Select [Date Time] from [dbo].[Synchronize]"
                + "WHERE ([MAC Address] = '" + MACAddress + "' )";
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            Synchronize item = new Synchronize();
            foreach (DataRow row in dt.Rows)
            {
                item = GetItem<Synchronize>(row);
            }
            sqlconn.Close();
            return item;
        }
        public int Update(Synchronize synchronize)
        {
            //read
            OpenSQLConnection();
            string Conjunction = " ";
            string query = "Update [dbo].[Synchronize] Set ";
            if (synchronize.user != null && synchronize.user.Length > 0)
            {
                query = query + Conjunction + " [User] = " + synchronize.user;
                Conjunction = " , ";
            }
            if (synchronize.customerGroup != null && synchronize.customerGroup.Length > 0)
            {
                query = query + Conjunction + " [Customer Group] = " + synchronize.customerGroup;
                Conjunction = " , ";
            }
            if (synchronize.item != null && synchronize.item.Length > 0)
            {
                query = query + Conjunction + " [Item] = " + synchronize.item;
                Conjunction = " , ";
            }
            if (synchronize.mapping != null && synchronize.mapping.Length > 0)
            {
                query = query + Conjunction + " [Mapping] = " + synchronize.mapping;
                Conjunction = " , ";
            }
            if (synchronize.printer != null && synchronize.printer.Length > 0)
            {
                query = query + Conjunction + " [Printer] = " + synchronize.printer;
                Conjunction = " , ";
            }
            if (synchronize.packingHeader != null && synchronize.packingHeader.Length > 0)
            {
                query = query + Conjunction + " [Packing Header] = " + synchronize.packingHeader;
                Conjunction = " , ";
            }
            if (synchronize.packingLine != null && synchronize.packingLine.Length > 0)
            {
                query = query + Conjunction + " [Packing Line] = " + synchronize.packingLine;
                Conjunction = " , ";
            }
            if (synchronize.outerCarton != null && synchronize.outerCarton.Length > 0)
            {
                query = query + Conjunction + " [Outer Carton] = " + synchronize.outerCarton;
                Conjunction = " , ";
            }
            if (synchronize.innerCarton != null && synchronize.innerCarton.Length > 0)
            {
                query = query + Conjunction + " [Inner Carton] = " + synchronize.innerCarton;
                Conjunction = " , ";
            }
            if (synchronize.scanLabelString != null && synchronize.scanLabelString.Length > 0)
            {
                query = query + Conjunction + " [Scan Label String] = " + synchronize.scanLabelString;
                Conjunction = " , ";
            }
            if (synchronize.prescan != null && synchronize.prescan.Length > 0)
            {
                query = query + Conjunction + " [Prescan] = " + synchronize.prescan;
                Conjunction = " , ";
            }
            if (synchronize.prescanOuterCarton != null && synchronize.prescanOuterCarton.Length > 0)
            {
                query = query + Conjunction + " [Prescan Outer Carton] = " + synchronize.prescanOuterCarton;
                Conjunction = " , ";
            }
            if (synchronize.prescanInnerCarton != null && synchronize.prescanInnerCarton.Length > 0)
            {
                query = query + Conjunction + " [Prescan Inner Carton] = " + synchronize.prescanInnerCarton;
                Conjunction = " , ";
            }
            if (synchronize.directPrint != null && synchronize.directPrint.Length > 0)
            {
                query = query + Conjunction + " [DirectPrint] = " + synchronize.directPrint;
                Conjunction = " , ";
            }
            if (synchronize.directPrintOuterCarton != null && synchronize.directPrintOuterCarton.Length > 0)
            {
                query = query + Conjunction + " [DirectPrint Outer Carton] = " + synchronize.directPrintOuterCarton;
                Conjunction = " , ";
            }
            if (synchronize.directPrintInnerCarton != null && synchronize.directPrintInnerCarton.Length > 0)
            {
                query = query + Conjunction + " [DirectPrint Inner Carton] = " + synchronize.directPrintInnerCarton;
                Conjunction = " , ";
            }
            if (synchronize.packingMapping != null && synchronize.packingMapping.Length > 0)
            {
                query = query + Conjunction + " [Packing Mapping] = " + synchronize.packingMapping;
                Conjunction = " , ";
            }
            if (synchronize.scannedPackingHeader != null && synchronize.scannedPackingHeader.Length > 0)
            {
                query = query + Conjunction + " [Scanned Packing Header] = " + synchronize.scannedPackingHeader;
                Conjunction = " , ";
            }
            if (synchronize.scannedPackingLine != null && synchronize.scannedPackingLine.Length > 0)
            {
                query = query + Conjunction + " [Scanned Packing Line] = " + synchronize.scannedPackingLine;
                Conjunction = " , ";
            }
            if (synchronize.scannedPackingMapping != null && synchronize.scannedPackingMapping.Length > 0)
            {
                query = query + Conjunction + " [Scanned Packing Mapping] = " + synchronize.scannedPackingMapping;
                Conjunction = " , ";
            }
            if (synchronize.closedPrescan != null && synchronize.closedPrescan.Length > 0)
            {
                query = query + Conjunction + " [Closed Prescan] = " + synchronize.closedPrescan;
                Conjunction = " , ";
            }
            if (synchronize.closedPrescanOuterCarton != null && synchronize.closedPrescanOuterCarton.Length > 0)
            {
                query = query + Conjunction + " [Closed Prescan Outer Carton] = " + synchronize.closedPrescanOuterCarton;
                Conjunction = " , ";
            }
            if (synchronize.closedPrescanInnerCarton != null && synchronize.closedPrescanInnerCarton.Length > 0)
            {
                query = query + Conjunction + " [Closed Prescan Inner Carton] = " + synchronize.closedPrescanInnerCarton;
                Conjunction = " , ";
            }
            if (synchronize.labelHeader != null && synchronize.labelHeader.Length > 0)
            {
                query = query + Conjunction + " [Label Header] = " + synchronize.labelHeader;
                Conjunction = " , ";
            }
            if (synchronize.labelLine != null && synchronize.labelLine.Length > 0)
            {
                query = query + Conjunction + " [Label Line] = " + synchronize.labelLine;
                Conjunction = " , ";
            }
            if (synchronize.entriesProcess != null && synchronize.entriesProcess.Length > 0)
            {
                query = query + Conjunction + " [Entries Process] = " + synchronize.entriesProcess;
                Conjunction = " , ";
            }
            query = query + " WHERE ([MAC Address] = '" + synchronize.MACAddress + "' )";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }

        /// <summary>依 MAC Address 刪除一筆 Synchronize。</summary>
        public int Delete(Synchronize synchronize)
        {
            if (synchronize == null || string.IsNullOrEmpty(synchronize.MACAddress)) return 0;
            OpenSQLConnection();
            try
            {
                const string sql = "DELETE FROM [dbo].[Synchronize] WHERE [MAC Address] = @mac";
                var cmd = new SqlCommand(sql, sqlconn);
                cmd.Parameters.AddWithValue("@mac", synchronize.MACAddress);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                sqlconn.Close();
            }
        }
    }
}

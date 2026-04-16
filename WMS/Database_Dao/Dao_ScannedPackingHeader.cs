using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Database_Dao
{
    public class Dao_ScannedPackingHeader
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        public List<ScannedPackingHeader> Select(ScannedPackingHeader scannedPackingHeader)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Scanned Packing Header] ";
            if (!string.IsNullOrEmpty(scannedPackingHeader.No))
            {
                query = query + "where [No_] = '" + scannedPackingHeader.No + "'";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingHeader> data = new List<ScannedPackingHeader>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingHeader item = GetItem<ScannedPackingHeader>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        private static ScannedPackingHeader GetItem<T>(DataRow dr)
        {
            ScannedPackingHeader synchronize = new ScannedPackingHeader();
            Type temp = typeof(ScannedPackingHeader);
            ScannedPackingHeader obj = Activator.CreateInstance<ScannedPackingHeader>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    var dp = pro.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault();
                    var name1 = "";
                    if (dp != null)
                        name1 = string.Join("", dp.DisplayName.Split(' ', '.', '_', '-')).ToUpper();
                    else
                        name1 = pro.Name.ToUpper();
                    var name2 = pro.Name.ToUpper();
                    if ((name1 == string.Join("", column.ColumnName.Split(' ', '.', '_', '-')).ToUpper()) || (name2 == string.Join("", column.ColumnName.Split(' ', '.', '_', '-')).ToUpper()))
                    {
                        var propertyType = pro.PropertyType;
                        if ((dr[column.ColumnName].ToString() != null) & (dr[column.ColumnName].ToString() != ""))
                        {
                            if (propertyType == typeof(string))
                            {
                                pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                            else if (propertyType.IsEnum)
                            {
                                var convertedValue = Enum.Parse(propertyType, dr[column.ColumnName].ToString(), true);
                                pro.SetValue(obj, convertedValue, null);
                            }
                            else if (typeof(IConvertible).IsAssignableFrom(propertyType))
                            {
                                var convertedValue = Convert.ChangeType(dr[column.ColumnName], propertyType, null);
                                pro.SetValue(obj, convertedValue, null);
                            }
                        }
                        else
                        {
                            if ((propertyType.ToString() == "System.DateTime"))
                            {
                                var convertedValue = Convert.ChangeType((DateTime)SqlDateTime.MinValue, propertyType, null);
                                pro.SetValue(obj, convertedValue, null);
                            }
                            if (propertyType == typeof(string))
                            {
                                pro.SetValue(obj, "", null);
                            }
                        }
                    }
                    else
                        continue;
                }
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.PropertyType == typeof(string))
                    {
                        if (pro.GetValue(obj) == null)
                            pro.SetValue(obj, "", null);
                    }
                }
            }
            return obj;
        }
        public List<ScannedPackingHeader> SelectScannedPackingHeader_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Scanned Packing Header] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingHeader> data = new List<ScannedPackingHeader>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingHeader item = GetItem<ScannedPackingHeader>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(ScannedPackingHeader data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Scanned Packing Header] VALUES (DEFAULT, N'" +
                           data.No + "',N'" +
                           data.BilltoCustomerNo + "',N'" +
                           data.BilltoName + "',N'" +
                           data.BilltoName2 + "'," +
                           data.TotalCartons + ",N'" +
                           data.CustomerGroup + "'," +
                           "@datetime1" + "," +
                           "@boo1" + "," +
                           "@boo2" + ")";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(data.LastUpdatedDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@boo1", (data.Stop ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (data.Finish ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }

        /// <summary>更新一筆，以 No 為鍵。</summary>
        public int Update(ScannedPackingHeader updateFrom, ScannedPackingHeader updateTo)
        {
            if (updateFrom == null || updateTo == null || string.IsNullOrEmpty(updateFrom.No)) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand(
                    "UPDATE [dbo].[Scanned Packing Header] SET [Total Cartons] = @totalCartons, [Customer Group] = @cg, [Last Updated Date Time] = @dt, [Stop] = @stop, [Finish] = @finish WHERE [No_] = @keyNo",
                    sqlconn);
                cmd.Parameters.AddWithValue("@keyNo", updateFrom.No);
                cmd.Parameters.AddWithValue("@totalCartons", updateTo.TotalCartons);
                cmd.Parameters.AddWithValue("@cg", (object)updateTo.CustomerGroup ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dt", updateTo.LastUpdatedDateTime);
                cmd.Parameters.AddWithValue("@stop", updateTo.Stop ? 1 : 0);
                cmd.Parameters.AddWithValue("@finish", updateTo.Finish ? 1 : 0);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }

        /// <summary>依 No 刪除一筆。</summary>
        public int Delete(ScannedPackingHeader data)
        {
            if (data == null || string.IsNullOrEmpty(data.No)) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("DELETE FROM [dbo].[Scanned Packing Header] WHERE [No_] = @no", sqlconn);
                cmd.Parameters.AddWithValue("@no", data.No);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }
    }
}

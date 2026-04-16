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
    public class Dao_ScannedPackingMapping
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static ScannedPackingMapping GetItem<T>(DataRow dr)
        {
            ScannedPackingMapping synchronize = new ScannedPackingMapping();
            Type temp = typeof(ScannedPackingMapping);
            ScannedPackingMapping obj = Activator.CreateInstance<ScannedPackingMapping>();
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

        public List<ScannedPackingMapping> Select(ScannedPackingMapping scannedPackingMappingA)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Scanned Packing Mapping]";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(scannedPackingMappingA.PackingNo))
            {
                query = query + conjunction + " [Packing No_] = '" + scannedPackingMappingA.PackingNo + "'";
                conjunction = " AND ";
            }
            if (!string.IsNullOrEmpty(scannedPackingMappingA.PrescanNo))
            {
                query = query + conjunction + " [Prescan No_] = '" + scannedPackingMappingA.PrescanNo + "'";
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingMapping> data = new List<ScannedPackingMapping>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingMapping item = GetItem<ScannedPackingMapping>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<ScannedPackingMapping> SelectScannedPackingMapping_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Scanned Packing Mapping] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingMapping> data = new List<ScannedPackingMapping>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingMapping item = GetItem<ScannedPackingMapping>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(ScannedPackingMapping data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Scanned Packing Mapping] VALUES (DEFAULT, N'" +
                           data.PackingNo + "'," +
                           data.PrescanNo + ",N'" +
                           data.CreateUser + "'," +
                           "@datetime1" + ",N'" +
                           data.LastModifyUser + "'," +
                           "@datetime2" + ")";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            Console.WriteLine(query);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(data.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(data.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));

            int effectedRows = cmd.ExecuteNonQuery();

            sqlconn.Close();
            return effectedRows;
        }

        /// <summary>更新一筆，以 PackingNo + PrescanNo 為鍵。</summary>
        public int Update(ScannedPackingMapping updateFrom, ScannedPackingMapping updateTo)
        {
            if (updateFrom == null || updateTo == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand(
                    "UPDATE [dbo].[Scanned Packing Mapping] SET [Packing No_] = @packNo, [Prescan No_] = @prescanNo, [Create User] = @createUser, [Creation Date] = @createDate, [Last Modify User] = @lastUser, [Last Modify Date] = @lastDate WHERE [Packing No_] = @keyPack AND [Prescan No_] = @keyPrescan",
                    sqlconn);
                cmd.Parameters.AddWithValue("@keyPack", updateFrom.PackingNo);
                cmd.Parameters.AddWithValue("@keyPrescan", updateFrom.PrescanNo);
                cmd.Parameters.AddWithValue("@packNo", (object)updateTo.PackingNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@prescanNo", (object)updateTo.PrescanNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createUser", (object)updateTo.CreateUser ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createDate", updateTo.CreationDate);
                cmd.Parameters.AddWithValue("@lastUser", (object)updateTo.LastModifyUser ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lastDate", updateTo.LastModifyDate);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }

        /// <summary>依 PackingNo + PrescanNo 刪除一筆。</summary>
        public int Delete(ScannedPackingMapping data)
        {
            if (data == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("DELETE FROM [dbo].[Scanned Packing Mapping] WHERE [Packing No_] = @packNo AND [Prescan No_] = @prescanNo", sqlconn);
                cmd.Parameters.AddWithValue("@packNo", (object)data.PackingNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@prescanNo", (object)data.PrescanNo ?? DBNull.Value);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }
    }
}

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
    public class Dao_ClosedPrescan
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static ClosedPrescan GetItem<T>(DataRow dr)
        {
            ClosedPrescan synchronize = new ClosedPrescan();
            Type temp = typeof(ClosedPrescan);
            ClosedPrescan obj = Activator.CreateInstance<ClosedPrescan>();
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
        public List<ClosedPrescan> Select(ClosedPrescan closedPrescan = null)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Closed Prescan] ";
            if (!string.IsNullOrEmpty(closedPrescan.DocumentNo))
            {
                query = query + " WHERE [Document No_] = '" + closedPrescan.DocumentNo + "'";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ClosedPrescan> data = new List<ClosedPrescan>();
            foreach (DataRow row in dt.Rows)
            {
                ClosedPrescan item = GetItem<ClosedPrescan>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<ClosedPrescan> SelectClosedPrescan_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Closed Prescan] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ClosedPrescan> data = new List<ClosedPrescan>();
            foreach (DataRow row in dt.Rows)
            {
                ClosedPrescan item = GetItem<ClosedPrescan>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(ClosedPrescan closedPrescan)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Closed Prescan] VALUES (DEFAULT, N'" +
                           closedPrescan.DocumentNo + "',N'" +
                           closedPrescan.Type + "',N'" +
                           closedPrescan.CustomerGroup + "',N'" +
                           closedPrescan.CreateUser + "'," +
                           "@datetime1,N'" +
                           closedPrescan.LastModifyUser + "'," +
                           "@datetime2," +
                           "@boo1 ," +
                           "@boo2 ,N'" +
                           closedPrescan.ClosedUser +"' ," +
                           "@datetime3)";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(closedPrescan.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(closedPrescan.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@boo1", (closedPrescan.Suspend ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (closedPrescan.Finish ? 1 : 0));
            cmd.Parameters.AddWithValue("@datetime3", Convert.ToDateTime(closedPrescan.ClosedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));

            int effectedRows = cmd.ExecuteNonQuery();

            sqlconn.Close();
            return effectedRows;
        }

        /// <summary>更新一筆，以 DocumentNo 為鍵。</summary>
        public int Update(ClosedPrescan updateFrom, ClosedPrescan updateTo)
        {
            if (updateFrom == null || updateTo == null || string.IsNullOrEmpty(updateFrom.DocumentNo)) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand(
                    "UPDATE [dbo].[Closed Prescan] SET [Document No_] = @docNo, [Type] = @type, [Customer Group] = @cg, [Last Modify User] = @lastUser, [Last Modify Date] = @lastDate, [Suspend] = @suspend, [Finish] = @finish WHERE [Document No_] = @keyDoc",
                    sqlconn);
                cmd.Parameters.AddWithValue("@keyDoc", updateFrom.DocumentNo);
                cmd.Parameters.AddWithValue("@docNo", (object)updateTo.DocumentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@type", (object)updateTo.Type ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cg", (object)updateTo.CustomerGroup ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lastUser", (object)updateTo.LastModifyUser ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lastDate", updateTo.LastModifyDate);
                cmd.Parameters.AddWithValue("@suspend", updateTo.Suspend ? 1 : 0);
                cmd.Parameters.AddWithValue("@finish", updateTo.Finish ? 1 : 0);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }

        /// <summary>依 DocumentNo 刪除一筆。</summary>
        public int Delete(ClosedPrescan data)
        {
            if (data == null || string.IsNullOrEmpty(data.DocumentNo)) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("DELETE FROM [dbo].[Closed Prescan] WHERE [Document No_] = @docNo", sqlconn);
                cmd.Parameters.AddWithValue("@docNo", data.DocumentNo);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }
    }
}

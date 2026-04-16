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
    public class Dao_Prescan
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static Prescan GetItem<T>(DataRow dr)
        {
            Prescan synchronize = new Prescan();
            Type temp = typeof(Prescan);
            Prescan obj = Activator.CreateInstance<Prescan>();
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
        public List<Prescan> Select(Prescan prescan = null)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Prescan]";
            if (!string.IsNullOrEmpty(prescan.DocumentNo))
            {
                query = query + " WHERE [Document No_] = '" + prescan.DocumentNo + "'";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<Prescan> data = new List<Prescan>();
            foreach (DataRow row in dt.Rows)
            {
                Prescan item = GetItem<Prescan>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<Prescan> SelectPrescan_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Prescan] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<Prescan> data = new List<Prescan>();
            foreach (DataRow row in dt.Rows)
            {
                Prescan item = GetItem<Prescan>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Update(Prescan updateFrom, Prescan updateTo)
        {
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Prescan] " +
                  "SET [Document No_] = '" + updateTo.DocumentNo + "'," +
                  "[Type] = '" + updateTo.Type + "', " +
                  "[Customer Group] = '" + updateTo.CustomerGroup + "', " +
                  "[Create User] = '" + updateTo.CreateUser + "', " +
                  "[Creation Date] = @datetime1 , " +
                  "[Last Modify User] = '" + updateTo.LastModifyUser + "', " +
                  "[Last Modify Date] = @datetime2 , " +
                  "[Suspend] = @boo1 , " +
                  "[Finish] = @boo2  " + 
                  "WHERE [Document No_] = '" + updateFrom.DocumentNo + "'";

            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(updateTo.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(updateTo.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@boo1", (updateTo.Suspend ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (updateTo.Finish ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Insert(Prescan prescan)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Prescan] VALUES (DEFAULT, N'" +
                           prescan.DocumentNo + "',N'" +
                           prescan.Type + "',N'" +
                           prescan.CustomerGroup + "',N'" +
                           prescan.CreateUser + "'," +
                           "@datetime1  ,N'" +
                           prescan.LastModifyUser + "'," +
                            "@datetime2 ," +
                           "@boo1," +
                            "@boo2)";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(prescan.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(prescan.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@boo1", (prescan.Suspend ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (prescan.Finish ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(Prescan item)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Prescan] WHERE [Document No_] = '" + item.DocumentNo + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

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
    public class Dao_PackingMapping
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static PackingMapping GetItem<T>(DataRow dr)
        {
            PackingMapping synchronize = new PackingMapping();
            Type temp = typeof(PackingMapping);
            PackingMapping obj = Activator.CreateInstance<PackingMapping>();
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
        public List<PackingMapping> Select(PackingMapping packingMappingA = null)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Packing Mapping]";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(packingMappingA.PackingNo))
            {
                query = query + conjunction + " [Packing No_] = '" + packingMappingA.PackingNo + "'";
                conjunction = " AND ";
            }
            if (!string.IsNullOrEmpty(packingMappingA.PrescanNo))
            {
                query = query + conjunction + " [Prescan No_] = '" + packingMappingA.PrescanNo + "'";
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<PackingMapping> data = new List<PackingMapping>();
            foreach (DataRow row in dt.Rows)
            {
                PackingMapping item = GetItem<PackingMapping>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<PackingMapping> SelectPackingMapping_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Packing Mapping] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<PackingMapping> data = new List<PackingMapping>();
            foreach (DataRow row in dt.Rows)
            {
                PackingMapping item = GetItem<PackingMapping>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Update(PackingMapping updateFrom, PackingMapping updateTo)
        {
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Packing Mapping] " +
                  "SET [Packing No_] = '" + updateTo.PackingNo + "'," +
                  "[Prescan No_] = " + updateTo.PrescanNo + ", " +
                  "[Create User] = '" + updateTo.CreateUser + "', " +
                  "[Creation Date] = @datetime1 , " +
                  "[Last Modify User] = '" + updateTo.LastModifyUser + "', " +
                  "[Last Modify Date] = @datetime2  " +
                  "WHERE [Packing No_] = '" + updateFrom.PackingNo + "' " ;

            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(updateTo.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(updateTo.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Insert(PackingMapping data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Packing Mapping] VALUES (DEFAULT, N'" +
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
        public int Delete(PackingMapping data)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Packing Mapping] WHERE [Packing No_] = '" + data.PackingNo + "'" ;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

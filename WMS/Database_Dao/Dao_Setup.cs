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
    public class Dao_Setup
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }

        private static Setup GetItem<T>(DataRow dr)
        {
            Setup synchronize = new Setup();
            Type temp = typeof(Setup);
            Setup obj = Activator.CreateInstance<Setup>();
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
        public int Insert(Setup item)
        {
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("INSERT INTO [dbo].[Setup] ([Primary Key], [Excel Path]) VALUES (@pk, @path)", sqlconn);
                cmd.Parameters.AddWithValue("@pk", (object)item.PrimaryKey ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@path", (object)item.ExcelPath ?? DBNull.Value);
                return cmd.ExecuteNonQuery();
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
        public int Update(Setup itemA, Setup itemB)
        {
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("UPDATE [dbo].[Setup] SET [Excel Path] = @path WHERE [Primary Key] = @pk", sqlconn);
                cmd.Parameters.AddWithValue("@pk", itemA.PrimaryKey);
                cmd.Parameters.AddWithValue("@path", (object)itemB.ExcelPath ?? DBNull.Value);
                return cmd.ExecuteNonQuery();
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
        public int Delete(Setup item)
        {
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("DELETE FROM [dbo].[Setup] WHERE [Primary Key] = @pk", sqlconn);
                cmd.Parameters.AddWithValue("@pk", (object)item.PrimaryKey ?? DBNull.Value);
                return cmd.ExecuteNonQuery();
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
        public List<Setup> Select(Setup itemA)
        {
            OpenSQLConnection();
            try
            {
                string query = "SELECT * FROM [dbo].[Setup]";
                if (!string.IsNullOrEmpty(itemA?.PrimaryKey))
                    query += " WHERE [Primary Key] = @pk";
                var adapter = new SqlDataAdapter(query, sqlconn);
                if (!string.IsNullOrEmpty(itemA?.PrimaryKey))
                    adapter.SelectCommand.Parameters.AddWithValue("@pk", itemA.PrimaryKey);
                var dt = new DataTable();
                adapter.Fill(dt);
                var data = new List<Setup>();
                foreach (DataRow row in dt.Rows)
                    data.Add(GetItem<Setup>(row));
                return data;
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
    }
}

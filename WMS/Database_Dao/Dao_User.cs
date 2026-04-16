using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using ExtensionMethods;


namespace WMS.Database_Dao
{
    public class Dao_User
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;
        Byte[] stimestamp ;
        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }

        public User GetItem<T>(DataRow dr)
        {
            User user = new User();
            Type temp = typeof(User);
            User obj = Activator.CreateInstance<User>();

            Byte[] btimestamp = { 0, 0, 0, 0, 0, 0, 0, 0 };
            string tsA = "";
            string tsB = "";
            if (stimestamp != null)
            tsA = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            if (dr["timestamp"].GetType().ToString() == "System.String")
            {
                tsB = dr["timestamp"].ToString();
            }
            else
            {
                btimestamp = (Byte[])dr["timestamp"];
                tsB = "0x" + String.Join("", btimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            }
            Console.WriteLine("Old: " + tsA + "     ::: New: " + tsB + "      Compare: " + String.Compare(tsA, tsB));
            if ((String.Compare(tsA, tsB) < 0) || (String.IsNullOrEmpty(tsA)))
            {
                stimestamp = btimestamp.ToArray();
            }

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
                        //pro.SetValue(obj, dr[column.ColumnName], null);
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
        public byte[] gettimestamp()
        {
            return stimestamp;
        }

        public List<User> Select(User userA)
        {
            if (stimestamp != null)
                Array.Clear(stimestamp, 0, stimestamp.Length);
            Dao_Synchronize daoSynchronize = new Dao_Synchronize();
            OpenSQLConnection();
            try
            {
                string query = "SELECT * FROM [dbo].[User]";
                if (userA != null && !string.IsNullOrEmpty(userA.UserID))
                    query += " WHERE [User ID] = @userid";
                var adapter = new SqlDataAdapter(query, sqlconn);
                if (userA != null && !string.IsNullOrEmpty(userA.UserID))
                    adapter.SelectCommand.Parameters.AddWithValue("@userid", userA.UserID);
                var dt = new DataTable();
                adapter.Fill(dt);
                var data = new List<User>();
                foreach (DataRow row in dt.Rows)
                    data.Add(GetItem<User>(row));
                return data;
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
        public List<User> SelectUser_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            try
            {
                string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
                string query = "SELECT * FROM [dbo].[User] WHERE [timestamp] > " + ts;
                var adapter = new SqlDataAdapter(query, sqlconn);
                var dt = new DataTable();
                adapter.Fill(dt);
                var data = new List<User>();
                foreach (DataRow row in dt.Rows)
                    data.Add(GetItem<User>(row));
                return data;
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }

        public int Update(String UpdateUser, User user)
        {
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("UPDATE [dbo].[User] SET [User ID] = @userid, [Password] = @pwd, [Last Modify User] = @lmu, [Last Modify Date] = @datetime WHERE [User ID] = @key", sqlconn);
                cmd.Parameters.AddWithValue("@key", UpdateUser);
                cmd.Parameters.AddWithValue("@userid", (object)user.UserID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pwd", (object)user.Password ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lmu", (object)user.LastModifyUser ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@datetime", user.LastModifyDate);
                return cmd.ExecuteNonQuery();
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
        public int Insert(User newuser)
        {
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("INSERT INTO [dbo].[User]([User ID],[Password],[Create User],[Creation Date],[Last Modify User],[Last Modify Date]) VALUES (@userid,@pwd,@cu,@cd,@lmu,@lmd)", sqlconn);
                cmd.Parameters.AddWithValue("@userid", (object)newuser.UserID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pwd", (object)newuser.Password ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cu", (object)newuser.CreateUser ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cd", newuser.CreationDate);
                cmd.Parameters.AddWithValue("@lmu", (object)newuser.LastModifyUser ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lmd", newuser.LastModifyDate);
                return cmd.ExecuteNonQuery();
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
        public int Delete(User user)
        {
            OpenSQLConnection();
            try
            {
                var icmd = new SqlCommand("INSERT INTO [Entries Process]([Table],Action,Key1) VALUES('User','Delete',@key1)", sqlconn);
                icmd.Parameters.AddWithValue("@key1", (object)user.UserID ?? DBNull.Value);
                icmd.ExecuteNonQuery();
                var cmd = new SqlCommand("DELETE FROM [dbo].[User] WHERE [User ID] = @userid", sqlconn);
                cmd.Parameters.AddWithValue("@userid", (object)user.UserID ?? DBNull.Value);
                return cmd.ExecuteNonQuery();
            }
            finally { try { sqlconn?.Close(); } catch { } }
        }
    }
}

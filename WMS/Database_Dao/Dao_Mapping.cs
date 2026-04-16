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
    public class Dao_Mapping
    {

        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }

        String TableName = "[dbo].[Mapping]";
        public int Insert(Mapping mapping)
        {
            OpenSQLConnection();
            string query = "INSERT INTO  " + TableName +
                "([No_],[Item No_],[Scan Item No_],[Cross Reference No_],[Creation Date],[Create User],[Last Modify Date],[Last Modify User], " +
                "Description, Vendor,  MSL, BAND, Spare1, Spare2, Spare3)" +
                " VALUES( " + mapping.No + ",'" + mapping.ItemNo + "','" + mapping.ScanItemNo + "','" + mapping.CrossReferenceNo + "'," +
                              " @datetime1 ,'" + mapping.CreateUser + "', @datetime2 ,'" + mapping.LastModifyUser + "','" +
                              mapping.Description + "','" + mapping.Vendor + "','" + mapping.MSL + "','" + mapping.BAND + "','" + 
                              mapping.Spare1 + "','" + mapping.Spare2 + "','" + mapping.Spare3 + "')";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(mapping.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(mapping.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Update(int No, Mapping mapping)
        {
            //inset , delete , update
            OpenSQLConnection();
            string query = "UPDATE " + TableName + " " +
                "SET [Item No_] = '" + mapping.ItemNo + "' , [Scan Item No_] = '" + mapping.ScanItemNo + "' , [Cross Reference No_] = '" + mapping.CrossReferenceNo +
                "' , [Last Modify Date] = @datetime , [Last Modify User] = '" + mapping.LastModifyUser + "'" +
                ",Description = '" + mapping.Description + "',Vendor = '" + mapping.Vendor + "',MSL = '" + mapping.MSL +
                "',BAND = '" + mapping.BAND + "',Spare1 = '" + mapping.Spare1 + "',Spare2 = '" + mapping.Spare2 + "',Spare3 = '" + mapping.Spare3 +
                "WHERE [No_] = '" + No + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime", Convert.ToDateTime(mapping.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(Mapping mapping)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string iquery = "Insert into [Entries Process]([Table],Action,Key1) VALUES('"+ TableName+"','Delete','" + mapping.No + "')";
            SqlCommand icmd = new SqlCommand(iquery, sqlconn);
            icmd.ExecuteNonQuery();
            string query = "DELETE FROM " + TableName + " WHERE [No_] = " + mapping.No;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public DataTable MappingList()
        {
            //read
            OpenSQLConnection();
            string query = "Select * from " + TableName;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            sqlconn.Close();
            return dt;
        }
        public List<Mapping> Select(Mapping mapping=null)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Mapping]";
            string conjunction = " where ";
            if (mapping.No > 0)
            {
                query = query + conjunction + " [No_] = '" + mapping.No + "'";
                conjunction = " and ";
            }
            if (!string.IsNullOrEmpty(mapping.ItemNo))
            {
                query = query + conjunction + " [Item No_] = '" + mapping.ItemNo + "'";
                conjunction = " and ";
            }
            if (!string.IsNullOrEmpty(mapping.ScanItemNo))
            {
                query = query + conjunction + " [Scan Item No_] = '" + mapping.ScanItemNo + "'";
                conjunction = " and ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<Mapping> data = new List<Mapping>();
            foreach (DataRow row in dt.Rows)
            {
                Mapping item = GetItem<Mapping>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }

        public List<Mapping> SelectMapping_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Mapping] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<Mapping> data = new List<Mapping>();
            foreach (DataRow row in dt.Rows)
            {
                Mapping item = GetItem<Mapping>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        
        private static Mapping GetItem<T>(DataRow dr)
        {
            Mapping synchronize = new Mapping();
            Type temp = typeof(Mapping);
            Mapping obj = Activator.CreateInstance<Mapping>();
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
    }
}

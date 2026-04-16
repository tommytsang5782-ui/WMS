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
    public class Dao_LabelHeader
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        String TableName = "[dbo].[Label Header]";
        public int Insert(LabelHeader labelHeader)
        {
            OpenSQLConnection();
            string query = "INSERT INTO  " + TableName + " " +
                "([Code],[Description],[Create User],[Creation Date],[Last Modify User],[Last Modify Date],[Width],[Length]," +
                "[Gap Distance],[Offset Distance],[Quantity],[Copy],[Timeout])" +
                " VALUES( '" + labelHeader.Code + "','" + labelHeader.Description + "','" + labelHeader.CreateUser + "', @datetime1 ,'" + labelHeader.LastModifyUser +
                          "', @datetime2 ," + labelHeader.Width + "," + labelHeader.Length + "," + labelHeader.GapDistance + "," + labelHeader.OffsetDistance
                           + "," + labelHeader.Quantity + "," + labelHeader.Copy + "," + labelHeader.Timeout + ")";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(labelHeader.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(labelHeader.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Update(String code, LabelHeader labelHeader)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string query = "UPDATE " + TableName + " " +
                "SET [Code] = '" + labelHeader.Code + "', [Description] = '" + labelHeader.Description + "', [Last Modify User] = '" + labelHeader.LastModifyUser +
                          "',[Last Modify Date] = @datetime2 , [Width] = " + labelHeader.Width + ", [Length] = " + labelHeader.Length + ", [Gap Distance] = " +
                          labelHeader.GapDistance + ", [Offset Distance] = " + labelHeader.OffsetDistance + ", [Quantity] = " + labelHeader.Quantity + ", [Copy] = " +
                          labelHeader.Copy + ", [Timeout] = " + labelHeader.Timeout +
                " WHERE [Code] = '" + code + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(labelHeader.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(LabelHeader labelHeader)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string iquery = "Insert into [Entries Process]([Table],Action,Key1) VALUES('" + TableName + "','Delete','" + labelHeader.Code + "')";
            SqlCommand icmd = new SqlCommand(iquery, sqlconn);
            icmd.ExecuteNonQuery();
            string query = "DELETE FROM " + TableName + " WHERE [Code] = '" + labelHeader.Code + "'";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        private static LabelHeader GetItem<T>(DataRow dr)
        {
            LabelHeader synchronize = new LabelHeader();
            Type temp = typeof(LabelHeader);
            LabelHeader obj = Activator.CreateInstance<LabelHeader>();
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
        public List<LabelHeader> SelectLabelHeader_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Label Header] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<LabelHeader> data = new List<LabelHeader>();
            foreach (DataRow row in dt.Rows)
            {
                LabelHeader item = GetItem<LabelHeader>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }

        public List<LabelHeader> Select(LabelHeader labelHeader)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from " + TableName;
            if (!string.IsNullOrEmpty(labelHeader.Code))
            {
                query += " WHERE [Code] = '" + labelHeader.Code + "'";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<LabelHeader> data = new List<LabelHeader>();
            foreach (DataRow row in dt.Rows)
            {
                LabelHeader item = GetItem<LabelHeader>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
    }
}

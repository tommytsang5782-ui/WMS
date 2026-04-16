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
    public class Dao_ScanLabelString
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static ScanLabelString GetItem<T>(DataRow dr)
        {
            ScanLabelString synchronize = new ScanLabelString();
            Type temp = typeof(ScanLabelString);
            ScanLabelString obj = Activator.CreateInstance<ScanLabelString>();
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
        public List<ScanLabelString> Select(ScanLabelString scanLabelString)
        {
            //read
            OpenSQLConnection();
            string strConjunction = " WHERE ";
            string query = "Select * from [dbo].[Scan Label String]";
            if (scanLabelString.EntryNo > 0)
            {
                query += strConjunction + " [Entry No_] = " + scanLabelString.EntryNo;
                strConjunction = " , ";
            }
            if (!string.IsNullOrEmpty(scanLabelString.DocumentNo))
            {
                query += strConjunction + " [Document No_] = '" + scanLabelString.DocumentNo + "' ";
                strConjunction = " , ";
            }
            if (scanLabelString.DocumentLineNo > 0)
            {
                query += strConjunction + " [Document Line No_] = " + scanLabelString.DocumentLineNo;
                strConjunction = " , ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScanLabelString> data = new List<ScanLabelString>();
            foreach (DataRow row in dt.Rows)
            {
                ScanLabelString item = GetItem<ScanLabelString>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<ScanLabelString> SelectScanLabelString_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Scan Label String] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScanLabelString> data = new List<ScanLabelString>();
            foreach (DataRow row in dt.Rows)
            {
                ScanLabelString item = GetItem<ScanLabelString>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }

        public int Update(int UpdateLabelStringNo, ScanLabelString scanLabelString)
        {
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Scan Label String] " +
            "SET [Label String] = '" + scanLabelString.LabelString + "'," +
            "[Document No_] = '" + scanLabelString .DocumentNo + "'," +
            "[Document Line No_] = " + scanLabelString.DocumentLineNo + ", " +
            "[Prescan] = @boo1 ," +
            "[Last Modify User] = '" + scanLabelString.LastModifyUser + "'," +
            "[Last Modify Date] = @datetime1 ," +
            "[Carton ID] = '" + scanLabelString.CartonID + "' " +
            "WHERE ([Entry No_] = " + UpdateLabelStringNo + ")";

            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@boo1", (scanLabelString.Prescan ? 1 : 0));
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(scanLabelString.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Insert(ScanLabelString data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Scan Label String] VALUES (DEFAULT, " +
                           data.EntryNo + ",N'" +
                           data.LabelString + "',N'" +
                           data.DocumentNo + "'," +
                           data.DocumentLineNo + "," +
                           "@boo1" + ",N'" +
                           data.CreateUser + "'," +
                           "@datetime1" + ",N'" +
                           data.CreateUser + "'," +
                           "@datetime1" + ",N'" +
                           data.CreateUser + "'," +
                           "@boo2" + ")";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@datetime1", Convert.ToDateTime(data.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@datetime2", Convert.ToDateTime(data.LastModifyDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.AddWithValue("@boo1", (data.Prescan ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (data.Closed ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(ScanLabelString data)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Scan Label String] WHERE [Entry No_] = " + data.EntryNo;
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

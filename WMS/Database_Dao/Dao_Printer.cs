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
    public class Dao_Printer
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static Printer GetItem<T>(DataRow dr)
        {
            Printer synchronize = new Printer();
            Type temp = typeof(Printer);
            Printer obj = Activator.CreateInstance<Printer>();
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
        public List<Printer> Select(Printer printer = null)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Printer] ";
            if (!string.IsNullOrEmpty(printer.Code))
            {
                query = query + " WHERE [Code] = '" + printer.Code + "'";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<Printer> data = new List<Printer>();
            foreach (DataRow row in dt.Rows)
            {
                Printer item = GetItem<Printer>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<Printer> SelectPrinter_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * FROM [dbo].[Printer] Where timestamp > " + ts;
            //if (Code != null)
            //    query = query + " WHERE [Code] = '" + Code + "'";
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<Printer> data = new List<Printer>();
            foreach (DataRow row in dt.Rows)
            {
                Printer item = GetItem<Printer>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }

        public int Insert(Printer printer)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Printer]([Code],[Description],IP,Port) VALUES ('" +
                           printer.Code + "','" +
                           printer.Description + "','" +
                           printer.IP + "'," +
                           printer.Port + ")";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Update(String UpdatePrinterCode, Printer printer)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Printer] " +
                "SET [Code] = '" + printer.Code + "', " +
                "Description = '" + printer.Description + "', " +
                "IP = '" + printer.IP + "', " +
                "Port = " + printer.Port + " " +
                "WHERE [Code] = '" + UpdatePrinterCode + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(Printer printer)
        {
            OpenSQLConnection();
            string iquery = "Insert into [Entries Process]([Table],Action,Key1) VALUES('Printer','Delete','" + printer.Code + "')";
            SqlCommand icmd = new SqlCommand(iquery, sqlconn);
            icmd.ExecuteNonQuery();
            string query = "DELETE FROM [dbo].[Printer] WHERE [Code] = '" + printer.Code + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

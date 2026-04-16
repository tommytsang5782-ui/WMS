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
    public class Dao_CustomerGroup
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static CustomerGroup GetItem<T>(DataRow dr)
        {
            CustomerGroup synchronize = new CustomerGroup();
            Type temp = typeof(CustomerGroup);
            CustomerGroup obj = Activator.CreateInstance<CustomerGroup>();
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
        public List<CustomerGroup> Select(CustomerGroup customerGroup = null)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Customer Group] ";
            if (!string.IsNullOrEmpty(customerGroup.Code))
            {
                query = query + " WHERE [Code] = '" + customerGroup.Code + "'";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<CustomerGroup> data = new List<CustomerGroup>();
            foreach (DataRow row in dt.Rows)
            {
                CustomerGroup item = GetItem<CustomerGroup>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<CustomerGroup> SelectCustomerGroup_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Customer Group] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<CustomerGroup> data = new List<CustomerGroup>();
            foreach (DataRow row in dt.Rows)
            {
                CustomerGroup item = GetItem<CustomerGroup>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(CustomerGroup customerGroup)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Customer Group]([Code],[Description],[Big Label URL],[Small Label URL]) VALUES ('" +
                           customerGroup.Code + "',N'" +
                           customerGroup.Description + "',N'" +
                           customerGroup.BigLabelURL + "',N'" +
                           customerGroup.SmallLabelURL + "')";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            Console.WriteLine(query);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Update(String UpdateCGCode, CustomerGroup customerGroup)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Customer Group] " +
                "SET [Code] = '" + customerGroup.Code + "' , [Description] = N'" + customerGroup.Description +
                "' , [Big Label URL] = N'" + customerGroup.BigLabelURL + "',[Small Label URL] = N'" + customerGroup.SmallLabelURL +
                "' WHERE [Code] = '" + UpdateCGCode + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(CustomerGroup customerGroup)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string iquery = "Insert into [Entries Process]([Table],Action,Key1) VALUES('Customer Group','Delete','" + customerGroup.Code + "')";
            SqlCommand icmd = new SqlCommand(iquery, sqlconn);
            icmd.ExecuteNonQuery();
            string query = "DELETE FROM [dbo].[Customer Group] WHERE [Code] = '" + customerGroup.Code + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

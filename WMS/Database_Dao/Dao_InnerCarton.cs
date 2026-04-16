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
    public class Dao_InnerCarton
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static InnerCarton GetItem<T>(DataRow dr)
        {
            InnerCarton synchronize = new InnerCarton();
            Type temp = typeof(InnerCarton);
            InnerCarton obj = Activator.CreateInstance<InnerCarton>();
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
        public List<InnerCarton> Select(InnerCarton innerCarton = null)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Inner Carton]";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(innerCarton.DocumentNo))
            {
                query = query + conjunction + " [Document No_] = '" + innerCarton.DocumentNo + "'";
                conjunction = " AND ";
            }
            if (innerCarton.DocumentLineNo > 0)
            {
                query = query + conjunction + " [Document Line No_] = " + innerCarton.DocumentLineNo;
                conjunction = " AND ";
            }

            if (innerCarton.OuterCartonLineNo > 0)
            {
                query = query + conjunction + " [Outer Carton Line No_] = " + innerCarton.OuterCartonLineNo;
                conjunction = " AND ";
            }
            if (innerCarton.LineNo > 0)
            {
                query = query + conjunction + " [Line No_] = " + innerCarton.LineNo;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<InnerCarton> data = new List<InnerCarton>();
            foreach (DataRow row in dt.Rows)
            {
                InnerCarton item = GetItem<InnerCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<InnerCarton> SelectInnerCarton_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Inner Carton] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<InnerCarton> data = new List<InnerCarton>();
            foreach (DataRow row in dt.Rows)
            {
                InnerCarton item = GetItem<InnerCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Update(InnerCarton updateFrom, InnerCarton updateTo)
        {
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Inner Carton] " +
                  "SET [Document No_] = '" + updateTo.DocumentNo + "'," +
                  "[Document Line No_] = " + updateTo.DocumentLineNo + ", " +
                  "[Outer Carton Line No_] = " + updateTo.OuterCartonLineNo + ", " +
                  "[Line No_] = " + updateTo.LineNo + ", " +
                  "[Big Carton ID] = '" + updateTo.BigCartonID + "', " +
                  "[Carton ID] = '" + updateTo.CartonID + "', " +
                  "[CS P/N] = '" + updateTo.CSPN + "', " +
                  "[Item No_] = '" + updateTo.ItemNo + "', " +
                  "[Date Code] = '" + updateTo.DateCode + "', " +
                  "[Lot No_] = " + updateTo.LotNo + ", " +
                  "[Quantity] = " + updateTo.Quantity + ", " +
                  "[Closed] = @boo1 , " +
                  "[Selected] = @boo2 , " +
                  "[Cross Reference No_] = '" + updateTo.CrossReferenceNo + "', " +
                  "[Seq No_] = " + updateTo.SeqNo + " " +
                  "WHERE [Document No_] = '" + updateFrom.DocumentNo + "' AND " +
                  "[Document Line No_] = " + updateFrom.DocumentLineNo + " AND " +
                  "[Line No_] = " + updateFrom.LineNo;

            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@boo1", (updateTo.Closed ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (updateTo.Selected ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Insert(InnerCarton data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Inner Carton] VALUES (DEFAULT, N'" +
                           data.DocumentNo + "'," +
                           data.OuterCartonLineNo + "," +
                           data.LineNo + "," +
                           data.NoOfCarton + ",N'" +
                           data.CartonID + "',N'" +
                           data.CSPN + "',N'" +
                           data.ItemNo + "',N'" +
                           data.DateCode + "',N'" +
                           data.LotNo + "'," +
                           data.Quantity + "," +
                           "@boo1 ," +
                           "@boo2 ,N'" +
                           data.CrossReferenceNo + "'," +
                           data.SeqNo + ",N'" +
                           data.Description + "',N'" +
                           data.DCMMDD + "',N'" +
                           data.DCYYMMDD + "',N'" +
                           data.DCYYYYMMDD + "',N'" +
                           data.Vendor + "'," +
                           data.TotalCarton + "," +
                           data.MSL + ",N'" +
                           data.PO + "',N'" +
                           data.BAND + "',N'" +
                           data.Origin + "',N'" +
                           data.LabelDateMMDD + "',N'" +
                           data.LabelDateYYMMDD + "'," +
                           data.Morethatonelabel + ",N'" +
                           data.BigCartonID + "',N'" +
                           data.Spare1 + "',N'" +
                           data.Spare2 + "',N'" +
                           data.LabelDate + "')";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            Console.WriteLine(query);
            cmd.Parameters.AddWithValue("@boo1", (data.Closed ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (data.Selected ? 1 : 0));

            int effectedRows = cmd.ExecuteNonQuery();

            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(InnerCarton data)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Inner Carton] WHERE [Document No_] = '" + data.DocumentNo + "'," +
                  "[Document Line No_] = " + data.DocumentLineNo + ", " +
                  "[Outer Carton Line No_] = " + data.OuterCartonLineNo + ", " +
                  "[Line No_] = " + data.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

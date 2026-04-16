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
    public class Dao_ClosedPrescanOuterCarton
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static ClosedPrescanOuterCarton GetItem<T>(DataRow dr)
        {
            ClosedPrescanOuterCarton synchronize = new ClosedPrescanOuterCarton();
            Type temp = typeof(ClosedPrescanOuterCarton);
            ClosedPrescanOuterCarton obj = Activator.CreateInstance<ClosedPrescanOuterCarton>();
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
        public List<ClosedPrescanOuterCarton> Select(ClosedPrescanOuterCarton closedPrescanOuterCarton = null)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Closed Prescan Outer Carton]";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(closedPrescanOuterCarton.DocumentNo))
            {
                query = query + conjunction + " [Document No_] = '" + closedPrescanOuterCarton.DocumentNo + "'";
                conjunction = " AND ";
            }
            if (closedPrescanOuterCarton.LineNo > 0)
            {
                query = query + conjunction + " [Line No_] = " + closedPrescanOuterCarton.LineNo;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ClosedPrescanOuterCarton> data = new List<ClosedPrescanOuterCarton>();
            foreach (DataRow row in dt.Rows)
            {
                ClosedPrescanOuterCarton item = GetItem<ClosedPrescanOuterCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<ClosedPrescanOuterCarton> SelectClosedPrescanOuterCarton_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Closed Prescan Outer Carton] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ClosedPrescanOuterCarton> data = new List<ClosedPrescanOuterCarton>();
            foreach (DataRow row in dt.Rows)
            {
                ClosedPrescanOuterCarton item = GetItem<ClosedPrescanOuterCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(ClosedPrescanOuterCarton data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Closed Prescan Outer Carton] VALUES (DEFAULT, N'" +
                           data.DocumentNo + "'," +
                           data.LineNo + "," +
                           data.NoOfCarton + ",N'" +
                           data.CartonID + "',N'" +
                           data.CSPN + "',N'" +
                           data.ItemNo + "',N'" +
                           data.DateCode + "',N'" +
                           data.LotNo + "'," +
                           data.Quantity + "," +
                           "@boo1 ," +
                           data.SelectedQuantity + ",N'" +
                           data.CrossReferenceNo + "'," +
                           data.SeqNo + ",N'" +
                           data.DCMMDD + "',N'" +
                           data.DCYYMMDD + "',N'" +
                           data.DCYYYYMMDD + "',N'" +
                           data.Description + "',N'" +
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
            cmd.Parameters.AddWithValue("@boo1", (data.Closed ? 1 : 0));

            int effectedRows = cmd.ExecuteNonQuery();

            sqlconn.Close();
            return effectedRows;
        }

        /// <summary>更新一筆，以 DocumentNo + LineNo 為鍵。</summary>
        public int Update(ClosedPrescanOuterCarton updateFrom, ClosedPrescanOuterCarton updateTo)
        {
            if (updateFrom == null || updateTo == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand(
                    "UPDATE [dbo].[Closed Prescan Outer Carton] SET [Document No_] = @docNo, [Line No_] = @lineNo, [Closed] = @closed WHERE [Document No_] = @keyDoc AND [Line No_] = @keyLine",
                    sqlconn);
                cmd.Parameters.AddWithValue("@keyDoc", updateFrom.DocumentNo);
                cmd.Parameters.AddWithValue("@keyLine", updateFrom.LineNo);
                cmd.Parameters.AddWithValue("@docNo", (object)updateTo.DocumentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lineNo", updateTo.LineNo);
                cmd.Parameters.AddWithValue("@closed", updateTo.Closed ? 1 : 0);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }

        public int Delete(ClosedPrescanOuterCarton data)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Closed Prescan Outer Carton] WHERE [Document No_]= '" + data.DocumentNo + "' , [Document Line No_]=" + data.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

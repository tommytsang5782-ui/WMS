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
    public class Dao_ClosedPrescanInnerCarton
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static ClosedPrescanInnerCarton GetItem<T>(DataRow dr)
        {
            ClosedPrescanInnerCarton synchronize = new ClosedPrescanInnerCarton();
            Type temp = typeof(ClosedPrescanInnerCarton);
            ClosedPrescanInnerCarton obj = Activator.CreateInstance<ClosedPrescanInnerCarton>();
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
        public List<ClosedPrescanInnerCarton> Select(ClosedPrescanInnerCarton closedPrescanInnerCarton = null)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Closed Prescan Inner Carton]";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(closedPrescanInnerCarton.DocumentNo))
            {
                query = query + conjunction + " [Document No_] = '" + closedPrescanInnerCarton.DocumentNo + "'";
                conjunction = " AND ";
            }
            if (closedPrescanInnerCarton.OuterCartonLineNo > 0)
            {
                query = query + conjunction + " [Outer Carton Line No_] = " + closedPrescanInnerCarton.OuterCartonLineNo;
                conjunction = " AND ";
            }
            if (closedPrescanInnerCarton.LineNo > 0)
            {
                query = query + conjunction + " [Line No_] = " + closedPrescanInnerCarton.LineNo;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ClosedPrescanInnerCarton> data = new List<ClosedPrescanInnerCarton>();
            foreach (DataRow row in dt.Rows)
            {
                ClosedPrescanInnerCarton item = GetItem<ClosedPrescanInnerCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<ClosedPrescanInnerCarton> SelectClosedPrescanInnerCarton_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * FROM [dbo].[Closed Prescan Inner Carton] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ClosedPrescanInnerCarton> data = new List<ClosedPrescanInnerCarton>();
            foreach (DataRow row in dt.Rows)
            {
                ClosedPrescanInnerCarton item = GetItem<ClosedPrescanInnerCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(ClosedPrescanInnerCarton data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Closed Prescan Inner Carton] VALUES (DEFAULT, N'" +
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

        /// <summary>更新一筆，以 DocumentNo + OuterCartonLineNo + LineNo 為鍵。</summary>
        public int Update(ClosedPrescanInnerCarton updateFrom, ClosedPrescanInnerCarton updateTo)
        {
            if (updateFrom == null || updateTo == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand(
                    "UPDATE [dbo].[Closed Prescan Inner Carton] SET [Document No_] = @docNo, [Outer Carton Line No_] = @outerLine, [Line No_] = @lineNo, [Closed] = @closed, [Selected] = @selected WHERE [Document No_] = @keyDoc AND [Outer Carton Line No_] = @keyOuter AND [Line No_] = @keyLine",
                    sqlconn);
                cmd.Parameters.AddWithValue("@keyDoc", updateFrom.DocumentNo);
                cmd.Parameters.AddWithValue("@keyOuter", updateFrom.OuterCartonLineNo);
                cmd.Parameters.AddWithValue("@keyLine", updateFrom.LineNo);
                cmd.Parameters.AddWithValue("@docNo", (object)updateTo.DocumentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@outerLine", updateTo.OuterCartonLineNo);
                cmd.Parameters.AddWithValue("@lineNo", updateTo.LineNo);
                cmd.Parameters.AddWithValue("@closed", updateTo.Closed ? 1 : 0);
                cmd.Parameters.AddWithValue("@selected", updateTo.Selected ? 1 : 0);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }

        /// <summary>依 DocumentNo + OuterCartonLineNo + LineNo 刪除一筆。</summary>
        public int Delete(ClosedPrescanInnerCarton data)
        {
            if (data == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("DELETE FROM [dbo].[Closed Prescan Inner Carton] WHERE [Document No_] = @docNo AND [Outer Carton Line No_] = @outerLine AND [Line No_] = @lineNo", sqlconn);
                cmd.Parameters.AddWithValue("@docNo", (object)data.DocumentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@outerLine", data.OuterCartonLineNo);
                cmd.Parameters.AddWithValue("@lineNo", data.LineNo);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }
    }
}

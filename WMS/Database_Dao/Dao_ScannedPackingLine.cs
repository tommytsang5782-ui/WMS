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
    public class Dao_ScannedPackingLine
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        public List<ScannedPackingLine> Select(ScannedPackingLine scannedPackingLine)
        {
            OpenSQLConnection();
            string query = "Select * FROM [dbo].[Scanned Packing Line] ";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(scannedPackingLine.DocumentNo))
            {
                query = query + conjunction + " [Document No_] = '" + scannedPackingLine.DocumentNo + "'";
                conjunction = " AND ";
            }
            if (scannedPackingLine.LineNo > 0)
            {
                query = query + conjunction + " [Line No_] = " + scannedPackingLine.LineNo;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingLine> data = new List<ScannedPackingLine>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingLine item = GetItem<ScannedPackingLine>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        private static ScannedPackingLine GetItem<T>(DataRow dr)
        {
            ScannedPackingLine synchronize = new ScannedPackingLine();
            Type temp = typeof(ScannedPackingLine);
            ScannedPackingLine obj = Activator.CreateInstance<ScannedPackingLine>();
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
        public List<ScannedPackingLine> SelectScannedPackingLine_timestamp(Byte[] stimestamp)
        {
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Scanned Packing Line] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingLine> data = new List<ScannedPackingLine>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingLine item = GetItem<ScannedPackingLine>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<ScannedPackingLine> SelectScannedPackingLine_DocNo(ScannedPackingLine scannedPackingLine)
        {
            OpenSQLConnection();
            string query = "Select * from [dbo].[Scanned Packing Line] Where [Document No_] = '" + scannedPackingLine.DocumentNo + "'";
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<ScannedPackingLine> data = new List<ScannedPackingLine>();
            foreach (DataRow row in dt.Rows)
            {
                ScannedPackingLine item = GetItem<ScannedPackingLine>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Insert(ScannedPackingLine data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Scanned Packing Line] VALUES (DEFAULT, N'" +
                           data.DocumentNo + "'," +
                           data.LineNo + "," +
                           data.NumberOfCartons + ",N'" +
                           data.ItemNo + "',N'" +
                           data.CrossReferenceNo + "'," +
                           data.QuantityPerCarton + "," +
                           data.SubtotalQuantity + ",N'" +
                           data.CartonID + "')";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }

        /// <summary>更新一筆，以 DocumentNo + LineNo 為鍵。</summary>
        public int Update(ScannedPackingLine updateFrom, ScannedPackingLine updateTo)
        {
            if (updateFrom == null || updateTo == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand(
                    "UPDATE [dbo].[Scanned Packing Line] SET [Document No_] = @docNo, [Line No_] = @lineNo, [No_ of Cartons] = @nCartons, [Item No_] = @itemNo, [Cross Reference No_] = @crNo, [Quantity per Carton] = @qty, [Subtotal Quantity] = @sub, [Carton ID] = @cartonId WHERE [Document No_] = @keyDoc AND [Line No_] = @keyLine",
                    sqlconn);
                cmd.Parameters.AddWithValue("@keyDoc", updateFrom.DocumentNo);
                cmd.Parameters.AddWithValue("@keyLine", updateFrom.LineNo);
                cmd.Parameters.AddWithValue("@docNo", (object)updateTo.DocumentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lineNo", updateTo.LineNo);
                cmd.Parameters.AddWithValue("@nCartons", updateTo.NumberOfCartons);
                cmd.Parameters.AddWithValue("@itemNo", (object)updateTo.ItemNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@crNo", (object)updateTo.CrossReferenceNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@qty", updateTo.QuantityPerCarton);
                cmd.Parameters.AddWithValue("@sub", updateTo.SubtotalQuantity);
                cmd.Parameters.AddWithValue("@cartonId", (object)updateTo.CartonID ?? DBNull.Value);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }

        /// <summary>依 DocumentNo + LineNo 刪除一筆。</summary>
        public int Delete(ScannedPackingLine data)
        {
            if (data == null) return 0;
            OpenSQLConnection();
            try
            {
                var cmd = new SqlCommand("DELETE FROM [dbo].[Scanned Packing Line] WHERE [Document No_] = @docNo AND [Line No_] = @lineNo", sqlconn);
                cmd.Parameters.AddWithValue("@docNo", (object)data.DocumentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lineNo", data.LineNo);
                return cmd.ExecuteNonQuery();
            }
            finally { sqlconn.Close(); }
        }
    }
}

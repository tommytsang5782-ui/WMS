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
    public class Dao_PrescanInnerCarton
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static PrescanInnerCarton GetItem<T>(DataRow dr)
        {
            PrescanInnerCarton synchronize = new PrescanInnerCarton();
            Type temp = typeof(PrescanInnerCarton);
            PrescanInnerCarton obj = Activator.CreateInstance<PrescanInnerCarton>();
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
        public List<PrescanInnerCarton> Select(PrescanInnerCarton prescanInnerCarton = null)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Prescan Inner Carton]";
            string conjunction = " WHERE 1 = 1 AND ";
            if (!string.IsNullOrEmpty(prescanInnerCarton.DocumentNo))
            {
                query = query + conjunction + " [Document No_] = '" + prescanInnerCarton.DocumentNo + "'";
                conjunction = " AND ";
            }
            if (prescanInnerCarton.OuterCartonLineNo > 0)
            {
                query = query + conjunction + " [Outer Carton Line No_] = " + prescanInnerCarton.OuterCartonLineNo;
                conjunction = " AND ";
            }
            if (prescanInnerCarton.LineNo > 0)
            {
                query = query + conjunction + " [Line No_] = " + prescanInnerCarton.LineNo;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<PrescanInnerCarton> data = new List<PrescanInnerCarton>();
            foreach (DataRow row in dt.Rows)
            {
                PrescanInnerCarton item = GetItem<PrescanInnerCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<PrescanInnerCarton> SelectPrescanInnerCarton_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Prescan Inner Carton] Where 1 = 1 AND timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<PrescanInnerCarton> data = new List<PrescanInnerCarton>();
            foreach (DataRow row in dt.Rows)
            {
                PrescanInnerCarton item = GetItem<PrescanInnerCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Update(PrescanInnerCarton updateFrom, PrescanInnerCarton updateTo)
        {
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Prescan Inner Carton] " +
                  "SET [No_ of Carton] = '" + updateTo.NoOfCarton + "', " +
                  "[Carton ID] = '" + updateTo.CartonID + "', " +
                  "[CS P_N] = '" + updateTo.CSPN + "', " +
                  "[Item No_] = '" + updateTo.ItemNo + "', " +
                  "[Date Code] = '" + updateTo.DateCode + "', " +
                  "[Lot No_] = '" + updateTo.LotNo + "', " +
                  "[Quantity] = " + updateTo.Quantity + ", " +
                  "[Closed] = @boo1 , " +
                  "[Selected] = @boo2 , " +
                  "[Cross Reference No_] = '" + updateTo.CrossReferenceNo + "', " +
                  "[Seq No_] = " + updateTo.SeqNo + " , " +
                  "[DC MMDD] = '" + updateTo.DCMMDD + "' , " +
                  "[DC YYMMDD] = '" + updateTo.DCYYMMDD + "' , " +
                  "[DC YYYYMMDD] = '" + updateTo.DCYYYYMMDD + "' , " +
                  "[Description] = '" + updateTo.Description + "' , " +
                  "[Vendor] = '" + updateTo.Vendor+ "' , " +
                  "[Total Carton] = " + updateTo.TotalCarton + " , " +
                  "[MSL] = '" + updateTo.MSL + "' , " +
                  "[PO] = '" + updateTo.PO + "' , " +
                  "[BAND] = '" + updateTo.BAND + "' , " +
                  "[Origin] = '" + updateTo.Origin + "' , " +
                  "[Label Date MMDD] = '" + updateTo.LabelDateMMDD + "' , " +
                  "[Label Date YYMMDD] = '" + updateTo.LabelDateYYMMDD + "' , " +
                  "[More that one label]= " + updateTo.Morethatonelabel + " , " +
                  "[Big Carton ID] = '" + updateTo.BigCartonID + "' , " +
                  "[Spare 1] = '" + updateTo.Spare1 + "' , " +
                  "[Spare 2] = '" + updateTo.Spare2 + "' , " +
                  "[Label Date] = '" + updateTo.LabelDate + "' " +
                  "WHERE [Document No_] = '" + updateFrom.DocumentNo + "' AND " +
                  "[Outer Carton Line No_] = " + updateFrom.OuterCartonLineNo + " AND " +
                  "[Line No_] = " + updateFrom.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@boo1", (updateTo.Closed ? 1 : 0));
            cmd.Parameters.AddWithValue("@boo2", (updateTo.Selected ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Insert(PrescanInnerCarton data)
        {
            OpenSQLConnection();
            string query = "INSERT INTO [dbo].[Prescan Inner Carton] VALUES (DEFAULT, N'" +
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
        public int Delete(PrescanInnerCarton data)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Prescan Inner Carton] WHERE [Document No_] = '" + data.DocumentNo + "'," +
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

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
    public class Dao_PrescanOuterCarton
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static PrescanOuterCarton GetItem<T>(DataRow dr)
        {
            PrescanOuterCarton synchronize = new PrescanOuterCarton();
            Type temp = typeof(PrescanOuterCarton);
            PrescanOuterCarton obj = Activator.CreateInstance<PrescanOuterCarton>();
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
        public List<PrescanOuterCarton> Select(PrescanOuterCarton prescanOuterCarton = null)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Prescan Outer Carton]";
            string conjunction = " WHERE 1 = 1 AND ";
            if (!string.IsNullOrEmpty(prescanOuterCarton.DocumentNo))
            {
                query = query + conjunction + " [Document No_] = '" + prescanOuterCarton.DocumentNo + "'";
                conjunction = " AND ";
            }
            if (prescanOuterCarton.LineNo > 0)
            {
                query = query + conjunction + " [Line No_] = " + prescanOuterCarton.LineNo;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<PrescanOuterCarton> data = new List<PrescanOuterCarton>();
            foreach (DataRow row in dt.Rows)
            {
                PrescanOuterCarton item = GetItem<PrescanOuterCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<PrescanOuterCarton> SelectPrescanOuterCarton_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Prescan Outer Carton] Where 1 = 1 AND timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<PrescanOuterCarton> data = new List<PrescanOuterCarton>();
            foreach (DataRow row in dt.Rows)
            {
                PrescanOuterCarton item = GetItem<PrescanOuterCarton>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public int Update(PrescanOuterCarton updateFrom, PrescanOuterCarton updateTo)
        {
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Prescan Outer Carton] " +
                  "SET [Document No_] = '" + updateTo.DocumentNo + "'," +
                  "[Line No_] = " + updateTo.LineNo + ", " +
                  "[No_ of Carton] = " + updateTo.NoOfCarton + ", " +
                  "[Carton ID] = '" + updateTo.CartonID + "', " +
                  "[CS P_N] = '" + updateTo.CSPN + "', " +
                  "[Item No_] = '" + updateTo.ItemNo + "', " +
                  "[Date Code] = '" + updateTo.DateCode + "', " +
                  "[Lot No_] = '" + updateTo.LotNo + "', " +
                  "[Quantity] = " + updateTo.Quantity + ", " +
                  "[Closed] = @boo1  " + ", " +
                  "[Selected Quantity] = " + updateTo.SelectedQuantity + ", " +
                  "[Cross Reference No_] = '" + updateTo.CrossReferenceNo + "', " +
                  "[Seq No_] = " + updateTo.SeqNo + " , " +
                  "[DC MMDD] = '" + updateTo.DCMMDD + "', " +
                  "[DC YYMMDD] = '" + updateTo.DCYYMMDD + "', " +
                  "[DC YYYYMMDD] = '" + updateTo.DCYYYYMMDD + "', " +
                  "Description = '" + updateTo.Description + "', " +
                  "Vendor = '" + updateTo.Vendor + "', " +
                  "[Total Carton] = " + updateTo.TotalCarton + ", " +
                  "MSL = '" + updateTo.MSL + "', " +
                  "PO = '" + updateTo.PO + "', " +
                  "BAND = '" + updateTo.BAND + "', " +
                  "Origin = '" + updateTo.Origin + "', " +
                  "[Label Date MMDD] = '" + updateTo.LabelDateMMDD + "', " +
                  "[Label Date YYMMDD] = '" + updateTo.LabelDateYYMMDD + "', " +
                  "[More that one label] = '" + updateTo.Morethatonelabel + "', " +
                  "[Big Carton ID] = '" + updateTo.BigCartonID + "', " +
                  "[Spare 1] = '" + updateTo.Spare1 + "', " +
                  "[Spare 2] = '" + updateTo.Spare2 + "', " +
                  "[Label Date] = '" + updateTo.LabelDate + "' " +
                  "WHERE [Document No_] = '" + updateFrom.DocumentNo + "' AND " +
                  "[Line No_] = " + updateFrom.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@boo1", (updateTo.Closed ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int DeletePrescanOuterCarton(PrescanOuterCarton prescanOuterCarton)
        {
            //inset , delete , update   
            OpenSQLConnection();
            //string iquery = "Insert into [Entries Process]([Table],Action,Key1,Key2) VALUES('Label Line','Delete','" + labelLine.Code + "','" + labelLine.LineNo + "')";
            //SqlCommand icmd = new SqlCommand(iquery, sqlconn);
            //icmd.ExecuteNonQuery();
            string query = "DELETE FROM [dbo].[Prescan Outer Carton] WHERE [Document No_] = '" + prescanOuterCarton.DocumentNo + "' AND [Line No_] = " + prescanOuterCarton.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Insert(PrescanOuterCarton prescanOuterCarton)
        {
            OpenSQLConnection();
            string query = "INSERT INTO  [dbo].[Prescan Outer Carton] " +
                "([Document No_],[Document Line No_],[Line No_],[No_ of Carton],[Carton ID],[CS P_N],[Item No_],[Date Code]," +
                "[Lot No_],Quantity,Closed,[Selected Quantity],[Cross Reference No_],[Seq No_],[DC MMDD],[DC YYMMDD],[DC YYYYMMDD]," +
                "Description,Vendor,[Total Carton],MSL,PO,BAND,Origin,[Label Date MMDD],[Label Date YYMMDD],[More that one label]," +
                "[Big Carton ID],Spare 1,Spare 2,[Label Date])" +
                " VALUES( '" + prescanOuterCarton.DocumentNo + "'," + prescanOuterCarton.LineNo + "," +
                prescanOuterCarton.NoOfCarton + ",'" + prescanOuterCarton.CartonID + "','" +
                prescanOuterCarton.CSPN + "','" + prescanOuterCarton.ItemNo + "','" +
                prescanOuterCarton.DateCode + "','" + prescanOuterCarton.LotNo + "'," +
                prescanOuterCarton.Quantity + "," + "@boo1" + "," + 
                prescanOuterCarton.SelectedQuantity + ",'" + prescanOuterCarton.CrossReferenceNo + "'," +
                prescanOuterCarton.SeqNo + ",'" + prescanOuterCarton.DCMMDD + "','" +
                prescanOuterCarton.DCYYMMDD + "','" + prescanOuterCarton.DCYYYYMMDD + "','" +
                prescanOuterCarton.Description + "','" + prescanOuterCarton.Vendor + "'," +
                prescanOuterCarton.TotalCarton + "," + prescanOuterCarton.MSL + ",'" +
                prescanOuterCarton.PO + "','" + prescanOuterCarton.BAND + "','" +
                prescanOuterCarton.Origin + "','" + prescanOuterCarton.LabelDateMMDD + "','" +
                prescanOuterCarton.LabelDateYYMMDD + "'," + prescanOuterCarton.Morethatonelabel + ",'" +
                prescanOuterCarton.BigCartonID + "','" + prescanOuterCarton.Spare1 + "','" +
                prescanOuterCarton.Spare2 + "','" + prescanOuterCarton.LabelDate + "' )";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@boo1", (prescanOuterCarton.Closed ? 1 : 0));
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(PrescanOuterCarton data)
        {
            OpenSQLConnection();
            string query = "DELETE FROM [dbo].[Prescan Outer Carton] WHERE [Document No_]= '" + data.DocumentNo + "' , [Document Line No_]=" + data.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

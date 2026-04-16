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
    public class Dao_LabelLine
    {
        Dao_Connection dao_Connection = new Dao_Connection();
        SqlConnection sqlconn = null;

        public void OpenSQLConnection()
        {
            dao_Connection.StartDB();
            sqlconn = dao_Connection.GetSqlconn;
            sqlconn.Open();
        }
        private static LabelLine GetItem<T>(DataRow dr)
        {
            LabelLine synchronize = new LabelLine();
            Type temp = typeof(LabelLine);
            LabelLine obj = Activator.CreateInstance<LabelLine>();
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
        public List<LabelLine> Select(LabelLine labelLine)
        {
            //read
            OpenSQLConnection();
            string query = "Select * from [dbo].[Label Line]";
            string conjunction = " WHERE ";
            if (!string.IsNullOrEmpty(labelLine.Code))
            {
                query += conjunction + " [Code] = '" + labelLine.Code + "'";
                conjunction = " AND ";
            }
            if (labelLine.LineNo > 0)
            {
                query += conjunction + " [Line No_] = " + labelLine.LineNo ;
                conjunction = " AND ";
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<LabelLine> data = new List<LabelLine>();
            foreach (DataRow row in dt.Rows)
            {
                LabelLine item = GetItem<LabelLine>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        public List<LabelLine> SelectLabelLine_timestamp(Byte[] stimestamp)
        {
            //read
            OpenSQLConnection();
            string ts = "0x" + String.Join("", stimestamp.Select(b => ("00" + Convert.ToString(b, 16)).Right(2)));
            string query = "Select * from [dbo].[Label Line] Where timestamp > " + ts;
            SqlDataAdapter sqlda = new SqlDataAdapter(query, sqlconn);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            List<LabelLine> data = new List<LabelLine>();
            foreach (DataRow row in dt.Rows)
            {
                LabelLine item = GetItem<LabelLine>(row);
                data.Add(item);
            }
            sqlconn.Close();
            return data;
        }
        //Line-----------------------------------------------------------------------------------
        public int Insert(LabelLine labelLine)
        {
            OpenSQLConnection();
            string query = "INSERT INTO  [dbo].[Label Line] " +
                "([Code],[Line No_],[Type],[X],[Y],[Font],[X-multiplication],[Y-multiplication],[Code Type]," +
                "[Height],[Human Readable],[ECC level],[Cell Width],[Mode],[Rotation],[Narrow],[Wide],[Alignment],[Content])" +
                " VALUES( '" + labelLine.Code + "'," + labelLine.LineNo + ",'" + labelLine.Type + "'," + labelLine.X + "," + labelLine.Y +
                ",'" + labelLine.Font + "'," + labelLine.XMultiplication + "," + labelLine.YMultiplication + ",'" + labelLine.CodeType + "'," + labelLine.Height +
                "," + labelLine.HumanReadable + ",'" + labelLine.ECClevel + "','" + labelLine.CellWidth + "','" + labelLine.Mode + "'," + labelLine.Rotation +
                 "," + labelLine.Narrow + "," + labelLine.Wide + "," + labelLine.Alignment + ",'" + labelLine.Content + "')";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Update(String code, LabelLine labelLine)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string query = "UPDATE [dbo].[Label Line] " +
                "SET [Code] = '" + labelLine.Code + "', [Line No_] = " + labelLine.LineNo + ", [Type] = '" + labelLine.Type + "', [X] = " + labelLine.X +
                ", [Y] = " + labelLine.Y + ", [Font] = '" + labelLine.Font + "', [X-multiplication] = " + labelLine.XMultiplication +
                ", [Y-multiplication] = " + labelLine.YMultiplication + ", [Code Type] = '" + labelLine.CodeType + "', [Height] = " + labelLine.Height +
                ", [Human Readable] = " + labelLine.HumanReadable + ", [ECC level] = '" + labelLine.ECClevel + "', [Cell Width] = '" + labelLine.CellWidth +
                "', [Mode] = '" + labelLine.Mode + "', [Rotation] = " + labelLine.Rotation + ", [Narrow] = " + labelLine.Narrow +
                ", [Wide] = " + labelLine.Wide + ", [Alignment] = " + labelLine.Alignment + ", [Content] = '" + labelLine.Content + "'" +
                " WHERE [Code] = '" + code + "'";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
        public int Delete(LabelLine labelLine)
        {
            //inset , delete , update   
            OpenSQLConnection();
            string iquery = "Insert into [Entries Process]([Table],Action,Key1,Key2) VALUES('Label Line','Delete','" + labelLine.Code + "','" + labelLine.LineNo + "')";
            SqlCommand icmd = new SqlCommand(iquery, sqlconn);
            icmd.ExecuteNonQuery();
            string query = "DELETE FROM [dbo].[Label Line] WHERE [Code] = '" + labelLine.Code + "' , [Line No.] = " + labelLine.LineNo;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            int effectedRows = cmd.ExecuteNonQuery();
            sqlconn.Close();
            return effectedRows;
        }
    }
}

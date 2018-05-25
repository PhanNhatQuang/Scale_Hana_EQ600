using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Termie
{
    public static class SqlHelper
    {
        private static SQLiteConnection connection;
        private static string database;
        public static List<DataModel> s_ListModels = new List<DataModel>();
        public static DataTable s_table_AllModels = new DataTable();
        public static DataTable s_table_10Models = new DataTable();
        public static DataTable s_table_ModelsFromTo = new DataTable();
        public static void Initialize()
        {
            database = "Scales.db";
            string connectionString;
            connectionString = "Data Source=" + database + ";Version=3;New=False;Compress=True;";
            connection = new SQLiteConnection(connectionString);

    }


        ///////////////////////////////////////////////////////////////////////////


        private static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.ErrorCode)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }


        ///////////////////////////////////////////////////////////////////////////


        //Close connection
        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        ///////////////////////////////////////////////////////////////////////////


        public static int InsertValue(DataModel model)
        {
            int result = -1;
            //open connection
            if (OpenConnection() == true)
            {
                try
                {
                    string query = "INSERT INTO table_Values(Name, Value, Unit, DateTime, ThanhTien) " +
                                    "VALUES('" + model.m_name + "', " + model.m_value.ToString(new CultureInfo("en-US")) + " ,'" + model.m_unit + "', datetime('now','localtime'), " + model.m_thanhTien.ToString(new CultureInfo("en-US")) + ")";

                    //create command and assign the query and connection from the constructor         DateTimeSQLite(DateTime.Now)
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    //Execute command
                    result = cmd.ExecuteNonQuery();
                    //close connection
                    CloseConnection();
                }
                catch(Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.ToString());
                }
            }            
            return result;
        }


        ///////////////////////////////////////////////////////////////////////////

        public static int InsertMatHang(string name, int price, int hotkey)
        {
            int result = -1;
            //open connection
            if (OpenConnection() == true)
            {
                try
                {
                    string query = "INSERT INTO table_MatHang(Name, Price, HotKey) " +
                                    "VALUES('" + name + "' ," + price.ToString(new CultureInfo("en-US")) + ",";

                     query = query + hotkey.ToString()+ ")";                    
                    //create command and assign the query and connection from the constructor         DateTimeSQLite(DateTime.Now)
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    //Execute command
                    result = cmd.ExecuteNonQuery();
                    //close connection
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.ToString());
                }
            }
            return result;
        }


        ///////////////////////////////////////////////////////////////////////////
        public static int UpdateMatHang(int ID, string name, int price, int hotkey)
        {
            int result = -1;
            //open connection
            if (OpenConnection() == true)
            {
                try
                {
                    string query = "UPDATE table_MatHang SET Name = '" + name + "', Price = " + 
                        price.ToString(new CultureInfo("en-US")) + ", HotKey = " + hotkey.ToString() + " WHERE ID = " + ID.ToString();
                    //create command and assign the query and connection from the constructor         DateTimeSQLite(DateTime.Now)
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    //Execute command
                    result = cmd.ExecuteNonQuery();
                    //close connection
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.ToString());
                }
            }
            return result;
        }


        ///////////////////////////////////////////////////////////////////////////
        public static int DeleteMatHang(int ID)
        {
            int result = -1;
            //open connection
            if (OpenConnection() == true)
            {
                try
                {
                    string query = "DELETE FROM table_MatHang WHERE ID =  " + ID.ToString();
                    
                    //create command and assign the query and connection from the constructor         DateTimeSQLite(DateTime.Now)
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    //Execute command
                    result = cmd.ExecuteNonQuery();
                    //close connection
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.ToString());
                }
            }
            return result;
        }


        ///////////////////////////////////////////////////////////////////////////


        public static void getAllValues()
        {
            DataTable dTable = new DataTable();
            string query = "SELECT * FROM table_Values ORDER BY ID DESC";
            
            //Execute command
            if (OpenConnection() == true)
            {
                try
                {
                    //Create Command
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    //SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    //MyAdapter.SelectCommand = cmd;
                    //MyAdapter.Fill(dTable);
                    //s_table_AllProducts = dTable;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    s_table_AllModels = new DataTable();
                    s_table_AllModels.Columns.Add("ID");
                    s_table_AllModels.Columns.Add("Name");
                    s_table_AllModels.Columns.Add("Value");                                     
                    s_table_AllModels.Columns.Add("Unit");
                    s_table_AllModels.Columns.Add("ThanhTien");
                    s_table_AllModels.Columns.Add("DateTime");
                    
                    while (reader.Read())
                    {
                        DataRow dr = s_table_AllModels.NewRow();
                        string id = reader.GetInt64(0).ToString();
                        string value = reader.GetFloat(2).ToString();
                        string datetime_string = reader[4].ToString();
                        string name = reader[1].ToString();
                        string thanh_tien = reader.GetDouble(5).ToString("#,0.###");
                        string unit = reader[3].ToString();
                        DateTime dateTime = reader.GetDateTime(4);
                        //bool success = DateTime.TryParseExact(datetime_string, CultureInfo.CurrentUICulture.DateTimeFormat.GetAllDateTimePatterns(), CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                        dr["ID"] = id;
                        dr["Value"] = value;
                        //if (success)
                        {
                            dr["DateTime"] = dateTime;
                        }
                        dr["Unit"] = unit;
                        dr["ThanhTien"] = thanh_tien;
                        dr["Name"] = name;

                        s_table_AllModels.Rows.Add(dr);
                    }
                        //close Connection
                    CloseConnection();
                    //foreach (DataRow row in dTable.Rows)
                    //{
                    //    DataModel model = new DataModel();
                    //    model.m_ID = int.Parse(row["ID"].ToString());
                    //    model.m_value = float.Parse(row["Value"].ToString());
                    //    model.m_dateTime_string = row["DateTime"].ToString();
                    //    SqlHelper.s_ListModels.Add(model);
                    //}
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        public static void get10Values()
        {
            DataTable dTable = new DataTable();
            string query = "SELECT * FROM table_Values ORDER BY ID DESC LIMIT 10";

            //Execute command
            if (OpenConnection() == true)
            {
                try
                {
                    //Create Command
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    s_table_10Models = new DataTable();
                    s_table_10Models.Columns.Add("ID");
                    s_table_10Models.Columns.Add("Name");
                    s_table_10Models.Columns.Add("Value");
                    s_table_10Models.Columns.Add("Unit");                    
                    s_table_10Models.Columns.Add("ThanhTien");
                    s_table_10Models.Columns.Add("DateTime");
                    while (reader.Read())
                    {
                        DataRow dr = s_table_10Models.NewRow();
                        string id = reader.GetInt64(0).ToString();
                        string value = reader.GetFloat(2).ToString();
                        string datetime_string = reader[4].ToString();
                        string name = reader[1].ToString();
                        string thanh_tien = reader.GetDouble(5).ToString("#,0.###");
                        string unit = reader[3].ToString();
                        DateTime dateTime = reader.GetDateTime(4); 
                        //bool success = DateTime.TryParseExact(datetime_string, CultureInfo.CurrentUICulture.DateTimeFormat.GetAllDateTimePatterns(), CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                        dr["ID"] = id;
                        dr["Value"] = value;
                       // if (success)
                        {
                            dr["DateTime"] = dateTime;
                        }
                        dr["Unit"] = unit;
                        dr["ThanhTien"] = thanh_tien;
                        dr["Name"] = name;
                        s_table_10Models.Rows.Add(dr);
                    }
                    //close Connection
                    CloseConnection();
                    //foreach (DataRow row in dTable.Rows)
                    //{
                    //    DataModel model = new DataModel();
                    //    model.m_ID = int.Parse(row["ID"].ToString());
                    //    model.m_value = float.Parse(row["Value"].ToString());
                    //    model.m_dateTime_string = row["DateTime"].ToString();
                    //    SqlHelper.s_ListModels.Add(model);
                    //}
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        public static void getAllValuesFromTo(DateTime from, DateTime to)
        {
            DataTable dTable = new DataTable();
            string query = "SELECT * FROM table_Values WHERE DateTime >= '" + from.ToString("yyyy-MM-dd") + "' AND DateTime <= '" + to.ToString("yyyy-MM-dd") + "' ORDER BY ID DESC";

            //Execute command
            if (OpenConnection() == true)
            {
                try
                {
                    //Create Command
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);

                    SQLiteDataReader reader = cmd.ExecuteReader();
                    s_table_ModelsFromTo = new DataTable();
                    s_table_ModelsFromTo.Columns.Add("ID");
                    s_table_ModelsFromTo.Columns.Add("Name");
                    s_table_ModelsFromTo.Columns.Add("Value");
                    s_table_ModelsFromTo.Columns.Add("Unit");
                    s_table_ModelsFromTo.Columns.Add("ThanhTien");
                    s_table_ModelsFromTo.Columns.Add("DateTime");
                    
                    while (reader.Read())
                    {
                        DataRow dr = s_table_ModelsFromTo.NewRow();
                        string id = reader.GetInt64(0).ToString();
                        string value = reader.GetFloat(2).ToString();
                        string datetime_string = reader[4].ToString();
                        string name = reader[1].ToString();
                        string thanh_tien = reader.GetDouble(5).ToString("#,0.###");
                        string unit = reader[3].ToString();
                        DateTime dateTime = reader.GetDateTime(4);
                        //bool success = DateTime.TryParseExact(datetime_string, CultureInfo.CurrentUICulture.DateTimeFormat.GetAllDateTimePatterns(), CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                        dr["ID"] = id;
                        dr["Value"] = value;
                        //if (success)
                        {
                            dr["DateTime"] = dateTime;
                        }
                        dr["Unit"] = unit;
                        dr["ThanhTien"] = thanh_tien;
                        dr["Name"] = name;
                        s_table_ModelsFromTo.Rows.Add(dr);
                    }
                    //close Connection
                    CloseConnection();

                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.Message);
                }
            }
        }


        ///////////////////////////////////////////////////////////////////////////

        public static void DeleteAllValuesFromTo(DateTime from, DateTime to)
        {
            int result = -1;
            DataTable dTable = new DataTable();
            string query = "DELETE FROM table_Values WHERE DateTime >= '" + from.ToString("yyyy-MM-dd") + "' AND DateTime <= '" + to.ToString("yyyy-MM-dd") + "'";

            //Execute command
            if (OpenConnection() == true)
            {
                try
                {
                    //create command and assign the query and connection from the constructor         DateTimeSQLite(DateTime.Now)
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    //Execute command
                    result = cmd.ExecuteNonQuery();
                    //close connection
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.ToString());
                }
            }
        }


        ///////////////////////////////////////////////////////////////////////////
        public static DataTable getAllMatHang()
        {
            string query = "SELECT * FROM table_MatHang ORDER BY ID DESC";
            DataTable DanhSachMatHang = new DataTable();
            //Execute command
            if (OpenConnection() == true)
            {
                try
                {
                    //Create Command
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    
                    DanhSachMatHang.Columns.Add("ID");
                    DanhSachMatHang.Columns.Add("Name");
                    DanhSachMatHang.Columns.Add("Price");
                    DanhSachMatHang.Columns.Add("HotKey");
                    while (reader.Read())
                    {
                        DataRow dr = DanhSachMatHang.NewRow();
                        string id = reader.GetInt64(0).ToString();
                        string name = reader[1].ToString();
                        //string Price = reader.GetInt64(2).ToString("#,0.###");
                        string Price = reader.GetInt64(2).ToString();
                        string Hotkey = reader[3].ToString();   

                        dr["ID"] = id;
                        dr["Name"] = name;
                        dr["Price"] = Price;
                        dr["HotKey"] = Hotkey;

                        DanhSachMatHang.Rows.Add(dr);
                    }
                    //close Connection
                    CloseConnection();
                    return DanhSachMatHang;
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.Message);
                }
            }
            return DanhSachMatHang;
        }
        ///////////////////////////////////////////////////////////////////////////
        public static DataTable getMatHangByName(string _name)
        {
            string query = "SELECT * FROM table_MatHang WHERE Name LIKE '%"+ _name +"%'  ORDER BY ID DESC";
            DataTable DanhSachMatHang = new DataTable();
            //Execute command
            if (OpenConnection() == true)
            {
                try
                {
                    //Create Command
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    DanhSachMatHang.Columns.Add("ID");
                    DanhSachMatHang.Columns.Add("Name");
                    DanhSachMatHang.Columns.Add("Price");
                    DanhSachMatHang.Columns.Add("HotKey");
                    while (reader.Read())
                    {
                        DataRow dr = DanhSachMatHang.NewRow();
                        string id = reader.GetInt64(0).ToString();
                        string name = reader[1].ToString();
                        //string Price = reader.GetInt64(2).ToString("#,0.###");
                        string Price = reader.GetInt64(2).ToString();
                        string Hotkey = reader[3].ToString();

                        dr["ID"] = id;
                        dr["Name"] = name;
                        dr["Price"] = Price;
                        dr["HotKey"] = Hotkey;

                        DanhSachMatHang.Rows.Add(dr);
                    }
                    //close Connection
                    CloseConnection();
                    return DanhSachMatHang;
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    MessageBox.Show(ex.Message);
                }
            }
            return DanhSachMatHang;
        }
        ///////////////////////////////////////////////////////////////////////////

        public static string DateTimeSQLite(DateTime datetime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}";
            return string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
        }
        

        ///////////////////////////////////////////////////////////////////////////


    }
}

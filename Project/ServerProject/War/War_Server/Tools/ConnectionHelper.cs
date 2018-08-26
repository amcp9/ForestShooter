using System;
using MySql.Data.MySqlClient;

namespace War_Server.Tools
{
    //用于创建MySQL连接与关闭MySQL连接
    public class ConnectionHelper
    {
        public const string CONNECTIONSTRING = "Database=game01;Data Source=127.0.0.1;port=3306;User Id=root;Password=33217276;";


        public static MySqlConnection Connect()
        {
            MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                connection.Open();
                return connection;
            }
            catch(Exception e)
            {
                Console.WriteLine("连接数据库时，出现异常：" + e.Message);
                return null;
            }
        }

        public static void CloseConnection(MySqlConnection connection)
        {
            if(connection != null) connection.Close();
            else
            {
                Console.WriteLine("Mysql连接为空！");
            }
        }
    }
}

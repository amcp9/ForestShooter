using System;
using MySql.Data.MySqlClient;

namespace DatabaseAction
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //选择需要打开的数据库与IP地址与端口号，用户名以及密码
            string s = "Database=game01;Data Source=127.0.0.1;port=3306;User Id=root;Password=33217276;";
            MySqlConnection mySqlConnection = new MySqlConnection(s);
            mySqlConnection.Open();

#region search
            //把指令以字符串的形式传递
            MySqlCommand cmd = new MySqlCommand("select * from user where id = 1", mySqlConnection);
            MySqlDataReader reader =  cmd.ExecuteReader();

            while(reader.Read())
            {
                string username = reader.GetString("username");
                string password1 = reader.GetString("password");

                Console.WriteLine(username + ":" + password1);
            }


            #endregion
//#region insert
            //string name = "asd";
            //string password = "dsa";
            ////使用参数的形式向user表中添加一行数据
            ////MySqlCommand cmd_2 = new MySqlCommand("insert into user set username = @un,password = @pw", mySqlConnection);
            ////cmd_2.Parameters.AddWithValue("un", name);//添加变量
            ////cmd_2.Parameters.AddWithValue("pw", password);//添加变量
            ////cmd_2.ExecuteNonQuery();//执行语句
            //#endregion
#region delete
            //删除ID为1的那一行数据
            //MySqlCommand cmd_3 = new MySqlCommand("delete from user where id =@id", mySqlConnection);
            //cmd_3.Parameters.AddWithValue("id", 1);
            //cmd_3.ExecuteNonQuery();
            #endregion
#region update
            //更改ID为1的数据的密码
            //MySqlCommand cmd_4 = new MySqlCommand("update user set password = @pwd where id =1", mySqlConnection);
            //cmd_4.Parameters.AddWithValue("pwd", "3321277");
            //cmd_4.ExecuteNonQuery();
#endregion
            reader.Close();
            mySqlConnection.Close();
            Console.ReadKey();
        }
    }
}

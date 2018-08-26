using System;
using MySql.Data.MySqlClient;
using War_Server.Model;
namespace War_Server.DAO
{
    public class UserDAO
    {
        //验证用户名和密码
        public User VerifyUser(MySqlConnection connection,string username,string pwd)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @pwd;", connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("pwd", pwd);
                reader = cmd.ExecuteReader();
                Console.WriteLine("UserDAO cmd Execute!");

                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    return new User(id, username, pwd);
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("验证用户名密码时出现异常：" + e.Message);
            }
            finally
            {
                if(reader != null)
                {
                    Console.WriteLine("reader close");
                    reader.Close();
                }
            }
            return null;

        }

        public bool GetUserByUsername(MySqlConnection connection,string name)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", connection);
                cmd.Parameters.AddWithValue("username", name);
                reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
            catch (Exception e)
            {
                Console.WriteLine("对比用户名时出现异常：" + e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return false;
        }

        public void AddUser(MySqlConnection connection, string username, string pwd)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user (username,password) value(@username,@pwd);", connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("pwd", pwd);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("新增用户时出现异常：" + e.Message);
            }
        }
    }
}

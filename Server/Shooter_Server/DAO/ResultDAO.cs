using System;
using War_Server.Model;
using MySql.Data.MySqlClient;
namespace War_Server.DAO
{
    public class ResultDAO
    {
        public Result GetResultByUserId(MySqlConnection connection,int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid = @id;", connection);
                cmd.Parameters.AddWithValue("id", userId);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");
                    return new Result(id, userId, totalCount,winCount);
                }
                else
                {
                    return new Result(-1, userId, 0, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetResultByUserId时出现异常：" + e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    Console.WriteLine("reader close");
                    reader.Close();
                }
            }
            return null;
        }
    }
}

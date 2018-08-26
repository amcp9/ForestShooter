using System;
using Common;
using War_Server.Servers;
using War_Server.DAO;
using War_Server.Model;

namespace War_Server.Controller
{
    public class UserController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            request = RequestCode.User;
        }

        public string Login(string data,Client client,Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.GetMySqlConnection,strs[0], strs[1]);
            //验证是否成功获取数据
            if (user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else 
            {
                Result result = resultDAO.GetResultByUserId(client.GetMySqlConnection, user.Id);
                client.SetUserData(user, result);
                server.SystemChat("玩家[" + client.GetUserName() + "]已登录");
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(), user.Username, result.TotalCount, result.WinCount);
            }
        }

        public string Register(string data,Client client,Server server)
        {
            string[] strs = data.Split(',');
            string name = strs[0], pwd = strs[1];
            bool isHasName = userDAO.GetUserByUsername(client.GetMySqlConnection,name);
            if(isHasName)
            {
                return ((int)ReturnCode.Fail).ToString(); 
            }
            userDAO.AddUser(client.GetMySqlConnection, name, pwd);
            return ((int)ReturnCode.Success).ToString();
        }
    }
}

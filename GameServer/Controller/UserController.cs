using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;

namespace GameServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();

        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] str = data.Split(',');
            User user = userDAO.VerifyUser(client.MySQLConn, str[0], str[1]);
            if (user == null)
            {
                return ((int)ReturnCode.Failed).ToString();
            }
            else
            {
                Result res = resultDAO.GetResultBuUserid(client.MySQLConn, user.ID);
                client.SetUserData(user,res);
                return string.Format("{0},{1},{2},{3}",((int)ReturnCode.Success).ToString(),user.Username,res.TotalCount,res.WinCount);
            }
        }

        public string Register(string data, Client client, Server server)
        {
            string[] str = data.Split(',');
            string username = str[0];
            string password = str[1];
            bool res = userDAO.GetUserByUsername(client.MySQLConn, username);
            if (res)
            {
                return ((int)ReturnCode.Failed).ToString();
            }

            userDAO.AddUser(client.MySQLConn, username, password);
            return ((int)ReturnCode.Success).ToString();
        }
    }
}

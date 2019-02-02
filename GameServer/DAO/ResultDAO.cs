using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
    class ResultDAO
    {
        public Result GetResultBuUserid(MySqlConnection conn,int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid = @userid", conn);
                cmd.Parameters.AddWithValue("userid", userId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalcount = reader.GetInt32("totalcount");
                    int wincount = reader.GetInt32("wincount");
                    Result result = new Result(id, userId, totalcount, wincount);
                    return result;
                }
                else
                {
                    Result result = new Result(-1, userId, 0, 0);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetResultBuUserid的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }

        public void UpdateOrAddResult(MySqlConnection conn,Result res)
        {
            try
            {
                MySqlCommand cmd = null;

                if (res.ID <= -1)
                {
                    cmd = new MySqlCommand("insert into result set totalcount = @totalcount,wincount = @wincount, userid = @userid", conn);
                }
                else
                {
                    cmd = new MySqlCommand("update result set totalcount = @totalcount,wincount = @wincount where userid = @userid", conn);
                }
                cmd.Parameters.AddWithValue("totalcount", res.TotalCount);
                cmd.Parameters.AddWithValue("wincount", res.WinCount);
                cmd.Parameters.AddWithValue("userid", res.UserId);
                cmd.ExecuteNonQuery();
                if (res.ID<=-1)
                {
                    Result tempRes = GetResultBuUserid(conn, res.UserId);
                    res.ID = tempRes.ID;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在 UpdateOrAddResult的时候出现异常：" + e);
            }
        }
    }
}

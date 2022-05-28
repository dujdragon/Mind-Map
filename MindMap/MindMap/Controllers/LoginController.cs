using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MindMap.Core;
using MindMap.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MindMap.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // 判断用户名和密码是否匹配
        [HttpGet]
        public string Login(string username, string pwd)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@username", username) };
            DataTable res = sqlHelper.ExecuteTable("Select * from UsersLogin where username=@username", sqlParameters);
            if (res.Rows.Count == 0) return "user not exist!";
            else if (res.Rows[0]["pwd"].ToString() != pwd) return "password does not match username!";
            else return "successfully login!";
        }
        // 用户注册
        [HttpPost]
        public string Register(string username, string pwd)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@username", username) };
            DataTable res = sqlHelper.ExecuteTable("Select * from UsersLogin where username=@username", sqlParameters);
            if (res.Rows.Count != 0) return "username already exists!";
            sqlParameters = new SqlParameter[] { new SqlParameter("@username", username), new SqlParameter("@pwd", pwd) };
            sqlHelper.ExecuteNonQuery("insert into UsersLogin(username, pwd) values(@username, @pwd)", sqlParameters);
            return "successfully register!";
        }
        // 修改密码
        [HttpPut]
        public string Update(string username, string pwd, string CreateTime)
        {
            SqlHelper sqlHelper = new SqlHelper();
            DataTable res = sqlHelper.ExecuteTable("Select * from UsersLogin where username=@username", new SqlParameter("@username", username));
            if (res.Rows.Count > 0)
            {
                User user = new User();
                user.Id = (int)res.Rows[0]["Id"];
                user.Username = username??(string)res.Rows[0]["username"];
                user.Password = pwd??(string)res.Rows[0]["pwd"];

                sqlHelper.ExecuteNonQuery("Update UsersLogin Set pwd = @pwd where username = @username", new SqlParameter("@username", user.Username), new SqlParameter("@pwd", user.Password));
            }
           
            return "successfully update password!";
        }
        // 注销账户
        [HttpDelete]
        public string Remove(string username)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@username", username) };
            sqlHelper.ExecuteNonQuery("delete from UsersLogin where username=@username", sqlParameters);
            return "successfully logout!";
        }
    }
}

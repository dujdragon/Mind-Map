using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MindMapApi.Bll;
using MindMapApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MindMapApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsersBll usersBll;
        //依赖注入
        public LoginController(UsersBll usersBll)
        {
            this.usersBll = usersBll;
        }

        //一个状态变量，记录当前登录用户的Id，使用户只能对自己登录的账号进行信息的修改，未登录时设为-1
        private static int LoginUserId = -1;

        // 用户登录：判断用户名和密码是否匹配
        [HttpGet]
        public string Login(string username, string pwd)
        {
            UsersLogin user = usersBll.Get(username);
            if(user == null)
            {
                return "user doesn't exist!";
            }
            if (user.pwd != pwd)
            {
                return "wrong username or password!";
            }
            LoginUserId = user.id;
            return "successfully login!";
        }
        // 用户注册
        [HttpPost]
        public string Register(string username, string pwd)
        {
            UsersLogin user = usersBll.Get(username);
            if (user != null)
            {
                return "username already exists!";
            }
            usersBll.Insert(new UsersLogin
            {
                username = username,
                pwd = pwd,
                createdate = DateTime.Now
            });
            return "successfully register!";
        }

        //修改用户名:修改当前登录用户的用户名
        [HttpPut]
        public string UpdataUsername(string newUsername)
        {
            UsersLogin user = usersBll.Get(LoginUserId);
            user.username = newUsername;
            usersBll.Update(user);
            return "successfully update username!";
        }


        // 修改密码：修改当前登录用户的密码
        [HttpPut]
        public string UpdatePwd(string newPwd, string confirmNewPwd)
        {
            if (newPwd != confirmNewPwd)
            {
                return "different password input!";
            }
            UsersLogin user = usersBll.Get(LoginUserId);
            user.pwd = newPwd;
            usersBll.Update(user);
            return "successfully update password!";
        }

        //退出登录：将LoginUserId设为-1
        [HttpPut]
        public string Logout()
        {
            LoginUserId = -1;
            return "successfully logout!";
        }

        // 注销账户：先删除用户所有文件，再删除用户
        [HttpDelete]
        public string RemoveUser(string username)
        {
            UsersLogin user = usersBll.Get(username);
            if(user == null)
            {
                return "user doesn't exist!";
            }
            FileController fileController = new FileController(new FilesBll());
            fileController.DeleteAllFiles(username);
            usersBll.Delete(user);
            return "user is sucessfully removed!";
        }
    }
}

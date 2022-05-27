using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MindMap.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MindMap.Controllers
{
    [Route("[controller][action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
         //获得目录下所有的文件或者文件夹
        [HttpGet]
        public List<FileObj> GetFileList(string username, string filepath)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@username", username), new SqlParameter("@filepath", filepath) };
            DataTable data = sqlHelper.ExecuteTable("Select * from UsersFiles where username=@username and filepath=@filepath", sqlParameters);
            if (data.Rows.Count == 0) return null;
            List<FileObj> res = new List<FileObj>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                FileObj fileObj = new FileObj(data.Rows[i]["filename"].ToString(), data.Rows[i]["type"].ToString());
                res.Add(fileObj);
            }
            return res;
        }
         //获得指定路径下的文件 [需要完善]
        [HttpGet]
        public string GetFile(string username, string filepath, string filename)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@username", username), new SqlParameter("@filepath", filepath), new SqlParameter("@filename", filename) };
            DataTable data = sqlHelper.ExecuteTable("Select * from UsersFiles where username=@username and filepath=@filepath and filename=@filename", sqlParameters);


            return data.Rows[0][filepath].ToString() + '\n' + data.Rows[0][filename].ToString();
        }
         //创建文件
        //[HttpPost]
        //public string CreateFileObj(string username, string filepath, string filename, int type)
        //{
        //    SqlHelper sqlHelper = new SqlHelper();
        //    SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter(@username, username), new SqlParameter(@filepath, filepath), new SqlParameter(@filename, filename) , new SqlParameter(@type, type)};
        //    DataTable res = sqlHelper.ExecuteTable("insert into UsersFiles(username, filename, filepath, type) values(@username, @filename, @filepath, @type)", sqlParameters);
        //    if (res.Rows.Count != 0) return "username already exists!";
        //    sqlParameters = new SqlParameter[] { new SqlParameter(@username, username), new SqlParameter("@pwd", pwd) };
        //    sqlHelper.ExecuteNonQuery("insert into UsersLogin(username, pwd) values(@username, @pwd)", sqlParameters);
        //    return "successfully register!";
        //}
        //// 修改密码
        //[HttpPut]
        //public string Update(string username, string pwd)
        //{
        //    SqlHelper sqlHelper = new SqlHelper();
        //    SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter(@username, username), new SqlParameter(@pwd, pwd) };
        //    sqlHelper.ExecuteNonQuery(Update UsersLogin Set pwd = @pwd where username = @username, sqlParameters);
        //    return successfully update password!;
        //}
        // 注销账户
        //[HttpDelete]
        //public string Remove(string username)
        //{
        //    SqlHelper sqlHelper = new SqlHelper();
        //    SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter(@username, username) };
        //    sqlHelper.ExecuteNonQuery(delete from UsersLogin where username=@username, sqlParameters);
        //    return successfully logout!;
        //}
    }

    public class FileObj
    {
        public FileObj(string filename, string type)
        {
            this.filename = filename;
            this.type = type == "True" ? 1 : 0;
        }
        public string filename { get; set; }
        public int type { get; set; }

        public override string ToString()
        {
            return $"name : {filename}, type : {type}\n";
        }
    }
}

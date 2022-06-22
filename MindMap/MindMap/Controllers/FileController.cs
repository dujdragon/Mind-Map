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
    [Route("[controller][action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FilesBll filesBll;

        public FileController(FilesBll filesBll)
        {
            this.filesBll = filesBll;
        }

        //获得指定目录下所有的文件或者文件夹
        [HttpGet]
        public List<UsersFiles> GetFileByPath(string username, string filepath)
        {
            return filesBll.GetAllByPath(filepath).FindAll(files => files.username == username);
        }

         //根据文件名搜索文件
        [HttpGet]
        public List<UsersFiles> GetFilesByName(string username, string filename)
        {
            return filesBll.GetByFileName(filename).FindAll(files => files.username == username);
        }

        //获得该用户的所有文件
        [HttpGet]
        public List<UsersFiles> GetAllFiles(string userName)
        {
            return filesBll.GetAllByUserName(userName);
        }

        //创建新文件，同目录下文件名不允许相同
        [HttpPost]
        public string CreateFile(string username, string filepath, string filename, bool type)
        {
            if(filesBll.GetByFileNameAndPath(filename,filepath) != null)
            {
                return "there's a file with the same name in this path!";
            }
            filesBll.Insert(new UsersFiles
            {
                username = username,
                filepath = filepath,
                filename = filename,
                type = type,
                createdate = DateTime.Now
            });
            return "successfully create!";
        }

        //文件重命名
        [HttpPut]
        public string UpdateFileName(string username, string filepath, string filename, string newName)
        {
            UsersFiles file = filesBll.Get(username, filename, filepath);
            file.filename = newName;
            filesBll.Update(file);
            return "successfully update file name!";
        }
        
        //删除该用户所有文件
       [HttpDelete]
        public string DeleteAllFiles(string username)
        {
            foreach(UsersFiles file in GetAllFiles(username))
            {
                filesBll.Delete(file);
            }
            return "successfully delete user's all files!";
        }
    }
}

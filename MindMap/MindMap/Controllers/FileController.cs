using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MindMapApi.Bll;
using MindMapApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FilesBll filesBll;

        public FileController(FilesBll filesBll)
        {
            this.filesBll = filesBll;
        }

        // 获得指定目录下所有的文件或者文件夹
        [HttpGet]
        public List<UsersFiles> GetFileByPath(string username, string filepath)
        {
            var res = filesBll.GetAllByPath(filepath).FindAll(files => files.username == username);
            return res;
        }

         // 根据文件名搜索文件
        [HttpGet]
        public string GetFileByName(string username, string filename, string filepath)
        {
            //return filesBll.GetByFileName(filename).FindAll(files => files.username == username);
            string path = "D:\\Data\\" + username + "\\" + filepath + "\\" + filename + ".jm";
            byte[] buffer = System.IO.File.ReadAllBytes(path);
            string s = Encoding.UTF8.GetString(buffer);
            return s;
        }

        // 获得该用户的所有文件
        [HttpGet]
        public List<UsersFiles> GetAllFiles(string userName)
        {
            return filesBll.GetAllByUserName(userName);
        }

        [HttpGet]
        public string GetAllFolders(string userName)
        {
            var data = filesBll.GetAllByUserName(userName);
            string res = "";
            for (int i = 0; i < data.Count; i++)
                if (data[i].filepath == "/")
                    res += data[i].filename + " ";
            return res;
        }

        [HttpGet]
        public string GetAllFile(string userName, string pathname)
        {
            var data = filesBll.GetAllByUserName(userName);
            string res = "";
            for (int i = 0; i < data.Count; i++)
                if (data[i].filepath != "/" && data[i].filepath == pathname)
                    res += data[i].filename + " ";
            return res;
        }

        // 创建新文件，同目录下文件名不允许相同,文件默认不可共享
        [HttpPost]
        public string CreateFile(dynamic Json) // 0文件，1文件夹 
        {
            string username = Json.username;
            string filepath = Json.filepath;
            string filename = Json.filename;
            bool type = Json.type;
            string tag = Json.tag;
            // string username, string filepath, string filename, bool type
            if (filesBll.GetByFileNameAndPath(filename,filepath) != null)
            {
                return "samename";
            }
            filesBll.Insert(new UsersFiles
            {
                username = username,
                filepath = filepath,
                filename = filename,
                type = type,
                tag = tag,
                createdate = DateTime.Now
            });
            string sPath = "D:\\Data\\";
            if (type)
            {
                if (!Directory.Exists(sPath + username + "\\" + filename))
                    Directory.CreateDirectory(sPath + username + "\\" + filename);//创建路径
            }
            return "success";
        }

        // 保存文件，同目录下文件名不允许相同
        [HttpPost]
        public string SaveFile(dynamic Json) // 1是文件，0是文件夹 
        {
            string data = Json.data;
            string filename = Json.filename;
            string foldername = Json.foldername;
            string username = Json.username;
            string SavePath = "D:\\Data\\" + username + "\\" + foldername + "\\" + filename + ".jm";
            FileInfo fileInfo = new FileInfo(SavePath);
            StreamWriter streamWriter = new StreamWriter(SavePath);
            if (!fileInfo.Exists)
            {
                //创建文件
                fileInfo.Create().Close();
            }
            streamWriter.WriteLine(data);
            streamWriter.Flush();
            //关闭流
            streamWriter.Close();
            return "success";
        }

        // 文件重命名
        [HttpPut]
        public string UpdateFileName(string username, string filepath, string filename, string newName)
        {
            UsersFiles file = filesBll.Get(username, filename, filepath);
            file.filename = newName;
            filesBll.Update(file);
            return "successfully update file name!";
        }
        
        // 删除该用户所有文件
       [HttpDelete]
        public string DeleteAllFiles(string username)
        {
            foreach(UsersFiles file in GetAllFiles(username))
            {
                filesBll.Delete(file);
            }
            return "successfully delete user's all files!";
        }

        [HttpGet]
        public JObject GetJson(string json)
        {
            return JObject.Parse(json);
        }

        //修改tag
        [HttpPut]
        public string UpdateTag(int id, string newTag)
        {
            UsersFiles file = filesBll.Get(id);
            file.tag = newTag;
            filesBll.Update(file);
            return "successfully update tag!";
        }

        //设置文件共享状态（文件默认不可共享）
        [HttpPut]
        public string ShareEnable(int id, bool is_shared)
        {
            UsersFiles file = filesBll.Get(id);
            file.is_shared = is_shared;
            filesBll.Update(file);
            return "success!";
        }

        //显示所有可共享文件
        [HttpGet]
        public List<UsersFiles> GetAllSharedFiles()
        {
            return filesBll.GetSharedFiles();
        }
    }
}

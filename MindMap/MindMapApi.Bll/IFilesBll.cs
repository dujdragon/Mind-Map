using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapApi.Bll
{
    public interface IFilesBll<T> where T : class
    {
        int Insert(T t);
        int Update(T t);
        int Delete(T t);
        List<T> GetAllByUserName(string username);
        List<T> GetAllByPath(string path);
        T Get(int id);
        List<T> GetByFileName(string filename);
        T GetByFileNameAndPath(string fileName, string path);
        T Get(string username, string filename, string path);
        List<T> GetSharedFiles();
    }
}

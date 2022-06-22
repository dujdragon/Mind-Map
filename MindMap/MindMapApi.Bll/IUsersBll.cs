using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapApi.Bll
{
    public interface IUsersBll<T> where T : class
    {
        int Insert(T t);
        int Update(T t);
        int Delete(T t);
        List<T> GetAll();
        T Get(int id);
        T Get(string name);
    }
}

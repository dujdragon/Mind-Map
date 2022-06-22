using MindMapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapApi.Bll
{
    public class FilesBll : IFilesBll<UsersFiles>
    {
        MindMapEntities db = new MindMapEntities();
        public int Delete(UsersFiles file)
        {
            db.Entry(file).State = System.Data.Entity.EntityState.Deleted;
            return db.SaveChanges();
        }

        public UsersFiles Get(int id)
        {
            return db.UsersFiles.Where(file => file.id == id).FirstOrDefault();
        }

        public List<UsersFiles> GetByFileName(string filename)
        {
            return db.UsersFiles.Where(file => file.filename == filename).ToList();
        }

        public List<UsersFiles> GetAllByUserName(string username)
        {
            return db.UsersFiles.Where(files => files.username == username).ToList();
        }

        public List<UsersFiles> GetAllByPath(string path)
        {
            return db.UsersFiles.Where(files => files.filepath == path).ToList();
        }

        public int Insert(UsersFiles file)
        {
            db.Entry(file).State = System.Data.Entity.EntityState.Added;
            return db.SaveChanges();
        }

        public int Update(UsersFiles file)
        {
            db.Entry(file).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }

        public UsersFiles GetByFileNameAndPath(string fileName, string path)
        {
            return db.UsersFiles.Where(file => file.filename == fileName && file.filepath == path).FirstOrDefault();
        }

        public UsersFiles Get(string username, string filename, string path)
        {
            return db.UsersFiles.Where(file => file.username == username && file.filename == filename && file.filepath == path).FirstOrDefault();
        }
    }
}

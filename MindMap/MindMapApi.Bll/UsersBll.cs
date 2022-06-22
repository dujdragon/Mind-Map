using MindMapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapApi.Bll
{
    public class UsersBll : IUsersBll<UsersLogin>
    {
        MindMapEntities db = new MindMapEntities();
        public int Delete(UsersLogin user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
            return db.SaveChanges();
        }

        public UsersLogin Get(int id)
        {
            return db.UsersLogin.Where(user => user.id == id).FirstOrDefault();
        }

        public UsersLogin Get(string name)
        {
            return db.UsersLogin.Where(user => user.username == name).FirstOrDefault();
        }

        public List<UsersLogin> GetAll()
        {
            return db.UsersLogin.ToList();
        }

        public int Insert(UsersLogin user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Added;
            return db.SaveChanges();
        }

        public int Update(UsersLogin user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }
    }
}

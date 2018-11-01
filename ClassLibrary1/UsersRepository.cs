using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ClassLibrary1
{
    public class UsersRepository:ConnectiomClass
    {
        public UsersRepository():base()
        { }

        public bool Login(string email, string password)
        {
            if (Entity.Users.SingleOrDefault(x => x.Email == email && x.Password == password) == null)
                return false;
            else
                return true;
        }

        public IQueryable<Role> GetRolesOfUser(string email)
        {
            return Entity.Users.SingleOrDefault(x => x.Email == email).Roles.AsQueryable();
        }

        /*
        public void IncrementNoOfAttempts(string email)
        { }

        public void BlockUser(string email)
        {}

        public void ResetNoOfAttempts(string email)
        {}

        public User GetUser(string email)
        { }

        public bool IsUserBlocked(string email)
        { }
        */
    }
}

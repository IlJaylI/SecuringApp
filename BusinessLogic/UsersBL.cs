using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using Common;

namespace BusinessLogic
{
    public class UsersBL
    {
        public bool Login(string email, string password)
        {
            
            UsersRepository ur = new UsersRepository();
            if(ur.IsUserBlocked(email))
            {
                throw new CustomException("Your Account is blocked. Please contact admin");
            }
            else
            {
                return new UsersRepository().Login(email, password);
            }
            //IsUserBlocked
                //if not 
                //check username and password
                    //if not ok
                        //increment no of attempts
                        //check whether no of attempts >= 3
                            //if yes
                                //block user return false
                            //if no
                                //return false
                        //if ok 
                            //reset no of attempts return true
                   
                    //if blocked return false
        }

        public IQueryable<Role> GetRolesOfUser(string email)
        {
            return new UsersRepository().GetRolesOfUser(email);
        }

        public User GetUser(string email)
        {
            return new UsersRepository().GetUser(email);
        }
    }
}

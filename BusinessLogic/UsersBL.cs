using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
                return new UsersRepository().Login(email, Encryption.HashPassword(password));
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

        public void Register(User u)
        {
            UsersRepository ur = new UsersRepository();
            RolesRepository rr = new RolesRepository();
            ur.Entity = rr.Entity;//avoid an error of mvc not allowed to edit "different" entities

            //distributed transactions Add reference(system.transactions)

            using (TransactionScope ts = new TransactionScope())
            {
                if (ur.GetUser(u.Email) == null)
                {
                    //used to generate the a new guid for every user
                    u.Id = Guid.NewGuid();//not set to increment
                    u.Password = Encryption.HashPassword(u.Password);
                    var keys = Encryption.GenerateAsymeticKeys();
                    u.PublicKey = keys.PublicKey;
                    u.PrivateKey = keys.PrivateKey;

                    ur.AddUser(u);

                    var role = rr.GetRole(1);
                    rr.AllocateRoleToUser(u, role);

                }
                else
                {
                    throw new CustomException("Email is already taken.");
                }

                ts.Complete();//confirming the succesfull transaction 
            }//if something goes wrong it exists the transaction and the database will not be affected

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ClassLibrary1
{
    public class RolesRepository : ConnectiomClass
    {
        public RolesRepository() : base()
        {

        }

        public Role GetRole(int roleId)
        {
            return Entity.Roles.SingleOrDefault(x => x.Id == roleId);
        }

        public void AllocateRoleToUser(User u, Role r)
        {
            //line below interacts with the middle table to create a new entry into it
            u.Roles.Add(r);
            Entity.SaveChanges();
        }
    }
}

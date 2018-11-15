using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ClassLibrary1
{
    public class ConnectiomClass
    {
        public TJEntities Entity { get; set; }

        public ConnectiomClass()
        {
            Entity = new TJEntities();
        }//connection intailisation

        public ConnectiomClass(bool isAdmin)
        {
            if(isAdmin)
            {
                Entity = new TJEntities();// default connection full privalages
            }
            else
            {
                Entity = new TJEntities();
                Entity.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["TJEntities_G"].ConnectionString;//changing connection to be on guest limited permissions
                //config manger allows reading of web config 
            }
        }
    }
}

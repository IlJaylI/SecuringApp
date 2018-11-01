using System;
using System.Collections.Generic;
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
    }
}

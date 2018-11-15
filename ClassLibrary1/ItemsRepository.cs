using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ClassLibrary1
{
    public class ItemsRepository : ConnectiomClass
    {
        public ItemsRepository():base()
        {}//intialises the connection using the base class method neccesary for all repos

        public ItemsRepository(bool isAdmin):base(isAdmin)
        { }

        #region Select
        public Item GetItem(int id)
        {
            return Entity.Items.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Item> GetItems() 
        {
            //IQueryable all data remains in the sql domain
            //using IQueryable rather than entity.items.tolist makes the performance faster
            //select * From Items
            return Entity.Items;
        }
        #endregion

        #region Insert
        public void AddItem(Item i)
        {
            Entity.Items.Add(i);//putting item into memory
            Entity.SaveChanges();//saveing the data stored into memory
        }
        #endregion


        #region Delete
        public void DeleteItem(Item i)
        {
            Entity.Items.Remove(i);
            Entity.SaveChanges();
        }
        #endregion
    }
}

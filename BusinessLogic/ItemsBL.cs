using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using ClassLibrary1;

namespace BusinessLogic
{
    public class ItemsBL
    {

        public  IQueryable<Item> GetItems()
        {
            return new ItemsRepository().GetItems();
        }

        public Item GetItem(int id)
        {
            return new ItemsRepository().GetItem(id);
        }

        public void AddItem(string name, decimal price, int category, string imagePath)
        {
            Item i = new Item();
            i.Name = name;
            i.Price = price;
            i.Category_fk = category;

            if (string.IsNullOrEmpty(imagePath) == false)
            {
                i.ImagePath = imagePath;
            }

            new ItemsRepository().AddItem(i);
        }

        public void DeleteItem(int id)
        {
            ItemsRepository ir = new ItemsRepository();

            var myItem = ir.GetItem(id);
            if(myItem != null)//clr: common engine run time throws an exception if item doesnt exist
            {
                ir.DeleteItem(myItem);
            }
            
        }
    }
}

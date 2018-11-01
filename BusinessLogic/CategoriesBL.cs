using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using Common;

namespace BusinessLogic
{
    public class CategoriesBL
    {
        public IQueryable<Category> GetCategories()
        {
            return new CategoriesRepositrory().GetCategories();
        }
    }
}

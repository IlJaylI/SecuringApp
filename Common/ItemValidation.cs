using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ItemValidation
    {
        //items to validate copied from items in common autgenerated class of item

        //form requires the data to continue property has hidden elements
        [Required(AllowEmptyStrings = false, ErrorMessage ="Please input Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please input Price")]
        [Range(1,1000000,ErrorMessage ="Prices should be greater than 0")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Category")]
        [Range(1,10,ErrorMessage = "Please Select Category")]
        public int Category_fk { get; set; }

        /*
         * Other Validators 
         * Compare Validator: Password - Confirm Password scenerio
         * Regular Expression(Regex): Uniform values
         * 
         * Display - not a validator but applies to this class specifies what to diplay in that field
         */
    }

    //metadata means item description
    [MetadataType(typeof(ItemValidation))]//signifies description is find in itemvalidation above
    public partial class Item
    { }
}

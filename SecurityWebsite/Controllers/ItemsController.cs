using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common;
using BusinessLogic;
using System.IO;

namespace SecurityWebsite.Controllers
{
    public class ItemsController : Controller
    {
        // GET: Items // Roles = "rolename from database"
        [Authorize(Roles = "Admin")]//only authenticated ppl can access the method will be redirected to login
        public ActionResult Index()
        {
            ItemsBL myItems = new ItemsBL();//user.IsInRole("admin");in parameters to allowpermissions
            var list = myItems.GetItems().ToList();

            return View(list);
        }

        public ActionResult Details(string id)
        {
            try
            {
                var orginalValue = Encryption.DecryptQueryString(id);

                ItemsBL myItems = new ItemsBL();
                var item = myItems.GetItem(Convert.ToInt32(orginalValue));

                return View(item);
            }
            catch
            {
                TempData["errormessage"] = "Acces denied or value invalid";


                return RedirectToAction("Index");
            }
        }

        //[HttpGet] the opposite of post used if post isnt present
        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]//complments antiforgery token !applied to every form!
        [HttpPost]//specifies what method to handle the button click result
        public ActionResult Create(Item i, HttpPostedFileBase fileData)//can put the item db field one by one and they need to be exactly the same
        {
            //fileData is the equivelnt in the create html
            try
            {
                if(ModelState.IsValid)
                {
                    /*
                    for(int j = 0; j <10; j ++) //read on byte at a time
                    {
                        int myByte = fileData.InputStream.ReadByte();
                        //implement if condition to check for an image
                    }
                    */

                    //byte[] bytesRead = new byte[10];//reads the first 10 bytes at one go
                    //fileData.InputStream.Read(bytesRead, 0, 10);

                    //gary kessler filesignitures

                        string uniqueFilename = Guid.NewGuid() + Path.GetExtension(fileData.FileName);
                        // unique id plus extenstion

                        //I:\Securing Application\SecuringApp\SecurityWebsite\Images
                        string absolutePath = Server.MapPath(@"\Images") + @"\";//this is returns the absolute path of the local images folder thus creating the needed pat to save the new image
                                                                                //fileData.SaveAs(absolutePath + uniqueFilename);//saving the image revomed due to encryption

                        //var user = new UserBL().getUser(User.identitiy.name);
                        //MemoryStream msEncrypted = Encryption.HybridEncrypt(fileData.InputStream, publickey);
                       //System.IO.File.WriteAllBytes(abolutepath + uniquefilename);

                        //call the signdata here by also getting the user.privatekey
                        //remeber to store the signature in the i.signature

                        //filedata.SaveAs(abolutepath +uniquefilename) line decremented
                        i.ImagePath = @"\Images\" + uniqueFilename;
                        //<img src="" only requires relative therefore no use in saving the entire image string path into the db

                        new ItemsBL().AddItem(i.Name, i.Price, i.Category_fk, i.ImagePath);
                        TempData["message"] = "Item added succesfully";

                        return RedirectToAction("Index");//name of method to to redirect to look above
                    
                }

                return View(i);                
            }
            catch (Exception ex)
            {
                //log the error message
                TempData["errormessage"] = "Item was not added";

                return View(i);//returning the form with the details he entered
            }
        }


        public ActionResult Delete(int id)//refers to html in index that how it auto connects to link
        {
            try
            {
                new ItemsBL().DeleteItem(id);
                Logger.Log("",Request.Path,"Item"+id+"Deleted");

                TempData["message"] = "Item Deleted";
            }
            catch (Exception ex)
            {
                Logger.Log("", Request.Path, "Error: " + ex.Message);//logging using logger class in common

                TempData["errormessage"] = "Item not Deleted";
            }

            return RedirectToAction("Index");//return to main list
        }


        public ActionResult Download(int id)
        {
            Item item = new ItemsBL().GetItem(id);

            if (item.ImagePath != null)
            {
                string abolutePath = Server.MapPath(item.ImagePath);

                if (System.IO.File.Exists(abolutePath))
                {
                    byte[] data = System.IO.File.ReadAllBytes(abolutePath);

                    MemoryStream msIn = new MemoryStream(data);
                    msIn.Position = 0;

                    //var audiofilerecord = new audiobl().getaudioid(id);
                    //var user = new UserBL.getUser();

                    //call the verifydata passing the signature retrieved from the audiofilerecord.signature,user.publickey
                    //and pass the msIn.ToArray();

                    //if the output of the verifydata method is false stop the dowload

                    //MemoryStream msDecrypted = Encryption.HybridDecrypt(msIn, user.PrivateKey);
                    //return File(msDecryped.toArray(), System.Net.Mime.MediaTypeNames.Application.Octet,
                    //Path.GetFileName(item.ImagePath));

                    return File(data, System.Net.Mime.MediaTypeNames.Application.Octet,
                                    Path.GetFileName(item.ImagePath));
                }
                else return null;
            }
            else return null;
            
        }
    }
}
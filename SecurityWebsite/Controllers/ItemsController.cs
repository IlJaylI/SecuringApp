﻿using System;
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
            ItemsBL myItems = new ItemsBL();
            var list = myItems.GetItems().ToList();

            return View(list);
        }

        public ActionResult Details(int id)
        {
            ItemsBL myItems = new ItemsBL();
            var item = myItems.GetItem(id);

            return View(item);
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
                        fileData.SaveAs(absolutePath + uniqueFilename);//saving the image

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
    }
}
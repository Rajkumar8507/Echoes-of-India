using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Xml.Linq;
using Tourist_places.Models;

namespace Tourist_places.Controllers
{
    public class TouristController : Controller
    {

        private readonly TouristContext _context;

        public TouristController()
        {
            _context = new TouristContext();
        }
        public ActionResult Home()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(users model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Message = "Username and Password cannot be empty.";
                
            }
            users u = _context.users_c.ToList().Find(t => t.UserName ==model.UserName || t.Password == model.Password);
            if (u == null)
            {
                users newUser = new users
                {
                    UserName = model.UserName,
                    Password = model.Password
                };
                _context.users_c.Add(newUser);
                _context.SaveChanges();
            }
            else
            {
                ViewBag.Message="user already exists with this data try to register with new one";
            }
          return View();               
        }


        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Users(users model)
        {
            
            users u = _context.users_c.ToList().Find(t => t.UserName ==model.UserName && t.Password == model.Password);
            if (u != null)
            {
                return RedirectToAction("return_user");
            }
            else
            {
                ViewBag.Message = "invalid credentials please register ";
            }
            return View();
        }
        public ActionResult return_user(string Name)
        {
            List<TouristPlace> tplist;
            if (!string.IsNullOrEmpty(Name))
            {
                tplist = _context.touristPlaces
                                 .Include("touristPlaceType")
                                 .Where(tp => tp.TouristPlaceName.Contains(Name))
                                 .ToList();
                if(tplist==null)
                {
                    ViewBag.Message = "there is no matching Place";
                }
            }
            else
            {
                tplist = _context.touristPlaces.Include("touristPlaceType").ToList();
            }

            return View(tplist);
        }
        public ActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin(string AdminName,string AdminPassword)
        {
           string adminname = "rajkumar";
           string  adminpassword = "7";
            if (AdminName ==adminname  && AdminPassword == adminpassword)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Enter valid credentials";  
            }

            return View();
        }
        public ActionResult Index(string Name)
        {
            List<TouristPlace> tplist;
            if (!string.IsNullOrEmpty(Name))
            {
                tplist = _context.touristPlaces
                                 .Include("touristPlaceType")
                                 .Where(tp => tp.TouristPlaceName.Contains(Name))
                                 .ToList();
            }
            else
            {
                tplist = _context.touristPlaces.Include("touristPlaceType").ToList();
            }

            return View(tplist);
        }



        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> typelist = new List<SelectListItem>();
            foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
            {
                typelist.Add(
                new SelectListItem()
                {
                    Text = ttype.TouristPlaceTypeName,
                    Value = ttype.TouristPlaceTypeId.ToString()
                });
            }
            ViewBag.TouristPlaceTypeId = typelist;
            return View();
        }

        [HttpPost]
        public ActionResult Create(TouristPlace tp)
        {
           
            if (Request.Files.Count > 0)
            {
                tp.Files = Request.Files[0];
                tp.ImagePath = "/Images/" + tp.Files.FileName;

                
                if (!tp.Files.ContentType.Equals("image/jpeg"))
                {
                    ModelState.AddModelError("TpImageFile", "Invalid Image Type");
                }

                
                if (tp.Files.ContentLength <= 0 || tp.Files.ContentLength > 100000)
                {
                    ModelState.AddModelError("TpImageFile", "Invalid File Size");
                }
            }
            else
            {
                ModelState.AddModelError("TpImageFile", "Please upload a file.");
            }

            if (ModelState.IsValid)
            {
                
                var imagePath = Server.MapPath(tp.ImagePath);
                var directory = Path.GetDirectoryName(imagePath);

                
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                
                tp.Files.SaveAs(imagePath);

                
                _context.touristPlaces.Add(tp);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            
            List<SelectListItem> typelist = new List<SelectListItem>();
            foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
            {
                typelist.Add(
                    new SelectListItem()
                    {
                        Text = ttype.TouristPlaceTypeName,
                        Value = ttype.TouristPlaceTypeId.ToString()
                    });
            }
            ViewBag.TouristPlaceTypeId = typelist;

            return View(tp);
        }

        public ActionResult Edit(int id)
        {
            TouristPlace tp = _context.touristPlaces.Include("touristPlaceType").ToList().Find(t => t.TouristPlaceId == id);
            List<SelectListItem> typelist = new List<SelectListItem>();
            foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
            {
                typelist.Add(
                new SelectListItem()
                {
                    Text = ttype.TouristPlaceTypeName,
                    Value = ttype.TouristPlaceTypeId.ToString()
                });
            }
            ViewBag.TouristPlaceTypeId = typelist;
            return View(tp);
        }

        [HttpPost]
        public ActionResult Edit(TouristPlace tp)
        {
            tp.Files = Request.Files[0];
            if (tp.Files.ContentLength > 0 && tp.Files.ContentLength < 100000)
            {

                if (!tp.Files.ContentType.Equals("image/jpeg"))
                {
                    ModelState.AddModelError("TpImageFile", "Invalid Image Type");
                    List<SelectListItem> typelist = new List<SelectListItem>();
                    foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
                    {
                        typelist.Add(
                        new SelectListItem()
                        {
                            Text = ttype.TouristPlaceTypeName,
                            Value = ttype.TouristPlaceTypeId.ToString()
                        });
                    }
                    ViewBag.TouristPlaceTypeId = typelist;
                    return View(tp);
                }

                tp.ImagePath = "/Images/" + tp.Files.FileName;
            }
            else
            {
                tp.Files = null;
            }


            if (ModelState.IsValid)
            {
                TouristPlace etp = _context.touristPlaces.Find(tp.TouristPlaceId);
                etp.TouristPlaceName = tp.TouristPlaceName;
                etp.TouristPlaceDescription = tp.TouristPlaceDescription;
                etp.TouristPlaceTypeId = tp.TouristPlaceTypeId;
               
                if (tp.Files != null)
                {
                    if (!etp.ImagePath.Equals(tp.ImagePath))
                    {
                        etp.ImagePath = tp.ImagePath;
                        tp.Files.SaveAs(Server.MapPath(etp.ImagePath));
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> typelist = new List<SelectListItem>();
                foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
                {
                    typelist.Add(
                    new SelectListItem()
                    {
                        Text = ttype.TouristPlaceTypeName,
                        Value = ttype.TouristPlaceTypeId.ToString()
                    });
                }
                ViewBag.TouristPlaceTypeId = typelist;
                return View(tp);
            }
        }
        public ActionResult Delete(int id)
        {
            var touristPlace = _context.touristPlaces.Find(id);
            if (touristPlace == null)
            {
                return HttpNotFound();
            }
            return View(touristPlace);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var touristPlace = _context.touristPlaces.Find(id);
            if (touristPlace != null)
            {
                _context.touristPlaces.Remove(touristPlace);
                _context.SaveChanges();
            }
            return RedirectToAction("Index"); 
        }
        





    }
}

        
    
    
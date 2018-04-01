using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PcStoreWebsite.Models;
using System.Threading;

namespace PcStoreWebsite.Controllers
{
    public class CBLoginController : Controller
    {
       
        public ActionResult ViewMyOrders()
        {
            if (Session["UserId"] == null)
            {
                return View("Login");
            }
            var MyID = Session["UserId"].ToString();
            //Now we make the user login automaticlly login in after registartion.
            using (DBConnection db = new DBConnection())
            {
                var usr = db.Orders.Where(u => u.UserId == MyID).FirstOrDefault();
                var MyUserId = Session["UserId"].ToString();
                //if we found orders for the user
                if (Session["UserId"] != null)
                {
                    //List<Orders> myOrder = (from x in db.Orders where MyID == usr.UserId.ToString() select x).ToList<Orders>();
                    //List<Products> Product = (from x in db.Products select x).ToList<Products>();
                    List<Products> myProducts = (from x in db.Products join y in db.Orders on MyUserId equals y.UserId where (x.ProductId == y.ProductId) && (MyUserId == y.UserId) select x).ToList<Products>();
                    //List<Products> myRealProducts;
                    //for(int i=0;i<myOrder.Count;i++)
                    //{
                    //    myRealProducts.Add
                    //}

                    return View(myProducts.ToList());

                }
                else
                {
                    //There are no orders.
                    return View();
                }
            }
        }
        public ActionResult Test()
        {
            return View();
        }
        
        public ActionResult Registration()
        {
            return View();
        }
    
        [HttpPost]
        public ActionResult Registration(Login User)
        {
            Encryption enc = new Encryption();
            //Save to Data Base
            if (User.Email == null)
            {
                User.Email = "";
            }
            if (User.Catagory == null)
            {
                User.Catagory = "Other";
            }
            if (User.UserPass == null)
            {
                return RedirectToAction("Registration");
            }
            if (User.Credits == null)
            {
                User.Credits = "10000";
            }
            if (ModelState.IsValid)
            {
                string hashedPassword = enc.CreateHash(User.UserPass);
                User.UserPass = hashedPassword;

                using (DBConnection db = new DBConnection())
                {
                    db.Login.Add(User);
                    db.SaveChanges();
                }
                ModelState.Clear();
                //ViewBag.Message = User.UserId + "" + "Successfully Registered!";    
            }


            //Now we make the user login automaticlly login in after registartion.
            using (DBConnection db = new DBConnection())
            {

                var usr = db.Login.Where(u => u.UserId == User.UserId && u.UserPass == User.UserPass).FirstOrDefault();
                if (usr != null)
                {
                    Session["UserId"] = usr.UserId.ToString();
                    Session["UserPass"] = usr.UserPass.ToString();
                    Session["isAdmin"] = usr.isAdmin.ToString();
                    Session["Email"] = usr.Email.ToString();
                    Session["Catagory"] = usr.Catagory.ToString();                    
                    Session["Credits"] = usr.Credits.ToString();
                    Session["Flag"] = usr.Flag.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "ERROR: Username or password are not correct.");
                }
            }

            return View("LoggedIn");
        }
        public ActionResult ViewTableIndex()
        {
            Thread.Sleep(4000);
            if (Session["UserId"] != null)
            {
                if (Session["isAdmin"].ToString().Equals("True"))
                {
                    using (DBConnection db = new DBConnection())
                    {
                        return Json(db.Login.ToList(), JsonRequestBehavior.AllowGet);
                    }
                }
                ViewBag.ErrorMessage = "You are not an Admin.";
                return View("Login");
            }
            return View("Login");

        }


        public ActionResult Index()
        {

            if (Session["UserId"] != null)
            {
                if (Session["isAdmin"].ToString().Equals("True"))
                {
                    using (DBConnection db = new DBConnection())
                    {
                        return View(db.Login.ToList());
                        //return Json(db.Login.ToList(), JsonRequestBehavior.AllowGet);
                        //return View(db.Login.ToList());
                    }
                }
                ViewBag.ErrorMessage = "You are not an Admin.";
                return View("Login");

            }
            else
            {
                ViewBag.ErrorMessage = "You are not an Admin.";
                return View("Login");
            }
        }



        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }
        // GET: CBLogin
        public ActionResult Login()
        {

            //if user already logged in
            if (Session["UserId"] != null)
            {
                return RedirectToAction("LoggedIn");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(Login user)
        {

            Encryption enc = new Encryption();
            using (DBConnection db = new DBConnection())
            {
                   
                //var usr = db.Login.Where(u => u.UserId == user.UserId && u.UserPass == user.UserPass).FirstOrDefault();
                var usr = db.Login.Where(u => u.UserId == user.UserId).FirstOrDefault();
                if (usr != null)
                {
                    if (enc.ValidatePassword(user.UserPass, usr.UserPass))
                    {
                        Session["UserId"] = usr.UserId.ToString();
                        Session["UserPass"] = usr.UserPass.ToString();
                        Session["isAdmin"] = usr.isAdmin.ToString();
                        Session["Email"] = usr.Email.ToString();
                        Session["Credits"] = usr.Credits.ToString();
                        Session["Catagory"] = usr.Catagory.ToString();
                        Session["Flag"] = usr.Flag.ToString();
                        return RedirectToAction("LoggedIn");
                    }
                    else
                    {
                        ModelState.AddModelError("", "ERROR: Username or password are not correct.");
                    }
                }
                
            }
            return View();

        }
        public ActionResult MyProfile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                
                return View();
            }
        }

        
        
        //after Update
        [HttpPost]
        public ActionResult MyProfile(Login Usr)
        {
            //Save to Data Base
            if (ModelState.IsValid)
            {
                using (DBConnection db = new DBConnection())
                {
                    //The Default values

                    Usr.UserId = Session["UserId"].ToString();
                    Usr.Credits = Session["Credits"].ToString();
                    Usr.isAdmin = Convert.ToBoolean(Session["isAdmin"]);
                    Usr.Flag = Convert.ToBoolean(Session["Flag"]);
                    var current = Session["UserId"].ToString();
                    //Login usr = (from x in db.Login where x.UserId == Usr.UserId select x).ToList<Login>();
                    var usr = db.Login.Where(u => u.UserId == current).FirstOrDefault();
                    
                    //Remove Old one from data base
                    db.Login.Remove(usr);
                    //Update now
                    db.Login.Add(Usr);
                    db.SaveChanges();
                }
                ModelState.Clear();
                //ViewBag.Message = User.UserId + "" + "Successfully Registered!";    
            }
            if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                return RedirectToAction("Logout");
            }
        }

     
        /// <summary>
        /// saving the user details
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUs()
        {
            if (Session["UserId"] == null)
            {
                return View("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(ContactUs cus)
        {
            
            if(Session["UserId"]==null)
            {
                return View("Login");
            }

            //Save to Data Base
            if (ModelState.IsValid)
            {
                using (DBConnection db = new DBConnection())
                {
                    db.ContactUs.Add(cus);
                    db.SaveChanges();
                }
                ModelState.Clear();
            }
            return View();
        }


        //here we present the products to the user.
        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {

                // return View();

               // var MyID = Session["UserId"].ToString();
                //Now we make the user login automaticlly login in after registartion.
                using (DBConnection db = new DBConnection())
                {
                    var myCategory = Session["Catagory"].ToString();
                    List<Products> ViewProductsAccordingToMyCatagory = (from x in db.Products where (myCategory.ToString()==x.Category.ToString()) select x).ToList<Products>();
                    return View(ViewProductsAccordingToMyCatagory.ToList());
                }
            }
            else return RedirectToAction("Login");
        }

        public ActionResult AlreadyPaid()
        {
            return View();
        }
        public ActionResult WrongPage()
        {
            return View();
        }
        public ActionResult BuyItem(int id)
        {
            if(Session["UserId"]==null)
            {
                return RedirectToAction("WrongPage");
            }
            using (DBConnection db = new DBConnection())
            {
                Products getProduct = (from x in db.Products where x.ProductId == id select x).ToList<Products>().ElementAt(0);
                //List<Orders> getOrder = (from x in db.Orders where (x.ProductId == id) && (x.UserId.ToString() == Session["UserId"].ToString()) select x).ToList<Orders>();

                var UserId = Session["UserId"].ToString();
                List<Orders> getOrder = (from x in db.Orders where (x.ProductId == id) &&(x.UserId == UserId) select x).ToList<Orders>();

                if (getOrder!=null && getOrder.Count>0)
                {
                    return RedirectToAction("AlreadyPaid");
                }
                if (getProduct!=null)
                {
                    return View(getProduct);
                }
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult BuyItem(Products prod)
        {
            Session["Credits"] = Convert.ToInt32(Session["Credits"]) - prod.Price;
            
            //update the credits;
            using (DBConnection db = new DBConnection())
            {
                Orders newOrder = new Orders();
                newOrder.ProductId = prod.ProductId;
                newOrder.UserId = Session["UserId"].ToString();
                db.Orders.Add(newOrder);
                db.SaveChanges();
            }
            ViewBag.Message = "Order Number"+prod.ProductId +"has been successfully paid.";    
            return RedirectToAction("ViewMyOrders");
        }

    }


  
}
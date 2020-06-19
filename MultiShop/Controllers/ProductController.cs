using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiShop.Controllers
{
    
    public class ProductController : Controller
    {
        MultiShopDbContext db = new MultiShopDbContext();
        //
        // GET: /Product/
        public ActionResult Category(int CategoryID=0)
        {
            if(CategoryID!=0)
            {
                
                var model = db.Products.Where(p => p.CategoryId == CategoryID);
                return View(model);
            }
            
            return View();
        }

        public ActionResult Search(String SupplierId = "", int CategoryId = 0, String Keywords = "")
        {
            if (SupplierId != "")
            {
                var model = db.Products
                    .Where(p => p.SupplierId == SupplierId);
                return View(model);
            }
            else if (CategoryId != 0)
            {
                var model = db.Products
                    .Where(p => p.CategoryId == CategoryId);
                return View(model);
            }
            else if (Keywords != "")
            {
                var model = db.Products
                    .Where(p => p.Name.Contains(Keywords));
                return View(model);
            }
            return View(db.Products);
        }

        public ActionResult Detail(int id,string SupplierID)
        {
            var model = db.Products.Find(id);

            
            model.Views++;
            db.SaveChanges();

           
            var views = Request.Cookies["views"];
            
            if (views == null)
            {
                views = new HttpCookie("views");
            }
            
            views.Values[id.ToString()] = id.ToString();
            
            views.Expires = DateTime.Now.AddMonths(1);
       
            Response.Cookies.Add(views);

           
            var keys = views.Values
                .AllKeys.Select(k => int.Parse(k)).ToList();
            
            ViewBag.Views = db.Products
                .Where(p => keys.Contains(p.Id));
            return View(model);
        }
	}
}
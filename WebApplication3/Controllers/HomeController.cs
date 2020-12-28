using MusicStoree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoree.EFContext;

namespace MusicStoree.Controllers
{
    public class HomeController : Controller
    {
        MusicStoreDB db = new MusicStoreDB();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "StoreManager");
            }
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);
            return View(albums);
        }
        
        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return db.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}
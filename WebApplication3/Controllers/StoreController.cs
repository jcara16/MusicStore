using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MusicStoree.Models;
using MusicStoree.EFContext;

namespace MusicStoree.Controllers
{
    public class StoreController : Controller
    {
        private MusicStoreDB db = new MusicStoreDB();

        // GET: Store
        public ActionResult Index()
        {
            return View(db.Genres.ToList());
        }

        //
        // GET: /Store/Browse?genre=Disco
        public ActionResult Browse(string genre)
        {
            //Retrieve Genre and its Associated Albums from database
            var genreModel = db.Genres.Include("Albums").Single(g => g.Name == genre);
            //List<Album> albums = genreModel.Albums;
            return View(genreModel);
        }

        public ActionResult Search(string searchString)
        {
            var albums = from a in db.Albums
                         select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(s => s.Title.Contains(searchString));
            }

            return View(albums);
        }

        //
        // GET: /Store/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            return View(album);
        }
        // GET: /Store/GenreMenu
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = db.Genres.ToList();
            return PartialView(genres);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}

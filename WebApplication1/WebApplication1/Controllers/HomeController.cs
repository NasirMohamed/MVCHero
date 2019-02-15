using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private HeroListEntities4 db = new HeroListEntities4();
        // GET: Home
        public ActionResult Index(string heroMedium, string searchString)
        {
            //medium search = making a list for drop down
            var MediumList = new List<string>();
            //LINQ query to db
            var MediumQuery = from d in db.HeroLists
                             orderby d.Medium
                             select d.Medium;
            //add unique values
            MediumList.AddRange(MediumQuery.Distinct());
            //put in ViewBag for use in View
            ViewBag.heroMedium = new SelectList(MediumList);


            //get all heroes from the db
            var heros = from m in db.HeroLists
                         select m;

            //if a medium search has been done, reduce list of heroes to only the ones  from the selected medium

            if (!String.IsNullOrEmpty(heroMedium))
            {
                heros = heros.Where(x => x.Medium == heroMedium);
            }


            //if a name search has been done, reduce list of heroes to only the ons matching the search
            if (!String.IsNullOrEmpty(searchString))
            {
                heros = heros.Where(s => s.Name.Contains(searchString));
            }
            return View(heros);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HeroList hero)
        {
            if (hero.Picture == null)
            {
                hero.Picture = "http://oi64.tinypic.com/iodgjr.jpg";
            }
            if (ModelState.IsValid)
            {
                db.HeroLists.Add(hero);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(hero);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeroList hero = db.HeroLists.Find(id);
            if (hero == null)
            {
                return HttpNotFound();
            }
            return View(hero);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeroList hero = db.HeroLists.Find(id);
            return View(hero);
        }

        [HttpPost]
        public ActionResult Edit(HeroList hero)
        {
            if (hero.Picture == null)
            {
                hero.Picture = "http://oi64.tinypic.com/iodgjr.jpg";
            }

            if (ModelState.IsValid)
            {

                db.Entry(hero).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(hero);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeroList hero = db.HeroLists.Find(id);
            return View(hero);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            HeroList hero = db.HeroLists.Find(id);
            db.HeroLists.Remove(hero);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
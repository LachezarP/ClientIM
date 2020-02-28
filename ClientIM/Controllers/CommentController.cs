﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    public class CommentController : Controller
    {

        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Comment
        public ActionResult Index(int id)
        {
            ViewBag.id = id;

            Models.Picture thePicutre = db.Pictures.SingleOrDefault(c => c.picture_id == id);

            return View(thePicutre);
        }

        // GET: Comment/Create
        public ActionResult Create(int id)
        {
            ViewBag.id = db.Pictures.SingleOrDefault(p => p.picture_id == id);

            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                int personId = Int32.Parse(Session["person_id"].ToString());
                Models.Comment newComment = new Models.Comment {
                    picture_id = id,
                    person_id = personId,
                    comment1 = collection["comment1"],
                    timestamp = DateTime.Now.ToString(),
                    read = "Not read"
                };
                db.Comments.Add(newComment);
                db.SaveChanges();

                return RedirectToAction("Index", "Comment", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            Models.Comment theComment = db.Comments.SingleOrDefault(c => c.comment_id == id);

            return View(theComment);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Models.Comment theComment = db.Comments.SingleOrDefault(c => c.comment_id == id);

                theComment.comment1 = collection["comment1"];
                db.SaveChanges();

                return RedirectToAction("Index","Comment", new { id = theComment.picture_id});
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Comment theComment = db.Comments.SingleOrDefault(c => c.comment_id == id);

            return View(theComment);
        }

        // POST: Comment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Comment theComment = db.Comments.SingleOrDefault(c => c.comment_id == id);

                db.Comments.Remove(theComment);
                db.SaveChanges();

                return RedirectToAction("Index", "Comment", new { id = theComment.picture_id });
            }
            catch
            {
                return View();
            }
        }
    }
}

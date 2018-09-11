using EFApproaches.DAL.Entities;
using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EFApproaches.Controllers
{
    public class CourseController : BaseController
    {
        #region private members
        private static int minimumStudentsToStayOpen = 10;
        #endregion
        #region constructor
        public CourseController() { }
        public CourseController(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        #endregion
        // GET: Course
        public ActionResult Index()
        {
            return View(unitOfWork.CourseRepo.DataSet);
        }
        #region CRUD Actions
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = unitOfWork.CourseRepo.GetById(id);

            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.MinimumStudentsToStayOpen = minimumStudentsToStayOpen;
            return View(course);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Credits")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.CourseRepo.Create(course);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("ExceptionError", "Unable to save changes. Try again, and if the problem persists see your system administrator, error: " + ex.Message);
            }
            //If model is not valid
            return View(course);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = unitOfWork.CourseRepo.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Students/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //If student cannot be updated return default view with student object
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            Course courseToUpdate = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                courseToUpdate = unitOfWork.CourseRepo.GetById(id);
                if (null == courseToUpdate)
                {
                    ModelState.AddModelError("NullModelError", "The course you tried to update was not found");
                    courseToUpdate = new Course();
                }
                if (TryUpdateModel(courseToUpdate, "",
                    new string[] { "Title", "Credits" }))
                {
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("SaveChangesExceptionError", "Unable to save changes. " + ex.Message);
                return View(new Course());
            }
            return View(courseToUpdate);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Course course = unitOfWork.CourseRepo.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Post(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
               Course course = unitOfWork.CourseRepo.GetById(id);
                if (null == course)
                {
                    return HttpNotFound();
                }
                unitOfWork.CourseRepo.Delete(course);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }


        #endregion
    }
}
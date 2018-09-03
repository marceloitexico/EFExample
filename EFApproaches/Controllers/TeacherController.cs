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
    public class TeacherController : BaseController
    {
        #region private members
        private static byte inexistentTeacherID = 0;
        #endregion private members
        #region constructor
        public TeacherController() { }
        public TeacherController(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        #endregion constructor

        // GET: Teachers
        public ActionResult Index()
        {
            return View(unitOfWork.TeacherRepo.DataSet);
        }
        #region CRUD Actions

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = unitOfWork.TeacherRepo.GetById(id);

            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName, Title, EmailAddress")] Teacher Teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //fill the email of the Teacher
                    unitOfWork.TeacherRepo.Create(Teacher);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception dataEx)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("ExecptionError", "Unable to save changes. Try again, and if the problem persists see your system administrator, error: " + dataEx.Message);
            }
            //If model is not valid
            return View(Teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher Teacher = unitOfWork.TeacherRepo.GetById(id);
            if (Teacher == null)
            {
                return HttpNotFound();
            }
            return View(Teacher);
        }

        // POST: Teachers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //If Teacher cannot be updated return default view with Teacher object
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            Teacher teacherToUpdate = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                teacherToUpdate = unitOfWork.TeacherRepo.GetById(id);
                if (null == teacherToUpdate)
                {
                    ModelState.AddModelError("NullModelError", "The Teacher you tried to update was not found");
                    teacherToUpdate = new Teacher();
                }
                if (TryUpdateModel(teacherToUpdate, "",
                    new string[] { "LastName", "FirstMidName", "Title", "EmailAddress" }))
                {
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("SaveChangesExceptionError", "Unable to save changes. " + ex.Message);
                return View(new Teacher());
            }
            return View(teacherToUpdate);
        }

        // GET: Teachers/Delete/5
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
            Teacher Teacher = unitOfWork.TeacherRepo.GetById(id);
            if (Teacher == null)
            {
                return HttpNotFound();
            }
            return View(Teacher);
        }

        // POST: Teachers/Delete/5
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
                Teacher Teacher = unitOfWork.TeacherRepo.GetById(id);
                if (null == Teacher)
                {
                    return HttpNotFound();
                }
                unitOfWork.TeacherRepo.Delete(Teacher);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        #endregion CRUD Actions
        #region other Actions

        #endregion
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
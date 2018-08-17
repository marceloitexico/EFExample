using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EFApproaches.DAL.Implementations;
using EFApproaches.DAL.Entities;
using System.Configuration;
using EFApproaches.DAL.Interfaces;

namespace EFApproaches.Controllers
{
    public class StudentController : BaseController
    {
        #region private members
        #endregion private members
        #region constructor
        public StudentController() { }
        public StudentController(IUnitOfWork unitOfWork) : base(unitOfWork){}
        #endregion constructor

        // GET: Students
        public ActionResult Index()
        {
            return View(unitOfWork.StudentRepo.DataSet);
        }
        #region CRUD Actions

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = unitOfWork.StudentRepo.GetById(id); 

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //fill the email of the student
                    student.GenerateEmailFromName(schoolDomain);

                    unitOfWork.StudentRepo.Create(student);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception dataEx)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("ExecptionError","Unable to save changes. Try again, and if the problem persists see your system administrator, error: " + dataEx.Message);
            }
           //If model is not valid
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = unitOfWork.StudentRepo.GetById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //If student cannot be updated return default view with student object
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            Student studentToUpdate = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                studentToUpdate = unitOfWork.StudentRepo.GetById(id);
                if (null == studentToUpdate)
                {
                    ModelState.AddModelError("NullModelError", "The student you tried to update was not found");
                    studentToUpdate = new Student();
                }
                if (TryUpdateModel(studentToUpdate, "",
                    new string[] { "LastName", "FirstMidName", "EmailAddress", "EnrollmentDate" }))
                {
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex )
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("SaveChangesExceptionError", "Unable to save changes. " + ex.Message);
                return View(new Student());
            }
            return View(studentToUpdate);
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
            Student student = unitOfWork.StudentRepo.GetById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
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
                Student student = unitOfWork.StudentRepo.GetById(id);
                if (null == student)
                {
                    return HttpNotFound();
                }
                unitOfWork.StudentRepo.Delete(student);
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

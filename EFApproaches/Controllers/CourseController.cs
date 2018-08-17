using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFApproaches.Controllers
{
    public class CourseController : BaseController
    {
        #region constructor
        public CourseController() { }
        public CourseController(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        #endregion
        // GET: Course
        public ActionResult Index()
        {
            return View(unitOfWork.CourseRepo.DataSet);
        }
    }
}
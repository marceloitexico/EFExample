using EFApproaches.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFApproaches.Controllers
{
    public class BaseController : Controller
    {
        protected static UnitOfWork unitOfWork;
        public BaseController()
        {
            unitOfWork = new UnitOfWork();
        }
        public BaseController(SchoolContext ctx)
        {
            unitOfWork = new UnitOfWork(ctx);
        }
    }
}
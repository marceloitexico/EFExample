using EFApproaches.DAL.Implementations;
using EFApproaches.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFApproaches.Controllers
{
    public class BaseController : Controller
    {
        protected string schoolDomain = ConfigurationManager.AppSettings["SchoolDomain"];
        protected static IUnitOfWork unitOfWork;
        //private IUnitOfWork unitOfWork1;

        public BaseController()
        {
            unitOfWork = new UnitOfWork();
        }

        public BaseController(IUnitOfWork unitofwork)
        {
            unitOfWork = unitofwork;
        }
    }
}
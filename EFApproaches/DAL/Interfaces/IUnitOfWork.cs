using EFApproaches.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Interfaces
{
    public class IUnitOfWork
    {
        protected static SchoolContext dbContext;
    }
}
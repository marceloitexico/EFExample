using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFApproaches.DAL.Interfaces
{
    public interface IStudent
    {
        void GenerateEmailFromName(string domain);
    }
}
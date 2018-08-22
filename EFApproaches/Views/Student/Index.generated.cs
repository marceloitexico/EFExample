﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using EFApproaches;
    
    #line 1 "..\..\Views\Student\Index.cshtml"
    using EFApproaches.DAL.Entities;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Student/Index.cshtml")]
    public partial class _Views_Student_Index_cshtml : System.Web.Mvc.WebViewPage<IEnumerable<Student>>
    {
        public _Views_Student_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\Student\Index.cshtml"
  
    ViewBag.Title = "Index";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>Index</h2>\r\n<span");

WriteLiteral(" id=\"viewBagMsgContainer\"");

WriteLiteral(">");

            
            #line 9 "..\..\Views\Student\Index.cshtml"
                          Write(ViewBag.Message);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n<p>\r\n");

WriteLiteral("    ");

            
            #line 11 "..\..\Views\Student\Index.cshtml"
Write(Html.ActionLink("Create New", "Create"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</p>\r\n<table");

WriteLiteral(" class=\"table\"");

WriteLiteral(">\r\n    <tr>\r\n        <th>\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Views\Student\Index.cshtml"
       Write(Html.DisplayNameFor(model => model.FirstMidName));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </th>\r\n        <th>\r\n");

WriteLiteral("            ");

            
            #line 19 "..\..\Views\Student\Index.cshtml"
       Write(Html.DisplayNameFor(model => model.LastName));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </th>\r\n        <th>\r\n");

WriteLiteral("            ");

            
            #line 22 "..\..\Views\Student\Index.cshtml"
       Write(Html.DisplayNameFor(model => model.EmailAddress));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </th>\r\n        <th>\r\n");

WriteLiteral("            ");

            
            #line 25 "..\..\Views\Student\Index.cshtml"
       Write(Html.DisplayNameFor(model => model.EnrollmentDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </th>\r\n        <th></th>\r\n    </tr>\r\n\r\n");

            
            #line 30 "..\..\Views\Student\Index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Student\Index.cshtml"
     foreach (var item in Model)
    {

            
            #line default
            #line hidden
WriteLiteral("        <tr>\r\n            <td");

WriteLiteral(" id=\"firstMidNameValue\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 34 "..\..\Views\Student\Index.cshtml"
           Write(Html.DisplayFor(model => item.FirstMidName));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 37 "..\..\Views\Student\Index.cshtml"
           Write(item.LastName);

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 40 "..\..\Views\Student\Index.cshtml"
           Write(item.EmailAddress);

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 43 "..\..\Views\Student\Index.cshtml"
           Write(item.EnrollmentDate);

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 46 "..\..\Views\Student\Index.cshtml"
           Write(Html.ActionLink("Edit", "Edit", new { id = item.ID }));

            
            #line default
            #line hidden
WriteLiteral(" |\r\n");

WriteLiteral("                ");

            
            #line 47 "..\..\Views\Student\Index.cshtml"
           Write(Html.ActionLink("Details", "Details", new { id = item.ID }));

            
            #line default
            #line hidden
WriteLiteral(" |\r\n");

WriteLiteral("                ");

            
            #line 48 "..\..\Views\Student\Index.cshtml"
           Write(Html.ActionLink("Delete", "Delete", new { id = item.ID }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");

            
            #line 51 "..\..\Views\Student\Index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n</table>\r\n<div");

WriteLiteral(" id=\"amountMessagesContainer\"");

WriteLiteral(">\r\n");

            
            #line 55 "..\..\Views\Student\Index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Student\Index.cshtml"
     if(Model.Count<Student>() > 3)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" id=\"MoreThanTwoStudentsMessage\"");

WriteLiteral(">There are more than 3 students</div>\r\n");

            
            #line 58 "..\..\Views\Student\Index.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" id=\"MaxThreeStudentsMessage\"");

WriteLiteral(">There are 3 or less students</div>\r\n");

            
            #line 62 "..\..\Views\Student\Index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n\r\n");

        }
    }
}
#pragma warning restore 1591

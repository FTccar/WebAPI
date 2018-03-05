using Programming.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApplication1.Security
{
    public class APIAutorizeAttribute :  AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var actionRoles = Roles;
            var userName = HttpContext.Current.User.Identity.Name;
            UserDAL _user = new UserDAL();
            var user = _user.GetUserByName(userName);

            if(user != null && actionRoles.Contains(user.Role))
            {

            }
            else
            {
                HttpResponseMessage unauthorizedMessage = new HttpResponseMessage();
                unauthorizedMessage.StatusCode = HttpStatusCode.Unauthorized;
                actionContext.Response = unauthorizedMessage;
            }

            base.OnAuthorization(actionContext);
        } 
    }
}
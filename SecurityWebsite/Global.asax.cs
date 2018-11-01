using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SecurityWebsite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {

        }

        protected void Application_AuthenticateRequest(object sender,EventArgs args)
        {
            if(Context.User != null)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    //i`m intrested in getting the current logged in user roles
                    //get the roles of the current logged in user
                    var rolesBelongingToUser = new BusinessLogic.UsersBL().GetRolesOfUser(Context.User.Identity.Name);
                    //Context.Idemtity.name --- unique identifier returns the email due to cookie in accounts controller

                    string[] roles = rolesBelongingToUser.Select(x => x.Title).ToArray();

                    GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, roles);
                    Context.User = gp;
                }
            }

           

        }
    }
}

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Roomie.Models
{
    public partial class UserProfile
    {
        
        public static string NextProfile(string id, RoomieEntities db,HttpContextBase context)
        {
            var userId = context.User.Identity.GetUserId();
            var linkedUsers = db.ProfileLinkers.Where(pl => pl.LinkedProfile == userId).Select(pl => pl.UserProfile1);
            var avaliableUsers = db.UserProfiles.Where(up => up.Id != userId && up.Id != id)
                .Except(linkedUsers);
            var choosenUser = avaliableUsers.OrderBy(up => Guid.NewGuid()).FirstOrDefault();
            var rc = new RequestContext(context, new RouteData());
            var urlHelper = new UrlHelper(rc);
            //context.Response.Redirect(urlHelper.Action(actionName), false);
            string action;

            if (choosenUser == null)
            {
                action = urlHelper.Action("Index", "UserProfiles");
            }
            else
            {
                action = urlHelper.Action("Details", "UserProfiles", new { id = choosenUser.Id });
            }
            //context.Response.Redirect(action, false);
            return action;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace RateLimit.Controllers
{
    public class ProfilesController : Controller
    {
        ProfilesService profilesService;

        public ProfilesController(ProfilesService profilesService)
        {
            this.profilesService = profilesService;
        }

        [Route("[action]")]
        public IActionResult Profiles(string filter, string sortKey, int? pageSize, int? pageNum)
        {
            ViewBag.Filter = filter;
            ViewBag.Sort = sortKey;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNum = pageNum;

            pageSize ??= 100;
            pageNum ??= 0;
            sortKey ??= "";

            var prop = typeof(Profile).GetProperty(sortKey);

            var profiles = profilesService.GetProfiles(
                profile => filter == null || profile.FirstName.Contains(filter,StringComparison.InvariantCultureIgnoreCase) || profile.LastName.Contains(filter, StringComparison.InvariantCultureIgnoreCase),
                profile => prop?.GetValue(profile),
                (int)pageSize,
                (int)pageNum);
            
            return View(profiles);
        }
    }
}
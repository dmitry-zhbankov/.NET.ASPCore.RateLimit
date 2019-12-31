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
        public IActionResult Profiles(string filter, string sort, int? pageSize, int? pageNum)
        {
            profilesService.GetProfiles(
                (profile) => profile.FirstName.Contains(filter) || profile.LastName.Contains(filter),
                (profile) => profile.GetType().GetProperty(sort)?.GetType(), pageSize, pageNum);
            return View();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace RateLimit.Controllers
{
    public class ProfilesController : Controller
    {
        ProfilesService _profilesService;

        public ProfilesController(ProfilesService profilesService)
        {
            this._profilesService = profilesService;
        }

        [Route("[action]")]
        [RateLimitAttributeFactory(5)]
        public async Task<IActionResult> Profiles(string filter, string sortKey, int? pageSize, int? pageNum)
        {
            pageSize ??= 20;
            pageNum ??= 1;
            sortKey ??= "";

            var prop = typeof(Profile).GetProperty(sortKey);

            var count = 0;

            var profiles = await Task.Run(
                () => _profilesService.GetProfiles(
                  profile => filter == null || profile.FirstName.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || profile.LastName.Contains(filter, StringComparison.InvariantCultureIgnoreCase),
                  profile => prop?.GetValue(profile),
                  (int)pageSize,
                  (int)pageNum,
                  out count));

            await Task.Delay(2000);

            ViewBag.Filter = filter;
            ViewBag.Sort = sortKey;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNum = pageNum;
            ViewBag.Count = count;

            return View(profiles);
        }
    }
}
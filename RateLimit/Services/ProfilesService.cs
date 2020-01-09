using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RateLimit.Models;

namespace RateLimit.Services
{
    public class ProfilesService
    {
        private ICollection<Profile> _profiles;

        public ProfilesService(string jsonFile)
        {
            var fileStream = new FileStream(jsonFile, FileMode.Open);

            using var streamReader = new StreamReader(fileStream);
            var jsonStr = streamReader.ReadToEnd();

            _profiles = new List<Profile>(JsonSerializer.Deserialize<IEnumerable<Profile>>(jsonStr));
        }

        public IEnumerable<Profile> GetProfiles<TKey>(Func<Profile, bool> filter, Func<Profile, TKey> sort, int pageSize, int pageNum, out int count)
        {
            var res = _profiles.Where(filter).ToList();
            count = res.Count();
            res = res
                .OrderBy(sort)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
            return res;
        }
    }
}

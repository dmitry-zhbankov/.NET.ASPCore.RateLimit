using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RateLimit
{
    public class ProfilesService
    {
        ICollection<Profile> profiles { get; set; }

        public ProfilesService(string jsonFile)
        {
            var jsonStr = string.Empty;
            using (var fileStream = new FileStream("profiles.json", FileMode.Open))
            {
                var streamReader = new StreamReader(fileStream);
                streamReader.ReadToEnd();
            }
            profiles = new List<Profile>(JsonSerializer.Deserialize<IEnumerable<Profile>>(jsonStr));
        }

        public IEnumerable<Profile> GetProfiles<TKey>(Func<Profile, bool> filter, Func<Profile, TKey> sort, int? pageSize, int? pageNum )
        {
            var ps = pageSize ?? 0;
            var pn = pageNum ?? 0;
            return profiles.Where(filter).OrderBy(sort).Skip(ps * pn).Take(ps);
        }
    }
}

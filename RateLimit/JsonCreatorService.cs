using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RateLimit
{
    public class JsonCreatorService
    {
        public JsonCreatorService()
        {
            using var fileStream = new FileStream("profiles.json", FileMode.Create);
            using var jsonWriter = new Utf8JsonWriter(fileStream);
            var rnd = new Random();
            var profiles = Enumerable.Range(0, 100).Select(i => new Profile()
            {
                Id = Guid.NewGuid(),
                FirstName = $"FirstName{rnd.Next(0, 100)}",
                LastName = $"FirstName{rnd.Next(0, 100)}",
                Birthday = new DateTime(rnd.Next(1990, DateTime.Now.Year), rnd.Next(1, 12), rnd.Next(1, 28))
            });
            JsonSerializer.Serialize(jsonWriter, profiles);
        }
    }
}

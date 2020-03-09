using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace Todo.Services
{
    public static class Gravatar
    {
        private static IMemoryCache Cache { get; }

        static Gravatar()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        public static string GetHash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(emailAddress.Trim().ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToLowerInvariant();
            }
        }

        public static async Task<string> GetUserNameAsync(string hash, string url)
        {
            if (!Cache.TryGetValue(hash, out string userName))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var uri = new Uri(url + $"/{hash}.json", UriKind.Absolute);

                        client.Timeout = new TimeSpan(0, 0, 0, 30);
                        client.DefaultRequestHeaders.Add("User-Agent", "Storm-Ideas-App");

                        var response = await client.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            var stringResponse = await response.Content.ReadAsStringAsync();
                            var jsonObject = JObject.Parse(stringResponse);

                            userName = jsonObject["entry"]?.FirstOrDefault()?["preferredUsername"].ToString();

                            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

                            Cache.Set(hash, userName, cacheOptions);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Error logging here
                }
            }

            return userName;
        }
    }
}
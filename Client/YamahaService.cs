using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YamahaExtendedControl.Responses;

namespace Client
{
    public class YamahaService
    {
        string _baseUrl;
        public YamahaService(string url)
        {
            _baseUrl = url;
        }

        public async Task<bool> ConnectAsync()
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(_baseUrl);
                var s = await client.GetStringAsync("YamahaExtendedControl/v1/main");
                if (string.IsNullOrEmpty(s))
                    return false;
                var j = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicResponse>(s);
                return j.response_code == 3;
            }
        }
    }
}

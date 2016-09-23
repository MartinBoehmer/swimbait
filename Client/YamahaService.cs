using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YamahaExtendedControl;
using YamahaExtendedControl.Responses;

namespace Client
{
    public class YamahaService
    {
        string _baseUrl;
        const string _baseApiUrl = "YamahaExtendedControl";
        const string _apiVersion = "v1";

        HttpClient _client;
        public YamahaService(string url)
        {
            _baseUrl = $"{url}/{_baseApiUrl}/{_apiVersion}/";
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<bool> ConnectAsync()
        {
            var s = await _client.GetStringAsync("main");
            if (string.IsNullOrEmpty(s))
                return false;
            var j = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicResponse>(s);
            return j.response_code == 3;
        }

        public async Task<StatusResponse> GetStatusAsync()
        {
            var s = await _client.GetStringAsync("main/getStatus");
            if (string.IsNullOrEmpty(s))
                return null;
            var j = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusResponse>(s);
            return j;
        }

        public async Task<bool> SetPowerAsync(bool on)
        {
            var s = await _client.GetStringAsync($"system/sendIrCode?code={(on ? Constants.IRCODE_ON : Constants.IRCODE_OFF)}");
            if (string.IsNullOrEmpty(s))
                return false;
            var j = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicResponse>(s);
            return j.response_code == 0;
        }

    }
}

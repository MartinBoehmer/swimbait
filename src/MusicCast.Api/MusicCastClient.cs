using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MusicCast;
using MusicCast.Responses;

namespace MusicCast
{
    
    public class MusicCastClient
    {
        string _baseUrl;
        const string _baseApiUrl = "YamahaExtendedControl";
        const string _apiVersion = "v1";

        HttpClient _client;

        /// <summary>
        /// This should probably take a dictionary of device names and their endpoints
        /// It will want to send commands to multiple devies to command them to pair up as one say
        /// </summary>
        public MusicCastClient(string url)
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
            var s = await _client.GetStringAsync($"main/setPower?power={(on ? Constants.ON : Constants.OFF)}");
            if (string.IsNullOrEmpty(s))
                return false;
            var j = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicResponse>(s);
            return j.response_code == 0;
        }

        public async Task<bool> SetPowerAsync(string code)
        {
            var s = await _client.GetStringAsync($"system/sendIrCode?code={code}");
            if (string.IsNullOrEmpty(s))
                return false;
            var j = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicResponse>(s);
            return j.response_code == 0;
        }

    }
}

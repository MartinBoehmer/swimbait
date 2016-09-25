using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swimbait.Server.Controllers.Requests;
using Swimbait.Server.Services;
using MusicCast.Responses;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/v1/system")]
    public class SystemController : BaseController
    {
        private readonly MusicCastHost _musicCastHost;

        public SystemController(ILoggerFactory loggerFactory, MusicCastHost musicCastHost) : base(loggerFactory)
        {
            _musicCastHost = musicCastHost;
        }
        

        [HttpGet("IsNewFirmwareAvailable")]
        public IActionResult IsNewFirmwareAvailable()
        {
            var r = new IsNewFirmwareAvailableResponse();
            r.available = false;
            return new ObjectResult(r);
        }

        [HttpGet("stopInitialJoin")]
        public IActionResult StopInitialJoin()
        {
            return new ObjectResult(new BasicResponse());
        }

        [HttpGet("getFuncStatus")]
        public IActionResult GetFuncStatus()
        {
            var response = new FuncStatusResponse();
            response.response_code = 0;
            response.auto_power_standby = true;
            return new ObjectResult(response);
        }

        [HttpGet("GetFeatures")]
        public IActionResult GetFeatures()
        {
            var response = new FeaturesResponse();
            response.netusb.preset.num = 40;
            var zone = new Zone();

            zone.func_list.Add("power");
            zone.func_list.Add("sleep");
            zone.func_list.Add("volume");
            zone.func_list.Add("mute");
            zone.func_list.Add("equalizer");
            zone.func_list.Add("prepare_input_change");
            zone.func_list.Add("link_control");

            response.zone.Add(zone);

            zone.input_list.Add("pandora");
            zone.input_list.Add("spotify");
            zone.input_list.Add("airplay");
            zone.input_list.Add("mc_link");
            zone.input_list.Add("server");
            zone.input_list.Add("net_radio");
            zone.input_list.Add("bluetooth");

            zone.link_control_list.Add("standard");
            zone.link_control_list.Add("stability");

            zone.id = "main";

            var volumeRangeStep = new RangeStep();
            volumeRangeStep.id = "volume";
            volumeRangeStep.min = 0;
            volumeRangeStep.max = 60;
            volumeRangeStep.step = 1;

            var equalizerRangeStep = new RangeStep();
            equalizerRangeStep.id = "equalizer";
            equalizerRangeStep.min = -10;
            equalizerRangeStep.max = 10;
            equalizerRangeStep.step = 1;

            zone.range_step.Add(volumeRangeStep);
            zone.range_step.Add(equalizerRangeStep);

            response.system.func_list.Add("wired_lan");
            response.system.func_list.Add("wireless_lan");
            response.system.func_list.Add("wireless_direct");
            response.system.func_list.Add("network_standby");
            response.system.func_list.Add("auto_power_standby");
            response.system.func_list.Add("bluetooth_tx_setting");
            response.system.func_list.Add("airplay");
            response.system.func_list.Add("stereo_pair");
            
            response.system.zone_num = 1;

            var pandoraInput = new InputList2();
            var spotifyInput = new InputList2();
            var airplayInput = new InputList2();
            var mcLinkInput = new InputList2();
            var serverInput = new InputList2();
            var bluetoothInput = new InputList2();
            var netRadioInput = new InputList2();

            response.system.input_list.Add(pandoraInput);
            response.system.input_list.Add(spotifyInput);
            response.system.input_list.Add(airplayInput);
            response.system.input_list.Add(mcLinkInput);
            response.system.input_list.Add(serverInput);
            response.system.input_list.Add(netRadioInput);
            response.system.input_list.Add(bluetoothInput);

            pandoraInput.id = "pandora";
            pandoraInput.distribution_enable = true ;
            pandoraInput.rename_enable = false      ;
            pandoraInput.account_enable = true      ;
            pandoraInput.play_info_type = "netusb"  ;
            
            spotifyInput.id = "spotify";
            spotifyInput.distribution_enable = true;
            spotifyInput.rename_enable = false;
            spotifyInput.account_enable = false;
            spotifyInput.play_info_type = "netusb";
            
            airplayInput.id = "airplay";
            airplayInput.distribution_enable = false;
            airplayInput.rename_enable = false;
            airplayInput.account_enable = false;
            airplayInput.play_info_type = "netusb";

            mcLinkInput.id = "mc_link";
            mcLinkInput.distribution_enable = false;
            mcLinkInput.rename_enable = true;
            mcLinkInput.account_enable = false;
            mcLinkInput.play_info_type = "netusb";

            serverInput.id = "server";
            serverInput.distribution_enable = true;
            serverInput.rename_enable = true;
            serverInput.account_enable = false;
            serverInput.play_info_type = "netusb";

            netRadioInput.id = "net_radio";
            netRadioInput.distribution_enable = true;
            netRadioInput.rename_enable = true;
            netRadioInput.account_enable = false;
            netRadioInput.play_info_type = "netusb";

            bluetoothInput.id = "bluetooth";
            bluetoothInput.distribution_enable = true;
            bluetoothInput.rename_enable = false;
            bluetoothInput.account_enable = false;
            bluetoothInput.play_info_type = "netusb";
            
            return new ObjectResult(response);
        }

        [HttpGet("getLocationInfo")]
        public IActionResult GetLocationInfo()
        {
            var response = new LocationInfoResponse();
            response.id = _musicCastHost.LocationId;
            return new ObjectResult(response);
        }

        [HttpGet("getTag")]
        public IActionResult GetTag(string id = null)
        {
            object response = null;

            if (string.IsNullOrEmpty(id))
            {
                var response1 = new GetTagResponse();

                var mainTag = "-1"; // default
                if (_musicCastHost.HasTag("main"))
                {
                    mainTag = _musicCastHost.GetTag("main");
                }
                var mainTagAsInt = Convert.ToInt32(mainTag);

                response1.zone_list.Add(new IntegerInputList {id = "main", tag = mainTagAsInt});

                response1.input_list.Add("bluetooth", 0);
                response1.input_list.Add("server", 0);
                response1.input_list.Add("net_radio", 0);
                response1.input_list.Add("pandora", 0);
                response1.input_list.Add("spotify", 0);
                response1.input_list.Add("airplay", 0);
                response1.input_list.Add("mc_link", 0);

                response = response1;
            }
            else
            {
                var response2 = new GetTagResponse2();
                response2.id = id;
                var tagString = _musicCastHost.GetTag(id);
                var tagInt = Convert.ToInt32(tagString);
                response2.tag = tagInt;

                response = response2;
            }

            return new ObjectResult(response);
        }

        [HttpPost("SetNameText")]
        public IActionResult SetNameText()
        {
            var json = ReadBody();
            var request = JsonConvert.DeserializeObject<SetNameTextRequest>(json);
            if (request == null)
            {
                Log.LogWarning("Failed to read body");
                return MusicCastOk();
            }
            _musicCastHost.Name = request.text;
            Log.LogInformation($"Set nameText={request.text}");
            return MusicCastOk();
        }

        [HttpPost("SetLocationName")]
        public IActionResult SetLocationName()
        {
            
            var json = ReadBody();
            var request = JsonConvert.DeserializeObject<SetLocationNameRequest>(json);
            if (request == null)
            {
                Log.LogWarning("Failed to read body");
                return MusicCastOk();
            }
            Log.LogInformation($"Set name={request.name}");
            return MusicCastOk();
        }

        [HttpPost("SetLocationId")]
        public IActionResult SetLocationId()
        {
            var json = ReadBody();
            var request = JsonConvert.DeserializeObject<SetLocationIdRequest>(json);
            if (request == null)
            {
                Log.LogWarning("Failed to read body");
                return MusicCastOk();
            }
            Log.LogInformation($"Set id={request.id}");
            _musicCastHost.LocationId = request.id;
            return MusicCastOk();
        }


        [HttpGet("setTag")]
        public IActionResult SetTag(string id, string tag)
        {
            Log.LogInformation($"Set tag={tag}");
            _musicCastHost.SetTag(id, tag);
            return MusicCastOk();
        }



        [HttpGet("getNameText")]
        public IActionResult GetNameText(string id = null)
        {
            object response;
            if (string.IsNullOrEmpty(id))
            {
                var response1 = new NameTextResponse();
                response1.zone_list.AddText("main", _musicCastHost.Name);
                response1.input_list.AddText("bluetooth", "Bluetooth");
                response1.input_list.AddText("server", "Server");
                response1.input_list.AddText("net_radio", "Net Radio");
                response1.input_list.AddText("pandora", "Pandora");
                response1.input_list.AddText("spotify", "Spotify");
                response1.input_list.AddText("airplay", "AirPlay");
                response1.input_list.AddText("mc_link", "MC Link");
                response = response1;
            }
            else
            {
                var response2 = new NameTextResponse2();
                response2.id = id;
                response2.text = _musicCastHost.Name;
                response = response2;
            }
            return new ObjectResult(response);
        }

        [HttpGet("getNetworkStandby")]
        public IActionResult GetNetworkStandby()
        {
            var response = new NetworkStandbyResponse();
            response.network_standby = "on";
            return new ObjectResult(response);
        }

        [HttpGet("getDeviceInfo")]
        public IActionResult GetDeviceInfo()
        {
            var response = new DeviceInfoResponse();
            response.model_name = "WX-030";
            response.destination = "A";
            response.system_id = _musicCastHost.SerialNumber;
            response.system_version = _musicCastHost.SystemVersion;
            response.api_version = _musicCastHost.ApiVersion;
            response.netmodule_version= "0516    ";
            response.netmodule_checksum = "DF4473CE";
            response.system_version = _musicCastHost.SystemVersion;
            response.operation_mode = "normal";
            response.update_error_code = "00000000";

            return new ObjectResult(response);
        }

        [HttpGet("sendIrCode")]
        public IActionResult SendIRCode(string code)
        {
             var response = new BasicResponse();
             return new ObjectResult(response);
        }
    }
}

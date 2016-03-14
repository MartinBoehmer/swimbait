﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swimbait_srv2.Multicast.Requests
{
    public interface IMulticastRequest
    {
        byte[] AsBytes();
    }

    public abstract class MulticastRequest : IMulticastRequest
    {
        protected StringBuilder RequestBuilder { get; private set; }

        protected MulticastRequest()
        {
            RequestBuilder = new StringBuilder();
        }

        public byte[] AsBytes()
        {
            return Encoding.UTF8.GetBytes(RequestBuilder.ToString());
        }
    }

    public class SsdpDiscover : MulticastRequest
    {
        public SsdpDiscover(string endpointIp, int endpointPort)
        {
            RequestBuilder.Append(
                $@"M-SEARCH * HTTP/1.1
Host: {endpointIp}:{endpointPort}
Content-Length: 0
MAN: ""ssdp:discover""
MX: 2
ST: urn:schemas-upnp-org:device:MediaRenderer:1

");
        }
    }
}

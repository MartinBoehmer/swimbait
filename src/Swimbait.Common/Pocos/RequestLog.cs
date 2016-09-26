using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Common
{
    public class RequestLog
    {

        public int ActualPort { get; set; }

        public int YamahaPort { get; set; }

        public string Method { get; set; }

        public string PathAndQuery { get; set; }

        public string RequestBody { get; set; }


        public static RequestLog FromCsv(string csv)
        {
            var cols = csv.Split(',');
            var log = new RequestLog();
            // 0 = time
            log.ActualPort = Convert.ToInt32(cols[1]);
            log.YamahaPort = Convert.ToInt32(cols[2]);
            log.Method = cols[3];
            log.PathAndQuery = cols[4];
            log.RequestBody = cols[5];
            return log;
        }
    }
}

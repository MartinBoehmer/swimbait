using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string activityLog = @"D:\Downloads\swimbait\activity.txt";
            var activity = File.ReadAllLines(activityLog);

            foreach(var line in activity)
            {
                var cols = line.Split(',');
                var actualPort = cols[0];
                var yamahaPort = cols[1];
                var method = cols[2];
                var pathAndQuery = cols[3];
                var requestBody = cols[4];             


            }
        }


    }
}

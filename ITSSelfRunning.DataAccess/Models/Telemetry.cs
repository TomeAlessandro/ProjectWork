using System;
using System.Collections.Generic;
using System.Text;

namespace ITSSelfRunning.DataAccess.Models
{
    public class Telemetry
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime Moment { get; set; }
        public string UriSelfie { get; set; }
        public int Activity_Id { get; set; }
    }
}

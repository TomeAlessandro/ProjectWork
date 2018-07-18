using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSSelfRunning.Models
{
    public class Activity
    {
        public int IdActivity { get; set; }
        public string ActivityName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Location { get; set; }
        public int Status { get; set; }
        public int ActivityType { get; set; }
        public string UriGara { get; set; }
        public int Runner_Id { get; set; }
    }
}

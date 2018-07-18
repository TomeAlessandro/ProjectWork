using System;
using System.Collections.Generic;
using System.Text;

namespace ITSSelfRunning.DataAccess.Models
{
    public class Runner
    {
        public int IdRunner { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int? Sex { get; set; }

        public string PhotoUri { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFiles
{
    class SCRequests
    {
        public string MessageId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime FlightDate { get; set; }
        public string BoardPoint { get; set; }
        public string OffPoint { get; set; }
        public DateTime BoardTime { get; set; }
        public DateTime OffTime { get; set; }
        public string Config { get; set; }
        public string SCReason { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFiles.Data
{
    public class SubMessage
    {
        public int ID { get; set; }
        public string MessageID { get; set; }
        [Requiured]
        public string Content { get; set; }
        public int SITATEXID { get; set; }
    }
}

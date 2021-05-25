﻿using ReadFiles.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadFiles
{
    public class SCC_SITATEX
    {
        [Key]
        public int ID { get; set; }
        public string Header { get; set; }
        public string Priority { get; set; }
        public string Destinations { get; set; }
        public string Origin { get; set; }
        public string MessageId { get; set; }
        public string Text { get; set; }
        public string SubMessage { get; set; }
        public string MessageEnd { get; set; }

        public ICollection<SchedulesMessage> schedulesMessages { get; set; }
    }
    public class DestinationTypeB
    {
        [Key]
        public int ID { get; set; }
        public string Destionations { get; set; }
    }
}
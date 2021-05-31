using ReadFiles.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadFiles
{
    public class SITATEX
    {
        [Key]
        public int ID { get; set; }
        public string FileName { get; set; }
        public string Header { get; set; }
        public string Priority { get; set; }
        public string Destinations { get; set; }
        public string Origin { get; set; }
        [StringLength(20)]
        public string MessageId { get; set; }
        [StringLength(3)]
        public string SMI { get; set; }
        public string Text { get; set; }
        public string MessageEnd { get; set; }

        public ICollection<SubMessage> SubMessages { get; set; }
    }
}

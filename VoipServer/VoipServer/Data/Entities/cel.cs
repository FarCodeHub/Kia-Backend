using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VoipServer.Data.Entities
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("cel",Schema = "asterisk")]
    public class cel
    {
        public int id { get; set; }
        public string eventtype { get; set; }
        public DateTime eventtime { get; set; }
        public string cid_name { get; set; }
        public string cid_num { get; set; }
        public string cid_ani { get; set; }
        public string cid_rdnis { get; set; }
        public string cid_dnid { get; set; }
        public string exten { get; set; }
        public string context { get; set; }
        public string channame { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public string channel { get; set; }
        public string dstchannel { get; set; }
        public string appname { get; set; }
        public string appdata { get; set; }
        public int amaflags { get; set; }
        public string accountcode { get; set; }
        public string uniqueid { get; set; }
        public string linkedid { get; set; }
        public string peer { get; set; }
        public string userdeftype { get; set; }
        public string eventextra { get; set; }
        public string userfield { get; set; }
        public int seen { get; set; }
    }
}

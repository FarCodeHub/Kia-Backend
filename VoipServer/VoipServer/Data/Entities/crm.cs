using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VoipServer.Data.Entities
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("crm", Schema = "asterisk")]
    public class crm
    {
        public int id { get; set; }
        public string contact { get; set; }
        public string queue { get; set; }
    }
}
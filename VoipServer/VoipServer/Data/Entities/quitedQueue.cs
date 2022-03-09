using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VoipServer.Data.Entities
{
    // ReSharper disable once InconsistentNaming
    [Table("quitedQueue", Schema = "asterisk")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class quitedQueue
    {
        public int id { get; set; }
        public string uniqueid { get; set; }
    }
}
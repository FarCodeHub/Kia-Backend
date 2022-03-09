using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class Message
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
    }
}
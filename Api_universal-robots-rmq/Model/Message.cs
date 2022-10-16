using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api_universal_robots_rmq.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string? message { get; set; }

        public DateTime created { get; set; }

        public enum warningstate {advarsel, stoppet, nopower }

    }
}

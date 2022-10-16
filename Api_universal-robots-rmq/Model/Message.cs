using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api_universal_robots_rmq.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public WarningState State { get; set; }
    }

    public enum WarningState
    {
        Warning,
        Info,
        Error
    }
}

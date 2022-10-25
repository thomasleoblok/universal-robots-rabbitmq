using System.ComponentModel.DataAnnotations;

namespace MessageModel
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public int RobotId { get; set; }
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